namespace pubsApi.Models
{
    public class BookCreateDto
    {
        public string Title_id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string? Pub_id { get; set; }
        public decimal? Price { get; set; }
        public decimal? Advance { get; set; }
        public int? Royalty { get; set; }
        public int? Ytd_sales { get; set; }
        public string? Notes { get; set; }
        public DateTime Pubdate { get; set; }
        public List<AuthorDto> Authors { get; set; }  // Authors to be associated with the book
    }

}
