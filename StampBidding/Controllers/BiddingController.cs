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
    public class BiddingController : Controller
    {
        private readonly BidingSystemContext _context;

        public BiddingController(BidingSystemContext context)
        {
            _context = context;
        }

        // GET: Bidding
        public async Task<IActionResult> Index()
        {
            var bidingSystemContext = _context.Biddings.Include(b => b.Item).Include(b => b.User);
            return View(await bidingSystemContext.ToListAsync());
        }

        // GET: Bidding/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Biddings == null)
            {
                return NotFound();
            }

            var bidding = await _context.Biddings
                .Include(b => b.Item)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bidding == null)
            {
                return NotFound();
            }

            return View(bidding);
        }

        // GET: Bidding/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Uuid", "Uuid");
            return View();
        }

        // POST: Bidding/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,Date,Price,UserId")] Bidding bidding)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bidding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", bidding.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "Uuid", "Uuid", bidding.UserId);
            return View(bidding);
        }

        // GET: Bidding/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Biddings == null)
            {
                return NotFound();
            }

            var bidding = await _context.Biddings.FindAsync(id);
            if (bidding == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", bidding.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "Uuid", "Uuid", bidding.UserId);
            return View(bidding);
        }

        // POST: Bidding/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,Date,Price,UserId")] Bidding bidding)
        {
            if (id != bidding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bidding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BiddingExists(bidding.Id))
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
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", bidding.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "Uuid", "Uuid", bidding.UserId);
            return View(bidding);
        }

        // GET: Bidding/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Biddings == null)
            {
                return NotFound();
            }

            var bidding = await _context.Biddings
                .Include(b => b.Item)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bidding == null)
            {
                return NotFound();
            }

            return View(bidding);
        }

        // POST: Bidding/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Biddings == null)
            {
                return Problem("Entity set 'BidingSystemContext.Biddings'  is null.");
            }
            var bidding = await _context.Biddings.FindAsync(id);
            if (bidding != null)
            {
                _context.Biddings.Remove(bidding);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BiddingExists(int id)
        {
          return _context.Biddings.Any(e => e.Id == id);
        }
    }
}
