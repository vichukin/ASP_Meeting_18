using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models;
using ASP_Meeting_18.Models.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ASP_Meeting_18.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopDBContext context;

        public HomeController(ILogger<HomeController> logger, ShopDBContext context)
        {
            _logger = logger;
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? category,int? Min,int? Max, int page = 1)
        {
                IQueryable<Product> products = context.Products.Include(t => t.Category).Include(t => t.Photos);
                double maxprice = products.Max(t => t.Price);
                if (category != null&&category.ToString()!="All")
                    products = products.Where(t => t.Category!.Title == category);
                if (Min != null)
                    products = products.Where(t => t.Price >= Min);
                if (Max != null)
                    products = products.Where(t => t.Price <= Max);
                int itemsPerPage = 4;
                int pagecount = (int)(Math.Ceiling((decimal)products.Count() / itemsPerPage));
                products = products.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
                PriceSearchViewModel pvm = new PriceSearchViewModel()
                {
                    Max = (int)maxprice,
                    SelectedMax = Max,
                    SelectedMin = Min
                };
                HomeIndexViewModel vm = new HomeIndexViewModel
                {
                    Products = await products.ToListAsync(),
                    Category = category,
                    Page = page,
                    PageCount = pagecount,
                    PriceSearch = pvm
                };
                return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}