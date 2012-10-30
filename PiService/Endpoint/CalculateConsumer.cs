using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pi.Library.Message;

namespace Pi.Service.Endpoint
{
	public class CalculateConsumer
	{
		public static CalculateResponse Consume(CalculateRequest request)
		{
			CalculateResponse response = new CalculateResponse();
			response.Pi = CalculateGregoryApproximation(request.Terms);
			return response;
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
