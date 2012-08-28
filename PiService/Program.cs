using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Magnum;
using MassTransit;
using MassTransit.WindsorIntegration;

namespace PiService
{
	class Program
	{
		static void Main(string[] args)
		{
			//TODO Move IoC stuff to Install instances
			var container = new WindsorContainer();
			container.Register(AllTypes.FromThisAssembly().BasedOn<IConsumer>().LifestyleSingleton());

			var massTransitBus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseRabbitMqRouting();
				sbc.ReceiveFrom("rabbitmq://10.211.55.2/Pi");
				sbc.SetConcurrentConsumerLimit(1);
				sbc.UseJsonSerializer();
				sbc.Subscribe(subs => subs.LoadFrom(container));
			});
			container.Register(Component.For<IServiceBus>().Instance(massTransitBus).LifeStyle.Singleton);

			massTransitBus.WriteIntrospectionToConsole();
		}
	}
}
