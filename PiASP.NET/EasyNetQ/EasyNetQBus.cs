using System;
using System.Configuration;
using System.Threading;
using System.Web.UI.WebControls;

using EasyNetQ;

namespace PiASP.EasyNetQ
{
	public class EasyNetQBus
	{
		protected static EasyNetQBus CurrentInstance = null;
		public virtual IBus MessageBus { get; set; }

		protected EasyNetQBus()
		{
		}

		public static Func<EasyNetQBus> Current = () =>
		{
			if (CurrentInstance == null)
			{
				ConnectionStringSettings brokerConnectionString = ConfigurationManager.ConnectionStrings["AMQPBroker"];
				CurrentInstance = new EasyNetQBus()
				{
					MessageBus = RabbitHutch.CreateBus(brokerConnectionString.ConnectionString)
				};
			}

			return CurrentInstance;
		};

		public U SynchronousResponse<T, U>(T request)
		{
			U response = default(U);
			AutoResetEvent responseEvent = new AutoResetEvent(false);

			using (var publishChannel = MessageBus.OpenPublishChannel())
			{
				publishChannel.Request<T, U>(request, callbackMsg =>
				{
					response = callbackMsg;
					responseEvent.Set();
				});
			}

			responseEvent.WaitOne();
			return response;
		}
	}
}