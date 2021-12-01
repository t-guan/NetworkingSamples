using System;
using System.Collections.Generic;
using System.Text;

namespace Amazoom
{
    static public class Database
    {

        static public List<DatabaseEntry> database = new List<DatabaseEntry>();
        static public List<Tuple<int, int, int, int>> loadingDockLocations = new List<Tuple<int, int, int, int>>(); // (WarehouseID, DockID, Col, Row)

        static public int find_Database_Index(string product_Name)
        {
            int i = 0;
            while (Database.database[i].productName != product_Name)
            {
                i++;
            }
            return i;
        }
        // database[0] = "Lotion", "A3L", 1
        // database[1] = "Diamond", "B5R", 1=
    }

    
}
