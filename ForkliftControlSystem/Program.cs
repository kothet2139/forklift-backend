using ForkliftControlSystem.Api.Middleware;
using ForkliftControlSystem.Application.Interfaces;
using ForkliftControlSystem.Application.Interfaces.Repositories;
using ForkliftControlSystem.Application.Interfaces.Services;
using ForkliftControlSystem.Application.Services;
using ForkliftControlSystem.Infrastructure.Persistence;
using ForkliftControlSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ForkliftControlSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .Enrich.FromLogContext()
                        .CreateLogger();

            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddDbContext<ForkliftDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IForkliftRepository, ForkliftRepository>();
            builder.Services.AddScoped<IForkliftService, ForkliftService>();
            builder.Services.AddScoped<ICommandService, CommandService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.MapControllers();
            //You have to comment if you don't have database
            //await DatabaseInitializer.Initialize(app.Services);

            app.Run();
        }
    }
}
