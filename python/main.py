# kunne se vejret på mit device ved at trykke på en knap







while True:
  
  temp1 = sense.get_temperature_from_humidity()
  temp2 = sense.get_temperature_from_pressure()
  temp_cpu = get_cpu_temperature()

  humidity = sense.get_humidity()
  pressure = sense.get_pressure()

  temp = (temp1+temp2)/2
  temp_corr = temp - ((temp_cpu-temp)/1.5)
  temp_corr = get_smooth(temp_corr)

  msg = temp_corr , " C"


  for event in sense.stick.get_events():
      if (event.action == "pressed"):
          sense.show_message(msg, text_colour=(255,255,255), back_colour=(0,0,0), scroll_speed=0.08)

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