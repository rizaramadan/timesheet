using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Timesheet.Services
{
    public interface IUserService
    {
        public long GetCurrentUserId();
    }

    public class UserService : IUserService
    {
        readonly IHttpContextAccessor _hca;

        public UserService(IHttpContextAccessor h) => _hca = h;

        public long GetCurrentUserId()
        {
            var userId = _hca?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return long.TryParse(userId, out var result) ? result : -1L;
        }
    }
}
