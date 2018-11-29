from sense_hat import SenseHat
from random import randint
import time
import os

sense = SenseHat()

import requests
import json

send_url = 'http://api.ipstack.com/87.59.144.208?access_key=61dc2795b08ce4aeb90d1dab18f1c582'
r = requests.get(send_url)
j = json.loads(r.text)
lat = j['latitude']
lon = j['longitude']
city = j['city']

from weather import Weather, Unit

weather = Weather(unit=Unit.CELSIUS)

lookup = weather.lookup_by_latlng(lat,lon)
condition = lookup.condition
temp = condition.temp

#forecasts = lookup.forecast
#for forecast in forecasts:
#    print(forecast.text)
#    print(forecast.date)
#    print(forecast.high)
#    print(forecast.low)



# Get CPU temperature
def get_cpu_temperature():
    res = os.popen("vcgencmd measure_temp").readline()
    t = float(res.replace("temp=","").replace("'C\n",""))
    return t

# Smooth
def get_smooth(x):
    if not hasattr(get_smooth, "t"):
        get_smooth.t = [x,x,x]
    get_smooth.t[2] = get_smooth.t[1]
    get_smooth.t[1] = get_smooth.t[0]
    get_smooth.t[0] = x
    xs = (get_smooth.t[0]+get_smooth.t[1]+get_smooth.t[2])/3
    return xs



print("Started")
sense.show_message("Welcome to Berit", text_colour=(255,255,255), back_colour=(0,0,0), scroll_speed=0.08)


while True:

  ## Temp from pi
  temp1 = sense.get_temperature_from_humidity()
  temp2 = sense.get_temperature_from_pressure()
  temp_cpu = get_cpu_temperature()

  humidity = sense.get_humidity()
  pressure = sense.get_pressure()

  temp = (temp1+temp2)/2
  temp_corr = temp - ((temp_cpu-temp)/1.5)
  temp_corr = get_smooth(temp_corr)
  ## --------------

  msg = "Current temp in "
  msg += city
  msg += ": "
  msg += str(temp)  
  msg += " C"


  for event in sense.stick.get_events():
      if (event.action == "pressed"):
          sense.show_message(msg, text_colour=(255,255,255), back_colour=(0,0,0), scroll_speed=0.08)
          print(msg)

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