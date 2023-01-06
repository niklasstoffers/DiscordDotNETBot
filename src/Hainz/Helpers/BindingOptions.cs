using System.Linq.Expressions;

namespace Hainz.Helpers;

public class BindingOptions<T> where T : class
{
    public List<Binding<T>> Bindings { get; } = new();

    public BindingOptions<T> Bind<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
    {
        Bindings.Add(new Binding<T>(expression, value));
        return this;
    }
}