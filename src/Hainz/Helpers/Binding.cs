using System.Linq.Expressions;

namespace Hainz.Helpers;

public class Binding<T> where T : class
{
    public LambdaExpression PropSelectorLambda { get; init; }
    public object? Value { get; init; }

    public Binding(LambdaExpression propSelectorLambda, object? value)
    {
        PropSelectorLambda = propSelectorLambda;
        Value = value;
    }

    public void Apply(T instance)
    {
        if (Value == null)
            return;

        MemberExpression? propSelectorExpression;
        if (PropSelectorLambda.Body is UnaryExpression unaryExpression && unaryExpression.IsLiftedToNull)
            propSelectorExpression = unaryExpression.Operand as MemberExpression;
        else
            propSelectorExpression = PropSelectorLambda.Body as MemberExpression;

        if (propSelectorExpression?.Expression == null)
            throw new InvalidOperationException("Invalid binding");

        var propDeclaratorCastExpression = Expression.Convert(propSelectorExpression.Expression, typeof(object));
        var declaratorResolveExpression = Expression.Lambda<Func<T, object>>(propDeclaratorCastExpression, true, PropSelectorLambda.Parameters);
        var declaratorInstance = declaratorResolveExpression.Compile()(instance);

        var propertyInfo = declaratorInstance.GetType().GetProperty(propSelectorExpression.Member.Name);
        propertyInfo!.SetValue(declaratorInstance, Value);
    }
}