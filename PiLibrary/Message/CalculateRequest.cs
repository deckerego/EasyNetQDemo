using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MassTransit;

namespace Pi.Library.Message
{
	public class CalculateRequest : CorrelatedBy<Guid>
	{
		public Guid CorrelationId { get; set; }
		public int Terms { get; set; }

		public CalculateRequest()
		{
			CorrelationId = Guid.NewGuid();
		}
	}
}
