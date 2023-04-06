using ASP_Meeting_18.Extentions;
using ASP_Meeting_18.Models.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASP_Meeting_18.Infrastructure.ModelBinders
{
    public class CartModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException();
            string sessionKey = "cart";
            Cart? cart = null;
            IEnumerable<CartItem>? cartItems = null;
            if (bindingContext.HttpContext.Session != null)
            {
                cartItems = bindingContext.HttpContext
                .Session.Get<IEnumerable<CartItem>>("cart");

            }
           if(cartItems== null)
            {
                cartItems=new List<CartItem>();
                bindingContext.HttpContext.Session.Set("cart", cartItems);
            }
            cart = new Cart(cartItems);
            bindingContext.Result = ModelBindingResult.Success(cart);
            return Task.CompletedTask;
        }
    }
}
