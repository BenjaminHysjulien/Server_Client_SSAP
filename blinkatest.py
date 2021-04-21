import board
import digitalio
import busio

print("Hello blinka!")

# try to creat a digital input
pin = digitalio.DigitalInOut(board.D4)
print("Digital IO ok!")

# Try create an I2C device
ic2 = busio.I2C(board.SCL, board.SDA)
print("I2C ok!")

# try to create an SPI device
spi = busio.SPI(board.SCLK, board.MOSI, board.MISO)
print("SPI ok!")

print("Done!")