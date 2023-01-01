namespace Hainz.Data.Configuration;

public sealed class PersistenceConfiguration
{
    public string Host { get; init; } = null!;
    public int Port { get; init; }
    public string Database { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;

    public string ToConnectionString() =>
        $"Server={Host};" +
        $"Port={Port};" +
        $"Database={Database};" +
        $"Username={Username};" +
        $"Password={Password};";
}