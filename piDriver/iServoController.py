import socket
import sys

# code to parse command from c# to something the Pi will like
if (len(sys.argv) != 2):
    print('error is usage: use as \'iServoController.py [commandString]\'')

else:
    parameter = sys.argv[1];
    if( ('Mov' not in parameter) and ('Noti' not in parameter) and ('Res' not in parameter) ):
        print('invalid command');
    else:
        #Now we know command is legit
        #get number of pi
        pmList = parameter.split(':');
        piNum = pmList[0];

        #Now to get the actual command
        command = ' ';
        if('Mov' in parameter):
            cmdList = pmList[1].split(',');
            command = 'M,'+cmdList[1]+','+cmdList[2]+','+cmdList[3];
        elif('Noti' in parameter):
            cmdList = pmList[1].split(',');
            notiNum = int(cmdList[1]);
            notiNum = notiNum - 1;
            command = 'N,'+notiNum;
        elif('Res' in parameter):
            cmdList = pmList[1].split(',');
            command = 'I,'+cmdList[1]+','+cmdList[2]+',0.0';
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

