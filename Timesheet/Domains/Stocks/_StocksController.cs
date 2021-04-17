using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timesheet.Domains.Data;
using Timesheet.Services;

namespace Timesheet.Domains.Stocks
{
    [Authorize]
    public class StocksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IStockService _stockService;

        public StocksController(AppDbContext context, IUserService u, IStockService s)
        {
            _context = context;
            _userService = u;
            _stockService = s;
        }

        // GET: StockTrxes
        public async Task<IActionResult> Index()
        {
            long currentUserId = _userService.GetCurrentUserId();
            return View(await _context.StockTrxs.Where(x => x.CreatedBy == currentUserId).ToListAsync());
        }

        // GET: StockTrxes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            long currentUserId = _userService.GetCurrentUserId();

            var stockTrx = await _context.StockTrxs
                .Include(x => x.StockTrxAdditions)
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedBy == currentUserId);
            if (stockTrx == null)
            {
                return NotFound();
            }

            return View(stockTrx);
        }

        // GET: StockTrxes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StockTrxes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,State,Lot,BuyPrice,SellPrice,SoldAt,LessonLearned,Emiten")] StockTrx stockTrx)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockTrx);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stockTrx);
        }

        // GET: StockTrxes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            long currentUserId = _userService.GetCurrentUserId();
            var stockTrx = await _context.StockTrxs
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedBy == currentUserId);
            if (stockTrx == null)
            {
                return NotFound();
            }
            return View(stockTrx);
        }

        

        // POST: StockTrxes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Lot,BuyPrice,Emiten")] StockTrx stockTrx)
        {
            if (id != stockTrx.Id)
            {
                return NotFound();
            }


            long currentUserId = _userService.GetCurrentUserId();
            var stockTrxDb = await _context.StockTrxs
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedBy == currentUserId);
            if (stockTrxDb.CreatedBy != currentUserId) 
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    stockTrxDb.UpdateInfo(stockTrx);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockTrxExists(stockTrx.Id))
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
            return View(stockTrx);
        }

        public async Task<IActionResult> Add(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            long currentUserId = _userService.GetCurrentUserId();
            var stockTrx = await _context.StockTrxs
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedBy == currentUserId);
            if (stockTrx == null)
            {
                return NotFound();
            }
            return View(stockTrx);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(long id, [Bind("Id,Lot,BuyPrice")] StockTrx stockTrx)
        {
            if (id != stockTrx.Id)
            {
                return NotFound();
            }

            long currentUserId = _userService.GetCurrentUserId();
            var stockTrxDb = await _context.StockTrxs
                .Include(x => x.StockTrxAdditions)
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedBy == currentUserId);
            if (stockTrxDb.CreatedBy != currentUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _stockService.AddStock(stockTrxDb, stockTrx.Lot, stockTrx.BuyPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockTrxExists(stockTrx.Id))
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
            return View(stockTrx);
        }

        // GET: StockTrxes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            long currentUserId = _userService.GetCurrentUserId();
            var stockTrx = await _context.StockTrxs
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedBy == currentUserId);
            if (stockTrx == null)
            {
                return NotFound();
            }

            return View(stockTrx);
        }

        // POST: StockTrxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var stockTrx = await _context.StockTrxs.FindAsync(id);
            _context.StockTrxs.Remove(stockTrx);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockTrxExists(long id)
        {
            long currentUserId = _userService.GetCurrentUserId();
            return _context.StockTrxs.Any(e => e.Id == id && e.CreatedBy == currentUserId);
        }
    }
}
