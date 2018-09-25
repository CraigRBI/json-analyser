using System;
using System.Data;
using System.Threading;
using System.Collections.Generic;

namespace JsonAnalyser.DataEngine
{
    public class Order : ITransformable
    {
        private static int _orderId;
        private List<string> _meatTypes = new List<string>()
        {
            "ham",
            "salami",
            "pepperoni",
            "anchovies",
            "chili"
        };

        public Guid OrderId 
        {
            get 
            {
                byte[] bytes = new byte[16];
                BitConverter.GetBytes(_orderId).CopyTo(bytes, 0);

                return new Guid(bytes);
            }
        }
        public DateTime OrderDate { get; set; }
        public string[] Toppings { get; set; }

        // Custom model specific data transformations
        public DataTable CsvFormat
        {
            get 
            {
                Interlocked.Increment(ref _orderId);

                var dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("OrderId");
                dt.Columns.Add("OrderDate");
                dt.Columns.Add("Topping");
                dt.Columns.Add("IsToppingVegetarian");

                foreach (var topping in Toppings)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["OrderId"] = OrderId;
                    newRow["OrderDate"] = OrderDate;
                    newRow["Topping"] = topping;
                    newRow["IsToppingVegetarian"] = !_meatTypes.Contains(topping);
                    dt.Rows.Add(newRow);
                }

                return dt; 
            }
        }
    }
}