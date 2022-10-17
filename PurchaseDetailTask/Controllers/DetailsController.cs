using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PurchaseDetailTask.Models;

namespace PurchaseDetailTask.Controllers
{
    public class DetailsController : Controller
    {
        private readonly DataContext _context;

        public DetailsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Details.Include(d => d.Sells);
            return View(await dataContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Details == null)
            {
                return NotFound();
            }

            var details = await _context.Details
                .Include(d => d.Sells)
                .FirstOrDefaultAsync(m => m.DetailsId == id);
            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }

        public IActionResult Create()
        {
            ViewData["SellID"] = new SelectList(_context.Sells, "SellID", "SellID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Details details)
        {
            decimal total = 0;
            //loop start
            total += details.Total;
            Sell sells = new Sell();
            sells.InvoiceNo = "H001";
            sells.TotalPurchase = details.PurchasePrice;
            sells.TotalPrice = details.SellPrice;
            sells.TotalDiscount = details.Discount;
            sells.TotalProfit = total;
            _context.Add(sells);
            await _context.SaveChangesAsync();

            ////if (ModelState.IsValid)

            //_context.Add(details);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            ////}
            //ViewData["SellID"] = new SelectList(_context.Sells, "SellID", "SellID", details.SellID);
            //return View(details);
            
            total += details.Total;
            Details detail = new Details
            {
                
                DetailsId = details.DetailsId,
                Product = details.Product,
                Quantity = details.Quantity,
                PurchasePrice = details.PurchasePrice,
                SellPrice = details.SellPrice,
                Discount = details.Discount,
                Total = details.Total,
                SellID = sells.SellID,
            };

            _context.Add(detail);
            await _context.SaveChangesAsync();

            //_context.Database.OpenConnection();
            //_context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [Items].[Items] ON;");
            //List<Sell> sells = new List<Sell>();
            
            

            //_context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [Items].[Items] OFF;");
            //_context.Database.CloseConnection();

            return RedirectToAction("Index");   

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Details == null)
            {
                return NotFound();
            }

            var details = await _context.Details.FindAsync(id);
            if (details == null)
            {
                return NotFound();
            }
            ViewData["SellID"] = new SelectList(_context.Sells, "SellID", "SellID", details.SellID);
            return View(details);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Details details)
        {
            if (id != details.DetailsId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(details);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailsExists(details.DetailsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["SellID"] = new SelectList(_context.Sells, "SellID", "SellID", details.SellID);
            return View(details);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Details == null)
            {
                return NotFound();
            }

            var details = await _context.Details
                .Include(d => d.Sells)
                .FirstOrDefaultAsync(m => m.DetailsId == id);
            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Details == null)
            {
                return Problem("Entity set 'DataContext.Details'  is null.");
            }
            var details = await _context.Details.FindAsync(id);
            if (details != null)
            {
                _context.Details.Remove(details);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailsExists(int id)
        {
            return _context.Details.Any(e => e.DetailsId == id);
        }
    }
}
