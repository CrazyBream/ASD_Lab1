using ASD_Lab1.BLL.Services;
using ASD_Lab1.DAL.Repositories;
using ASD_Lab1.PL.UI;
using ASD_Lab1.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ASD_Lab1.PL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var serviceProvider = new ServiceCollection()
                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ASD_Lab1_DB;Trusted_Connection=True;TrustServerCertificate=True;"))
                .AddScoped<IDeviceRepository, DeviceRepository>()
                .AddScoped<DeviceService>()
                .AddScoped<ConsoleInterface>()
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            var ui = serviceProvider.GetRequiredService<ConsoleInterface>();
            ui.RunDemo();
        }
    }
}