using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sha_InvoiceManagementSystem.Models;

namespace Sha_InvoiceManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Cashier")]
    public class InvoicesController : Controller
    {
        private readonly ShaTaskContext _context;
        
        // Constructor
        public InvoicesController(ShaTaskContext context) {
            _context = context;
        }

        // GET: Invoices
        public IActionResult Index()
        {
            return RedirectToAction("InvoiceData");
        }

        // GET: InvoiceHeaders/Create
        public IActionResult Create()
        {
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "CashierName");
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName");
            return View();
        }

        // POST: InvoiceHeaders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerName,Invoicedate,CashierId,BranchId")] InvoiceHeader invoiceHeader)
        {
            try
            {
                _context.Add(invoiceHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddItems", new { id = invoiceHeader.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating invoice: {ex.Message}";
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: AddItems
        public IActionResult AddItems(int id)
        {
            var invoice = _context.InvoiceHeaders
                .Include(h => h.Cashier)
                .Include(h => h.Branch)
                .Include(h => h.InvoiceDetails)
                .FirstOrDefault(h => h.Id == id);

            if (invoice == null)
                return NotFound();

            ViewBag.InvoiceId = id;
            ViewBag.CustomerName = invoice.CustomerName;
            ViewBag.CashierName = invoice.Cashier?.CashierName;
            ViewBag.BranchName = invoice.Branch?.BranchName;

            var items = invoice.InvoiceDetails.ToList();
            var total = items.Sum(i => i.ItemCount * i.ItemPrice);
            ViewBag.Total = total;

            // Add debug information
            if (items.Count > 0)
            {
                var debugInfo = $"Loaded {items.Count} items. Total: {total}";
                TempData["DebugMessage"] = debugInfo;
            }

            return View(items); 
        }

        // POST: AddItems
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItems(int id, [Bind("ItemName,ItemCount,ItemPrice")] InvoiceDetail invoiceDetail)
        {
            try
            {
                // Set the InvoiceHeaderId directly
                invoiceDetail.InvoiceHeaderId = id;
                
                // Add to database
                _context.Add(invoiceDetail);
                await _context.SaveChangesAsync();
                
                // Add success message
                TempData["DebugMessage"] = $"Item added successfully: {invoiceDetail.ItemName} - Qty: {invoiceDetail.ItemCount} - Price: {invoiceDetail.ItemPrice}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding item: {ex.Message}";
            }
            
            return RedirectToAction(nameof(AddItems), new { id = id });
        }

        // POST: FinishInvoice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishInvoice(int id)
        {
            try
            {
                var invoice = await _context.InvoiceHeaders
                    .Include(h => h.InvoiceDetails)
                    .FirstOrDefaultAsync(h => h.Id == id);

                if (invoice == null)
                    return NotFound();

                // Calculate total from invoice details
                var total = invoice.InvoiceDetails.Sum(d => d.ItemCount * d.ItemPrice);
                
                // Add success message
                TempData["SuccessMessage"] = $"Invoice completed successfully! Total: {total.ToString("0.00")} EGP";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error finishing invoice: {ex.Message}";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _context.InvoiceHeaders
                .Include(h => h.Cashier)
                .Include(h => h.Branch)
                .Include(h => h.InvoiceDetails)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (invoice == null)
                return NotFound();

            // Calculate total
            var total = invoice.InvoiceDetails.Sum(d => d.ItemCount * d.ItemPrice);
            ViewBag.Total = total;

            return View(invoice);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _context.InvoiceHeaders
                .Include(h => h.Cashier)
                .Include(h => h.Branch)
                .Include(h => h.InvoiceDetails)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (invoice == null)
                return NotFound();

            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "CashierName", invoice.CashierId);
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName", invoice.BranchId);
            
            return View(invoice);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerName,Invoicedate,CashierId,BranchId")] InvoiceHeader invoiceHeader)
        {
            try
            {
                if (id != invoiceHeader.Id)
                    return NotFound();

                _context.Update(invoiceHeader);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Invoice updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating invoice: {ex.Message}";
                ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "CashierName", invoiceHeader.CashierId);
                ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName", invoiceHeader.BranchId);
                return View(invoiceHeader);
            }
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _context.InvoiceHeaders
                .Include(h => h.Cashier)
                .Include(h => h.Branch)
                .Include(h => h.InvoiceDetails)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (invoice == null)
                return NotFound();

            // Calculate total
            var total = invoice.InvoiceDetails.Sum(d => d.ItemCount * d.ItemPrice);
            ViewBag.Total = total;

            return View(invoice);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var invoice = await _context.InvoiceHeaders
                    .Include(h => h.InvoiceDetails)
                    .FirstOrDefaultAsync(h => h.Id == id);

                if (invoice == null)
                    return NotFound();

                // Delete all invoice details first
                if (invoice.InvoiceDetails != null && invoice.InvoiceDetails.Any())
                {
                    _context.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);
                }

                // Delete the invoice header
                _context.InvoiceHeaders.Remove(invoice);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Invoice deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting invoice: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: EditItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int id, [Bind("Id,ItemName,ItemCount,ItemPrice,InvoiceHeaderId")] InvoiceDetail invoiceDetail)
        {
            try
            {
                _context.Update(invoiceDetail);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Item updated successfully!";
                return RedirectToAction("Edit", new { id = invoiceDetail.InvoiceHeaderId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating item: {ex.Message}";
                return RedirectToAction("Edit", new { id = invoiceDetail.InvoiceHeaderId });
            }
        }

        // POST: DeleteItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var item = await _context.InvoiceDetails
                    .Include(i => i.InvoiceHeader)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (item == null)
                    return NotFound();

                var invoiceId = item.InvoiceHeaderId;
                _context.InvoiceDetails.Remove(item);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Item deleted successfully!";
                return RedirectToAction("Edit", new { id = invoiceId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting item: {ex.Message}";
                return RedirectToAction("Edit", new { id = id });
            }
        }

        // GET: InvoiceData
        public async Task<IActionResult> InvoiceData()
        {
            var invoices = await _context.InvoiceHeaders
                .Include(i => i.Cashier)
                .Include(i => i.Branch)
                .Include(i => i.InvoiceDetails)
                .ToListAsync();

            return View(invoices);
        }
    }
} 