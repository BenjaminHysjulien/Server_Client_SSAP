#!/usr/bin/env python3
import time
import board
import busio
import adafruit_ads1x15.ads1015 as ADS
import queue
import threading
import urllib
from gpiozero import OutputDevice
from time import sleep
from serial import Serial
import csv
from adafruit_ads1x15.analog_in import AnalogIn

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


while(True):
    with Serial('/dev/ttyS0', 115200) as s:
        # waits for a single character
            rx = s.read(1)

            # print the received character
            
            if (rx == b'A'): #Then a singal to start recording has been sent.
                print("RX: {0}".format(rx))
                print("Recording Data: Start")
                t_stop = time.time()+record_time # How much time is being recorded
                while time.time() < t_stop:
                    q0.put("{:>5.3f}".format(chan0_0x49.voltage))
                    #print("{:>5}\t{:>5.3f}".format(chan0_0x49.value, chan3_0x49.voltage))
                    
                    
                    q1.put("{:>5.3f}".format(chan1_0x49.voltage))
                    #print("{:>5}\t{:>5.3f}".format(chan1_0x49.value, chan3_0x49.voltage))
                    
                    
                    q2.put("{:>5.3f}".format(chan2_0x49.voltage))
                    #print("{:>5}\t{:>5.3f}".format(chan2_0x49.value, chan3_0x49.voltage))
                    
                    
                    q3.put("{:>5.3f}".format(chan3_0x49.voltage))
                    #print("{:>5}\t{:>5.3f}".format(chan3_0x49.value, chan3_0x49.voltage))
                print("Recording Data: Stop")
                s.flush();
    with Serial('/dev/ttyS0', 115200) as s2:
        while not q0.empty():
            if(flag == False):              
                #Unload buffer
                
                RS485_direction.on()
                delim = ','
                
                value = q0.get()
                s2.write(value.encode())
                s2.write(delim.encode())
                sleep(0.001)
                value1 = q1.get()
                s2.write(value1.encode())
                s2.write(delim.encode())
                sleep(0.001)
                value2 = q2.get()
                s2.write(value2.encode())
                s2.write(delim.encode())
                sleep(0.001)                
                value3 = q3.get()
                s2.write(value3.encode())
                s2.write(delim.encode())
                sleep(0.001)
                if(q0.empty()):
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
