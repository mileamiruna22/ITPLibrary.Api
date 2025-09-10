namespace ITPLibrary.Api.Core.Dtos
{
    public class OrderItemDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
    }
}