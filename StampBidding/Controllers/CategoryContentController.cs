using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StampBidding.Models;

namespace StampBidding.Controllers
{
    public class CategoryContentController : Controller
    {
        private readonly BidingSystemContext _context;

        public CategoryContentController(BidingSystemContext context)
        {
            _context = context;
        }

        // GET: CategoryContent
        public async Task<IActionResult> Index()
        {
            var bidingSystemContext = _context.CategoryContents.Include(c => c.Category);
            return View(await bidingSystemContext.ToListAsync());
        }

        // GET: CategoryContent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryContents == null)
            {
                return NotFound();
            }

            var categoryContent = await _context.CategoryContents
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryContent == null)
            {
                return NotFound();
            }

            return View(categoryContent);
        }

        // GET: CategoryContent/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: CategoryContent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Text")] CategoryContent categoryContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", categoryContent.CategoryId);
            return View(categoryContent);
        }

        // GET: CategoryContent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryContents == null)
            {
                return NotFound();
            }

            var categoryContent = await _context.CategoryContents.FindAsync(id);
            if (categoryContent == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", categoryContent.CategoryId);
            return View(categoryContent);
        }

        // POST: CategoryContent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Text")] CategoryContent categoryContent)
        {
            if (id != categoryContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryContentExists(categoryContent.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", categoryContent.CategoryId);
            return View(categoryContent);
        }

        // GET: CategoryContent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryContents == null)
            {
                return NotFound();
            }

            var categoryContent = await _context.CategoryContents
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryContent == null)
            {
                return NotFound();
            }

            return View(categoryContent);
        }

        // POST: CategoryContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryContents == null)
            {
                return Problem("Entity set 'BidingSystemContext.CategoryContents'  is null.");
            }
            var categoryContent = await _context.CategoryContents.FindAsync(id);
            if (categoryContent != null)
            {
                _context.CategoryContents.Remove(categoryContent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryContentExists(int id)
        {
          return _context.CategoryContents.Any(e => e.Id == id);
        }
    }
}
