using ASP_Meeting_18.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ASP_Meeting_18.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category? Category { get; set; } = default!;
        public List<Photo>? Photos { get; set; } 
        public List<CartItem>? CartItems { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Title: {Title}<br>");
            sb.AppendLine($"Category: {Category.Title}<br>");
            sb.AppendLine($"Price: {Price}$");
            return sb.ToString();
        }
    }
}
