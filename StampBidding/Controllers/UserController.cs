

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StampBidding.Models;

using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net;

namespace StampBidding.Controllers
{
    public class UserController : Controller
    {
        private readonly BidingSystemContext _context;

        public UserController(BidingSystemContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var bidingSystemContext = _context.Users.Include(u => u.Role);
            return View(await bidingSystemContext.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Uuid == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Uuid,RoleId,MemberId,Firstname,Lastname,Email,Password,Country,Provence,City,PostalCode,Street,Housenumber,AddressSuffix,PhoneNumber")] User user)
        {
            if (true)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Uuid,RoleId,MemberId,Firstname,Lastname,Email,Password,Country,Provence,City,PostalCode,Street,Housenumber,AddressSuffix,PhoneNumber")] User user)
        {
            if (id != user.Uuid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Uuid))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Uuid == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'BidingSystemContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Receipt(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            List<Bidding> bidding =  _context.Biddings.Where(b => b.UserId == user.Uuid).ToList();
            List<Bidding> highestBid = new List<Bidding>();
            if (bidding == null)
            {
                return NotFound();
            }
            else
            {
                foreach (var bid in bidding)
                {
                    var higher = _context.Biddings.Where(b => b.ItemId == bid.ItemId).ToList();
                    if (higher == null)
                    {
                        highestBid.Add(bid);
                    }
                    else {
                        foreach (var item in higher)
                        {
                            if (Int16.Parse(bid.Price) > Int16.Parse(item.Price))
                            {
                                highestBid.Add(bid);
                            }
                        }
                    }
                }
            }
            
            Document pdfDocument = new Document();
            PdfWriter.GetInstance(pdfDocument,
                      new FileStream("C:\\Users/" + Environment.UserName + "/Desktop/Receipt.PDF", FileMode.Create));
            pdfDocument.Open();
            pdfDocument.Add(new Paragraph("Name: " + user.Firstname + " " + user.Lastname));
            pdfDocument.Add(new Paragraph("ID: " + user.MemberId));

            PdfPTable table = new PdfPTable(2);
            table.AddCell("Item ID");
            table.AddCell("Price");

            var totalPrice = 0;

            foreach (var bid in highestBid)
            {
                table.AddCell(bid.ItemId.ToString());
                table.AddCell(bid.Price);

                totalPrice += Int16.Parse(bid.Price);
            }
            pdfDocument.Add(table);

            pdfDocument.Add(new Paragraph("Total price: " + totalPrice));

            pdfDocument.Close();

            return RedirectToAction(nameof(Index));
        }
        
        private bool UserExists(string id)
        {
          return _context.Users.Any(e => e.Uuid == id);
        }
    }
}
