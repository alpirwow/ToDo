using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Services
{
    public class IdentityUser : IIdentityUser
    {
        public Guid? Id { get; private set; }

        public IdentityUser(IHttpContextAccessor accessor)
        {
            var userId = accessor.HttpContext?.Request.Headers["ToDo-User-Id"].FirstOrDefault();

            if (!string.IsNullOrEmpty(userId))
                Id = Guid.Parse(userId);
        }
    }
}
