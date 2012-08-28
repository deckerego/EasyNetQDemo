using System;
using System.Text;
using System.Collections.Generic;

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassTransit;
using MassTransit.TestFramework;
using MassTransit.TestFramework.Fixtures;
using MassTransit.Transports.Loopback;
using Pi.Library.Message;

namespace Pi.Service.Endpoint
{
	[TestClass]
	public class CalculateConsumerTest : LocalTestFixture<LoopbackTransportFactory>
	{
		[TestInitialize]
		public void TestFixtureSetUp()
		{
			LocalUri = new Uri("loopback://localhost/UnitTest");
			LocalTestFixtureSetup();
		}

		[TestCleanup]
		public void TestFixtureTearDown()
		{
			LocalTestFixtureTeardown();
		}

		[TestMethod]
		public void should_provide_primes()
		{
			Assert.AreEqual(1, CalculateConsumer.NextMersennePrime(1));
			Assert.AreEqual(3, CalculateConsumer.NextMersennePrime(2));
			Assert.AreEqual(7, CalculateConsumer.NextMersennePrime(3));
		}

		[TestMethod]
		public void should_generate_a_poor_approximation()
		{
			Assert.AreEqual(3.05799912139378, CalculateConsumer.CalculateGregoryApproximation(1000), 0.0001);
		}

		[TestMethod]
		public void should_generate_response()
		{
			LocalBus.SubscribeInstance(new CalculateConsumer());

			CalculateRequest request = new CalculateRequest() { Terms = 10 };


			LocalBus.PublishRequest(request, response =>
			{
				response.Handle<CalculateResponse>(message =>
				{
					Assert.AreEqual(3.05865035384706, message.Pi, 0.000001);
				});
				response.SetTimeout(TimeSpan.FromSeconds(10));
			});
		}
	}
}
