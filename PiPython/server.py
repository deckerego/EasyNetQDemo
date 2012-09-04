from bottle import Bottle, route, run, debug
from send_message import RequestReply
import uuid

app = Bottle()

@app.route('/pi/<terms:int>')
def pi(terms):
	request = createMessage(terms)
	request_reply = RequestReply()
	response = request_reply.get_reply(request)
	return "Pi is sorta kinda (in a way): %s" % response['Pi']

# TODO Externalize responseAddress
def createMessage(terms):
	fields = { 
		'terms': terms
	}
	return fields

if (__name__ == '__main__'):
    debug(True)
    run(app, host = '127.0.0.1', port = '1080', reloader = True)