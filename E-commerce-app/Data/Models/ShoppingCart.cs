using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app.Data.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        public List<ShoppingCartProduct> ShoppingCartProducts { get; set; }

        public string UserId { get; set;  }
        public AppUser User { get; set; }
        public ShoppingCart()
        {
            ShoppingCartProducts = new List<ShoppingCartProduct>();
        }
    }
}
