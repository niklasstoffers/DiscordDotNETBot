using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hainz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration(ConfigureApp)
    .ConfigureServices(ConfigureServices)
    .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
    .RunConsoleAsync();

void ConfigureApp(IConfigurationBuilder configBuilder)
{
    configBuilder.AddJsonFile("appsettings.json", optional: true);
}

void ConfigureServices(IServiceCollection services) 
{
    services.AddHostedService<Bot>();
}

void ConfigureContainer(ContainerBuilder builder) 
{
    var assembly = Assembly.GetExecutingAssembly();
    builder.RegisterAssemblyModules(assembly);
}