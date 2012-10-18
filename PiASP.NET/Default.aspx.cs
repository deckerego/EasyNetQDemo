using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

using Pi.Library.Message;
using PiASP.EasyNetQ;

namespace PiASP
{
	public partial class Default : System.Web.UI.Page
	{
		protected void SendMessage(object sender, EventArgs e)
		{
			CalculateRequest request = new CalculateRequest() { Terms = Convert.ToInt32(MessageText.Text) };
			CalculateResponse response = EasyNetQBus.Current().SynchronousResponse<CalculateRequest, CalculateResponse>(request);
			MessageResponse.Text = response.Pi.ToString();
		}
	}
}