using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app.Data.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string Comment { get; set; }

        [Range(1,5)]
        public int Rating { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
