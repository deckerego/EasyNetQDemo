EasyNetQDemo
============

A (hopefully) working demonstration that can show how EasyNetQ services can allow ASP.NET and Python applications to drink from the same firehose.
 
Running the .NET Bits
----------------------

The .NET portion was written in VisualStudio 2010 and should open cleanly as a VisualStudio solution with four projects:

* *PiLibrary* - POCOs that provide our data model as well as request/response message pairs
* *PiService* - The business logic that calculates values for Pi. Very poorly. It will attach to RabbitMQ and respond to inbound requests.
* *PiTest* - Because we're all loyal test-driven developers, this contains our unit tests for the endpoint
* *PiASP.NET* - An ASP.NET web application that will allow a user to request a poorly calculated approximation of pi

The solution should build in both VisualStudio as well as Mono, however unit tests will only execute within VisualStudio.

To run the application, launch the PiService executable either as a stand-alone application or using the interactive debugger within VisualStudio. PiASP.NET can be deployed to IIS or run within VisualStudio's application server - just set the properties on the project likewise.

Running the Python Bits
-----------------------

The folder PiPython contains all the code required, however the Bottle and Pika libraries should be installed within your Python environment. Once they are, executing:

    python ./server.py

Should launch a Bottle instance and permit you to test things out. Point your browser to the URL echo'd out to the console when you launch the application, such as http://localhost:1080/pi/11 - and a message will be sent to your running PiService.
