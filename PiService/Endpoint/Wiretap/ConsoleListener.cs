using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MassTransit;

using Pi.Library.Message;

namespace Pi.Service.Endpoint
{
	class ConsoleListener : Consumes<CalculateRequest>.All
	{
		public void Consume(CalculateRequest inbound)
		{
			Console.WriteLine(string.Format("Just saw request {0} for pi to {1} decimals", inbound.CorrelationId, inbound.Terms));
		}
	}
}
