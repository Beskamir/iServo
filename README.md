# iServo (Roomba Waiters)

The iServo project done for the CPSC 584 course at the University of Calgary taught by Dr. Ehud Sharlin
Based heavily on the work of https://github.com/simondlevy/BreezyCreate2.git

The Robot used is the iRobot Create 2 programmable Roomba.

# What has been added on top of the BreezyCreate2 API
  - Made two methods, moveFWD, and rotate that allows one to move the roomba a certain distance fwd in mm, and also to rotate the roomba by a specified angle in degrees  
  - Allowed for grid logic (able to move the roomba across an arbritrary grid). So one can specify that the roomba move to position 2,3. The roomba must be initialized to an x and y position as well as given a heading (a value from 0-359 that specifies what direction the front of the roomba is pointing on a cartesian grid). 0 degrees is positive x, 90 degrees is positive y.

The above are generic changes that can be applied to a mutlitude of projects, there are more changes to the API that are iServo specific.
