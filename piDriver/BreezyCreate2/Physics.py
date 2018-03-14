from breezycreate2 import Robot
import time
import math

FWD_VEL = 250.0 #250 mm/s

TAN_VEL = 100.0 #50 mm/s
ROOMBA_RAD = 103.0  #103 mm

#amount of time needed for a full turn
FULL_TIME_TURN = (ROOMBA_RAD * 2 * math.pi) / TAN_VEL

#roomba is the robot
#dis is the distance to move in mm. a negative dis means backward
def moveFWD(roomba, dis):
	deltaT = 0.0;
	if(dis < 0):
		deltaT = -1 * (dis / FWD_VEL);
		roomba.setForwardSpeed(-1 * FWD_VEL);
	else:
		deltaT = dis / FWD_VEL;
		roomba.setForwardSpeed(FWD_VEL);
		
	time.sleep(deltaT);
	roomba.setForwardSpeed(0);

#roomba is the robot
#theta is the angle in degrees to turn by, -angle means left turn
def rotate(roomba, theta):
	deltaT = 0.0; 
	if(theta < 0):
		fracTheta = -1*(theta / 360);
		deltaT = FULL_TIME_TURN * fracTheta;
		roomba.setTurnSpeed(-1 * TAN_VEL);
	else:
		fracTheta = theta / 360;
		deltaT = FULL_TIME_TURN * fracTheta;
		roomba.setTurnSpeed(TAN_VEL);	
	time.sleep(deltaT);
	roomba.setTurnSpeed(0);
