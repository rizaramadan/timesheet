using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheet.Models
{
    public interface IAuditable 
    {
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }

    public class ActivityGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ActivityType 
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public enum ActivityDuration
    {
        Unknown = 0,
        Minutes,
        Hours
    }

    public class Activity : IAuditable
    {
        public long Id { get; set; }
        public long ActivityGroupId { get; set; }
        public DateTime Date { get; set; }
        public ActivityDuration Duration { get; set; }
        public string Info { get; set; }
        public long ActivityTypeId { get; set; }
        public int Frequency { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
