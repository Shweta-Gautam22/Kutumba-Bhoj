using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutumbaBhoj.Domain.Models
{
    public class Dish
    {
        [Key]
        public int DishId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public struct Rating
        {
            public int Lower { get; set; }
            public int Upper { get; set; }
            public int FinalRating { get; set; }
        }

    }
}
