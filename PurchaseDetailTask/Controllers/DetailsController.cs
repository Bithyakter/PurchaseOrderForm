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
        public async Task<IActionResult> Create([Bind("DetailsId,Product,Quantity,PurchasePrice,SellPrice,Discount,Total,SellID")] Details details)
        {
            if (ModelState.IsValid)
            {
                _context.Add(details);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SellID"] = new SelectList(_context.Sells, "SellID", "SellID", details.SellID);
            return View(details);
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
        public async Task<IActionResult> Edit(int id, [Bind("DetailsId,Product,Quantity,PurchasePrice,SellPrice,Discount,Total,SellID")] Details details)
        {
            if (id != details.DetailsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            }
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
