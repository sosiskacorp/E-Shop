namespace E_Shop.Models.DTOs
{
    public class StockDisplayModel
    {
        public int Id { get; set; }
        public int ClothingId { get; set; }
        public int Quantity { get; set; }
        public string? ClothingName { get; set; }
    }
}