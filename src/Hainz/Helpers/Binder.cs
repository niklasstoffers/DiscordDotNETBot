namespace Hainz.Helpers;

public static class Binder
{
    public static void Bind<T>(T value, BindingOptions<T> options) where T : class
    {
        foreach (var binding in options.Bindings)
            binding.Apply(value);
    }
}