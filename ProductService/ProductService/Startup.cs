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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProductService.Data;
using ProductService.Services;

[assembly: WebJobsStartup(typeof(ProductService.Startup))]
namespace ProductService
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                    .AddEnvironmentVariables().Build();

            var connStr = configuration.GetConnectionString("SystemDbConnectionString");
            builder.Services.AddDbContext<ProductContext>(
              option => option.UseSqlServer(connStr));
            builder.Services.AddHttpClient();
            builder.Services.RegisterHandlers(Assembly.GetExecutingAssembly());
            builder.Services.AddSingleton<IMediator, Mediator>();
            builder.Services.AddSingleton<IMessageDeliveryService, MessageDeliveryService>();
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
            var dbContext = scope.ServiceProvider.GetService<ProductContext>();

            dbContext.Database.Migrate();
        }
    }
}
