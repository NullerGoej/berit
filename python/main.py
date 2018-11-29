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

print("Started")
sense.show_message("Welcome to Berit", text_colour=(255,255,255), back_colour=(0,0,0), scroll_speed=0.08)

while True:

  


  for event in sense.stick.get_events():
      if (event.action == "pressed"):

          lookup = weather.lookup_by_latlng(lat,lon)
          condition = lookup.condition
          temp = condition.temp

          msg = "Current temp in "
          msg += city
          msg += ": "
          msg += str(temp)  
          msg += " C"

          sense.show_message(msg, text_colour=(255,255,255), back_colour=(0,0,0), scroll_speed=0.05)
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