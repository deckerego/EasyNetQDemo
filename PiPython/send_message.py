import pika
import ast
import uuid

class RequestReply():
	def __init__(self, exchange):
		connect_params = pika.ConnectionParameters('localhost')
		self.connection = pika.BlockingConnection(connect_params)
		self.channel = self.connection.channel()
		self.channel.exchange_declare(exchange=exchange, durable=True, type='fanout')
		self.exchange = exchange

	def __create_callback(self):
		result = self.channel.queue_declare(durable=False, exclusive=False, auto_delete=True)
		self.callback_queue = result.method.queue
		self.channel.queue_bind(queue=self.callback_queue, exchange=self.exchange)
		self.channel.basic_consume(self.__on_response, no_ack=False, queue=self.callback_queue)

	def __destroy_callback(self):
		self.channel.queue_unbind(queue=self.callback_queue, exchange=self.exchange)
		self.channel.queue_delete(queue=self.callback_queue)

	def request(self, exchange, message):
		self.response = None
		self.message = message
		pub_props = pika.BasicProperties(reply_to=self.callback_queue)
		self.channel.basic_publish(exchange=exchange, routing_key='', properties=pub_props, body=str(self.message))
		print "sent message %s\n" % self.message

	def __on_response(self, ch, method, props, body):
		print "received response %s\n" % (body)
		response = ast.literal_eval(body);

		if (self.message['requestId'] == response['requestId']):
			self.channel.basic_ack(delivery_tag=method.delivery_tag)
			self.response = response
		else:
			self.channel.basic_reject(delivery_tag=method.delivery_tag, requeue=False)

	def get_reply(self, exchange, message):
		self.__create_callback()
		self.request(exchange, message)
		while self.response is None:
			print "waiting for response...\n"
			self.connection.process_data_events()
		self.__destroy_callback()
		return self.response