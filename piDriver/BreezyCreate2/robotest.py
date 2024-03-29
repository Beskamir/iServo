#!/usr/bin/env python3

'''
robotest.py - Test the features of BreezyCreate2

This code is part of BreezyCreate2

The MIT License

Copyright (c) 2016 Simon D. Levy

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
'''

from breezycreate2 import Robot
import time

# Create a Create2. This will automatically try to connect to your robot over serial
bot = Robot(250, 100, "COM3")


# bot.initGrid(1,0,90)
# bot.gridMoveTo(0,0)
# bot.gridMoveTo(-1,0)
# bot.gridMoveTo(2,2)
# bot.gridMoveTo(1,0)
# bot.gridMoveTo(0,-2)
# bot.gridMoveTo(0,0)
# bot.gridMoveTo(2.5,1.25)



# Report bumper hits and wall proximity for 30 seconds
# start_time = time.time()
# while (time.time() - start_time) < 10:
#     print('Bumpers: ' + str(bot.getBumpers()) + '    Wall: ' + str(bot.getWallSensor()))

# Close the connection
bot.close()
