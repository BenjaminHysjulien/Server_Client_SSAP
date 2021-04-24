#!/usr/bin/env python3
import time
import board
import busio   # also used for the max31855 temperature sensor
import adafruit_ads1x15.ads1015 as ADS
import queue
import threading
import urllib
from gpiozero import OutputDevice
from time import sleep
from serial import Serial
import csv
from adafruit_ads1x15.analog_in import AnalogIn
import RPi.GPIO as GPIO
import digitalio  # for temperature circuit
import adafruit_max31855  # for the temperature sensor circuit 

record_time = 3

RS485_direction = OutputDevice(17)

# enable reception mode. This bit controls the RS485 direction. "off" puts it in recieve mode. 
RS485_direction.off() #Start with read onlly

flag = True

#declare FIFO for buffer. 
q0 = queue.Queue()
q1 = queue.Queue()
q2 = queue.Queue()
q3 = queue.Queue()
qt = queue.Queue()

#Set up i2c ports 
i2c = busio.I2C(board.SCL, board.SDA) 

#Create teh ADC object and point it towards the correct ports 
#adc0 = ADS.ADS1015(i2c)
adc1 = ADS.ADS1015(i2c, address=0x49) #49 ADDR tied to VCC
#0x48 ADDR tied to ground. 

#Setup chan x4 for each of the analog reads on the ADDR 0x49
chan0_0x49 = AnalogIn(adc1, ADS.P0);  
chan1_0x49 = AnalogIn(adc1, ADS.P1);
chan2_0x49 = AnalogIn(adc1, ADS.P2); 
chan3_0x49 = AnalogIn(adc1, ADS.P3);
#G

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

# Setup for the max31855 temperature sensing circuit 
spi = busio.SPI(board.SCK, MOSI=board.MOSI, MISO=board.MISO)
cs = digitalio.DigitalInOut(board.D5)
max31855 = adafruit_max31855.MAX31855(spi, cs)

temp_offset = -2.7  # linearly adjust temperature offset (degree C) for circuit temperature conversion
temp_flag = False   # when temperature reading command request recieved, set flag to true


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


while(True):
    with Serial('/dev/ttyS0', 9600) as s:
        # waits for a single character
            rx = s.read(1)
            if (rx == b'A'): #Then a singal to start recording has been sent.
             #   print("RX: {0}".format(rx))
                print("Recording Data: Start")
                t_stop = time.time()+record_time # How much time is being recorded
                while time.time() < t_stop:
                    q1.put("{:>5.3f}".format(chan1_0x49.voltage))                   
                    
                    q2.put("{:>5.3f}".format(chan2_0x49.voltage))

                    q3.put("{:>5.3f}".format(chan3_0x49.voltage))
                    
                print("Recording Data: Stop")
                s.flush();
                
            if (rx == b'S'):  # servo anchor activated
                print("Activating servo anchors.")
                servoAng(servo1, 70)  # estimate angle to set anchor - will need physical testing
                servoAng(servo2, 110)
                # angles different becasue the endcaps are mirrored from each other, not exactly the same
                
            elif (rx == b'R'):  # servo anchor deactivated
                print("Deactivating servo anchors.")
                servoAng(servo1, 180)  # angle for arm to be retracted inward
                servoAng(servo2, 0)
                
            elif (rx == b'T'):  # servo anchor deactivated
                print("Getting temperature reading.")
                for j in range(10):
                    tempC = max31855.temperature
                    tempC = tempC + temp_offset
                    tempF = tempC * 9 / 5 + 32
                    qt.put("{:>5.3f}".format(tempC))
                    sleep(0.02)
                    temp_flag = True
                
                
    with Serial('/dev/ttyS0', 9600) as s2:
        while not q1.empty():
            if(flag == False):              
                #Unload buffer
                
                RS485_direction.on()
                delim = ','
                
                value = q1.get()
                s2.write(value.encode())
                s2.write(delim.encode())
                sleep(0.001)
                value1 = q2.get()
                s2.write(value1.encode())
                s2.write(delim.encode())
                sleep(0.001)
                value2 = q3.get()
                s2.write(value2.encode())
                s2.write(delim.encode())
                sleep(0.001)                
                if(q1.empty()):
                    print("Serial Data Transmission from B is complete")
                    RS485_direction.off()
                    flag = True
            else:
                rx = s2.read(1)
                if rx == b'B':
                    flag = False
                    print("RX: {0}".format(rx))
                s2.flush()

        s2.flush()
        if(temp_flag):
            # check if recieved "ready for tamperature data" signal
            rx = s2.read(1)
            if rx == b'B':
                print("RX: {0}".format(rx))
                for j in range(10):
                    RS485_direction.on()
                    delim = ','
    
                            # prepare data to transmit 
                    value = qt.get()
                    s2.write(value.encode())
                    s2.write(delim.encode())
                    sleep(0.02)
                    temp_flag = False
            RS485_direction.off()
            s2.flush()
            
            

servoOff(servo1)
servoOff(servo2)
GPIO.cleanup()