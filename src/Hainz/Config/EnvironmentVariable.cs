namespace Hainz.Config;

public static class EnvironmentVariable
{
    public static string GetDotNetEnvironment() => Environment.GetEnvironmentVariable(EnvironmentVariableName.DotNetEnvironment) ?? EnvironmentName.Default;

    public static int? GetPersistencePort() => GetEnvironmentVariableAsInt(EnvironmentVariableName.PersistencePort);
    public static string? GetPersistenceHostname() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistenceHostname);
    public static string? GetPersistenceUsername() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistenceUsername);
    public static string? GetPersistencePassword() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistencePassword);
    public static string? GetPersistenceDatabase() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistenceDatabase);

    public static string? GetCacheHostname() => Environment.GetEnvironmentVariable(EnvironmentVariableName.CacheHostname);
    public static int? GetCachePort() => GetEnvironmentVariableAsInt(EnvironmentVariableName.CachePort);
    
    private static int? GetEnvironmentVariableAsInt(string variableName)
    {
        string? variableValue = Environment.GetEnvironmentVariable(variableName);
        if (!string.IsNullOrEmpty(variableValue))
            return int.Parse(variableValue);
        return null;
    }
}