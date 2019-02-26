using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

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
        static int Main(string[] args)
        {
           //Do we have input arguments? We are expecting:
           // string -- sensor deviceid
           // string -- name of temperature file to be processed
           // string -- MongoDB database name to connect to
           // string -- MongoDB collection name to update

          if (args.Length == 0) {
   
            Console.WriteLine("Please enter device name, input filename, database name, and collection name to process.");
            Console.WriteLine("Usage: <devicename> <input file name> <database name> <database collection name>");
            Console.WriteLine("Please note that this program is expecting\nto connect to a MongoDB version 4.x.y database"); 
            Console.WriteLine("You must have a running MongoDB database instance."); 
            return 1;
         
          }

          if (args.Length < 4) {
   
            Console.WriteLine("Please enter device name, input filename, database name, and collection name to process.");
            Console.WriteLine("Usage: <devicename> <input file name> <database name> <database collection name>");
            Console.WriteLine("Please note that this program is expecting\nto connect to a MongoDB version 4.x.y database"); 
            Console.WriteLine("You must have a running MongoDB database instance."); 
            return 1;
         
          }

          string deviceid = args[0];
     
          string fn = args[1];

          /* Instead of hardcoding the MongoDB database and collection name in the code,
           * get it from the command line. 
           */

          string dbn = args[2];

          string colln = args[3];

          Console.WriteLine("\ndeviceid captured " + deviceid + " filename captured " + fn + "\n");

          Console.WriteLine("Database " + dbn + " database collection " + colln + "\n");

          string[] lines;

          List<double> saved_dtval_list = new List<double>();

          List<double> saved_temps_list = new List<double>();

          Regex r = new Regex("crc");

          // Read the input file

          try {

                 lines = System.IO.File.ReadAllLines(@fn);

          } catch(IOException e) {

            Console.WriteLine("\nA file read error occured ", e);
            
            return 2;
         
         }

          // Print the input file. Attempt to capture the epoch date
          // and temperature value. Epoch dates and temperatures 
          // are saved to separate List<> collections.

          foreach (string s in lines) {

            Console.WriteLine(s);

             // get rid of lines that are all white space

             if (String.IsNullOrWhiteSpace(s)) {

                continue;
    
             }

            // get rid of the crc lines

           if (r.IsMatch(s)) {

              continue;

           }

            double dtval = IsNumeric(s);  //This should be the Unix epoch value
 
            if (dtval > 0) {

               Console.WriteLine("\nThe value of dtval is " + dtval);

               saved_dtval_list.Add(dtval);

               continue;

            }

            double temp = FindTemperature(s);

            saved_temps_list.Add(temp);

            Console.WriteLine("The temperature is " + temp);

          }
              
          return 0;
 
        }

       /* This method checks to see if the line contains a numeric double value. 
        * If so, this is the epoch date in seconds since January 1, 1970. 
        * We want to capture this value.
        */

       public static double IsNumeric(string str) {

           try {

             str = str.Trim( );

             double dtval = 0;

             dtval = double.Parse(str);

             return dtval;

          } catch (FormatException) {

             double dtval = 0;

             return dtval;
          }

        }

     /* This method splits the input string on an equals sign '=' 
      * and checks to see if the string on the right of the equals
      * sign is a number. If it is, this is the temperature value.
      * We save that temperature value and throw out everything else.
      */

      public static double FindTemperature(string str) {

          string[] t_array = str.Split('=');

          double scaled_value = 0.0;

          string specifier;

          CultureInfo culture;

          double t_value = 0;

          specifier = "F3";

          culture = CultureInfo.CreateSpecificCulture("en-US");
          
          foreach (var a in t_array) {

             Console.WriteLine("This is one side of the split --> " + a);

             // Try parsing a double value. Scale the value by dividing by 1000 to place the 
             // Decimal point correctly. Return the scaled value. Why scale the temperature?
             // Well it is given to us as a string in the format nnnnn from the sensor
             // by default. The temperature reading could be 15125 as reported by the sensor.
             // The temperature reading is meant to be 15.125 degrees Centigrade, so we need
             // to convert the string to a double and then divide it by 1000 to set the decimal
             // point. 

             if (Double.TryParse(a, out t_value)) {
                scaled_value = t_value/1000;
                Console.WriteLine("'{0}' --> {1}", a, t_value);
                Console.WriteLine("The scaled value is --> " + scaled_value.ToString(specifier, culture));
                break;
             }                                         // see if we get a true double.

          }

          return scaled_value;

      }
 
    }
}
