import socket
import threading
from breezycreate2 import Robot

# Define a function for the thread
# clSocket is the client socket
# command is a string encoding a command

#"M,num1,num2, num3" encodes a move command to go to position num1, num2 at num3*self.fwdVel speed (0.1 <= num 3 <= 1.0)
#"N,num" encodes a notify command with  level == num
#"I,num1,num2,num3" encodes a grid init command, to say robot is at position num1, num2 with heading num3
class myThread(threading.Thread):
    def __init__(self, sock, com, iServo):
        threading.Thread.__init__(self)
        self.socket = sock
        self.command = com
        self.bot = iServo

    def run(self):
        if(self.command[0] == 'M'):
            #gridMoveTo
            cmdList = self.command.split(',');
            x = float(cmdList[1]);
            y = float(cmdList[2]);
            spd = float(cmdList[3]);
            curSpd = self.bot.getFWDVel;
            self.bot.setFWDVel(crSpd*spd);
            self.bot.gridMoveTo(x,y);
            self.bot.setFWDVel = curSpd;
        elif(self.command[0] == 'N'):
            #Notify
            cmdList = self.command.split(',');
            lvl = int(cmdList[1]);
            self.bot.notify(lvl);
        elif(self.command[0] == 'I'):
            #Re-initialize
            cmdList = self.command.split(',');
            newX = float(cmdList[1]);
            newY = float(cmdList[2]);
            newH = float(cmdList[3]);
            self.bot.initGrid(newX, newY, newH);
            
        msg = 'Command:' + self.command + 'complete' + '\nBot: X: '
        + self.bot.x + " Y: " + self.bot.y+" [H]: " +self.heading;

        self.socket.send(msg.encode('ascii'));
        self.socket.close();

bot = Robot();
serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# get local machine name
#host = socket.gethostname()
host = "roomba-pi-1.local"
port = 2225

# bind to the port
serversocket.bind((host, port))

# queue up to 1 request
serversocket.listen(1)

run = True

while run:
    # establish a connection
    clientsocket, addr = serversocket.accept();
    msgPacket = clientsocket.recv(1024);
    message = msgPacket.decode('ascii');

    if (message == 'q'):
        run = False;
        err = 'Closing server: ' + str(host) + "\r\n"
        clientsocket.send(err.encode('ascii'))
        clientsocket.close();

    else:
        try:
            thread1 = myThread(clientsocket, message, bot)
            thread1.start()

        except:
            err = 'Could not start request from: ' + str(addr) + "\r\n"
            clientsocket.send(err.encode('ascii'))
            clientsocket.close();

serversocket.close()
