using System;
/* The purpose of this program is to read a file containing temperature 
 * readings emitted by a Maxim DS18B20 waterproof temperature sensor. 
 * The structure of the input file is like this:
 *
 * 1549757895
 * 05 01 4b 46 7f ff 0b 10 cd : crc=cd YES
 * 05 01 4b 46 7f ff 0b 10 cd t=16312
 * 
 * The first line is the Unix epoch timestamp -- created by 
 * the user executing `date +%s'. The timestamp is captured
 * and parsed to a 64-bit double.
 *
 * Second line containing the string 'crc=' can be skipped.
 *
 * Third line containing the string 't=' contains the actual 
 * temperature reported by the sensor in degrees Celsius. 
 * This value needs to be captured and the reading parsed
 * to a 64-bit double.
 *
 * There can be multiple epoch times denoting the time the 
 * temperature was taken and multiple temperature readings
 * after the time stamp.
 *
 * 
 */
namespace ds18b20_read
{
    class ds18b20_read 
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
