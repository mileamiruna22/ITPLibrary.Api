using ITPLibrary.Api.Data.Entities;

namespace ITPLibrary.Api.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; } 
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; } 
        public decimal PricePerUnit { get; set; } 
        public Book Book { get; set; }
    }
}