using System;
using System.Collections.Generic;
using System.Text;

namespace Amazoom
{
    public class ShelfSpace
    {
        // Multiple items of different types can be in a shelf space as long as they are
        // below the maxWeightCapacity

        public List<Product> shelfSpaceProducts;        // List of products in a ShelfSpace
        public int maxWeightCapacity;                // Maximum weight capacity
        public int currentWeight;                    // Total weight of products in a ShelfSpace

        public ShelfSpace()                             // Hardcoded maxWeightCapacity
        {
            this.shelfSpaceProducts = new List<Product>();
            maxWeightCapacity = 50000;                 // 50 kg             
            currentWeight = 0;
        }

    }
}
