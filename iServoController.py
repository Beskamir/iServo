import socket
import sys


if(len(sys.argv) != 2):
	print('error is usage: use as \'iServoController.py [commandString]\'');
	
else:
	command = sys.argv[1];

	# create a socket object
	s = socket.socket(socket.AF_INET, socket.SOCK_STREAM) 
	# get local machine name
	host = socket.gethostname()                           
	port = 2225

	# connection to hostname on the port.
	s.connect((host, port))                               
	s.send(command.encode('ascii'))

	# Receive no more than 1024 bytes
	msg = s.recv(1024)                                     

	s.close()
	print (msg.decode('ascii'))
