using System.Diagnostics.CodeAnalysis;

namespace Hainz.Config;

public static class EnvironmentVariable
{
    public static string GetDotNetEnvironment() => Get(EnvironmentVariableName.DotNetEnvironment, EnvironmentName.Default);
    public static bool GetPreventMigrations() => Get<bool>(EnvironmentVariableName.PreventMigrations, false).Value;

    public static int? GetPersistencePort() => Get<int>(EnvironmentVariableName.PersistencePort);
    public static string? GetPersistenceHostname() => Get(EnvironmentVariableName.PersistenceHostname);
    public static string? GetPersistenceUsername() => Get(EnvironmentVariableName.PersistenceUsername);
    public static string? GetPersistencePassword() => Get(EnvironmentVariableName.PersistencePassword);
    public static string? GetPersistenceDatabase() => Get(EnvironmentVariableName.PersistenceDatabase);

    public static string? GetCacheHostname() => Get(EnvironmentVariableName.CacheHostname);
    public static int? GetCachePort() => Get<int>(EnvironmentVariableName.CachePort);

    [return: NotNullIfNotNull("defaultValue")]
    public static string? Get(string variableName, string? defaultValue = null) =>
        Environment.GetEnvironmentVariable(variableName) ?? defaultValue;

    [return: NotNullIfNotNull("defaultValue")]
    public static T? Get<T>(string variableName, T? defaultValue = null) where T : struct
    {
        string? variableValue = Environment.GetEnvironmentVariable(variableName);
        if (!string.IsNullOrEmpty(variableValue))
            return (T)Convert.ChangeType(variableValue, typeof(T));
        return defaultValue;
    }
}