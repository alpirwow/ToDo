using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DataLayer.Repositories
{
    internal class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IConfiguration _configuration;
        private readonly PostgreSQLContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(IConfiguration configuration, PostgreSQLContext context, ILogger<DatabaseInitializer> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task InitializeDBAsync()
        {
            _logger.LogInformation("Initializing database...");

            var isConnected = await _context.Database.CanConnectAsync();

            if (!isConnected)
            {
                _logger.LogError("Can't connect to the database.");

                _logger.LogInformation("Waiting 10 seconds for up db.");
                await Task.Delay(10000);
                _logger.LogInformation("The wait is over.");
            }

            await CheckAndCreateDB();
            await AddDataIntoDB();

            _logger.LogInformation("Database initialization completed.");
        }

        private async Task CheckAndCreateDB()
        {
            _logger.LogInformation("Checking and creating database if it does not exist...");

            var databaseCreator = _context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

            if (databaseCreator != null && !await databaseCreator.ExistsAsync())
            {
                _logger.LogInformation("Database does not exist. Creating database...");
                await databaseCreator.CreateAsync();
            }

            _logger.LogInformation("Applying migrations...");
            await _context.Database.MigrateAsync();
            /*var allMigrations = _context.Database.GetMigrations();

            try
            {
                var addedMigrations = await _context.Database.GetAppliedMigrationsAsync();

                var newList = allMigrations.ToList();

                newList.RemoveAll(item => addedMigrations.Contains(item));

                var migrator = _context.Database.GetInfrastructure().GetService<IMigrator>();

                if (migrator is not null)
                    foreach (var item in newList)
                        await migrator.MigrateAsync(item);
                else
                {
                    _logger.LogError("migrator is null");
                    throw new Exception("migrator is null");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }*/

        }

        private async Task AddDataIntoDB()
        {
            _logger.LogInformation("Adding initial data into the database...");
            var isUsersExist = await _context.Users.AsNoTracking().AnyAsync();
            var isPrioritiesExist = await _context.Priorities.AsNoTracking().AnyAsync();
            var isToDoListExist = await _context.ToDoList.AsNoTracking().AnyAsync();

            if (isUsersExist || isPrioritiesExist || isToDoListExist)
            {
                _logger.LogInformation("Initial data already exists. Skipping data addition.");
                return;
            }

            var usersTask = GetDataFromJsonAsync<UserEntity>(_configuration["JsonDataPaths:Users"]!);
            var prioritiesTask = GetDataFromJsonAsync<PriorityEntity>(_configuration["JsonDataPaths:Priorities"]!);
            var toDoListTask = GetDataFromJsonAsync<ToDoItemEntity>(_configuration["JsonDataPaths:ToDoItems"]!);

            await Task.WhenAll(usersTask, prioritiesTask, toDoListTask);

            if (usersTask.Result?.Count > 0)
            {
                _logger.LogInformation("Adding users to the database...");
                await _context.Users.AddRangeAsync(usersTask.Result);
            }

            if (prioritiesTask.Result?.Count > 0)
            {
                _logger.LogInformation("Adding priorities to the database...");
                await _context.Priorities.AddRangeAsync(prioritiesTask.Result);
            }

            if (toDoListTask.Result?.Count > 0)
            {
                _logger.LogInformation("Adding ToDo items to the database...");
                await _context.ToDoList.AddRangeAsync(toDoListTask.Result);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Initial data added to the database successfully.");
        }

        private static async Task<List<T>?> GetDataFromJsonAsync<T>(string fullPath)
        {
            using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var data = await JsonSerializer.DeserializeAsync<List<T>>(stream);
            return data;
        }
    }

}
