using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using CsvHelper;

namespace JsonAnalyser.DataEngine
{
    public class DatabaseAccess
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

        public static void ImportCsvToTable(string tableName, string csvPath, bool hasUniqueId = false)
        {
            var cmd = new SqlCommand();

            using (CsvReader csv = new CsvReader(new StreamReader(csvPath), true))
            {
                switch(hasUniqueId)
                {
                    case false:
                        // Non-unique row id implies first column of CSV file contains non-unique id value
                        string[] headers = null;
                        while (csv.Read())
                        {
                            if (headers == null)
                            {
                                headers = csv.Context.Record;
                            }
                            else
                            {
                                var columnNames = new List<string>();
                                var columnValues = new List<string>();

                                for (int i = 0; i < headers.Length; i++)
                                {
                                    columnNames.Add(headers[i]);
                                    columnValues.Add("'" + csv[i] + "'");
                                }

                                cmd.CommandText =
                                       "INSERT INTO " + tableName + " (" + String.Join(",", columnNames) + ") " +
                                       "VALUES (" + String.Join(",", columnValues) + ") ";

                                using (SqlConnection db = new SqlConnection(_connectionString))
                                {
                                    if (db.State == ConnectionState.Closed)
                                    {
                                        db.Open();
                                    }

                                    cmd.Connection = db;
                                    cmd.ExecuteScalar();

                                    db.Close();
                                }
                            }
                        }
                        break;

                    case true:
                        throw new System.NotImplementedException();
                }
            }
        }

        public static void ClearTable(string tableName)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }

                var cmd = new SqlCommand();

                cmd.Connection = db;
                cmd.CommandText = "DELETE FROM " + tableName;
                cmd.ExecuteScalar();

                db.Close();
            }
        }

        public static DataTable ExecuteSelectQuery(SqlCommand cmd)
        {
            var dt = new DataTable();

            if (cmd.CommandText.ToLower().Contains("select"))
            {
                using (SqlConnection db = new SqlConnection(_connectionString))
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        db.Open();
                    }

                    cmd.Connection = db;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }

                    db.Close();
                }
            }

            return dt;
        }
    }
}
