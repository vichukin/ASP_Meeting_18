using ASP_Meeting_18.Models.Domain;

namespace ASP_Meeting_18.Models.ViewModels.CartViewModels
{
    public class CartIndexViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public string? returnUrl { get; set; }
    }
}
