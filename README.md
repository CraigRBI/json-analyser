# json-analyser
Instructions

1) Set up a sql server database using the schema provided in json-analyser/resources/orders-schema.sql

2) Update the database connection string in json-analyser/JsonAnalyser/app.config

3) After building the solution, a directory will need to be created in the location of the .exe called 'import'; this will contain a .json file with the order data (see compatible .json file in json-analyser/resources)

4) The console application has 3 functions/commands (transform, import, analyse \<date\>); running the application in this respective order will transform the input json data and output a csv file, import the csv data to the database, and render a table on the terminal/console with data returned from an embedded sql query
