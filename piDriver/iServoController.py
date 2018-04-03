import socket
import sys

# Tried doing iServoController.py 1 N,1
# Expected, roomba makes a sound and rotates back and forth
# actual, roomba makes 1 beep and 90 degree turn, does this 3 times
#	also the controller termainl no respond to ctrl+c or +d or +x
if (len(sys.argv) != 3):
    print('error is usage: use as \'iServoController.py [piNumber] [commandString]\'')

else:
    command = sys.argv[2]
    piNum = sys.argv[1]

    # create a socket object
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    # get local machine name
    # host = socket.gethostname() #was doing for testing chnage back if you want
    host = "roomba-pi-" + piNum + ".local"
    port = 2225

    # connection to hostname on the port.
    s.connect((host, port))
    s.send(command.encode('ascii'))

    # Receive no more than 1024 bytes
    msg = s.recv(1024)

    s.close()
    print(msg.decode('ascii'))
