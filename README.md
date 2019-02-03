# CSharpDataGenerator-FixedSizeSchema
C# example of creating fake IoT data and storing in MongoDB using a fixed size schema 

This is an example C# application that creates fake IoT data and stores it in MongoDB.  In the app there are gateways and these gateways have sensors.  The app will define an initial value for each gateways then fill out random values for the sensors based on the gateways value.
The default is 2 gateways, 3 sensors each.

Each document will be up to 200 samples per document.  This technique known as sized-based bucketing is described in  
https://www.mongodb.com/blog/post/time-series-data-and-mongodb-part-2-schema-design-best-practices
