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
            ////if (ModelState.IsValid)

            //_context.Add(details);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            ////}
            //ViewData["SellID"] = new SelectList(_context.Sells, "SellID", "SellID", details.SellID);
            //return View(details);

            Details detail = new Details
            {
                DetailsId = details.DetailsId,
                Product = details.Product,
                Quantity = details.Quantity,
                PurchasePrice = details.PurchasePrice,
                SellPrice = details.SellPrice,
                Discount = details.Discount,
                Total = details.Total,
                SellID = details.SellID,
            };

            _context.Add(detail);
            await _context.SaveChangesAsync();

            //_context.Database.OpenConnection();
            //_context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [Items].[Items] ON;");
            //List<Sell> sells = new List<Sell>();

            Sell sells = new Sell();
            sells.TotalPurchase = details.PurchasePrice;
            sells.TotalPrice = details.SellPrice;
            sells.TotalDiscount = details.Discount;
            sells.TotalProfit = details.Total;
            sells.SellID = detail.DetailsId;

            _context.Add(sells);
            await _context.SaveChangesAsync();

            //_context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [Items].[Items] OFF;");
            //_context.Database.CloseConnection();

            return RedirectToAction("Index");



            //if (ModelState.IsValid)
            //{
            //    Details detail = new Details();
            //    _context.Details.Add(Details);
            //    var sells = (from o in _context.Sells where o.SellID == Details.SellID select o).FirstOrDefault();

            //    if (sells == null)
            //    {
            //        sells = new Sell();
            //        sells.productID = Details.SellID;
            //        sells.quantity = Details.quantity;
            //        sells.status = "Receive";
            //        _context.Details.Add(sells);
            //    }
            //    else
            //    {
            //        sells.quantity += Details.quantity;
            //        sells.status = "Receive";
            //    }
            //    _context.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(Details);

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
