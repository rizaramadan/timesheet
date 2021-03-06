﻿using System;
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

namespace Timesheet.Domains.ActivityGroups
{
    [Authorize]
    public class ActivityGroupsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public ActivityGroupsController(AppDbContext context, IMemoryCache c)
        {
            _context = context;
            _cache = c;
        }

        // GET: ActivityGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActivityGroups.OrderBy(x => x.Order).ToListAsync());
        }

        // GET: ActivityGroups/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityGroup = await _context.ActivityGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityGroup == null)
            {
                return NotFound();
            }

            return View(activityGroup);
        }

        // GET: ActivityGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Order")] ActivityGroup activityGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityGroup);
                await _context.SaveChangesAsync();
                _cache.Remove(nameof(ActivityGroup));
                return RedirectToAction(nameof(Index));
            }
            return View(activityGroup);
        }

        // GET: ActivityGroups/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityGroup = await _context.ActivityGroups.FindAsync(id);
            if (activityGroup == null)
            {
                return NotFound();
            }
            return View(activityGroup);
        }

        // POST: ActivityGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,Order")] ActivityGroup activityGroup)
        {
            if (id != activityGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityGroup);
                    await _context.SaveChangesAsync();
                    _cache.Remove(nameof(ActivityGroup));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityGroupExists(activityGroup.Id))
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
            return View(activityGroup);
        }

        // GET: ActivityGroups/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityGroup = await _context.ActivityGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityGroup == null)
            {
                return NotFound();
            }

            return View(activityGroup);
        }

        // POST: ActivityGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var activityGroup = await _context.ActivityGroups.FindAsync(id);
            _context.ActivityGroups.Remove(activityGroup);
            await _context.SaveChangesAsync();
            _cache.Remove(nameof(ActivityGroup));
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityGroupExists(long id)
        {
            return _context.ActivityGroups.Any(e => e.Id == id);
        }
    }
}
