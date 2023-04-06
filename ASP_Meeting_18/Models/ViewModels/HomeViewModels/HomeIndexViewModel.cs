using ASP_Meeting_18.Data;

namespace ASP_Meeting_18.Models.ViewModels.HomeViewModels
{
	public class HomeIndexViewModel
	{
		public List<Product> Products { get; set; }
		public string? Category { get; set; }
		public int Page { get; set; }
		public int PageCount { get; set; }
		public PriceSearchViewModel PriceSearch {get;set;} 
	}
}
