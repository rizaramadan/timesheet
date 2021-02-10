using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheet.Data;
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
        public TimesheetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TimesheetViewModel> GetCreateViewModel()
        {
            var groups = await _context.ActivityGroups.AsNoTracking().ToListAsync();
            var types = await _context.ActivityTypes.AsNoTracking().ToListAsync();
            var TimesheetViewModel = new TimesheetViewModel
            {
                GroupSelectItem = groups.Select(x => new SelectListItem{ Value = $"{x.Id}", Text = x.Name }).ToList(),
                TypeSelectItem = types.Select(x => new SelectListItem { Value = $"{x.Id}", Text = x.Name }).ToList()
            };
            return TimesheetViewModel;
        }

        public async Task<TimesheetViewModel> GetEditViewModel(Activity activity)
        {
            var groups = await _context.ActivityGroups.AsNoTracking().ToListAsync();
            var types = await _context.ActivityTypes.AsNoTracking().ToListAsync();
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
