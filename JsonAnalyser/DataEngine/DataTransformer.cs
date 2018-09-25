using System;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using CsvHelper;

namespace JsonAnalyser.DataEngine
{
    public class DataTransformer
    {
        public static void TransformJsonToCsv<T>(string inputPath, string outputPath)
        {
            T modelContainer;

            // Import JSON data to class model container
            using (StreamReader reader = new StreamReader(inputPath))
            {
                string json = reader.ReadToEnd();
                modelContainer = JsonConvert.DeserializeObject<T>(json);
            }
           
            using (var csv = new CsvWriter(new StreamWriter(outputPath, false, System.Text.Encoding.UTF8)))
            {
                int idx = 1;

                foreach (var transformable in ((ITransformablesContainer)modelContainer).List)
                {
                    var csvTransform = transformable.CsvFormat;

                    if (idx == 1)
                    {
                        foreach (DataColumn column in csvTransform.Columns)
                        {
                            csv.WriteField(column.ColumnName);
                        }
                        csv.NextRecord();
                    }

                    foreach (DataRow row in csvTransform.Rows)
                    {
                        for (var i = 0; i < csvTransform.Columns.Count; i++)
                        {
                            csv.WriteField(row[i]);
                        }
                        csv.NextRecord();
                    }

                    idx++;
                }
            }
        }
    }
}
