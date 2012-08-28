using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MassTransit;

namespace Pi.Service
{
	public class PiService
	{
		private readonly IServiceBus Bus;

		public PiService(IServiceBus bus)
		{
			Bus = bus;
		}

		public void Start()
		{
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
