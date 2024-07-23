using DataLayer.Contexts;
using DataLayer.Interfaces;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Settings
{
    public static class DI
    {
        public static void Add(IConfiguration conf, IServiceCollection service)
        {
            AddContext(conf, service);
            AddRepositories(service);
        }

        private static void AddContext(IConfiguration conf, IServiceCollection service)
        {
            string connectionStr = conf.GetConnectionString("PostgreSQL")!;

            service.AddDbContext<PostgreSQLContext>(options => options.UseNpgsql(connectionStr));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(ICrudGenericRepository<,>), typeof(CrudGenericRepository<,>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPriorityRepository, PriorityRepository>();
            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();

            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        }
    }
}
