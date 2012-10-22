using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pi.Library.Message;

namespace DataWarehouse.Service.Endpoint
{
	public class BroadcastMessageListener
	{
		public static void Consume(BroadcastMessageRequest inbound)
		{
			Console.WriteLine(string.Format("Broadcast message: {0}", inbound.MessageText));
		}
	}
}
