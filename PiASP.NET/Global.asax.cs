using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using EasyNetQ;

namespace PiASP
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			IBus bus = RabbitHutch.CreateBus("host=10.211.55.2;port=5672;virtualHost=/;username=guest;password=guest");
			Application["MessageBus"] = bus;
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{
			IBus bus = (IBus)Application["MessageBus"];
			bus.Dispose();
		}
	}
}