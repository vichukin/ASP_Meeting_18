using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.ViewModels.ProductsViewModels;

namespace ASP_Meeting_18.Controllers.Admin
{
    public class ProductsController : Controller
    {
        private readonly ShopDBContext _context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

        public ProductsController(ShopDBContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            Environment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var shopDBContext = _context.Products.Include(p => p.Category).Include(t=>t.Photos);
            return View(await shopDBContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(t => t.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel vm)
        {
            if (ModelState.IsValid)
            {
                List<Photo> photos = new List<Photo>();

                vm.Product.Photos = photos;

                _context.Add(vm.Product);
                await _context.SaveChangesAsync();
                int count = 1;
                if (vm.Photos != null)
                {
                    //Закончить сохранение файла
                    foreach (var item in vm.Photos)
                    {
                        string filename = $"/images/{item.FileName}";
                        filename = $"/images/{Path.GetFileNameWithoutExtension(filename)}" + $"{Guid.NewGuid()}.{Path.GetExtension(filename)}";
                        Photo ph = new Photo() { PhotoUrl = filename, Filename = item.FileName, Product = vm.Product, ProductId = vm.Product.Id };
                        using (var file = new FileStream(Environment.WebRootPath + filename, FileMode.Create))
                        {
                            item.CopyTo(file);
                            //ph.PhotoUrl = item.Name;
                        }
                        //try
                        //{


                        //    using (var file = new FileStream(ph.PhotoUrl, FileMode.CreateNew))
                        //    {
                        //        item.CopyTo(file);
                        //        //ph.PhotoUrl = item.Name;
                        //    }
                        //}
                        //catch(IOException ex)
                        //{
                        //    using (var file = new FileStream(ph.PhotoUrl+$"{count}", FileMode.CreateNew))
                        //    {
                        //        item.CopyTo(file);
                        //        ph.PhotoUrl = item.Name;
                        //    }
                        //}
                        photos.Add(ph);
                        count++;
                    }
                }
                _context.Photos.AddRange(photos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(_context.Categories, "Id", "Title", vm.Product.Title);
            return View(vm);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,Count,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ShopDBContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
