from sense_hat import SenseHat
from random import randint
import time
import os

sense = SenseHat()

# get CPU temperature
def get_cpu_temperature():
    res = os.popen("vcgencmd measure_temp").readline()
    t = float(res.replace("temp=","").replace("'C\n",""))
    return t

def get_smooth(x):
    if not hasattr(get_smooth, "t"):
        get_smooth.t = [x,x,x]
    get_smooth.t[2] = get_smooth.t[1]
    get_smooth.t[1] = get_smooth.t[0]
    get_smooth.t[0] = x
    xs = (get_smooth.t[0]+get_smooth.t[1]+get_smooth.t[2])/3
    return xs




print("Started")


while True:
  
  temp1 = sense.get_temperature_from_humidity()
  temp2 = sense.get_temperature_from_pressure()
  temp_cpu = get_cpu_temperature()

  humidity = sense.get_humidity()
  pressure = sense.get_pressure()

  temp = (temp1+temp2)/2
  temp_corr = temp - ((temp_cpu-temp)/1.5)
  temp_corr = get_smooth(temp_corr)

  msg = str(round(temp_corr,2))  
  msg += " C"


  for event in sense.stick.get_events():
      if (event.action == "pressed"):
          sense.show_message("Hello", text_colour=(255,255,255), back_colour=(0,0,0), scroll_speed=0.08)

        #if (event.direction == "right"):
        #  px = px + 1
        #  py = py
        #elif (event.direction == "left"):
        #  px = px - 1
        #  py = py
        #elif (event.direction == "up"):
        #  px = px
        #  py = py - 1
        #elif (event.direction == "down"):
        #  px = px
        #  py = py + 1
        #elif (event.direction == "middle"):