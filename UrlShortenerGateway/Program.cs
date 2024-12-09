//using Ocelot.DependencyInjection;
//using Ocelot.Middleware;

//var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//builder.Services.AddOcelot(builder.Configuration);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", builder =>
//        builder.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader());
//});

//builder.Services.AddControllers();


//var app = builder.Build();
//// Use CORS
//app.UseCors("AllowAll");

//// Other middleware like routing, authentication, etc.
//app.UseRouting();

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();

//await app.UseOcelot();

//app.Run();

using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace UrlShortenerGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Ocelot configuration
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot();

            // Add CORS for development purposes
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();


            // Use CORS
            app.UseCors("AllowAll");

            // Use Ocelot middleware
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}
