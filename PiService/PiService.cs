using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EasyNetQ;

namespace Pi.Service
{
	public class PiService
	{
		private readonly IBus Bus;

		public PiService(IBus bus)
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
