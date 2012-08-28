using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Castle.Windsor;
using Castle.MicroKernel.Registration;
using MassTransit;

namespace PiASP
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			IWindsorContainer container = new WindsorContainer();
			var bus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseRabbitMq();
				sbc.UseRabbitMqRouting();
				sbc.ReceiveFrom("rabbitmq://10.211.55.2/TestWeb");
				sbc.SetConcurrentConsumerLimit(1);

				sbc.UseControlBus();

				sbc.Subscribe(subs =>
				{
					subs.LoadFrom(container);
				});
			});

			container.Register(Component.For<IServiceBus>().Instance(bus));
			Application["Container"] = container;
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{
			IWindsorContainer container = (IWindsorContainer)Application["Container"];
			container.Dispose();
		}
	}
}