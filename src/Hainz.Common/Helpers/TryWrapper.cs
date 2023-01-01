namespace Hainz.Common.Helpers;

public static class TryWrapper
{
    public static async Task<bool> TryAsync(Func<Task> action, Action<Exception>? onFailed = null)
    {
        try
        {
            await action();
            return true;
        }
        catch (Exception ex) 
        {
            onFailed?.Invoke(ex);
            return false;
        }
    }

    public static async Task<T?> TryAsync<T>(Func<Task<T>> action, T? defaultValue = default, Action<Exception>? onFailed = null)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            onFailed?.Invoke(ex);
            return defaultValue;
        }
    }
}