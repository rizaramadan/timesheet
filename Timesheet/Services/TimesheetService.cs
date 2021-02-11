using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheet.Domains.Data;
using Timesheet.Domains.ActivityGroups;
using Timesheet.Domains.ActivityTypes;
using Timesheet.Domains.Timesheets;
using Timesheet.Models;

namespace Timesheet.Services
{
    public interface ITimesheetService
    {
        Task<TimesheetViewModel> GetCreateViewModel();
        Task<TimesheetViewModel> GetEditViewModel(Activity activity);
    }

    public class TimesheetService : ITimesheetService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(30);

        public TimesheetService(AppDbContext context, IMemoryCache m)
        {
            _context = context;
            _cache = m;
        }

        public async Task<TimesheetViewModel> GetCreateViewModel()
        {
            var groups = await GetGroups();
            var types = await GetTypes();

            var TimesheetViewModel = new TimesheetViewModel
            {
                GroupSelectItem = groups.Select(x => new SelectListItem { Value = $"{x.Id}", Text = x.Name }).ToList(),
                TypeSelectItem = types.Select(x => new SelectListItem { Value = $"{x.Id}", Text = x.Name }).ToList()
            };
            return TimesheetViewModel;
        }

        private async Task<List<ActivityType>> GetTypes()
        {
            return await _cache.GetOrCreateAsync(nameof(ActivityType), entry =>
            {
                entry.SlidingExpiration = CacheExpiry;
                return _context.ActivityTypes.AsNoTracking().ToListAsync();
            });
        }

        private async Task<List<ActivityGroup>> GetGroups()
        {
            return await _cache.GetOrCreateAsync(nameof(ActivityGroup), entry =>
            {
                entry.SlidingExpiration = CacheExpiry;
                return _context.ActivityGroups.AsNoTracking().OrderBy(x => x.Order).ToListAsync();
            });
        }

        public async Task<TimesheetViewModel> GetEditViewModel(Activity activity)
        {
            var groups = await GetGroups();
            var types = await GetTypes();
            var TimesheetViewModel = new TimesheetViewModel
            {
                Id = activity.Id,
                ActivityGroupId = activity.ActivityGroupId,
                Date = activity.Date,
                Duration = activity.Duration,
                DurationType = activity.DurationType,
                Info = activity.Info,
                ActivityTypeId = activity.ActivityTypeId,
                Frequency = activity.Frequency,
                CreatedAt = activity.CreatedAt,
                CreatedBy = activity.CreatedBy,
                UpdatedAt = activity.UpdatedAt,
                UpdatedBy = activity.UpdatedBy,
                GroupSelectItem = groups.Select(x => new SelectListItem { Value = $"{x.Id}", Text = x.Name, Selected = x.Id == activity.ActivityGroupId }).ToList(),
                TypeSelectItem = types.Select(x => new SelectListItem { Value = $"{x.Id}", Text = x.Name, Selected = x.Id == activity.ActivityTypeId }).ToList()
            };
            return TimesheetViewModel;
        }
    }
}
