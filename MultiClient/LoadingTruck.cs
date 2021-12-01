using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace Amazoom
{
    public class LoadingTruck
    {
        public ConcurrentQueue<Product> LoadingProductList;                   // Queue of products in truck
        public int currentWeight = 0;
        public int maxWeight = 1000 * 1000 * 10;                              // 10 tonnes max weight
        
   
        public LoadingTruck()
        {
            this.LoadingProductList = new ConcurrentQueue<Product>();
        }

        /**
         *  Goes through list of loading docks in the warehouse and connects to the first available one
         *  If none are available, wait at the first loading dock
         * 
         * int warehouse_ID                     : ID of warehouse that the loading truck will go to
         * ref List<List<Block>> warehouse      : The warehouse
         */

        public void Dock(int warehouse_ID, ref List<List<Block>> warehouse) 
        {
            int i = 0;
            int first_dock = 0;
            int row = 0;
            int col = 0;
            // Get first index of correct warehouse
            while (Database.loadingDockLocations[i].Item1 != warehouse_ID) 
            {
                i++;
                first_dock = i;
            }

            // Go to open loading dock
            while (Database.loadingDockLocations[i].Item1 == warehouse_ID) 
            {
                col = Database.loadingDockLocations[i].Item3;
                row = Database.loadingDockLocations[i].Item4;

                if (warehouse[col][row].dock.isTruckPresent == false)
                {
                    warehouse[col][row].dock.semaphore.WaitOne();
                    warehouse[col][row].dock.isTruckPresent = true;
                    warehouse[col][row].dock.loadingTruck = this;

                    return;
                }

                // Skip to next dockID
                i += 2;
                if (i >= Database.loadingDockLocations.Count) 
                {
                    break;
                }
            }

            // No open loading docks, wait at first one
            col = Database.loadingDockLocations[first_dock].Item3;
            row = Database.loadingDockLocations[first_dock].Item4;
            warehouse[col][row].dock.semaphore.WaitOne();
            warehouse[col][row].dock.isTruckPresent = true;
            warehouse[col][row].dock.loadingTruck = this;


        }

    }
}
