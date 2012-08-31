import pika
import ast
import uuid

class RequestReply():
	def __init__(self, message):
		self.message = message

		connect_params = pika.ConnectionParameters('localhost')
		self.connection = pika.BlockingConnection(connect_params)
		self.channel = self.connection.channel()

		self.channel.exchange_declare(exchange="PiPython", durable=True, type='fanout')
		result = self.channel.queue_declare(durable=False, exclusive=False, auto_delete=True)
		self.callback_queue = result.method.queue
		self.channel.queue_bind(queue=self.callback_queue, exchange="PiPython")
		self.channel.basic_consume(self.on_response, no_ack=True, queue=self.callback_queue)

	def request(self):
		self.response = None
		pub_props = pika.BasicProperties(reply_to=self.callback_queue)
		self.channel.basic_publish(exchange='Pi', routing_key='', properties=pub_props, body=str(self.message))
		print "sent message %s\n" % self.message
		self.connection.process_data_events()

	def on_response(self, ch, method, props, body):
		print "received response %s\n" % (body)
		self.response = body

	def get_reply(self):
		self.request()
		while self.response is None:
			print "waiting for response...\n"
			self.connection.process_data_events()
		return ast.literal_eval(self.response)