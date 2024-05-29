using System.ComponentModel.DataAnnotations;

namespace E_Shop.Models.DTOs
{
    public class StockDTO
    {
        public int ClothingId { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public int Quantity { get; set; }
    }
}