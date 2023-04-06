using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Meeting_18.ViewComponents
{
	[ViewComponent]
	public class PriceSliderViewComponent : ViewComponent
	{
		private readonly ShopDBContext context;
		public PriceSliderViewComponent(ShopDBContext context)
		{
			this.context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync(PriceSearchViewModel Prices)
		{
			return View(Prices);
		}
	}
}
