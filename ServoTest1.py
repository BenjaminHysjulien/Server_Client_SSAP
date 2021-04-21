import RPi.GPIO as GPIO
import time

#GPIO.setmode(GPIO.BOARD)
GPIO.setmode(GPIO.BCM)
# Set pin 11 as output, servo1 as pin 11 as PWM
GPIO.setup(13, GPIO.OUT)
servo1 = GPIO.PWM(13, 50)  # 50 = 50Hz pulse

#start PWWM run, but with value of 0 (pulse off)
servo1.start(0)
#GPIO.output(13, True)
print("Waiting 2 seconds...")
time.sleep(2)

# move servo
print("rotating 180 degrees...")

# define variable duty
duty = 2

# Loop for duty values from 2 to 12 (0 to 180 degrees)
while duty <= 12:
    servo1.ChangeDutyCycle(duty)
    time.sleep(1)
    duty = duty + 1
    
# Wait some time
time.sleep(2)

#Turn back to 90
print("Back to 90 degrees...")
servo1.ChangeDutyCycle(7)
time.sleep(2)
# back to 0 degrees
print("Back to 0 degrees...")
servo1.ChangeDutyCycle(2)
time.sleep(0.7)
servo1.ChangeDutyCycle(0)

# Clean things up at the end
#GPIO.output(13, False)
servo1.stop()
GPIO.cleanup()
print("Goodbye")


