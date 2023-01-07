namespace Hainz.Config;

public static class EnvironmentVariable
{
    public static string GetDotNetEnvironment() => Environment.GetEnvironmentVariable(EnvironmentVariableName.DotNetEnvironment) ?? EnvironmentName.Default;
    public static bool GetPreventMigrations() => GetEnvironmentVariable(EnvironmentVariableName.PreventMigrations, bool.Parse) ?? false;

    public static int? GetPersistencePort() => GetEnvironmentVariable(EnvironmentVariableName.PersistencePort, int.Parse);
    public static string? GetPersistenceHostname() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistenceHostname);
    public static string? GetPersistenceUsername() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistenceUsername);
    public static string? GetPersistencePassword() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistencePassword);
    public static string? GetPersistenceDatabase() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistenceDatabase);

    public static string? GetCacheHostname() => Environment.GetEnvironmentVariable(EnvironmentVariableName.CacheHostname);
    public static int? GetCachePort() => GetEnvironmentVariable(EnvironmentVariableName.CachePort, int.Parse);

    private static T? GetEnvironmentVariable<T>(string variableName, Func<string, T> converter) where T : struct
    {
        string? variableValue = Environment.GetEnvironmentVariable(variableName);
        if (!string.IsNullOrEmpty(variableValue))
            return converter(variableValue);
        return null;
    }
}