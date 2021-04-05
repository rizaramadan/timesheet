using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timesheet.Domains.Data;

namespace Timesheet.Domains.FreeTimeEstimate
{
    [Authorize]
    public class FreeTimesController : Controller
    {
        private readonly AppDbContext _context;

        public FreeTimesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FreeTimes
        public async Task<IActionResult> Index()
        {
            return View(await _context.FreeTimes.ToListAsync());
        }

        // GET: FreeTimes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freeTime = await _context.FreeTimes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (freeTime == null)
            {
                return NotFound();
            }

            return View(freeTime);
        }

        // GET: FreeTimes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FreeTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DayOfWeek,HourStart,HourEnd,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy")] FreeTime freeTime)
        {
            if (ModelState.IsValid)
            {
                freeTime.Name = $"Day {freeTime.DayOfWeek}, {freeTime.HourStart}-{freeTime.HourEnd}";
                _context.Add(freeTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(freeTime);
        }

        // GET: FreeTimes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freeTime = await _context.FreeTimes.FindAsync(id);
            if (freeTime == null)
            {
                return NotFound();
            }
            var freeTimeVm = new FreeTimeViewModel();
            return View(freeTimeVm.FromFreeTime(freeTime));
        }

        // POST: FreeTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,DayOfWeek,HourStart,HourEnd")] FreeTime freeTime)
        {
            if (id != freeTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    freeTime.Name = $"Day {freeTime.DayOfWeek}, {freeTime.HourStart}-{freeTime.HourEnd}";
                    _context.Update(freeTime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FreeTimeExists(freeTime.Id))
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
            return View(freeTime);
        }

        // GET: FreeTimes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freeTime = await _context.FreeTimes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (freeTime == null)
            {
                return NotFound();
            }

            return View(freeTime);
        }

        // POST: FreeTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var freeTime = await _context.FreeTimes.FindAsync(id);
            _context.FreeTimes.Remove(freeTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FreeTimeExists(long id)
        {
            return _context.FreeTimes.Any(e => e.Id == id);
        }
    }
}
