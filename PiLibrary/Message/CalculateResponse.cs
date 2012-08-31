using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MassTransit;

namespace Pi.Library.Message
{
	public class CalculateResponse : CorrelatedBy<Guid>
	{
		public Guid CorrelationId { get; set; }
		public double Pi { get; set; }

		public CalculateResponse(CalculateRequest request)
		{
			CorrelationId = request.CorrelationId;
		}
	}
}
