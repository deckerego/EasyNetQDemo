using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Topshelf;

namespace Pi.Service
{
	class Program
	{
		static void Main(string[] args)
		{
			HostFactory.Run(c =>
			{
				c.SetServiceName("PiService");
				c.SetDisplayName("Pi Calculation Service");
				c.SetDescription("An EasyNetQ service for poorly calculating Pi");
				c.StartAutomatically();
				c.RunAsLocalService();

				c.Service<PiService>(s =>
				{
					s.ConstructUsing(builder => new PiService());
					s.WhenStarted(o => o.Start());
					s.WhenStopped(o => o.Stop());
					s.WhenPaused(o => o.Paused());
					s.WhenContinued(o => o.Continue());
				});
			});
		}
	}
}
