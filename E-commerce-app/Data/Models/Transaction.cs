using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app.Data.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public List<TransactionProduct> TransactionProducts { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
