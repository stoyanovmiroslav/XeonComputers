using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XeonComputers.Areas.Administrator.ViewModels.Products;
using XeonComputers.Data;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class ProductsController : Controller
    {
        private readonly XeonDbContext _context;

        public ProductsController(XeonDbContext context)
        {
            _context = context;
        }

        public IActionResult All()
        {
            var xeonDbContext = _context.Products.Include(p => p.ChildCategory).ToList();

            return View(xeonDbContext);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                                .Include(p => p.ChildCategory)
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new DetailsProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ChildCategoryName = product.ChildCategory.Name,
                Price = product.Price,
                ParnersPrice = product.Price,
                ProductType = product.ProductType,
                Specification = product.Specification
            };

            return View(productViewModel);
        }

        public IActionResult Create()
        {
            var categories = _context.ChildCategories.Select(x => new SelectListItem
                             {
                                Value = x.Id.ToString(),
                                Text = x.Name
                             }).ToList();

            var model = new CreateProductViewModel { ChildCategories = categories };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ProductType,Description,Specification,Price,ParnersPrice,ChildCategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(All));
            }
            ViewData["ChildCategoryId"] = new SelectList(_context.ChildCategories, "Id", "Id", product.ChildCategoryId);
            return View(product);
        }

        // GET: Administrator/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ChildCategoryId"] = new SelectList(_context.ChildCategories, "Id", "Id", product.ChildCategoryId);
            return View(product);
        }

        // POST: Administrator/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProductType,Description,Specification,Price,ParnersPrice,ChildCategoryId")] Product product)
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
                return RedirectToAction(nameof(All));
            }
            ViewData["ChildCategoryId"] = new SelectList(_context.ChildCategories, "Id", "Id", product.ChildCategoryId);
            return View(product);
        }

        // GET: Administrator/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ChildCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Administrator/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
