using Autofac;
using Hainz.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hainz
{
    public class Initializer
    {
        public IContainer BuildDIContainer()
        {
            var containerBuilder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            containerBuilder.RegisterAssemblyModules(assembly);
            
            return containerBuilder.Build();
        }
    }
}
