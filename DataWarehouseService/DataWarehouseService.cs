using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

using EasyNetQ;
using EasyNetQ.Topology;

using Pi.Library.Message;
using DataWarehouse.Service.Endpoint;
using System.Threading.Tasks;

namespace DataWarehouse.Service
{
	public class DataWarehouseService
	{
		private IBus Bus;

		protected void RegisterEndpoints()
		{
			string routingKey = EasyNetQ.TypeNameSerializer.Serialize(typeof(Pi.Library.Message.CalculateRequest));
			var wiretapCalculateRequestExchange = Exchange.DeclareDirect(RabbitBus.RpcExchange);
			var wiretapCalculateRequestQueue = Queue.DeclareDurable(routingKey+":DataWarehouseService:Wiretap");
			wiretapCalculateRequestQueue.BindTo(wiretapCalculateRequestExchange, routingKey);
			Bus.Advanced.Subscribe<CalculateRequest>(wiretapCalculateRequestQueue, (msg, info) =>
				Task.Factory.StartNew(() => ConsoleListener.Consume(msg.Body)));
		}

		public void Start()
		{
			ConnectionStringSettings config = ConfigurationManager.ConnectionStrings["AMQPBroker"];
			Bus = RabbitHutch.CreateBus(config.ConnectionString);
			RegisterEndpoints();
		}

		public void Stop()
		{
			Bus.Dispose();
		}

		public void Paused()
		{
		}

		public void Continue()
		{
		}
	}
}
