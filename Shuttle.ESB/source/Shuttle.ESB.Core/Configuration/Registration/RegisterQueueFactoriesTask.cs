using System;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class RegisterQueueFactoriesTask : RegistrationTask
    {
        public override void Execute(ServiceBusConfiguration configuration)
        {
            var factoryTypes = new List<Type>();

            ReflectionService.GetAssemblies(AppDomain.CurrentDomain.BaseDirectory).ForEach(assembly => factoryTypes.AddRange(ReflectionService.GetTypes<IQueueFactory>(assembly)));

            foreach (var type in factoryTypes.Union(ReflectionService.GetTypes<IQueueFactory>()))
            {
                try
                {
                    type.AssertDefaultConstructor(string.Format(Resources.DefaultConstructorRequired,
                                                                "Queue factory", type.FullName));

                    var instance = (IQueueFactory)Activator.CreateInstance(type);

                    if (!configuration.Queues.ContainsQueueFactory(instance.Scheme))
                    {
                        configuration.Queues.RegisterQueueFactory(instance);
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(string.Format("Queue factory not instantiated: {0}", ex.Message));
                }
            }
        }
    }
}