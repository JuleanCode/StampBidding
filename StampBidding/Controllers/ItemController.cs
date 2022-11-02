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
    public class ItemController : Controller
    {
        private readonly BidingSystemContext _context;

        public ItemController(BidingSystemContext context)
        {
            _context = context;
        }

        // GET: Item
        public async Task<IActionResult> Index(int id)
        {
            if (id.ToString() == null || _context.Items == null)
            {
                return NotFound();
            }
            StampBidding.Models.BidingSystemContext db = new BidingSystemContext();
            var bidingSystemContext = _context.Items.Include(i => i.Auction).Include(i => i.Buyer).Include(i => i.Receipt).Include(i => i.Seller).Include(i => i.Status).Where(i => i.AuctionId == id);
            return View(await bidingSystemContext.ToListAsync());
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Auction)
                .Include(i => i.Buyer)
                .Include(i => i.Receipt)
                .Include(i => i.Seller)
                .Include(i => i.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Item/Create
        public IActionResult Create()
        {
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id");
            ViewData["BuyerId"] = new SelectList(_context.Users, "Uuid", "Uuid");
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id");
            ViewData["SellerId"] = new SelectList(_context.Users, "Uuid", "Uuid");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SellerId,AuctionId,StatusId,BuyerId,ReceiptId,MinPrice")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id", item.AuctionId);
            ViewData["BuyerId"] = new SelectList(_context.Users, "Uuid", "Uuid", item.BuyerId);
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id", item.ReceiptId);
            ViewData["SellerId"] = new SelectList(_context.Users, "Uuid", "Uuid", item.SellerId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", item.StatusId);
            return View(item);
        }

        // GET: Item/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id", item.AuctionId);
            ViewData["BuyerId"] = new SelectList(_context.Users, "Uuid", "Uuid", item.BuyerId);
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id", item.ReceiptId);
            ViewData["SellerId"] = new SelectList(_context.Users, "Uuid", "Uuid", item.SellerId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", item.StatusId);
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SellerId,AuctionId,StatusId,BuyerId,ReceiptId,MinPrice")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id", item.AuctionId);
            ViewData["BuyerId"] = new SelectList(_context.Users, "Uuid", "Uuid", item.BuyerId);
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id", item.ReceiptId);
            ViewData["SellerId"] = new SelectList(_context.Users, "Uuid", "Uuid", item.SellerId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", item.StatusId);
            return View(item);
        }

        // GET: Item/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Auction)
                .Include(i => i.Buyer)
                .Include(i => i.Receipt)
                .Include(i => i.Seller)
                .Include(i => i.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'BidingSystemContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return _context.Items.Any(e => e.Id == id);
        }
    }
}
