using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.API;

namespace TodoApp.IntegrationTests.Fixtures
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationTestConfig = new ConfigurationBuilder()
                    .AddJsonFile("integrationtestsettings.json")
                    .Build();

                config.AddConfiguration(integrationTestConfig);
            });
        }
    }
}
