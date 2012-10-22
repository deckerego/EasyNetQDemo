using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Topshelf;

namespace DataWarehouse.Service
{
	class Program
	{
		static void Main(string[] args)
		{
			HostFactory.Run(c =>
			{
				c.SetServiceName("DataWarehouseService");
				c.SetDisplayName("Data Warehouse Monitoring Service");
				c.SetDescription("Listen to events and data messages, then record them!");
				c.StartAutomatically();
				c.RunAsLocalService();

				c.Service<DataWarehouseService>(s =>
				{
					s.ConstructUsing(builder => new DataWarehouseService());
					s.WhenStarted(o => o.Start());
					s.WhenStopped(o => o.Stop());
					s.WhenPaused(o => o.Paused());
					s.WhenContinued(o => o.Continue());
				});
			});
		}
	}
}
