using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Data
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContext CreateDbContext(string[] args)
        {
            //var configuration = new ConfigurationBuilder()
            //      .AddEnvironmentVariables().Build();

            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("local.settings.json")
               .Build();
            var settingsSection = configuration.GetConnectionString("SystemDbConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            //optionsBuilder.UseSqlServer("Server=IN-LT-17539;Database=userservicedb;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(settingsSection);


            return new UserContext(optionsBuilder.Options);
        }
    }
}
