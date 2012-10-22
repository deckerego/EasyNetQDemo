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
			Wiretap<CalculateRequest>(PiListener.Consume);
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

		public void Wiretap<T>(Action<T> endpoint)
		{
			string routingKey = EasyNetQ.TypeNameSerializer.Serialize(typeof(T));
			var wiretapCalculateRequestExchange = Exchange.DeclareDirect(RabbitBus.RpcExchange);
			var wiretapCalculateRequestQueue = Queue.DeclareDurable(routingKey + ":DataWarehouseService:Wiretap");
			wiretapCalculateRequestQueue.BindTo(wiretapCalculateRequestExchange, routingKey);
			Bus.Advanced.Subscribe<T>(wiretapCalculateRequestQueue, (msg, info) =>
				Task.Factory.StartNew(() => endpoint(msg.Body)));
		}
	}
}
