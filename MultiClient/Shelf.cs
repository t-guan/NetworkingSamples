using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Amazoom
{
    public class Shelf
    {
        public List<ShelfSpace> shelf_spaces;   // List of shelf spaces
        public int height;                      // Height of shelf

        public Shelf() //No Argument constructor for using the methods
        { }

        public Shelf(char[] items, int height) // Constructor
        {

        }

        public Shelf(int _height) // Constructor for empty shelf_spaces
        {
            this.height = _height;
            this.shelf_spaces = new List<ShelfSpace>();

            // Insert 'height' amount of ShelfSpace objects
            for(int i = 0; i < height; i++)
            {
                this.shelf_spaces.Add(new ShelfSpace());
            }
        }

        /**
         *  Find index in ShelfSpace.shelfSpaceProducts where _product is found within the specified _shelfRow
         *  Assumes product is in the ShelfSpace.shelfSpaceProducts
         *  
         *  string _product     : Name of product
         *  int _shelfRow       : Index of shelf_spaces list where _product is located
         *  return              : The index in ShelfSpace.shelfSpaceProducts where _product is found
         */
        public int FindProductIndex(string _product, int _shelfRow) 
        {
            int index = this.shelf_spaces[_shelfRow].shelfSpaceProducts.FindIndex(product => product.itemName == _product);

            return index;
        }

        /**
         *  Get a copy of the Product object with the itemName _product located in ShelfSpace.shelfSpaceProducts
         *  within the specified _shelfRow
         *  Assumes product is in the ShelfSpace.shelfSpaceProducts
         *  
         *  string _product     : Name of product
         *  int _shelfRow       : Index of shelf_spaces list where _product is located
         *  return              : A Product object with the itemName _product
         */
        public Product GetProductInfo(string _product, int _shelfRow) 
        {
            Product prod = this.shelf_spaces[_shelfRow].shelfSpaceProducts.Find(product => product.itemName == _product);

            return prod;
        }

        /**
         *  Removes the Product object with the itemName _product from the ShelfSpace in _shelfRow
         *  Assumes product is in the ShelfSpace.shelfSpaceProducts
         *  
         *  string _product     : Name of product
         *  int _shelfRow       : Index of shelf_spaces list where _product is located
         */
        public void RemoveProduct(string _product, int _shelfRow) 
        {
            Product product = GetProductInfo(_product, _shelfRow);

            this.shelf_spaces[_shelfRow].currentWeight -= product.weight;
            this.shelf_spaces[_shelfRow].shelfSpaceProducts.Remove(product);
        }

        /**
         *  Add the Product object _product to the ShelfSpace in _shelfRow
         *  Assumes that the Product object does not go over the weight capacity of the ShelfSpace
         *  
         *  Product _product     : Product object to be added
         *  int _shelfRow        : Index of shelf_spaces list of where to add product
         */
        public void AddProduct(Product _product, int _shelfRow)
        {
                        this.shelf_spaces[_shelfRow].currentWeight += _product.weight;
                        this.shelf_spaces[_shelfRow].shelfSpaceProducts.Add(_product);
        
        }
    }
}
