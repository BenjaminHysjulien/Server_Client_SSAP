import board
import busio
import digitalio
import adafruit_max31855
import time

spi = busio.SPI(board.SCK, MOSI=board.MOSI, MISO=board.MISO)
cs = digitalio.DigitalInOut(board.D5)
max31855 = adafruit_max31855.MAX31855(spi, cs)

temp_offset = -2.7

# print('Temperature: {} degrees C'.format(max31855.temperature))

while True:
    time.sleep(3.0)
    tempC = max31855.temperature
    tempC = tempC + temp_offset
    tempF = tempC * 9 / 5 + 32
    print("Temperature: {} C  {} F ".format(tempC, tempF))