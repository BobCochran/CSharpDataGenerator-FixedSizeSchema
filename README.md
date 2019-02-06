# CSharpDataGenerator-FixedSizeSchema
This project demonstrates creation of fake IoT data and storing that data in a MongoDB database and collection using a fixed-size bucketing schema.

Code is written in the C# programming language.

This is an example application that creates fake "Internet of Things" (IoT) data and stores it in a MongoDB database collection. Application code is configured to have conceptual "gateways". Each gateway is instrumented with sensors. The application defines an initial value for each gateway. It will then fill out random values for the sensors based on the given gateway's value. The sensor values could be temperature readings, for example.

The code in this project configures 2 gateways, each with 3 sensors.

Each document within the mongodb database collection contains up to 200 sensor values.

This showcases a storage technique known as sized-based bucketing which is described in  
[a blog post by Robert Walters.](https://www.mongodb.com/blog/post/time-series-data-and-mongodb-part-2-schema-design-best-practices)

## Required Software

MongoDB Database, Community Edition or Enterprise, any currently supported version.  

Microsoft .NET Core SDK, version 2.2.103. 

MongoDB NET Driver, version 2.7.3. 

## MongoDB Database Server Default Connection

The MongoDB Database server, for purposes of this project, is assumed to be running with no authentication. It is assumed that all connections will be on localhost port 27017. If this is not the case for your implementation of MongoDB, you will have to modify the connection string given inside program.cs code in this project.

## How to Use This Project

First install the required software as described above. Make sure that the .NET Core SDK works by creating a "Hello World" console application as described in the installation instructions for .NET Core.

Start the MongoDB database server. 

Clone or fork this project from GitHub.

Change into the CSharpDataGenerator-FixedSizeSchema directory.

If you have to create a specific connection string for purposes of authentication or port connections, edit program.cs according to your requirements at this time. Look for the variable "ConnectionString". A commented-out connection string suitable for MongoDB Atlas connectivity is provided.

To compile (build) the project, open a terminal window, navigate to the project directory, and execute the command 

```
dotnet build
```

The project should compile without errors. The project as provided in this repository, does in fact compile without errors.

To run the program.cs code, open a terminal window, navigate to the project directory, and execute the command

```
dotnet run
```

Note: this code, as written, will drop (delete) any collection named 'IoTData' that it finds within the database named 'SizeBasedTS'. If you do not want your collection dropped, edit the program.cs code accordingly. It is reccommended that this code be tested on a test (not a production) deployment of MongoDB. 

 
## Program Outputs
### Console Output
The following messages are printed to the console upon executing 'dotnet run':

```
Dropping IoTData
Generating Data for Gateway # 0
Number of sensors 3
Starting Temp value= 58
Generating Data for Gateway # 1
Number of sensors 3
Starting Temp value= 33
All done!
```

Counts and values for your particular execution of the code may be different from the counts and values shown above.

### Database Output
A database named 'SizeBasedTS'

A collection named 'IoTData'

### Sample Document Content

```
MongoDB Enterprise > db.IoTData.findOne()
{
	"_id" : ObjectId("5c5a24848aa0aaf8edc7ded0"),
	"day" : ISODate("2019-02-05T05:00:00Z"),
	"deviceid" : 0,
	"sensorid" : 1003,
	"first" : 1545811460,
	"last" : 1546010460,
	"nsamples" : 200,
	"samples" : [
		{
			"val" : 58.03,
			"time" : 1545811460
		},
		{
			"val" : 58.07,
			"time" : 1545812460
		},
[198 additional entries will appear in the "samples" array".]
```
## References

1. [Robert Walters](https://www.mongodb.com/blog/post/time-series-data-and-mongodb-part-2-schema-design-best-practices)
 
