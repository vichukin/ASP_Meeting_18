using ASP_Meeting_18.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_Meeting_18.Models.Domain
{
    public class Cart
    {
        List<CartItem> cartItems= new List<CartItem>();
        public IEnumerable<CartItem> CartItems => cartItems;
        public Cart(ShopDBContext context, string username)
        {
            var b = context.CartItems.Include(t=>t.Product).Include(t=>t.User).Where(t => t.Username == username).ToList();
            cartItems = context.CartItems.Where(t=>t.Username==username).ToList();
        }
        public Cart(IEnumerable<CartItem> cartItems)
        {
            this.cartItems = cartItems.ToList();
        }
        public void AddToCart( Product product, int count)
        {
            CartItem? item = cartItems.FirstOrDefault(t=>t.Product.Id == product.Id);
            if(item != null)
            {
                item.Count += count;
            }
            else
                cartItems.Add(new CartItem() { Product= product, Count = count });

        }
        public void RemoveFromCart(Product product )
        {
            //CartItem item = cartItems.FirstOrDefault(t.Product.Id == product.Id);
            cartItems.RemoveAll(t=>t.Product.Id==product.Id);
        }
        public double TotalPrice => cartItems.Sum(t => t.Product.Price * t.Count);
    }
}
