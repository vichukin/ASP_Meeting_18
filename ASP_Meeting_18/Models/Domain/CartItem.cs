using ASP_Meeting_18.Data;

namespace ASP_Meeting_18.Models.Domain
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Count { get; set; }
        public string Username { get; set; }
        public User? User { get; set; }
    }
}
