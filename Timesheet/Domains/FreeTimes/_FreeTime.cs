using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheet.Models;

namespace Timesheet.Domains.FreeTimeEstimate
{
    public class FreeTime : IAuditable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        //1: Monday, 7 Sunday
        public int DayOfWeek { get; set; }
        //0-23
        public int HourStart { get; set; }
        //0-23
        public int HourEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }

    public class FreeTimeViewModel : FreeTime
    {
        public static readonly List<SelectListItem> DayOfWeekSelectItem = new List<SelectListItem>
        {
            new SelectListItem{ Value = "1", Text = "Mon" },
            new SelectListItem{ Value = "2", Text = "Tue" },
            new SelectListItem{ Value = "3", Text = "Wed" },
            new SelectListItem{ Value = "4", Text = "Thu" },
            new SelectListItem{ Value = "5", Text = "Fri" },
            new SelectListItem{ Value = "6", Text = "Sat" },
            new SelectListItem{ Value = "7", Text = "Sun" }
        };
        public static List<SelectListItem> HourSelectItem { get; set; }

        

        static FreeTimeViewModel() {
            HourSelectItem = new List<SelectListItem>(24);
            for (int i = 0; i < 24; i++) {
                HourSelectItem.Add(new SelectListItem { Value = $"{i}", Text = $"{i}" });
            }
        }

        public FreeTimeViewModel FromFreeTime(FreeTime f) 
        {
            Id = f.Id;
            Name = f.Name;
            DayOfWeek = f.DayOfWeek;
            HourStart = f.HourStart;
            HourEnd = f.HourEnd;
            return this;
        }
    }
}
