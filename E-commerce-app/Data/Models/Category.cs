using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ProductCategory> Products { get; set; }

        public Category()
        {
            Products = new List<ProductCategory>();
        }
    }
}
