using Microsoft.EntityFrameworkCore;
using BookStore.API.Models;

namespace BookStore.API.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");
                
                // Dodanie książek
                entity.HasData(
                    new Book 
                    {
                        Id = 1,
                        Title = "Wiedźmin: Ostatnie życzenie",
                        Author = "Andrzej Sapkowski",
                        Price = 39.99m,
                        Stock = 10,
                        Description = "Pierwszy tom kultowej sagi o wiedźminie",
                        ISBN = "9788375780635",
                        CreatedDate = DateTime.Now
                    },
                    new Book 
                    {
                        Id = 2,
                        Title = "Hobbit, czyli tam i z powrotem",
                        Author = "J.R.R. Tolkien",
                        Price = 49.99m,
                        Stock = 5,
                        Description = "Klasyczna powieść fantasy",
                        ISBN = "9788324159819",
                        CreatedDate = DateTime.Now
                    },
                    new Book 
                    {
                        Id = 3,
                        Title = "1984",
                        Author = "George Orwell",
                        Price = 29.99m,
                        Stock = 8,
                        Description = "Rok 1984 w wersji orwellowskiej",
                        ISBN = "9788382022287",
                        CreatedDate = DateTime.Now
                    }
                );
            });
        }
    }
}
