import board
import busio
import digitalio
import adafruit_max31855

import RPi.GPIO as GPIO
import time

##### Servo Setup #####
#GPIO.setmode(GPIO.BOARD)
GPIO.setmode(GPIO.BCM)
servo1_pin = 26  # does not need to be PWM pin, the GPIO generates PWM signal
servo2_pin = 20

# Set servo pin as output for servo
GPIO.setup(servo1_pin, GPIO.OUT)
GPIO.setup(servo2_pin, GPIO.OUT)
servo1 = GPIO.PWM(servo1_pin, 50)  # 50 = 50Hz pulse
servo2 = GPIO.PWM(servo2_pin, 50)

#start PWM run, but with value of 0 (pulse off)
servo1.start(0)  # servo1 is male endcap
servo2.start(0)  # servo2 is female endcap

# define variable duty
duty = 2  # 2 equals 0 degrees, 12 equals 180, linear inbetween

# Temperature sensor circuit max31855
spi = busio.SPI(board.SCK, MOSI=board.MOSI, MISO=board.MISO)
cs = digitalio.DigitalInOut(board.D5)
max31855 = adafruit_max31855.MAX31855(spi, cs)

temp_offset = -2.7


def servoAng(servo, angDeg):
    '''
    Set angle of servo. Angles from 0 to 180 degrees.
    '''
    print("Move to angle " + str(angDeg))
    angDeg = int(angDeg)
    duty = float((angDeg / 180) * 10 + 2)  # range of 2 to 12 for 0 to 180 degrees (0 is servo off)
    servo.ChangeDutyCycle(duty)
    time.sleep(1.5)  # allow ample time to move servo
    servo.ChangeDutyCycle(0)  # stops sending the pulse signals, preventing twitching 
    

def servoOff(servo):
    '''
    Disables / turns-off servo. Stops sending the pulse signals.
    '''
    print("Turning off servo...")
    servo.ChangeDutyCycle(0)
    servo.stop()
    #GPIO.cleanup()


while True:
    userInput = input("Enter command: \n")
    if userInput == "exit":
        print("Exiting...")
        break
    elif userInput == "0":
        servoAng(servo2, 0)
        servoAng(servo1, 0)
    elif userInput == "90":
        servoAng(servo2, 90)
        servoAng(servo1, 90)
    elif userInput == "180":
        servoAng(servo2, 180)
        servoAng(servo1, 180)
    else:
        print("Command unknown.")
    print("Command entered: " + userInput)
    
    #time.sleep(3.0)
    tempC = max31855.temperature
    tempC = tempC + temp_offset
    tempF = tempC * 9 / 5 + 32
    print("Temperature: {} C  {} F ".format(tempC, tempF))
    
    
servoOff(servo1)
servoOff(servo2)
GPIO.cleanup()
