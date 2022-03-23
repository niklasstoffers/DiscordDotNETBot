using Hainz.InterfaceCommands.TypeReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public class HandlerInvoker
    {
        public InvokeHandler Invoker { get; private set; }

        public HandlerInvoker(MethodInfo handler)
        {
            ParameterInfo[] parameterInfos = handler.GetParameters();
            Invoker = (module, @params) =>
            {
                try
                {
                    object[] parameters = new object[parameterInfos.Length];
                    for(int i = 0; i < parameterInfos.Length; i++)
                    {
                        if (i >= @params.Length && !parameterInfos[i].IsOptional)
                            return;
                        TypeReader reader = TypeReaderBuilder.GetReader(parameterInfos[i].ParameterType);
                        if (!(reader.TryRead(@params[i], out parameters[i]) || parameterInfos[i].IsOptional))
                            return;
                    }

                    handler.Invoke(module, parameters);
                }
                catch { }
            };
        }
    }
}
