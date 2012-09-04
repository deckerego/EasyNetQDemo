import pika
import ast
import uuid

class RequestReply():
	def __init__(self):
		connect_params = pika.ConnectionParameters('localhost')
		self.connection = pika.BlockingConnection(connect_params)
		self.channel = self.connection.channel()
		self.callback_queue = None

	def __create_callback(self):
		result = self.channel.queue_declare(durable=False, exclusive=False, auto_delete=True)
		self.callback_queue = result.method.queue
		self.channel.basic_consume(self.__on_response, no_ack=True, queue=self.callback_queue)

	def request(self, message, msg_type):
		self.response = None
		self.message = message
		pub_props = pika.BasicProperties(reply_to=self.callback_queue, correlation_id=self.correlation_id, type=msg_type)
		self.channel.basic_publish(exchange='', routing_key=msg_type, properties=pub_props, body=str(self.message))
		print "sent message %s\n" % self.message

	def __on_response(self, ch, method, props, body):
		print "received response %s\n" % (body)
		self.response = ast.literal_eval(body);

	def get_reply(self, message, msg_type):
		self.__create_callback()
		self.correlation_id = str(uuid.uuid4())
		self.request(message, msg_type)

		while self.response is None:
			print "waiting for response...\n"
			self.connection.process_data_events()

		return self.response