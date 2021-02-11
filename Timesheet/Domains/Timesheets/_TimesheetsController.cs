using Microsoft.AspNetCore.Authorization;
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
using Timesheet.Domains.Data;
using Timesheet.Models;
using Timesheet.Services;

namespace Timesheet.Domains.Timesheets
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
            long userlong = GetUserId();
            var activites = _context.Activities
                .AsNoTracking()
                .Where(x => x.Date.Date == date.Date && x.CreatedBy == userlong)
                .OrderBy(x => x.CreatedAt);

            ViewData[nameof(ITimesheetService)] = await _timesheetService.GetCreateViewModel();
            return View(await activites.ToListAsync());
        }

        private long GetUserId()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userlong = long.Parse(userId);
            return userlong;
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
        public async Task<IActionResult> Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();
                activity.CreatedAt = DateTime.Now;
                activity.CreatedBy = userId;
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

            var userId = GetUserId();
            var activity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == id && x.CreatedBy == userId);
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
        public async Task<IActionResult> Edit(long id, Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = GetUserId();
                    var activityDb = await _context.Activities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.CreatedBy == userId);
                    activity.CreatedBy = activityDb.CreatedBy;
                    activity.CreatedAt = activityDb.CreatedAt;
                    activity.UpdatedBy = userId;
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

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var activity = await _context.Activities
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedBy == userId);
            if (activity == null)
            {
                return NotFound();
            }

            SetDateViewData(activity.Date);
            return View(await _timesheetService.GetEditViewModel(activity));
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var userId = GetUserId();
            var activity = await _context.Activities.FirstOrDefaultAsync(m => m.Id == id && m.CreatedBy == userId);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { day = activity.Date });
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
