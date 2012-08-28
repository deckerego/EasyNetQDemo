using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MassTransit;
using Castle.Windsor;

using Pi.Library.Message;

namespace PiASP
{
	public partial class Default : System.Web.UI.Page
	{
		private IServiceBus Bus;

		protected void Page_Init(object sender, EventArgs e)
		{
			IWindsorContainer container = (IWindsorContainer)Application["container"];
			Bus = container.Resolve<IServiceBus>();
		}

		protected void SendMessage(object sender, EventArgs e)
		{
			CalculateRequest request = new CalculateRequest() { Terms = Convert.ToInt32(MessageText.Text) };

			Bus.PublishRequest(request, response =>
			{
				response.Handle<CalculateResponse>(message => MessageResponse.Text = string.Format("{0}", message.Pi));
				response.SetTimeout(TimeSpan.FromSeconds(30));
			});
		}
	}
}