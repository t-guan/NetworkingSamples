using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MultiClient;

namespace Amazoom
{
    public static class Warehouse
    {

        /** Initializes the warehouse
         *  
         *  int width               : is the width of the warehouse in int square blocks
         *  int height              : is the height of the ware house in int square blocks
         *  (int,int) LoadingDock   : First int 
         *                              designates orientation of loading dock 1 = Vertical, 0 = horizontal
         *                            Second int 
         *                              designates starting location for loading dock if Vertical designates starting row, if horizontal designates starting column
         *                              Loading docks are always height or width of 2 going down or right from starting location
         *                               is an array indicating the location of the loading dock blocks, they must be on the outer edge of the warehouse.
         *                            Third int 
         *                              designates (top/bottom) or (right/left) side of warehouse for loading dock (1 = top/left) (0 = bottom/right) depending on int 1.
         *                              
         *  return                  : Initializes a 2D list array of Block class, corresponding to the Column,row grid of the warehouse.
         */
        public static List<List<Block>> initialize_warehouse(int Id_Num, int height, int width, int[,] LoadingDock)
        {
            //Possible Blocks
            //Top Left corner, Top Right Corner, Bottom Left corner, Bottom Right Corner, Top wall, Bottom Wall, Left wall, Right wall, No wall

            // Current_Width = 0 { Top Left Corner, Left Wall, Bottom Left Corner}
            //Current_Width = width { Top Right Corner, Right Wall, Bottom Right Corner}
            //Current_Height = 0 {Top Right Corner, Top Wall, Top Left Corner}
            //Current_Height = height {Bottom Left Corner, Bottom Wall, Bottom right Corner}
            //else = No wall
            List<List<Block>> Ware_House = new List<List<Block>>(); //Creates a list that contains warehouse columns, row list is implemented in the below for loop.
            for (int current_width = 0; current_width < width; current_width++) //Create "width" # of columns
            {
                List<Block> Ware_House_Rows = new List<Block>();
                for (int current_Height = 0; current_Height < height; current_Height++) //Create "height" # of rows
                {
                    // BLOCK Constructor: Block(int Column, int row, int[] Path, int[] shelf)
                    if (current_Height == 0 && current_width == 0)  //TopLeftCorner
                    {
                        int[] temp_path = { 0, 0, 1, 1 }; //Can move down and right
                        int[] temp_shleves = { 0, 0 }; //No shelves
                        Block Block_temp = new Block(Id_Num, Dictionary_Storage.Column_Dictionary[current_width], current_Height, temp_path, temp_shleves);
                        Ware_House_Rows.Add(Block_temp);
                    }
                    else if ((current_width == 0) && (current_Height == height - 1)) //Bottom Left Corner
                    {
                        int[] temp_path = { 1, 0, 0, 1 }; //Can move Up and right
                        int[] temp_shleves = { 0, 0 }; //No shelves
                        Block Block_temp = new Block(Id_Num, Dictionary_Storage.Column_Dictionary[current_width], current_Height, temp_path, temp_shleves);
                        Ware_House_Rows.Add(Block_temp);
                    }
                    else if ((current_width == width - 1) && (current_Height == height - 1))  //Bottom Right Corner
                    {
                        int[] temp_path = { 1, 1, 0, 0 }; //Can move up and left
                        int[] temp_shleves = { 0, 0 }; //No shelves
                        Block Block_temp = new Block(Id_Num, Dictionary_Storage.Column_Dictionary[current_width], current_Height, temp_path, temp_shleves);
                        Ware_House_Rows.Add(Block_temp);
                    }
                    else if ((current_width == width - 1) && (current_Height == 0)) //Top Right Corner
                    {
                        int[] temp_path = { 0, 1, 1, 0 }; //Can move down and left
                        int[] temp_shleves = { 0, 0 }; //No shelves
                        Block Block_temp = new Block(Id_Num, Dictionary_Storage.Column_Dictionary[current_width], current_Height, temp_path, temp_shleves);
                        Ware_House_Rows.Add(Block_temp);
                    } 
                    else if (current_width == 0) //Left Wall
                    {
                        int[] temp_path = { 1, 0, 1, 0 }; //Can move down and up
                        int[] temp_shleves = { 0, 1 }; //Right shelves
                        Block Block_temp = new Block(Id_Num, Dictionary_Storage.Column_Dictionary[current_width], current_Height, temp_path, temp_shleves);
                        Ware_House_Rows.Add(Block_temp);
                    }
                    else if (current_Height == 0) //Top Wall
                    {
                        int[] temp_path = { 0, 1, 1, 1 }; //Can move down, left, right
                        int[] temp_shleves = { 0, 0 }; //No shelves
                        Block Block_temp = new Block(Id_Num, Dictionary_Storage.Column_Dictionary[current_width], current_Height, temp_path, temp_shleves);
                        Ware_House_Rows.Add(Block_temp);
                    }
                    else if (current_width == width - 1) //Right Wall
                    {
                        int[] temp_path = { 1, 0, 1, 0 }; //Can move up, down
                        int[] temp_shleves = { 1, 0 }; //Left shelves
                        Block Block_temp = new Block(Id_Num, Dictionary_Storage.Column_Dictionary[current_width], current_Height, temp_path, temp_shleves);
                        Ware_House_Rows.Add(Block_temp);
                    }
                    else if (current_Height == height - 1) //Bottom Wall
                    {
                        int[] temp_path = { 1, 1, 0, 1 }; //Can move up, left, right
                        int[] temp_shleves = { 0, 0 }; //No shelves
                        Block Block_temp = new Block(Id_Num, Dictionary_Storage.Column_Dictionary[current_width], current_Height, temp_path, temp_shleves);
                        Ware_House_Rows.Add(Block_temp);
                    }
                    else //No Wall
                    {
                        int[] temp_path = { 1, 0, 1, 0}; //Can move up, down
                        int[] temp_shleves = { 1, 1 }; //Left and Right shelves
                        Block Block_temp = new Block(Id_Num, Dictionary_Storage.Column_Dictionary[current_width], current_Height, temp_path, temp_shleves);
                        Ware_House_Rows.Add(Block_temp);
                    }
                }
                Ware_House.Add(Ware_House_Rows); //Adds list of rows to the appropriate column before remaking the Ware_House_Row List
            }
            Set_LoadingDock(LoadingDock, ref Ware_House, Id_Num);
            Inventory.Inv.Add("Warehouse");
            return Ware_House;
        }

        /** Set_LoadingDock
         *  Sets the public bool isDock to true for the correct block class inside Ware_House 2D List
         *  Sets the loadingDockNum for the correct block class inside Ware_House 2D List
         *  Inputs: (int,3) LoadingDock: [ {x,x,x},{y,y,y}...]
         *                            First int 
         *                              designates orientation of loading dock 1 = Vertical, 0 = horizontal
         *                            Second int 
         *                              designates starting location for loading dock if Vertical designates starting row, if horizontal designates starting column
         *                              Loading docks are always height or width of 2 going down or right from starting location
         *                               is an array indicating the location of the loading dock blocks, they must be on the outer edge of the warehouse.
         *                            Third int 
         *                              designates (top/bottom) or (right/left) side of warehouse for loading dock (0 = top/left) (1 = bottom/right) depending on int 1.
         *                              
         */
        internal static void Set_LoadingDock(int[,] Loading_Dock, ref List<List<Block>> Ware_House, int Ware_House_ID) 
        {
            for(int i = 0; i < Loading_Dock.GetLength(0); i++)
            {
                int temp_ori = Loading_Dock[i, 0];
                int temp_loc = Loading_Dock[i, 1];
                int top_right = Loading_Dock[i, 2];

                if (temp_ori == 0) //Horizontal
                {
                 if(top_right == 0) //Top, Horizontal Loading Dock
                    {
                        Ware_House[temp_loc][0].isDock = true;
                        Ware_House[temp_loc][0].loadingDockNum = i;
                        Ware_House[temp_loc][0].dock = new LoadingDock();
                        Ware_House[temp_loc+1][0].isDock = true;
                        Ware_House[temp_loc+1][0].loadingDockNum = i;
                        Ware_House[temp_loc + 1][0].dock = new LoadingDock();

                        Database.loadingDockLocations.Add(new Tuple<int, int, int, int>(Ware_House_ID, i, temp_loc, 0));
                        Database.loadingDockLocations.Add(new Tuple<int, int, int, int>(Ware_House_ID, i, temp_loc+1, 0));
                    }
                 else if(top_right == 1) //Bottom, Horizontal Loading Dock
                    {
                        Ware_House[temp_loc][Ware_House[0].Count-1].isDock= true;
                        Ware_House[temp_loc][Ware_House[0].Count - 1].loadingDockNum = i;
                        Ware_House[temp_loc][Ware_House[0].Count - 1].dock = new LoadingDock();
                        Ware_House[temp_loc + 1][Ware_House[0].Count - 1].isDock = true;
                        Ware_House[temp_loc + 1][Ware_House[0].Count - 1].loadingDockNum = i;
                        Ware_House[temp_loc + 1][Ware_House[0].Count - 1].dock = new LoadingDock();

                        Database.loadingDockLocations.Add(new Tuple<int, int, int, int>(Ware_House_ID, i, temp_loc, Ware_House[0].Count-1));
                        Database.loadingDockLocations.Add(new Tuple<int, int, int, int>(Ware_House_ID, i, temp_loc + 1, Ware_House[0].Count - 1));
                    }
                }
                else if(temp_ori == 1) //Vertical
                {
                    if (top_right == 1) //Right wall, Vertical Loading Dock
                    {
                        Ware_House[Ware_House.Count - 1][temp_loc].isDock = true;
                        Ware_House[Ware_House.Count - 1][temp_loc].loadingDockNum = i;
                        Ware_House[Ware_House.Count - 1][temp_loc].dock = new LoadingDock();
                        Ware_House[Ware_House.Count - 1][temp_loc + 1].isDock = true;
                        Ware_House[Ware_House.Count - 1][temp_loc + 1].loadingDockNum = i;
                        Ware_House[Ware_House.Count - 1][temp_loc + 1].dock = new LoadingDock();

                        Database.loadingDockLocations.Add(new Tuple<int, int, int, int>(Ware_House_ID, i, Ware_House.Count - 1, temp_loc));
                        Database.loadingDockLocations.Add(new Tuple<int, int, int, int>(Ware_House_ID, i, Ware_House.Count - 1, temp_loc + 1));
                    }
                    else if (top_right == 0) //Left wall, Vertical Loading Dock
                    {
                        Ware_House[0][temp_loc].isDock = true;
                        Ware_House[0][temp_loc].loadingDockNum = i;
                        Ware_House[0][temp_loc].dock = new LoadingDock();
                        Ware_House[0][temp_loc + 1].isDock = true;
                        Ware_House[0][temp_loc + 1].loadingDockNum = i;
                        Ware_House[0][temp_loc + 1].dock = new LoadingDock();

                        Database.loadingDockLocations.Add(new Tuple<int, int, int, int>(Ware_House_ID, i, 0, temp_loc));
                        Database.loadingDockLocations.Add(new Tuple<int, int, int, int>(Ware_House_ID, i, 0, temp_loc + 1));
                    }
                }
            }
        }


        /**
         * 
         *
         *
         */
        static public List<Product> Initialize_Product_List(string File_path)
        {
            List<Product> Product_List = new List<Product>();
            using (var reader = new StreamReader(File_path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    Product temp = new Product(values[0],Convert.ToInt32(values[2]),values[1]);
                    Product_List.Add(temp);
                }
            }
            return Product_List;
        }

        static public List<List<Block>> initialize_Warehouse_Shelves(List<List<Block>> warehouse, List<Product> product_list, ref List<DatabaseEntry> database)
        {
            int max_columns = warehouse.Count;
            int max_row = warehouse[0].Count;
            int current_row = 0;
            int current_col = 0;
            bool Left_Shelf_full = false;
            bool Right_Shelf_full = false;
            int current_height = 0;
            int products_added = 0;
            
            Random r = new Random();

            for (int i = 0; i < product_list.Count; i++)
            {
                int temp_quantity = r.Next(95) + 5; //Intitalize product with random quantity from 5 - 100.

                if (warehouse[current_col][current_row].LeftShelf == null && warehouse[current_col][current_row].RightShelf == null) //Checks if either shelf exists if not go to next row.
                {
                    current_row++;
                    if (current_row > max_row - 1) //If we exceeded the last row in the column then go to the next column.
                    {
                        current_row = 0;
                        current_col++;
                        if (current_col > max_columns - 1) //If we exceeded the last column then intialization is complete.
                        {
                            Console.WriteLine("Warehouse is full, the warehouse shelves were filled and " + products_added.ToString() + " products were added to the warehouse.");
                            return warehouse;
                        }
                        if (warehouse[current_col][current_row].LeftShelf == null && warehouse[current_col][current_row].RightShelf == null)
                        {
                            current_row++;
                            if (warehouse[current_col][current_row].LeftShelf != null)
                                Left_Shelf_full = false;
                            if (warehouse[current_col][current_row].RightShelf != null)
                                Right_Shelf_full = false;
                        }
                    }
                }

                if (warehouse[current_col][current_row].LeftShelf == null)
                    Left_Shelf_full = true;
                if (warehouse[current_col][current_row].RightShelf == null)
                    Right_Shelf_full = true;

                //This nested if statement checks if a single item of the next product will fit in the shelf. If not go to next shelf before trying to add the item.
                if (warehouse[current_col][current_row].LeftShelf != null && product_list[i].weight > (warehouse[current_col][current_row].LeftShelf.shelf_spaces[current_height].maxWeightCapacity - warehouse[current_col][current_row].LeftShelf.shelf_spaces[current_height].currentWeight))
                {
                    Left_Shelf_full = true;

                    if(warehouse[current_col][current_row].RightShelf != null && product_list[i].weight > (warehouse[current_col][current_row].RightShelf.shelf_spaces[current_height].maxWeightCapacity - warehouse[current_col][current_row].RightShelf.shelf_spaces[current_height].currentWeight))
                    {
                        Right_Shelf_full = true;
                    }
                }


                if (Left_Shelf_full && Right_Shelf_full) //Checks if both shelves at current height are full. If so increment to next shelf height.
                {
                    current_height++;

                    if (current_height > 4) //If Shelf height exceeds max height of shelves then go to next block in column.
                    {
                        current_height = 0;
                        current_row++;
                        if(current_row > max_row - 1) //If we exceeded the last row in the column then go to the next column.
                        {
                            current_row = 0;
                            current_col++;
                            if(current_col > max_columns - 1) //If we exceeded the last column then intialization is complete.
                            {
                                Console.WriteLine("Warehouse is full, the warehouse shelves were filled and " + (products_added-1).ToString() + " products were added to the warehouse.");
                                return warehouse;
                            }
                        }
                    }

                    if (warehouse[current_col][current_row].LeftShelf != null)
                        Left_Shelf_full = false;
                    if (warehouse[current_col][current_row].RightShelf != null)
                        Right_Shelf_full = false;
                }


                if (warehouse[current_col][current_row].LeftShelf != null && !Left_Shelf_full) //Try to insert product into left shelf
                {
                    //Set quantity of items to insert to ensure weight limit is not reached.
                    int max_weight = warehouse[current_col][current_row].LeftShelf.shelf_spaces[current_height].maxWeightCapacity;
                    int current_weight = warehouse[current_col][current_row].LeftShelf.shelf_spaces[current_height].currentWeight;
                    
                    while (temp_quantity * product_list[i].weight > (max_weight - current_weight))
                    {
                        temp_quantity--;
                    }

                    for(int j = 0; j < temp_quantity; j++) //Add the correct number of the product to the shelf
                    {
                        warehouse[current_col][current_row].LeftShelf.AddProduct(product_list[i], current_height);
                        current_weight = warehouse[current_col][current_row].LeftShelf.shelf_spaces[current_height].currentWeight;
                    }
                    add_To_Warehouse_Database(current_col, current_row, current_height, warehouse[0][0].Warehouse_Id, temp_quantity, product_list[i].itemName, ref database, 'L');


                    if (current_weight >= (warehouse[current_col][current_row].LeftShelf.shelf_spaces[current_height].maxWeightCapacity - 5000)) //Set Left Shelf_full flag if within 5kg of max weight.
                    {
                        Left_Shelf_full = true;
                    }
                }

                else if(warehouse[current_col][current_row].RightShelf != null && !Right_Shelf_full) //If left shelf full or null, try to insert product into right shelf
                {
                    int max_weight = warehouse[current_col][current_row].RightShelf.shelf_spaces[current_height].maxWeightCapacity;
                    int current_weight = warehouse[current_col][current_row].RightShelf.shelf_spaces[current_height].currentWeight;
                    while (temp_quantity * product_list[i].weight > (max_weight - current_weight))
                    {
                        temp_quantity--;
                    }

                    for (int j = 0; j < temp_quantity; j++) //Add the correct number of the product to the shelf
                    {
                        warehouse[current_col][current_row].RightShelf.AddProduct(product_list[i], current_height);
                        current_weight = warehouse[current_col][current_row].RightShelf.shelf_spaces[current_height].currentWeight;
                    }
                    add_To_Warehouse_Database(current_col, current_row, current_height, warehouse[0][0].Warehouse_Id, temp_quantity, product_list[i].itemName, ref database, 'R');

                    if (current_weight >= (warehouse[current_col][current_row].RightShelf.shelf_spaces[current_height].maxWeightCapacity - 5000)) //Set Right Shelf_full flag.
                    {
                        Right_Shelf_full = true;
                    }
                }
                products_added++;
            }
            Console.WriteLine("Initalize warehouse shelves ran and added: " + (products_added-1).ToString() + " products to the warehouse.");
            return warehouse;
        }

        static public void add_To_Warehouse_Database(int col, int row, int shelf, int Warehouse_Id, int stock, string product_Id, ref List<DatabaseEntry> database, char Side)
        {
            //string _productName, char _column, int _row, char _shelfSide, int _warehouseLocation, int _stock
            if(Side == 'L')
            {
                DatabaseEntry temp = new DatabaseEntry(product_Id, Dictionary_Storage.Column_Dictionary[col], row, shelf,'L', Warehouse_Id, stock);
                database.Add(temp);
            }
            if (Side == 'R')
            {
                DatabaseEntry temp = new DatabaseEntry(product_Id, Dictionary_Storage.Column_Dictionary[col], row, shelf,'R', Warehouse_Id, stock);
                database.Add(temp);

            }

            return;
        }
    }
}
