using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseDetailTask.Models;

namespace PurchaseDetailTask.Controllers
{
    public class SalesController : Controller
    {
        private readonly DataContext _context;

        public SalesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //return View(await _context.Sells.ToListAsync());

            List<Sell> sells;

            sells = _context.Sells.ToList();

            return View(sells);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sells == null)
            {
                return NotFound();
            }

            var sell = await _context.Sells
                .FirstOrDefaultAsync(m => m.SellID == id);
            if (sell == null)
            {
                return NotFound();
            }

            return View(sell);
        }

        public IActionResult Create()
        {
            return View();

            //Sell sell = new Sell();
            //sell.Details.Add(new Details() { DetailsId = 1 });

            //return View(sell);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sell sell)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sell);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sell);
        }

        //public IActionResult Create(Sell sell)
        //{
        //    foreach (Details d in sell.Details)
        //    {
        //        if (d.Product == null || d.Product.Length == 0)
        //            sell.Details.Remove(d);
        //    }

        //    _context.Add(sell);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sells == null)
            {
                return NotFound();
            }

            var sell = await _context.Sells.FindAsync(id);
            if (sell == null)
            {
                return NotFound();
            }
            return View(sell);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SellID,InvoiceNo,TotalPrice,TotalDiscount,TotalPurchase,TotalProfit")] Sell sell)
        {
            if (id != sell.SellID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sell);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellExists(sell.SellID))
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
            return View(sell);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sells == null)
            {
                return NotFound();
            }

            var sell = await _context.Sells
                .FirstOrDefaultAsync(m => m.SellID == id);
            if (sell == null)
            {
                return NotFound();
            }

            return View(sell);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sells == null)
            {
                return Problem("Entity set 'DataContext.Sells'  is null.");
            }
            var sell = await _context.Sells.FindAsync(id);
            if (sell != null)
            {
                _context.Sells.Remove(sell);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellExists(int id)
        {
            return _context.Sells.Any(e => e.SellID == id);
        }
    }
}