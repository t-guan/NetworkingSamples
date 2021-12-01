using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;


namespace Amazoom
{
    public class Robot
    {
        public List<Product> robotProducts;                 // List of products in the robot
        public double maxWeightCapacity;                    // Maximum weight capacity
        public double currentWeight;                        // Total weight of products in a ShelfSpace
        public List<List<OrderEntry>> robot_Order = new List<List<OrderEntry>>();
        public bool isIdle;
        public Block locBlock;                            // Reference to block at location the robot is in
        public List<String> locationHistory = new List<String>(); 

        public Robot(List<List<Block>> warehouse)
        {
            this.robotProducts = new List<Product>();
            this.maxWeightCapacity = 100000;                // Carrying capacity of 100 kg
            this.currentWeight = 0;
            warehouse[0][0].block_Mux.WaitOne();
            this.locBlock = warehouse[0][0];
            this.isIdle = true;
        }


        /**
        *  Updates robot locations in locationHistory list using public location string variable
        */
        private void UpdateLocationHistory()
        {
            this.locationHistory.Add(this.locBlock.name);
            if (this.locationHistory.Count > 50)
            {
                this.locationHistory.RemoveAt(0);
            }
        }

        /**
        *  Prints robot locationHistory List using public locationHistory Variable.
        */
        public void printLocationHistory()
        {
            int i = 0;
            for (i=0; i<this.locationHistory.Count; i++)
            {
                Console.WriteLine(this.locationHistory[i]);
            }
        }

        // Query database where to put product, assumes robot has product already in it from truck.
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO-
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        //THIS NEEDS MORE WORK JUST NOT SURE ON INTENT OF THIS METHOD @MARCO
        public void StockProduct(string _product)
        {
            int database_index = Database.find_Database_Index(_product);
            Dictionary_Storage.Column_Dictionary_Reverse.TryGetValue(Database.database[database_index].column, out int col_Loc); //Gets column location
            int row_Loc = Database.database[database_index].row; //Gets row location
            
            
            
            Database.database[database_index].stock++; //Adds new product to database.
        }

        /**
         *  Retrieves product with itemName _product from _shelf in _shelfRow
         *  Assumes product is in the shelf
         *  
         *  string _product     : Name of product
         *  Shelf _shelf        : Shelf object, either LeftShelf or RightShelf
         *  int _shelfRow       : Index of shelf_space list in Shelf object where _product is located
         */
        public void RetrieveProduct(string _product, Shelf _shelf, int _shelfRow) 
        {
            Product product = _shelf.GetProductInfo(_product, _shelfRow);

            // Add the product and product weight to robot if possible
            if ( (product.weight + this.currentWeight) <= this.maxWeightCapacity)
            {
                this.robotProducts.Add(product);
                this.currentWeight += product.weight;

                // Remove the product from the ShelfSpace list of products and update quantity in database
                _shelf.RemoveProduct(_product, _shelfRow);
                int database_index = Database.find_Database_Index(_product);
                Database.database[database_index].stock--; 
            }
            else 
            {
                // TODO: Exception handling for if robot cannot carry the additional product
                Console.WriteLine("Cannot retrieve product: Overweight");
            }

        }

        /**
         *  Grabs a single item from the loading truck and adds it to robotProducts
         *  Assumes robot is in a block with a loading dock
         */
        public void UnloadTruck()
        {
            Product product;
            this.locBlock.dock.loadingTruck.LoadingProductList.TryPeek(out product);

            if ((product.weight + this.currentWeight) <= this.maxWeightCapacity)
            {
                this.locBlock.dock.loadingTruck.LoadingProductList.TryDequeue(out product);
                this.robotProducts.Add(product);
                this.currentWeight += product.weight;
            }
            else 
            {
                Console.WriteLine("Cannot retrieve product: Overweight");
            }
        }

        /**
         *  Adds a single item from robotProducts to the loading truck
         *  Assumes robot is in a block with a loading dock
         */
        public void LoadTruck()
        {
            Product product = robotProducts[0];

            if ((product.weight + this.locBlock.dock.loadingTruck.currentWeight) <= this.locBlock.dock.loadingTruck.maxWeight)
            {
                this.robotProducts.RemoveAt(0);
                this.currentWeight -= product.weight;
                this.locBlock.dock.loadingTruck.currentWeight += product.weight;
                this.locBlock.dock.loadingTruck.LoadingProductList.Enqueue(product);
            }
            else
            {
                Console.WriteLine("Cannot deliver product: Overweight");
            }
        }

        /**
        *  Finds the location of the product in the warehosue, and returns its string location.
        *  string product_Name            : Name of product as identified in the database
        *  
        *  return int col location of product
        *  return int row location of product
        *  return char shelf location of product 'L' or 'R'
        *  return int height location of product 
        */
        private (string,char,int) FindLocation(string product_Name)
        {
            int database_index = Database.find_Database_Index(product_Name);
            string product_Loc = Database.database[database_index].column + Database.database[database_index].row.ToString();
            int height = Database.database[database_index].shelf;
            char shelf = Database.database[database_index].shelfSide;
            return (product_Loc,shelf,height);
        }

        /**
         *  Moves robot to specifed column and row and adds reference to the block at that location
         *  The robot will just appear at the location, no movement implemented yet
         *  
         *  string location             : Location to move robot    "A1", "B15", "AE26" ...
         *  List<List<Block>> warehouse : The warehouse "object"
         */
        private void MoveRobot(string location, List<List<Block>> warehouse)
        {
            // Ware_House[i] is the column
            // Ware_House[i][j] is the row
            (int, int) locationIndex = ParseLocation(location); //Returns desired location of robot within warehouse.
            while((this.locBlock.col_num,this.locBlock.row)!= locationIndex) //Checks if current robot location is at desired locaiton
            {
                if(this.locBlock.col_num == 0 && this.locBlock.row != 0) //Move up
                {
                    warehouse[locBlock.col_num][locBlock.row-1].block_Mux.WaitOne(); //Wait until block up is free
                    this.locBlock.block_Mux.ReleaseMutex(); 
                    this.locBlock = warehouse[locBlock.col_num][locBlock.row-1]; 
                    UpdateLocationHistory();
                }
                else if ((this.locBlock.col_num == locationIndex.Item1 || this.locBlock.col_num == warehouse.Count - 1) && this.locBlock.row < warehouse[0].Count -1) //found desired col/reached max col move down.
                {
                    warehouse[locBlock.col_num][locBlock.row + 1].block_Mux.WaitOne(); //Wait until block down is free
                    this.locBlock.block_Mux.ReleaseMutex();
                    this.locBlock = warehouse[locBlock.col_num][locBlock.row + 1];
                    UpdateLocationHistory();
                }
                else if(this.locBlock.row == 0 && this.locBlock.col_num < warehouse.Count-1) //Move right
                {
                    warehouse[locBlock.col_num + 1][locBlock.row].block_Mux.WaitOne(); //Wait until block to right is free
                    this.locBlock.block_Mux.ReleaseMutex();
                    this.locBlock = warehouse[locBlock.col_num + 1][locBlock.row];
                    UpdateLocationHistory();
                }
                else if (this.locBlock.row == warehouse[0].Count-1 && this.locBlock.col_num != 0) // Move left
                {
                    warehouse[locBlock.col_num - 1][locBlock.row].block_Mux.WaitOne(); //Wait until block to left is free
                    this.locBlock.block_Mux.ReleaseMutex();
                    this.locBlock = warehouse[locBlock.col_num - 1][locBlock.row];
                    UpdateLocationHistory();
                }

                else //Move down
                {
                    warehouse[locBlock.col_num][locBlock.row + 1].block_Mux.WaitOne(); //Wait until block down is free
                    this.locBlock.block_Mux.ReleaseMutex();
                    this.locBlock = warehouse[locBlock.col_num][locBlock.row + 1];
                    UpdateLocationHistory();
                }
            }
        }

        /**
         *  Parse location string into warehouse col and row indices
         *  Assumes the location exists in the warehouse
         *  
         *  string location     : Location    "A1", "B15", "AE26" ...
         *  
         *  return int col      : Column index for warehouse
         *  return int row      : Row index for warehouse
         */
        private (int col, int row) ParseLocation(string location) 
        {
            Regex regex = new Regex("([a-zA-z]+)([0-9]+)");
            MatchCollection subLocations = regex.Matches(location);

            int col = -1;
            int digit = 1;
            // Convert from Hexavigesimal (base 26) to Decimal (base 10) with zero-index
            foreach (char letter in subLocations[0].Groups[1].Value.ToUpper())
            {
                col += (letter - 64) * (int)Math.Pow(26, subLocations[0].Groups[1].Length - digit);
                digit++;
            }

            int row = Int32.Parse(subLocations[0].Groups[2].Value);

            return (col, row);
        }

        public void Idle(List<List<Block>> warehouse)
        {
            MoveRobot(warehouse[0][0].name, warehouse);
            MoveRobot(warehouse[warehouse.Count - 1][warehouse[0].Count - 1].name,warehouse);
        }

        public void Collect_order(List<List<Block>> warehouse, string dock_location)
        {
            int i = 0;
            int weight = 0;
            while (robot_Order[0].Count > 0)
            {
                (string prod_loc, char shelf, int height) = FindLocation(robot_Order[0][i].productName);
                MoveRobot(prod_loc, warehouse);
                (int prod_col, int prod_row) = ParseLocation(prod_loc);

                if (shelf == 'L')
                {
                    Product temp_product = warehouse[prod_col][prod_row].LeftShelf.GetProductInfo(robot_Order[0][i].productName, height);
                    weight = temp_product.weight;
                }
                else if (shelf == 'R')
                {
                    Product temp_product = warehouse[prod_col][prod_row].RightShelf.GetProductInfo(robot_Order[0][i].productName, height);
                    weight = temp_product.weight;
                }

                while(robot_Order[0][i].quantity > 0 && this.currentWeight < (this.maxWeightCapacity - weight))
                {
                    if (shelf == 'L')
                    {
                        RetrieveProduct(robot_Order[0][0].productName, warehouse[prod_col][prod_row].LeftShelf, height);
                        robot_Order[0][i].quantity--;
                    }
                    else if (shelf == 'R')
                    {
                        RetrieveProduct(robot_Order[0][0].productName, warehouse[prod_col][prod_row].RightShelf, height);
                        robot_Order[0][i].quantity--;
                    }
                }

                if(this.currentWeight > 0.8 * maxWeightCapacity)
                {
                    MoveRobot(dock_location, warehouse);
                    while(this.robotProducts.Count > 0)
                    {
                        //LoadTruck();

                        //For DEBUGGING the collect order!
                        while(this.robotProducts.Count > 0)
                            this.robotProducts.RemoveAt(0);
                    }
                }

                if (robot_Order[0][0].quantity == 0) //else go back to top and continue grabbing the same product.
                {
                    robot_Order[0].RemoveAt(0);
                } 
                
            }

            //All items grabbed by robot by this point return remaining items to loading dock.
            MoveRobot(dock_location, warehouse);
            while (this.robotProducts.Count > 0)
            {
                //LoadTruck();

                //For DEBUGGING the collect order!
                while (this.robotProducts.Count > 0)
                    this.robotProducts.RemoveAt(0);
            }
            robot_Order.RemoveAt(0); //Remove completed order from robot
            return;
        }
    }
}
