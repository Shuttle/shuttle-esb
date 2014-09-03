using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Castle
{
	public class CastleMessageHandlerFactory : MessageHandlerFactory
	{
		private readonly IWindsorContainer container;

		private static readonly Type generic = typeof(IMessageHandler<>);

		private readonly List<Type> messageTypesHandled = new List<Type>();

		public CastleMessageHandlerFactory(IWindsorContainer container)
		{
			Guard.AgainstNull(container, "container");

			this.container = container;
		}

		public override IMessageHandler CreateHandler(object message)
		{
			var all = container.ResolveAll(generic.MakeGenericType(message.GetType()));

			return all.Length != 0 ? (IMessageHandler)all.GetValue(0) : null;
		}

		public override IEnumerable<Type> MessageTypesHandled
		{
			get { return new ReadOnlyCollection<Type>(messageTypesHandled); }
		}

		public override void Initialize(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			if (!container.Kernel.HasComponent(typeof(IServiceBus)))
			{
				container.Register(Component.For<IServiceBus>().Instance(bus));
			}

			messageTypesHandled.Clear();

			var handlers = container.Kernel.GetAssignableHandlers(typeof(IMessageHandler));

			foreach (var handler in handlers)
			{
				foreach (var type in handler.ComponentModel.Implementation.InterfacesAssignableTo(generic))
				{
					messageTypesHandled.Add(type.GetGenericArguments()[0]);
				}
			}
		}

		public override void ReleaseHandler(IMessageHandler handler)
		{
			base.ReleaseHandler(handler);

			container.Release(handler);
		}
	}
}