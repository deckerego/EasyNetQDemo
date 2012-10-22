using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pi.Library.Message;

namespace Pi.Service.Endpoint
{
	public class BroadcastMessageConsumer
	{
		List<string> SavedMessages;

		public BroadcastMessageConsumer()
		{
			SavedMessages = new List<string>();
		}

		public void Consume(BroadcastMessageRequest request)
		{
			SavedMessages.Add(request.MessageText);
		}

		public GetMessagesResponse Consume(GetMessagesRequest request)
		{
			return new GetMessagesResponse()
			{
				Messages = SavedMessages
			};
		}
	}
}
