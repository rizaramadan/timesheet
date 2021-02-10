using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public enum DurationType
    {
        Unknown = 0,
        [Display(Name = "Minute(s)")]
        Minutes,
        [Display(Name = "Hour(s)")]
        Hours
    }

    public class Activity : IAuditable
    {
        public long Id { get; set; }
        public long ActivityGroupId { get; set; }
        public ActivityGroup ActivityGroup { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public DurationType DurationType { get; set; }
        public string Info { get; set; }
        public long ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; }
        public int Frequency { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
