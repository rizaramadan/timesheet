using System.ComponentModel.DataAnnotations;

namespace Timesheet.Domains.Timesheets
{
    public enum DurationType
    {
        Unknown = 0,
        [Display(Name = "Minute(s)")]
        Minutes,
        [Display(Name = "Hour(s)")]
        Hours
    }
}
