using ASP_Meeting_18.Data;
using ASP_Meeting_18.Extentions;
using ASP_Meeting_18.Models.Domain;
using ASP_Meeting_18.Models.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Meeting_18.Controllers.Admin
{
    public class CartController : Controller
    {
        public ShopDBContext context { get; set; }
        public CartController(ShopDBContext context)
        {


            this.context = context;

        }
        //public IActionResult Index(string? returnUrl)
        public IActionResult Index(Cart cart, string? returnUrl)
        {
            //Cart cart = GetCart();
            if (returnUrl == null)
                returnUrl = Url.Action("Index", "home");
            CartIndexViewModel vm = new CartIndexViewModel()
            {
                Cart = cart,
                returnUrl = returnUrl
            };

            return View(vm);
        }
        //public async Task<IActionResult> AddtoCart(int id) 
        //{
        //    Cart cart = GetCart();
        //    Product product= await context.Products.FindAsync(id);
        //    if(product != null)
        //    {
        //        cart.AddToCart(product, 1);
        //        HttpContext.Session.Set<Cart>("cart", cart);
        //    }
        //    return RedirectToAction("Index");
        //}

        //public Cart GetCart()
        //{
        //    IEnumerable<CartItem>? cartItems = HttpContext.Session.Get<IEnumerable<CartItem>>("cart");

        //    Cart cart = null;
        //    if(cartItems==null)
        //    {
        //            cart= new Cart();
        //    }
        //    else
        //        cart = new Cart(cartItems);

        //    return cart;
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddToCart(int id, string? returnUrl)
        public async Task<IActionResult> AddToCart(Cart cart, int id, string? returnUrl)
        {
            //Cart cart = GetCart();
            Product? product = await context.Products.FindAsync(id);
            if (product != null)
            {
                cart.AddToCart(product, 1);
                HttpContext.Session.Set("cart", cart.CartItems);
            }
            return RedirectToAction("Index", returnUrl);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteFromCart(int id, string? returnUrl)
        public async Task<IActionResult> DeleteFromCart(Cart cart, int id, string? returnUrl)
        {
            //Cart cart = GetCart();
            Product? product = await context.Products.FindAsync(id);
            if (product != null)
            {
                cart.RemoveFromCart(product);
                HttpContext.Session.Set("cart", cart.CartItems);
            }
            return RedirectToAction("Index", returnUrl);
        }
        public Cart GetCart()
        {
            IEnumerable<CartItem>? cartItems = HttpContext
        .Session.Get<IEnumerable<CartItem>>("cart");
            Cart? cart = null;
            if (cartItems == null)
            {
                cart = new Cart();
            }
            else cart = new Cart(cartItems!);
            return cart;
        }
    }
}
