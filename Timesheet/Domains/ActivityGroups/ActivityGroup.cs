using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheet.Domains.ActivityGroups
{
    public class ActivityGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
    }
}
