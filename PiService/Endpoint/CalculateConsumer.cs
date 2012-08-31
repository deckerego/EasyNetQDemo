using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MassTransit;

using Pi.Library.Message;

namespace Pi.Service.Endpoint
{
	public class CalculateConsumer : Consumes<CalculateRequest>.All
	{
		public void Consume(CalculateRequest inbound)
		{
			CalculateResponse outbound = new CalculateResponse(inbound);
			outbound.Pi = CalculateGregoryApproximation(inbound.Terms);

			this.Context().Respond(outbound);
		}

		public static double CalculateGregoryApproximation(int terms)
		{
			double quarterPi = 0.0;
			for (int i = 1; i <= terms + 1; ++i)
			{
				double prime = NextMersennePrime(i);
				if (i % 2 == 1) quarterPi += 1.0 / prime;
				else quarterPi -= 1.0 / prime;
			}

			return quarterPi * 4.0;
		}

		public static double NextMersennePrime(int i)
		{
			return Math.Pow(2, i) - 1;
		}
	}
}
