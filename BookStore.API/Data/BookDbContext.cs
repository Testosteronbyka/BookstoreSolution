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
                // Mapowanie do istniejącej tabeli "books" (małe litery)
                entity.ToTable("books");
                
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("title");
                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("author");
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("price");
                entity.Property(e => e.Stock)
                    .HasColumnName("stock");
                entity.Property(e => e.Description)
                    .HasColumnName("description");
                entity.Property(e => e.ISBN)
                    .HasColumnName("isbn");
                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");
            });
        }
    }
}