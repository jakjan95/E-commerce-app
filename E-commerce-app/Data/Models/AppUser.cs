using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app.Data.Models
{
    public class AppUser : IdentityUser
    {

        public List<Review> Reviews { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public List<Transaction> Transactions { get; set; }
        
        
    }
}
