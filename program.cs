using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FixedTS
{
    class Program
    {
        static void Main(string[] args)
        {
            string DBName = "SizeBasedTS";
            string DBCollectionName = "IoTData";
            int Minutes = 60;
            int Senors = 3;
            int Gateways = 2;
            bool clearDB = true;
            string ConnectionString = "mongodb://localhost:27017";
            // MongoDB Atlas sample -> string ConnectionString = "mongodb+srv://MONGODBUSER:MONGODBPASSWORD@clusterxxxxxx.mongodb.net/test?retryWrites=true";
            DateTime RightNow = DateTime.UtcNow;
            long starttime_unixTime = ((DateTimeOffset)RightNow).ToUnixTimeSeconds();
            long enddtime_unixTime = starttime_unixTime - (60000 * Minutes);

            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DBName);
            var collection = db.GetCollection<BsonDocument>(DBCollectionName);
            if (clearDB)
            {
                Console.WriteLine("Dropping " + DBCollectionName);
                db.DropCollection(DBCollectionName);
            }
            Task[] tasks = new Task[Gateways];
            Random rand = new Random();
            for (int ctr = 0; ctr < Gateways; ctr++)
            {
                tasks[ctr] = new TaskFactory().StartNew(new Action<object>((gateway_num) =>
                {
                    try
                    {
                        Console.WriteLine("Generating Data for Gateway # " + gateway_num);
                        Console.WriteLine("Number of sensors " + Senors);

                        double last_value = rand.Next(32, 75);
                        Console.WriteLine("Starting Temp value= " + last_value);

                        for (long t = enddtime_unixTime; t < starttime_unixTime; t = t + 1000)
                        {
                            for (int sn = Senors; sn > 0; sn = sn - 1)
                            {
                                var sensornumber = (Convert.ToInt32(gateway_num) + 1) * 1000 + sn;
                                double x = getvalue(last_value);
                                var sample = new BsonDocument { { "val", x }, { "time", Convert.ToDouble(t) } };
                                last_value = x;
                                DateTime today = DateTime.Today.AddDays(1).AddDays(-1);
                                var filter = new BsonDocument {
                                        {"deviceid", Convert.ToInt32(gateway_num)},
                                        {"sensorid", sensornumber},
                                        {"nsamples", new BsonDocument { { "$lt",200}}},
                                        {"day", today}
                                };
                                var update = new BsonDocument {
                                        {"$push", new BsonDocument{ {"samples", sample} }},
                                        {"$min", new BsonDocument{ {"first", Convert.ToDouble(t)} } },
                                        {"$max", new BsonDocument{ {"last", Convert.ToDouble(t)} } },
                                        {"$inc", new BsonDocument{ {"nsamples", 1}}}
                                };
                                collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                    return;
                }
                ), ctr);

            }
            Task.WaitAll(tasks);


            Console.WriteLine("All done!");
        }
        private static double getvalue(double old_value)
        {
            Random rand = new Random();
            double new_value;

            var change_percent = rand.NextDouble() * .001;
            var change_amount = old_value * change_percent;
            if (rand.Next(0, 10) > 5)
            {
                new_value = old_value + change_amount;
            }
            else
            {
                new_value = old_value - change_amount;
            }

            return Math.Round(new_value, 2);
        }


    }
}
