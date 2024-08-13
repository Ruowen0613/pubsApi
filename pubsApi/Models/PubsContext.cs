using Microsoft.EntityFrameworkCore;
using PubsApi.Models;

namespace pubsApi.Models
{
    public class PubsContext: DbContext
    {
        public PubsContext(DbContextOptions<PubsContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Title> Titles { get; set; } = null!;
        public DbSet<TitleAuthor> TitleAuthors { get; set; } = null!;
        public DbSet<Sale> Sales { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define composite key for TitleAuthor
            modelBuilder.Entity<TitleAuthor>()
                .HasKey(ta => new { ta.Au_id, ta.Title_id });
            modelBuilder.Entity<TitleAuthor>()
            .ToTable("titleauthor"); // Specify the actual table name in the database

            // Other configurations can go here
            modelBuilder.Entity<TitleAuthor>()
                .HasOne(ta => ta.Author)
                .WithMany(a => a.TitleAuthors)
                .HasForeignKey(ta => ta.Au_id);

            modelBuilder.Entity<TitleAuthor>()
                .HasOne(ta => ta.Title)
                .WithMany(t => t.TitleAuthors)
                .HasForeignKey(ta => ta.Title_id);

            modelBuilder.Entity<Sale>()
                .HasKey(s => new { s.Stor_id, s.Ord_num, s.Title_id });
        }

    }
}
