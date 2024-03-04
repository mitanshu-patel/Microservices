using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Services;

[assembly: WebJobsStartup(typeof(UserService.Startup))]
namespace UserService
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                    .AddEnvironmentVariables().Build();

            var connStr = configuration.GetConnectionString("SystemDbConnectionString");
            //builder.Services.AddDbContext<UserContext>(
            //  option => option.UseSqlServer("Server=IN-LT-17539;Database=userservicedb;Trusted_Connection=True;"));

            builder.Services.AddDbContext<UserContext>(
              option => option.UseSqlServer(connStr));
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IMediator, Mediator>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.AddExtension<DbSeedConfigProvider>();
        }
    }

    [Extension("DbSeed")]
    public class DbSeedConfigProvider : IExtensionConfigProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbSeedConfigProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<UserContext>();

            dbContext.Database.Migrate();
        }
    }
}
