from bottle import Bottle, route, run, debug
import pika
import time
import ast

app = Bottle()

@app.route('/pi/<terms:int>')
def pi(terms):
	request = createMessage(terms)
	response = send(request)
	return "Pi is sorta kinda (in a way): %s" % response['message']['pi']

# TODO Externalize responseAddress
def createMessage(terms):
	fields = { 
		'responseAddress': "rabbitmq://10.211.55.2/PiPython",
		'message': {
			'terms': terms
		},
		'messageType': ["urn:message:Pi.Library.Message:CalculateRequest"]
	}
	return str(fields)

# TODO Break out connection logic
def send(message):
	connect_params = pika.ConnectionParameters('localhost')
	connection = pika.BlockingConnection(connect_params)
	channel = connection.channel()

	channel.exchange_declare(exchange="PiPython", durable=True, type='fanout')
	result = channel.queue_declare(queue='pi_reply', durable=False, exclusive=True)
	channel.queue_bind(queue="pi_reply", exchange="PiPython")
	callback_queue = result.method.queue

	pub_props = pika.BasicProperties(reply_to = callback_queue)

	channel.basic_publish(exchange='Pi', routing_key='', properties=pub_props, body=message)

	# TODO This needs fixed up - polling is ugly
	# FIXME I don't use correlation IDs, so who knows what messages I get
	while connection.is_open:
		method_frame, header_frame, response_body = channel.basic_get(queue=callback_queue)
		if method_frame.NAME == 'Basic.GetEmpty':
			connection.process_data_events()
			time.sleep(1)
		else:
			channel.basic_ack(delivery_tag=method_frame.delivery_tag)
			connection.close()

	return ast.literal_eval(response_body)

if (__name__ == '__main__'):
    debug(True)
    run(app, host = '127.0.0.1', port = '1080', reloader = True)