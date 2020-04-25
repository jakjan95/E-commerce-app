using E_commerce_app.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_app.Dtos
{
    public class ProductCreateDto
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string Image { get; set; }

        public List<Category> Categories { get; set; }
        public List<Category> SelectedCategories { get; set; }
       

        public ProductCreateDto()
        {
            Categories = new List<Category>();
            SelectedCategories = new List<Category>();
        }
    }
}