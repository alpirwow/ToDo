using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Settings
{
    public static class DI
    {
        public static void Add(IConfiguration conf, IServiceCollection service)
        {
            DataLayer.Settings.DI.Add(conf, service);
            AddServices(service);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IPriorityService, PriorityService>();
            services.AddScoped<IToDoItemService, ToDoItemService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIdentityUser, IdentityUser>();
        }
    }
}
