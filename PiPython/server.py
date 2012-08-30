from bottle import Bottle, route, run, debug
from send_message import SendMessage

app = Bottle()

@app.route('/pi/<terms:int>')
def pi(terms):
	request = createMessage(terms)
	sendThread = SendMessage(request)
	sendThread.start()
	response = sendThread.get_response()
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

if (__name__ == '__main__'):
    debug(True)
    run(app, host = '127.0.0.1', port = '1080', reloader = True)