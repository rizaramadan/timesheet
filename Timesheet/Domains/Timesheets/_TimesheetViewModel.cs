using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheet.Domains.Timesheets
{
    public class TimesheetViewModel : Activity
    {
        public static readonly List<SelectListItem> DurationTypes = new List<SelectListItem>
        {
            new SelectListItem{ Value = DurationType.Minutes.ToString(), Text = "Minute(s)" },
            new SelectListItem{ Value = DurationType.Hours.ToString(), Text = "Hour(s)" }
        };

        public List<SelectListItem> GroupSelectItem { get; set; }
        public List<SelectListItem> TypeSelectItem { get; set; }
    }
}
