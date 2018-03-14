import threading
import time
import socket
#from breezycreate2 import Robot

# Define a function for the thread
# clSocket is the client socket
# command is a string encoding a command

class myThread (threading.Thread):
	def __init__(self, sock, com):
		threading.Thread.__init__(self)
		self.socket = sock
		self.command = com
	def run(self):
		print ("Starting " + self.command)
		command(self.socket, self.command)
		print ("Exiting " + self.command)



def command(clSocket, command):
	msg = 'Recieved '+ command + "\r\n"
	clSocket.send(msg.encode('ascii'))
	clSocket.close()

serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM) 
serversocket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)

# get local machine name
host = socket.gethostname()                           
port = 2225                                           

# bind to the port
serversocket.bind((host, port))                                  

# queue up to 1 request
serversocket.listen(1)                                           

run = True

while run:
   # establish a connection
	clientsocket,addr = serversocket.accept(); 
	msgPacket = clientsocket.recv(1024);
	message = msgPacket.decode('ascii');

	if(message == 'q'):
		run = False;
		err = 'Closing server: ' + str(host) + "\r\n"
		clientsocket.send(err.encode('ascii'))
		clientsocket.close();
		
	else:
		try:
			thread1 = myThread(clientsocket, message)
			thread1.start()

		except:
			err = 'Could not start request from: ' + str(addr) +"\r\n"
			clientsocket.send(err.encode('ascii'))
			clientsocket.close();

serversocket.close()
