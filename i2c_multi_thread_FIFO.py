import time
import board
import busio
import adafruit_ads1x15.ads1015 as ADS
import queue 
import time
import threading
import urllib

from urllib.parse import urlparse
from adafruit_ads1x15.analog_in import AnalogIn
#from pyModbusTCP.client import ModbusClient

#Artifact of MODBUS attempt
#SERVER_IP = "local host"
#SERVER_PORT = 502 #if this doesn't work use port 80 for standard tcp.
#SERVER_U_ID = 1 #standard server modbus 

#mod_client = ModbusClient()

#Tell the client where the host is.
#mod_client.host(SERVER_IP)
#mod_client.port(SERVER_PORT)
#mod_client.ID(SERVER_U_ID)
#Modbus client doesn't not handle FIFO protocol.

#How many threads do we need 
num_thread_type = 1
#enclosure_queue = Queue()

q = queue.Queue()
#Set up i2c main bus for i2c protocal
i2c = busio.I2C(board.SCL, board.SDA)

#Create the ADC object using the i2c object created above.
ads = ADS.ADS1015(i2c)

#Setup single ended input. Can do double ended input by adding another channel to the arugment
chan = AnalogIn(ads, ADS.P0)

print("{:>5}\t{:>5}".format('raw', 'v'))

def unload_FIFO_1():
    while(True):
        data = q.get()
        print("you are now in worker function:")
        print(data)

 #       print(data)
 #       print(" ")
        time.sleep(1)  
    
#Function should just save into FIFO. Need to figure out how to initiate this thread on demand from master. 
#TODO: Should each signal have their own thread? 
def live_data_1():
    while True:
    #this is where instead of printing the information, we need to send it to a location of our choice on a FIFO. 
        print("Live Data Function" + "{:>5}\t{:>5.3f}".format(chan.value, chan.voltage))
        q.put(chan.voltage)
        time.sleep(1)

for i in range(num_thread_type):#If multiple are desired, set up to itterate through utilizing global veriable. 
    worker = threading.Thread(
        target = unload_FIFO,
        args = (),
        name = 'worker-{}'.format(i),
        )
    worker.setDaemon(True)
    worker.start()

for i in range(num_thread_type):
    worker = threading.Thread(
        target = live_data,
        args = (),
        name = 'worker-{}'.format(i),
        )
    worker.setDaemon(True)
    worker.start()


