using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Shop.Models
{
        [Table("Clothing")]
        public class Clothing
        {
            public int Id { get; set; }

            [Required]
            [MaxLength(40)]
            public string? Name { get; set; }

            [Required]
            [MaxLength(40)]
            public string? Brand { get; set; }

            [Required]
            public double Price { get; set; }

            public string? Image { get; set; }

            [Required]
            public int CategoryId { get; set; }

            public Category Category { get; set; }

            public List<OrderDetail> OrderDetail { get; set; }
            public List<CartDetail> CartDetail { get; set; }

            [NotMapped]
            public string CategoryName { get; set; }
        }
    }


