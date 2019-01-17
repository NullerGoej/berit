from sense_hat import SenseHat
from random import randint
from socket import *
from requests import *
import time
import os
import requests
import json

# importing the weather functions
# Weather api from yahoo
# https://pypi.org/project/weather-api/
# https://developer.yahoo.com/weather/
# if giving error: sudo pip3 install weather-api
from weather import Weather, Unit

# Setting a variable to use the SenseHat
sense = SenseHat()

# Timer variables to count
timerSec = 0
timerMin = 0

# Setting the weather unit
weather = Weather(unit=Unit.CELSIUS)

# Hvad gør denne?
s = socket(AF_INET, SOCK_DGRAM)

# WebApp api uri
weburi = 'https://beritapp2.azurewebsites.net/api/PiData'

# Getting our public ip for the weather by location
ip = get('https://api.ipify.org').text
publicIP = format(ip)

# Sending an api call with Vagner's api access key
send_url = 'http://api.ipstack.com/'
# Using our public ip to get our current location
send_url += publicIP
# Finalizing the url for the location api
send_url += '?access_key=61dc2795b08ce4aeb90d1dab18f1c582'

# Requesting the current location
r = requests.get(send_url)
j = json.loads(r.text)
# Getting the lat, lon and city from api
lat = j['latitude']
lon = j['longitude']
city = j['city']

city_msg = "Loaded city: "
city_msg += city

print(city_msg)

# Showing the user that Berit has started
print("Started")
sense.show_message("Welcome to Berit", text_colour=(255,255,255), back_colour=(0,0,0), scroll_speed=0.04)

# Hvad gør denne?
s.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)
while True:
  # Look up weather by lat and lon
  
  #lookup = weather.lookup_by_latlng(lat,lon)
  #condition = lookup.condition
  #temp = condition.temp
  #forecast = condition.text
  
  temp = 100
  forecast = "Sne"

  data = "Temperature: " + str(temp)
  
  timerSec += 1
 
  if (timerSec == 60):
      timerSec = 0
      timerMin += 1
     
  
  if (timerMin == 1):
      timerMin = 0
      # Send temp data to database every 10 minutes
      # Use the WebApp and a POST Request to send the data
      query = {'temperatur': temp}
      r = requests.post(weburi, verify=False, json=query)
      if (r.status_code == 200):
          print("Uploaded temperature to database with success!")
      else:
          print(r.status_code, r.reason)
          print(r.text[:300] + '...')
  
  # Hvad gør denne?
  s.sendto(bytes(data,"UTF-8"), ('<broadcast>', 6969))


  for event in sense.stick.get_events():
      if (event.action == "pressed"):
          

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

  time.sleep(1)
