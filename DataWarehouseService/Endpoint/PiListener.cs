using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pi.Library.Message;

namespace DataWarehouse.Service.Endpoint
{
	public class ConsoleListener
	{
		public static void Consume(CalculateRequest inbound)
		{
			Console.WriteLine(string.Format("Just saw request for pi to {0} decimals", inbound.Terms));
		}
	}
}
