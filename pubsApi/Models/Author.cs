using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace PubsApi.Models
{
    public class Author
    {
        [Key]
        public string Au_id { get; set; }
        public string Au_lname { get; set; }
        public string Au_fname { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public bool Contract { get; set; }

        // Navigation property
       // [JsonIgnore]
        public ICollection<TitleAuthor> TitleAuthors { get; set; }
    }
}
