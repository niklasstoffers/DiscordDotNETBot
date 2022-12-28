namespace Hainz.Hosting;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class RequireGatewayConnectionAttribute : Attribute
{
    public RequireGatewayConnectionAttribute() { }
}