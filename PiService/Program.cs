using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EasyNetQ;
using Topshelf;

using Pi.Library.Message;
using Pi.Service.Endpoint;

namespace Pi.Service
{
	class Program
	{
		static void Main(string[] args)
		{
			IBus bus = RabbitHutch.CreateBus("host=10.211.55.2;port=5672;virtualHost=/;username=guest;password=guest");
			bus.Respond<CalculateRequest, CalculateResponse>(CalculateConsumer.Consume);

			HostFactory.Run(c =>
			{
				c.SetServiceName("PiService");
				c.SetDisplayName("Pi Calculation Service");
				c.SetDescription("An EasyNetQ service for poorly calculating Pi");
				c.RunAsLocalService();

				bus.ToString();

				c.Service<PiService>(s =>
				{
					s.ConstructUsing(builder => new PiService(bus));
					s.WhenStarted(o => o.Start());
					s.WhenStopped(o =>
					{
						o.Stop();
						bus.Dispose();
					});
					s.WhenPaused(o => o.Paused());
					s.WhenContinued(o => o.Continue());
				});
			});
		}
	}
}
