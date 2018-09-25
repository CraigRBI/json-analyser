# json-analyser
Instructions -

1) Set up sql server database using schema provided in json-analyser/resources/orders-schema.sql
2) Update database connection string in json-analyser/JsonAnalyser/app.config
3) After building the solution, a directory will need to be created in the location of the .exe called 'import'; this will contain a .json file with the order data
4) The console application has 3 functions/commands (transform, import, analyse <data>); running the application in this respective order will convert the input json data to csv, import the csv data to the database, and render a table on the terminal with data returned from an embedded sql query
