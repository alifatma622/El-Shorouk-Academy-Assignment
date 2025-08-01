using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sha_InvoiceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sha_InvoiceManagementSystem.Controllers
{
    public class CashiersController : Controller
    {
        private readonly ShaTaskContext _context;
        
        // Constructor
        public CashiersController(ShaTaskContext context) {
            _context = context;
        }

        // GET: Cashiers
        public async Task<IActionResult> Index()
        {
            var cashiers = await _context.Cashiers
                                         .Include(c => c.Branch)
                                         .ToListAsync();
            return View(cashiers);
        }

        // GET: Cashiers/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName");
            return View();
        }

        // POST: Cashiers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CashierName,BranchId")] Cashier cashier)
        {
            _context.Add(cashier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Cashiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        // GET: Cashiers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return NotFound();
            }

            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName", cashier.BranchId);
            return View(cashier);
        }

        // POST: Cashiers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CashierName,BranchId")] Cashier cashier)
        {
            if (id != cashier.Id)
            {
                return NotFound();
            }

            _context.Update(cashier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Cashiers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var cashier = await _context.Cashiers
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cashier == null)
                return NotFound();

            return View(cashier);
        }

        // POST: Cashiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier != null)
                _context.Cashiers.Remove(cashier);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
