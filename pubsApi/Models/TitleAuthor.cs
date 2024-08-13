using pubsApi.Models;

namespace PubsApi.Models
{
    public class TitleAuthor
    {
        public string Au_id { get; set; }  // Foreign key to Author
        public string Title_id { get; set; }  // Foreign key to Book
        public byte? Au_ord { get; set; }
        public int? Royaltyper { get; set; }

        // Navigation properties
        public Author Author { get; set; }
        public Title Title { get; set; }
    }
}
