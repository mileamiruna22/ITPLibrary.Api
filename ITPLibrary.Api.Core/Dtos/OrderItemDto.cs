namespace ITPLibrary.Api.Core.Dtos
{
    public class OrderItemDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; } 
        public decimal PricePerUnit { get; set; }
    }
}