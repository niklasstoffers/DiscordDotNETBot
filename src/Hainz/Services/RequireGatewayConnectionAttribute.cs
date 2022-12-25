namespace Hainz.Services;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class RequireGatewayConnectionAttribute : Attribute
{
    public RequireGatewayConnectionAttribute()
    {
    }

    public bool AllowRestart { get; init; }
}