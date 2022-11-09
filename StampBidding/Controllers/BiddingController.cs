using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Web;
using StampBidding.Models;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;

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
            return View();
        }

        // POST: Bidding/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file)
        {
            if (file.Length > 0)
            {
                List<string> outputtext = new List<string>();
                var filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using(PdfReader reader = new PdfReader(filePath))
                {
                    for (int page = 1;  page <= reader.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string text = PdfTextExtractor.GetTextFromPage(reader, page, strategy);
                        text = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
                        outputtext.Add(text.ToString());
                    }
                }
                System.IO.File.Delete(filePath);
                List<int> bids = new List<int>();
                List<int> lots = new List<int>();
                bool loop = false;
                foreach(var text in outputtext)
                {
                    if (loop)
                    {
                        var splitted = text.Split("\nADDITIONAL AND ALTERNATIVE BIDS \nAddit");
                    }
                    else if(text.Contains("\nBLANK \nLOT \nNo \n \nBID LEAVE \nBLANK \n£"))
                    {
                        List<string> splitted = text.Split("£ p £ p £ \n \np \n").ToList();
                        splitted.Remove(splitted[0]);
                        List<string> splittedsecond = splitted[0].Split("       \nName \nAddress \nPostcode \nTelephone \nE-mail \nFor Auctioneer’s use  \nMembership Number \nI have read and agree to abide by the rules of the  \nModern British Philatelic Circle which govern  \nSignature  .............").ToList();
                        splittedsecond.Remove(splittedsecond[1]);
                        List<string> numbers = splittedsecond[0].Split(" ").ToList();
                        var splittednumbers = numbers[0].Split(" ");
                        loop = true;
                    }
                }
            }
            return View();
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
