from bottle import Bottle, route, run, debug
from send_message import EasyNetQBus
import uuid

app = Bottle()

@app.route('/pi/<terms:int>')
def pi(terms):
	request = createMessage(terms)
	bus = EasyNetQBus('127.0.0.1')
	response = bus.get_reply(request, 'Pi_Library_Message_CalculateRequest:Pi_Library')
	return "Pi is sorta kinda (in a way): %s" % response['Pi']

def createMessage(terms):
	fields = { 
		'terms': terms
	}
	return fields

if (__name__ == '__main__'):
    debug(True)
    run(app, host = '127.0.0.1', port = '1080', reloader = True)