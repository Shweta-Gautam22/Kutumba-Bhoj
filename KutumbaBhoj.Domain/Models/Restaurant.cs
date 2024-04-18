using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutumbaBhoj.Domain.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string Logo { get; set; }

        [NotMapped]
        public IFormFile? LogoFile { get; set; }



    }
}
