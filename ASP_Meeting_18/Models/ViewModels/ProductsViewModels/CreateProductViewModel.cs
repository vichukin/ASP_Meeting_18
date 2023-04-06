using ASP_Meeting_18.Data;
using Microsoft.Extensions.FileProviders;

namespace ASP_Meeting_18.Models.ViewModels.ProductsViewModels
{
    public class CreateProductViewModel
    {
        public Product Product { get; set; } = default!;
        public IEnumerable<IFormFile>? Photos { get; set; }

    }
}
