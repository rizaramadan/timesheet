﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Timesheet.Data;
using Timesheet.Models;
using Timesheet.Services;

namespace Timesheet.Controllers
{
    [Authorize]
    public class TimesheetsController : Controller
    { 
        private readonly AppDbContext _context;
        private readonly ILogger<TimesheetsController> _logger;
        private readonly ITimesheetService _timesheetService;


        public TimesheetsController(AppDbContext context, ILogger<TimesheetsController> logger, ITimesheetService t)
        {
            _logger = logger;
            _context = context;
            _timesheetService = t;
        }

        public async Task<IActionResult> Index(DateTime? day)
        {
            DateTime date = SetDateViewData(day);
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userlong = long.Parse(userId);
            var activites = _context.Activities
                .AsNoTracking()
                .Where(x => x.Date.Date == date.Date && x.CreatedBy == userlong)
                .OrderBy(x => x.CreatedAt);

            return View(await activites.ToListAsync());
        }

        // GET: Activities/Create
        public async Task<IActionResult> Create(DateTime? day)
        {
            SetDateViewData(day);
            return View(await _timesheetService.GetCreateViewModel());
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Activity activity)
        {
            if (ModelState.IsValid)
            {
                var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
                activity.CreatedAt = DateTime.Now;
                activity.CreatedBy = long.Parse(userId);
                activity.UpdatedAt = activity.CreatedAt;
                activity.UpdatedBy = activity.CreatedBy;
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { day = activity.Date });
            }
            return View(activity);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            SetDateViewData(activity.Date);
            return View(await _timesheetService.GetEditViewModel(activity));
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Models.Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
                    var userLong = long.Parse(userId); 
                    var activityDb = await _context.Activities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.CreatedBy == userLong);
                    activity.CreatedBy = activityDb.CreatedBy;
                    activity.CreatedAt = activityDb.CreatedAt;
                    activity.UpdatedBy = userLong;
                    activity.UpdatedAt = DateTime.Now;
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index), new { day = activity.Date });
            }
            return View(activity);
        }

        private DateTime SetDateViewData(DateTime? day)
        {
            var date = day ?? DateTime.Now;
            ViewData["date"] = date;
            return date;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
