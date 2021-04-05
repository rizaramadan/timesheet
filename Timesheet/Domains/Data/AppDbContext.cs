using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timesheet.Domains.ActivityGroups;
using Timesheet.Domains.ActivityTypes;
using Timesheet.Domains.FreeTimeEstimate;
using Timesheet.Domains.Timesheets;
using Timesheet.Models;

namespace Timesheet.Domains.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, long>
    {
        const string Added = "Added";
        const string Modified = "Modified";
        const string Deleted = "Deleted";

        #region Timesheet
        public DbSet<ActivityGroup> ActivityGroups { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Activity> Activities { get; set;  }
        #endregion

        #region scheduler
        public DbSet<FreeTime> FreeTimes { get; set; }
        #endregion

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        #region IAuditable automation
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            BeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public Task<int> SaveChangesHardDeleteAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            BeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        void BeforeSaving()
        {
            var httpContextAccessor = this.GetService<IHttpContextAccessor>();
            var userId = httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userLong = long.TryParse(userId, out var result) ? result : -1L;

            foreach (var entry in ChangeTracker.Entries())
            {
                var state = string.Empty;
                if (entry.Entity is IAuditable trackable)
                {
                    state = entry.State.ToString();
                    if (state == Added)
                    {
                        trackable.CreatedAt = trackable.UpdatedAt = DateTime.Now;
                        if (trackable.CreatedBy == 0)
                            trackable.CreatedBy = trackable.UpdatedBy = userLong;
                    }
                    else
                    {
                        entry.Property(nameof(IAuditable.CreatedBy)).IsModified = false;
                        entry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                        if (state == Modified)
                        {

                            trackable.UpdatedAt = DateTime.Now;
                            if (trackable.UpdatedBy == 0)
                                trackable.UpdatedBy = userLong;
                        }

                    }
                }
            }
        }
        #endregion
    }
}
