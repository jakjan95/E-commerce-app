using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app.Data.Models
{
    public class TransactionProduct
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Transaction Transaction { get; set; }
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
