using System;
using System.Collections.Generic;
using System.Text;

namespace Amazoom
{
    public class Product
    {
        public string itemName;
        public int weight;
        public string type;

        public Product(string Name, int Weight, string Type)
        {
            type = Type;
            itemName = Name;
            weight = Weight;
        }
    }
}
