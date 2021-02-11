using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Timesheet.Domains.ActivityGroups;
using Timesheet.Domains.ActivityTypes;
using Timesheet.Domains.Timesheets;
using Timesheet.Models;

namespace Timesheet.Domains.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, long>
    {
        public DbSet<ActivityGroup> ActivityGroups { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Activity> Activities { get; set;  }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }
    }
}
