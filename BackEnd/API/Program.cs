using API.Settings;
using DataLayer.Interfaces;

var builder = WebApplication.CreateBuilder(args);

DI.Add(builder.Configuration, builder.Services);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await dbInitializer.InitializeDBAsync();
}

DI.AddSettingsEndpoints(app);

app.Run();
