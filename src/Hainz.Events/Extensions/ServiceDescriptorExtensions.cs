using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Events.Extensions;

public static class ServiceDescriptorExtensions
{
    public static object GetInstance(this ServiceDescriptor descriptor, IServiceProvider serviceProvider) =>
        descriptor.ImplementationInstance ?? 
        descriptor.ImplementationFactory?.Invoke(serviceProvider) ??
        throw new ArgumentException("Service descriptor has no instance", nameof(descriptor));
}