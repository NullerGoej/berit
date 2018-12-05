from sense_hat import SenseHat
from random import randint
import time
import os

sense = SenseHat()

import requests
import json

# Sending an api call with Vagner's api access key
send_url = 'http://api.ipstack.com/87.59.144.208?access_key=61dc2795b08ce4aeb90d1dab18f1c582'
r = requests.get(send_url)
j = json.loads(r.text)
# Getting the lat, lon and city from api
lat = j['latitude']
lon = j['longitude']
city = j['city']

# importing the weather functions
# Weather api from yahoo
# https://developer.yahoo.com/weather/
from weather import Weather, Unit

# Setting the weather unit
weather = Weather(unit=Unit.CELSIUS)

# Showing the user that Berit has started
print("Started")
sense.show_message("Welcome to Berit", text_colour=(255,255,255), back_colour=(0,0,0), scroll_speed=0.04)

while True:

  


  for event in sense.stick.get_events():
      if (event.action == "pressed"):

          # Look up weather by lat and lon
          lookup = weather.lookup_by_latlng(lat,lon)
          condition = lookup.condition
          temp = condition.temp
          forecast = condition.text

          msg = "Temperature"
          msg += ": "
          msg += str(temp)  
          msg += " C"
          msg += " - Forecast"
          msg += ": "
          msg += str(forecast)

          print(msg)
          sense.show_message(msg, text_colour=(255,255,255), back_colour=(0,0,0), scroll_speed=0.05)

        # For later use
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