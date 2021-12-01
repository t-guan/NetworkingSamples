using System;
using System.Collections.Generic;
using System.Text;

namespace Amazoom
{
    public class DatabaseEntry
    {
        public string productName;
        public string column;
        public int row;
        public int shelf;
        public char shelfSide;
        public int warehouseLocation;       // Warehouse number
        public int stock;

        public DatabaseEntry(string _productName, string _column, int _row, int _shelf,char _shelfSide, int _warehouseLocation, int _stock)
        {
            productName = _productName;
            column = _column;
            row = _row;
            shelf = _shelf;
            shelfSide = _shelfSide;
            warehouseLocation = _warehouseLocation;
            stock = _stock;
        }
    }
}
