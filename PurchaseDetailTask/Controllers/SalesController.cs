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
            return View(await _context.Sells.ToListAsync());
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