using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Hainz.Framework 
{
    public class ContainerManager : IDisposable
    {
        private IContainer _container;
        private ILifetimeScope _scope;

        public ILifetimeScope Resolver => BuildResolver();

        private ILifetimeScope BuildResolver() 
        {
            if (_scope != null)
                return _scope;

            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyModules(assembly);

            builder.Register<IServiceProvider>(ctx => 
            {
                var autofacProvider = new AutofacServiceProvider(_scope);
                return autofacProvider;
            }).AsSelf().InstancePerLifetimeScope();

            _container = builder.Build();
            _scope = _container.BeginLifetimeScope();
            return _scope;
        }

        public void Dispose() 
        {
            _scope.Dispose();
            _container.Dispose();
        }
    }
}