using FactOrFictionCommon.Models;
using FactOrFictionFrontend.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactOrFictionFrontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.Migrate();

                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                    CreateRoles(roleManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        private static async Task CreateRoles(RoleManager<ApplicationRole> roleManager)
        {
            var roles = new List<ApplicationRole>
            {
                // These are just the roles I made up. You can make your own!
                new ApplicationRole {Name = ApplicationRole.ADMINISTRATOR},
                new ApplicationRole {Name = ApplicationRole.USER}
            };

            foreach (var role in roles)
            {
                if (await roleManager.RoleExistsAsync(role.Name)) continue;
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded) continue;

                throw new ApplicationException($"Could not create '{role.Name}' role.");
            }
        }
    }
}
