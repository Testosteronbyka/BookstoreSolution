namespace Bookstore.Web.Models
{
    public class CartItem
    {
        public Book Book { get; set; } = new Book();
        public int Quantity { get; set; }
        public decimal TotalPrice => Book.Price * Quantity;
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; } = "";
        public string ISBN { get; set; } = "";
        public DateTime CreatedDate { get; set; }
    }
}