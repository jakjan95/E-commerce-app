using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_app.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ProductCategory> Categories { get; set; }

        public Product()
        {
            Categories = new List<ProductCategory>();
        }


    }
}