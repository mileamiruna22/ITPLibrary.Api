using ITPLibrary.Api.Data.Entities;

namespace ITPLibrary.Api.Data.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}