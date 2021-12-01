using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Amazoom
{

    public class Block
    {

            
        //Block is a public class that acts as one coordinate destination for a robot in the amazon warehouse
        public string name;
        public string col;          // Warehouse column       A,B,C,D,E...AZ 
        public int col_num;         // Warehouse col         1,2,3,4,5... 
        public int row;             // Warehouse row         1,2,3,4,5...
        public int[] path;          // Block moveable spaces 0111,0101,0110...
        public Shelf LeftShelf;
        public Shelf RightShelf;
        public LoadingDock dock;
        public bool isDock = false;
        public int loadingDockNum;
        public int Warehouse_Id;
        public Mutex block_Mux;

        /**
        * char[] Path should only have length of 4
        *   [0] = UP, [1] = RIGHT, [2] = DOWN, [3] = LEFT
        *   I.E. Path=[0,1,1,1], means that the robot can move Right, Down, or Left, but not up
        * int[] shelf should only have lenngth of 2
        *   [0] =Left Shelf, [1]= right shelf
        *   I.E. If shelf[1,1] that means there are shelves on both sides of the block.
        */

        //Constructor for Block
        public Block(int Id_Num, string _col, int Number, int[] _path, int[] shelf)
        {
            Warehouse_Id = Id_Num;
            name = _col + Number.ToString();
            col = _col;
            col_num = Dictionary_Storage.Column_Dictionary_Reverse[_col];
            row = Number;
            path = _path;
            block_Mux = new Mutex();
            // TODO: Pass height into block constructor
            // Currently hardcoded shelf height
            if (shelf[0] == 1)
            {
                this.LeftShelf = new Shelf(5);
            }
            if(shelf[1] == 1)
            {
                this.RightShelf = new Shelf(5);
            }
        }

    }
}
