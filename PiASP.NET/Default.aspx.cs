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
			//Make the RPC request
			CalculateRequest request = new CalculateRequest() { Terms = Convert.ToInt32(MessageText.Text) };
			CalculateResponse response = EasyNetQBus.Current().SynchronousResponse<CalculateRequest, CalculateResponse>(request);
			string messageText = string.Format("Pi is not: {0}", response.Pi.ToString());

			//Publish a broadcast message
			BroadcastMessageRequest broadcast = new BroadcastMessageRequest() { MessageText = messageText };
			EasyNetQBus.Current().Publish<BroadcastMessageRequest>(broadcast);

			//Get the list of broadcast messages (RPC again)
			GetMessagesRequest messagesRequest = new GetMessagesRequest();
			GetMessagesResponse messagesResponse = EasyNetQBus.Current().SynchronousResponse<GetMessagesRequest, GetMessagesResponse>(messagesRequest);

			//Print out the last result. Bear in mind we're async - this may not be your last request!
			MessageList.DataSource = messagesResponse.Messages;
			MessageList.DataBind();
		}
	}
}