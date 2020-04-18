using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app.Data.Models
{
    public class ShoppingCartProduct
    {
        [Key]
        public int Id { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
