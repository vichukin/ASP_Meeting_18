using ASP_Meeting_18.Data;
using ASP_Meeting_18.Extentions;
using ASP_Meeting_18.Models.Domain;
using ASP_Meeting_18.Models.Services;
using ASP_Meeting_18.Models.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ASP_Meeting_18.Controllers
{
    public class CartController : Controller
    {
        public ShopDBContext context { get; set; }
        public UserManager<User> UserManager { get; set; }
        public EmailService email { get; set; }
        public CartController(ShopDBContext context, UserManager<User> userManager, EmailService email)
        {

            UserManager = userManager;
            this.context = context;
            this.email = email;

        }
        //public IActionResult Index(string? returnUrl)
        public IActionResult Index(string? returnUrl)
        {
            //Cart cart = GetCart();

            if (returnUrl == null)
                returnUrl = Url.Action("Index", "home");
            CartIndexViewModel vm = new CartIndexViewModel()
            {
                CartItems = context.CartItems.Include(t => t.Product).Include(t => t.User).Where(t => t.Username == User.Identity.Name).ToList(),
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
        public async Task<IActionResult> AddToCart(int id, string? returnUrl)
        {
            //Cart cart = new Cart(context,User.Identity.Name);
            Product? product = await context.Products.FindAsync(id);
            CartItem item = new CartItem()
            {
                Product = product,
                ProductId = id,
                Count = 1,
                Username = User.Identity.Name
            };
            if (product != null)
            {
                context.CartItems.Add(item);
                context.SaveChanges();
                //Cart cart = new Cart(context, User.Identity.Name);
                //HttpContext.Session.Set("cart", cart.CartItems);
                //email.Send("artuoh76@gmail.com", context.Users.FirstOrDefault(t => t.UserName == User.Identity.Name).Email, "Darova", "Privet");
            }
            CartIndexViewModel vm = new CartIndexViewModel
            {
                CartItems = context.CartItems.ToList(),
                returnUrl = returnUrl
            };
            return RedirectToAction("Index", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteFromCart(int id, string? returnUrl)
        public async Task<IActionResult> DeleteFromCart(int id, string? returnUrl)
        {
            //Cart cart = GetCart();
            //Cart cart = new Cart(context, User.Identity.Name);
            Product? product = await context.Products.FindAsync(id);
            if (product != null)
            {
                //cart.RemoveFromCart(product);
                //HttpContext.Session.Set("cart", cart.CartItems);
            }
            return RedirectToAction("Index", returnUrl);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteFromCart(int id, string? returnUrl)
        public async Task<IActionResult> BuyProduct(int id,int cartitemId, string? returnUrl)
        {
            //Cart cart = GetCart();
            //Cart cart = new Cart(context, User.Identity.Name);
            Product? product = context.Products.Include(t=>t.Category).FirstOrDefault(t=>t.Id==id);
            CartItem item = await context.CartItems.Include(t=>t.Product).FirstOrDefaultAsync(t=>t.Id==cartitemId);
            StringBuilder sb = new StringBuilder();
           string str = ("<h2>Congratulate you with your purchase</h2>"+product.ToString()+ "<br>Total price: "+ (item.Product.Price * item.Count).ToString()+"$");
            email.Send("artuoh76@gmail.com", context.Users.FirstOrDefault(t => t.UserName == User.Identity.Name).Email, $"{product.Title}", str);
            if (product != null)
            {
                product.Count--;
                context.Update(product);
                context.CartItems.Remove(item);
                context.SaveChanges();
                
                //cart.RemoveFromCart(product);
                //HttpContext.Session.Set("cart", cart.CartItems);
            }
            //return RedirectToAction(actionName: "Successfulpurchase", controllerName:"Cart",routeValues: returnUrl.ToString());
            return View("Successfulpurchase",returnUrl);
        }
        //public Cart GetCart()
        //{
        //    IEnumerable<CartItem>? cartItems = HttpContext
        //.Session.Get<IEnumerable<CartItem>>("cart");
        //    Cart? cart = null;
        //    if (cartItems == null)
        //    {
        //        cart = new Cart();
        //    }
        //    else cart = new Cart(cartItems!);
        //    return cart;
        //}
    }
}
