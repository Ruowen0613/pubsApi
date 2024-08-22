using PubsApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;



namespace PubsApi.Models
{
    public class Title
    {
        [Key]
        public string title_id { get; set; }  // Primary key
        public string title { get; set; }
        public string type { get; set; }
        public string? pub_id { get; set; }  // Foreign key to Publisher
        public decimal? price { get; set; }
        public decimal? advance { get; set; }
        public int? royalty { get; set; }
        public int? ytd_sales { get; set; }
        public string? notes { get; set; }
        public DateTime pubdate { get; set; }

        // Navigation property
        //[JsonIgnore]
        public ICollection<TitleAuthor> TitleAuthors { get; set; }
    }
}
