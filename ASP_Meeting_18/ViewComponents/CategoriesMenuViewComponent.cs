using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_Meeting_18.ViewComponents
{
    [ViewComponent]
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly ShopDBContext context;
       public CategoriesMenuViewComponent(ShopDBContext context) 
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string CurrentCategory, PriceSearchViewModel Prices)
        {
            IQueryable<Category> categories= this.context.Categories;
            List<string> categoryNames = context.Products.Include(t=>t.Category).Select(t=>t.Category.Title).Distinct().ToList();
            return View(new Tuple<List<string>, string?,PriceSearchViewModel>(categoryNames, CurrentCategory,Prices));
        }
    }
}
