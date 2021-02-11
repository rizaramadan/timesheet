using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Timesheet.Domains.Data;
using Timesheet.Models;

namespace Timesheet.Domains.ActivityTypes
{
    [Authorize]
    public class ActivityTypesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public ActivityTypesController(AppDbContext context, IMemoryCache c)
        {
            _context = context;
            _cache = c;
        }

        // GET: ActivityTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActivityTypes.ToListAsync());
        }

        // GET: ActivityTypes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        // GET: ActivityTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ActivityType activityType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityType);
                await _context.SaveChangesAsync();
                _cache.Remove(nameof(ActivityType));
                return RedirectToAction(nameof(Index));
            }
            return View(activityType);
        }

        // GET: ActivityTypes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityTypes.FindAsync(id);
            if (activityType == null)
            {
                return NotFound();
            }
            return View(activityType);
        }

        // POST: ActivityTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] ActivityType activityType)
        {
            if (id != activityType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityType);
                    await _context.SaveChangesAsync();
                    _cache.Remove(nameof(ActivityType));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTypeExists(activityType.Id))
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
            return View(activityType);
        }

        // GET: ActivityTypes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        // POST: ActivityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var activityType = await _context.ActivityTypes.FindAsync(id);
            _context.ActivityTypes.Remove(activityType);
            await _context.SaveChangesAsync();
            _cache.Remove(nameof(ActivityType));
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityTypeExists(long id)
        {
            return _context.ActivityTypes.Any(e => e.Id == id);
        }
    }
}
