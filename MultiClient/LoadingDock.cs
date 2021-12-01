using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Amazoom
{
    public class LoadingDock
    {
        public bool isTruckPresent;
        public LoadingTruck loadingTruck;          // Truck present at loading truck
        public Semaphore semaphore;


        public LoadingDock()
        {
            this.semaphore = new Semaphore(1, 1);
            isTruckPresent = false;
        }

        public void undockTruck()
        {
            isTruckPresent = false;
            this.loadingTruck = null;
        }
    }
}
