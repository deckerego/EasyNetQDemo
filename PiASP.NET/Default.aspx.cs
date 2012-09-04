using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

using EasyNetQ;

using Pi.Library.Message;

namespace PiASP
{
	public partial class Default : System.Web.UI.Page
	{
		private IBus Bus;

		protected void Page_Init(object sender, EventArgs e)
		{
			Bus = (IBus)Application["MessageBus"];
		}

		protected void SendMessage(object sender, EventArgs e)
		{
			CalculateRequest request = new CalculateRequest() { Terms = Convert.ToInt32(MessageText.Text) };
			AutoResetEvent responseEvent = new AutoResetEvent(false);

			using (var publishChannel = Bus.OpenPublishChannel())
			{
				publishChannel.Request<CalculateRequest, CalculateResponse>(request, response => 
				{
					MessageResponse.Text = response.Pi.ToString();
					responseEvent.Set();
				});
			}

			responseEvent.WaitOne();
		}
	}
}