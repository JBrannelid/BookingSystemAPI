using BookingSystemAPI.Data;
using BookingSystemAPI.Endpoints;
using BookingSystemAPI.Middleware;
using Microsoft.EntityFrameworkCore;

namespace BookingSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Container for Db Connection
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //Middleware API KEY
            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseAuthorization();

            // All Endpoints
            CustomerEndpoints.RegisterEndpoints(app);
            EmployeeEndpoints.RegisterEndpoints(app);
            BookingEndpoints.RegisterEndpoints(app);

            app.Run();
        }
    }
}