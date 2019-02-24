# Real C# Temperature Measurement and Database Storage Project
This directory has a working project in C# that accepts readings from a Maxim DS18B20 temperature sensor and stores them into a mongodb collection using size-based bucketing. Here is how the project works:
1. A DS18B20 sensor is connected to a single-board computer. For this project, the author used a Beaglebone Black Wireless device running a Debian release.
2. The date and time, formatted as Unix Epoch seconds, are echoed to a text file containing date and temperature readings. A cron job is used to store these readings in the text file.
3. The temperature, as recorded by the sensor, is read and echoed to the text file mentioned above.
4. The text file is read by our C# program ds18b20_read.cs which saves each date and temperature value to individual arrays. After the file is processed, date and temperature readings are added to a MongoDB collection using the size-based bucketing technique. 
You can take temperature readings at whatever intervals you want. The author decided to record the date/time/temperature every 20 minutes. That is 3 samples per hour, which when multiplied by 24 hours is 72 samples per calendar day. In this project, each calendar day can have at most 72 temperature samples for that reason. 

