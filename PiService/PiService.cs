using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

using EasyNetQ;
using EasyNetQ.Topology;

using Pi.Library.Message;
using Pi.Service.Endpoint;
using System.Threading.Tasks;

namespace Pi.Service
{
	public class PiService
	{
		private IBus Bus;

		protected void RegisterEndpoints()
		{
			BroadcastMessageConsumer broadcastEndpoint = new BroadcastMessageConsumer();

			Bus.Respond<CalculateRequest, CalculateResponse>(CalculateConsumer.Consume);
			Bus.Respond<GetMessagesRequest, GetMessagesResponse>(broadcastEndpoint.Consume);
			Bus.Subscribe<BroadcastMessageRequest>(GetSubscriptionId(), broadcastEndpoint.Consume);
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

		public string GetSubscriptionId()
		{
			return string.Format("{0}:{1}", this.GetType().FullName, System.Environment.MachineName);
		}
	}
}
