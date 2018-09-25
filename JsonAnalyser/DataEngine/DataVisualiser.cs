using System;
using System.Data;

namespace JsonAnalyser
{
    public class DataVisualiser
    {
        public static void OutputToConsole(DataTable dt)
        {
            int idx = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (idx == 0)
                {
                    var tableWidth = 0;

                    for (int colIdx = 0; colIdx < dt.Columns.Count; colIdx++)
                    {
                        Console.Write(String.Format("|{0,-" + dt.Columns[colIdx].ColumnName.Length + "}|", dt.Columns[colIdx].ColumnName));
                        tableWidth += dt.Columns[colIdx].ColumnName.Length + 2;
                    }

                    Console.WriteLine("");
                    Console.Write("{0}", new String('-', tableWidth)); // table header separator
                }

                Console.WriteLine("");

                for (int colIdx = 0; colIdx < dt.Columns.Count; colIdx++)
                {
                    Console.Write(String.Format(" {0,-" + dt.Columns[colIdx].ColumnName.Length + "} ", row[colIdx]));
                }

                idx++;
            }
            for (int i = 0; i < 3; i++)
                Console.WriteLine("");
        }
    }
}
