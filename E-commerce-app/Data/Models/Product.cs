using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_app.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string Image { get; set; }




        public List<ProductCategory> Categories { get; set; }
        public List<Review> Reviews { get; set; }
        public List<ShoppingCartProduct> ShoppingCartProducts { get; set; }
        public List<TransactionProduct> TransactionProducts { get; set; }

        public Product()
        {
            Categories = new List<ProductCategory>();
            Reviews = new List<Review>();
            ShoppingCartProducts = new List<ShoppingCartProduct>();
            TransactionProducts = new List<TransactionProduct>();
        }

        
    }
}