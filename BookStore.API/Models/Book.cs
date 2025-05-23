using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.API.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = "";
        
        [Required]
        public string Author { get; set; } = "";
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public int Stock { get; set; }
        
        public string Description { get; set; } = "";
        
        public string ISBN { get; set; } = "";
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}