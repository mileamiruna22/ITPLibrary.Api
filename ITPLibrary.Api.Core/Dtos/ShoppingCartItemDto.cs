namespace ITPLibrary.Api.Core.Dtos
{
    public class ShoppingCartItemDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Author { get; set; }
        public string Thumbnail { get; set; }
    }
}