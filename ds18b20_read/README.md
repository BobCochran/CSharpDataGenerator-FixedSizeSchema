# Real C# Temperature Measurement and Database Storage Project
This directory has a working project in C# that accepts readings from a Maxim DS18B20 temperature sensor and stores them into a mongodb collection using size-based bucketing. Here is how the project works:
1. A DS18B20 sensor is connected to a single-board computer. For this project, the author used a Beaglebone Black Wireless device running a Debian release.
2. The date and time, formatted as Unix Epoch seconds, are echoed to a text file containing date and temperature readings. A cron job is used to store these readings in the text file.
3. The temperature, as recorded by the sensor, is read and echoed to the text file mentioned above.
4. The text file is read by our C# program ds18b20_read.cs which saves each date and temperature value to individual arrays. After the file is processed, date and temperature readings are added to a MongoDB collection using the size-based bucketing technique. 
You can take temperature readings at whatever intervals you want. The author decided to record the date/time/temperature every 20 minutes. That is 3 samples per hour, which when multiplied by 24 hours is 72 samples per calendar day. In this project, each calendar day can have at most 72 temperature samples for that reason. 
The C# program ds18b20_read.cs should be run on the command line, either in a bash shell (for Linux implementations) or a command prompt (for Microsoft Windows implementations.) The command format is:

```
dotnet run -- deviceid input_file_name database_name collection_name
```
Where the deviceid field can be anything that can identify the temperature sensor itself. The author uses the sensor's serial number for this field. After the deviceid field, separate the argument with a space character, and provide the input filename to be read. After the input filename, separate the argument with a space character, and enter the MongoDB database name. After the database name, separate the argument with a space character, and enter the MongoDB collection name.

An example invocation executed from inside the program directory:

```
dotnet run -- 28-00000a2a6f2a mytemp3 mydb mycollection
```

### Editing ds18b20_read.cs
The provided C# source code has Unix line terminations rather than DOS (Microsoft Windows) line terminations. If you are editing the code in a Linux operating system, you are good to go. Otherwise you can Google to find methods for changing line terminations back to Microsoft Windows if you wish. One concise reference is [The Indiana University Knowledgebase](https://kb.iu.edu/d/acux).
