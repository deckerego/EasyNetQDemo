from datetime import datetime
from datetime import timedelta
import pika
import sys
import json
import uuid

class EasyNetQBus():
	def __init__(self, amqp_server):
		if(amqp_server is None):
			print "No server specified, creating empty connection"
			self.connection = None
			self.channel = None
			self.callback_queue = None
			return

		connect_params = pika.ConnectionParameters(amqp_server)
		self.connection = pika.BlockingConnection(connect_params)
		self.channel = self.connection.channel()
		self.callback_queue = None
		self.timeout = timedelta(seconds=60)

	def __create_callback(self):
		result = self.channel.queue_declare(durable=False, exclusive=True, auto_delete=True)
		self.callback_queue = result.method.queue
		self.channel.basic_consume(self.__on_response, no_ack=True, queue=self.callback_queue)

	def __destroy_callback(self):
		self.channel.queue_delete(queue=self.callback_queue)

	def request(self, message, msg_type, exchange_name='easy_net_q_rpc'):
		self.response = None
		self.message = json.dumps(message)
		self.correlation_id = str(uuid.uuid4())
		pub_props = pika.BasicProperties(reply_to=self.callback_queue, correlation_id=self.correlation_id, type=msg_type)
		self.channel.basic_publish(exchange=exchange_name, routing_key=msg_type, properties=pub_props, body=str(self.message))
		self.connection.process_data_events()
		print "sent message %s\n" % self.message

	def __on_response(self, ch, method, props, body):
		print "received response %s\n" % (body)
		self.response = json.loads(body)

	def get_reply(self, message, msg_type):
		self.__create_callback()
		self.request(message, msg_type)
		timeout = datetime.now() + self.timeout

		while (self.response is None) and (datetime.now() <= timeout):
			self.connection.process_data_events()

		self.__destroy_callback()

		return self.response
