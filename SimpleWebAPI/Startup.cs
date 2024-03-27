using Microsoft.AspNetCore.Mvc;

namespace SimpleWebAPI
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true);

            builder.AddEnvironmentVariables();
            configuration = builder.Build();

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello!");
                });
            });
        }
    }

    public class MessageController
    {
        [HttpGet("message")]
        public IActionResult GetMessage()
        {
            return new JsonResult("Hi!");
        }

    }

    // person > add id (auto gen?)
}
