using System;
using System.Collections.Generic;

namespace JsonAnalyser.DataEngine
{
    public class OrderContainer : ITransformablesContainer
    {
        public List<Order> Orders { private get; set; }

        public List<ITransformable> List 
        {
            get 
            {
                var list = new List<ITransformable>();
                foreach (var order in Orders)
                {
                    list.Add(order);
                }
                return list; 
            }
        }
    }
}
