using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using JsonAnalyser.DataEngine;

namespace JsonAnalyser
{
    class Program
    {
        private static string _jsonInputDir;
        private static string _csvOutputDir;

        private static string _function;
        private static string _functionArg;

        public static void Main(string[] args)
        {
            try 
            {
                InitProgram(args);

                switch (_function)
                {
                    case "transform":
                        // Transform JSON input data to CSV data
                        var jsonInputFile = Directory.GetFiles(_jsonInputDir).Where(x => x.Contains(".json"))
                                                     .OrderByDescending(x => new FileInfo(x).CreationTime).First();
                        var csvOutputFile = Path.Combine(_csvOutputDir, "orders.csv");
                        DataTransformer.TransformJsonToCsv<OrderContainer>(jsonInputFile, csvOutputFile);

                        Console.WriteLine("Transformed '{0}' => '{1}'.", jsonInputFile, csvOutputFile);
                        Console.WriteLine("");

                        break;

                    case "import":
                        // Re-import data into DB from most recently generated CSV file
                        var csvPath = Directory.GetFiles(_csvOutputDir).OrderByDescending(x => new FileInfo(x).CreationTime).First();
                        DatabaseAccess.ClearTable("Orders");
                        DatabaseAccess.ImportCsvToTable("Orders", csvPath);

                        Console.WriteLine("Imported data into database from '{0}'.", csvPath);
                        Console.WriteLine("");

                        break;

                    case "analyse":
                        // Implementation of analyse query; takes one console argument (date)
                        if (!String.IsNullOrEmpty(_functionArg))
                        {
                            DateTime orderDateDateTime;
                            var parseSuccess = DateTime.TryParse(_functionArg, out orderDateDateTime);
                            var queryResult = new DataTable();

                            if (parseSuccess)
                            {
                                var year = orderDateDateTime.Year;
                                var month = orderDateDateTime.Month;
                                var day = orderDateDateTime.Day;

                                var cmd = new SqlCommand();
                                cmd.CommandText =
                                       "SELECT " +
                                       "    DATEFROMPARTS (@year, @month, @day) AS 'Date Period of Report', " +
                                       "    COALESCE(SUM(IsVegetarian), 0) AS 'Number of Vegetarian Pizzas Sold', " +
                                       "    COALESCE(COUNT(DistinctDate), 0) - COALESCE(SUM(IsVegetarian), 0) AS 'Number of Non-vegetarian Pizzas Sold' " +
                                       "FROM " +
                                       "( " +
                                       "    SELECT " +
                                       "        OrderDate AS 'FilteredDate', " +
                                       "        COUNT(DISTINCT(OrderDate)) AS 'IsVegetarian' " +
                                       "    FROM Orders " +
                                       "    GROUP BY OrderDate " +
                                       "    HAVING " +
                                       "        SUM(CASE WHEN IsToppingVegetarian = 'False' THEN 1 ELSE 0 END) = 0 AND" +
                                       "        CAST(OrderDate AS DATE) = DATEFROMPARTS (@year, @month, @day)" +
                                       ") AS vegetarianList " +
                                       "FULL OUTER JOIN" +
                                       "( " +
                                       "    SELECT " +
                                       "        DISTINCT(OrderDate) AS 'DistinctDate' " +
                                       "    FROM Orders" +
                                       "    WHERE CAST(OrderDate AS DATE) = DATEFROMPARTS (@year, @month, @day)" +
                                       ") AS totalList " +
                                       "ON vegetarianList.FilteredDate = totalList.DistinctDate";

                                cmd.Parameters.Add(new SqlParameter("@year", year));
                                cmd.Parameters.Add(new SqlParameter("@month", month));
                                cmd.Parameters.Add(new SqlParameter("@day", day));

                                queryResult = DatabaseAccess.ExecuteSelectQuery(cmd);

                                DataVisualiser.OutputToConsole(queryResult);
                            }
                            else
                            {
                                Console.WriteLine("Analysis argument not recognised.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No analysis argument supplied.");
                        }

                        break;

                    default:
                        Console.WriteLine("Command not recognised.");

                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : {0}", ex.Message);
            }
        }

        public static void InitProgram(string[] args)
        {
            _jsonInputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input");
            _csvOutputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");

            if (!Directory.Exists(_jsonInputDir))
                Directory.CreateDirectory(_jsonInputDir);

            if (!Directory.Exists(_csvOutputDir))
                Directory.CreateDirectory(_csvOutputDir);


            _function = "";
            _functionArg = "";

            if (args.Count() > 0)
            {
                _function = args[0];

                if (args.Count() > 1)
                {
                    _functionArg = args[1];
                }
            }
        }
    }
}
