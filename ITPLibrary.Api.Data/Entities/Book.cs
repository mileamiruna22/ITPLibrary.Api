namespace ITPLibrary.Api.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }

        public string ShortDescription { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPromoted { get; set; }

        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
        public bool Popular { get; set; }
        public bool RecentlyAdded { get; set; }

        public string LongDescription { get; set; }
        public string Image { get; set; }
    }
}