namespace API.Settings
{
    public static class DI
    {
        public static void Add(IConfiguration conf, IServiceCollection service)
        {
            BusinessLayer.Settings.DI.Add(conf, service);
            AddSettingsREST(conf, service);
        }

        public static void AddSettingsEndpoints(WebApplication app)
        {
            var env = app.Environment;

            /*if (!env.IsProduction())
            {*/
                app.UseSwagger();
                app.UseSwaggerUI();
            /*}*/

            var corsSection = app.Configuration.GetSection("CORS");
            var corsChildren = corsSection.GetChildren().ToList();

            app.UseCors(options =>
            {
                foreach (var child in corsChildren)
                {
                    var url = child["URL"];

                    options.WithOrigins(url!)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            });

            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.MapControllers();
        }

        private static void AddSettingsREST(IConfiguration conf, IServiceCollection service)
        {
            service.AddControllersWithViews();
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();
        }
    }
}
