import threading
import pika
import ast

class SendMessage(threading.Thread):
	def __init__(self, message):
		threading.Thread.__init__(self)

		self.event = threading.Event()
		self.message = message

		connect_params = pika.ConnectionParameters('localhost')
		self.connection = pika.BlockingConnection(connect_params)
		self.channel = self.connection.channel()

		self.channel.exchange_declare(exchange="PiPython", durable=True, type='fanout')
		result = self.channel.queue_declare(queue='pi_reply', durable=False, exclusive=False)
		self.channel.queue_bind(queue="pi_reply", exchange="PiPython")
		self.callback_queue = result.method.queue
		self.channel.basic_consume(self.on_response, no_ack=True, queue=self.callback_queue)

	def run(self):
		self.response = None
		pub_props = pika.BasicProperties(reply_to = self.callback_queue)
		self.channel.basic_publish(exchange='Pi', routing_key='', properties=pub_props, body=self.message)
		print "sent message %s\n" % self.message
		self.connection.process_data_events()

	def on_response(self, ch, method, props, body):
		print "received response %s\n" % body
		self.response = body
		self.event.set()

	def get_response(self):
		self.event.wait()
		return ast.literal_eval(self.response)