using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Magnum;
using MassTransit;
using MassTransit.WindsorIntegration;
using Topshelf;

namespace Pi.Service
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

			container.Register(
				Component.For<IServiceBus>().Instance(massTransitBus).LifeStyle.Singleton,
				Component.For<PiService>().LifeStyle.Singleton
			);

			HostFactory.Run(c =>
			{
				c.SetServiceName("PiService");
				c.SetDisplayName("Pi Calculation Service");
				c.SetDescription("A MassTransit service for poorly calculating Pi");
				c.RunAsLocalService();

				massTransitBus.WriteIntrospectionToConsole();

				c.Service<PiService>(s =>
				{
					s.ConstructUsing(builder => container.Resolve<PiService>());
					s.WhenStarted(o => o.Start());
					s.WhenStopped(o =>
					{
						o.Stop();
						container.Dispose();
					});
					s.WhenPaused(o => o.Paused());
					s.WhenContinued(o => o.Continue());
				});
			});
		}
	}
}
