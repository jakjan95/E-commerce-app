using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace E_commerce_app.Data.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        public string Address { get; set; }

        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }
        public List<Review> Reviews { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}