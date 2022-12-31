namespace Hainz.Config;

public static class EnvironmentVariable
{
    public static string GetDotNetEnvironment() => Environment.GetEnvironmentVariable(EnvironmentVariableName.DotNetEnvironment) ?? EnvironmentName.Default;

    public static int? GetPersistencePort()
    {
        string? portString = Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistencePort);
        if (!string.IsNullOrEmpty(portString))
            return int.Parse(portString);
        return null;
    }

    public static string? GetPersistenceHostname() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistenceHostname);
    public static string? GetPersistenceUsername() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistenceUsername);
    public static string? GetPersistencePassword() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistencePassword);
    public static string? GetPersistenceDatabase() => Environment.GetEnvironmentVariable(EnvironmentVariableName.PersistenceDatabase);
}