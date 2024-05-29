using System.ComponentModel.DataAnnotations.Schema;

namespace E_Shop.Models
{
    [Table("Stock")]
    public class Stock
    {
        public int Id { get; set; }
        public int ClothingId { get; set; }
        public int Quantity { get; set; }

        public Clothing? Clothing { get; set; }
    }
}