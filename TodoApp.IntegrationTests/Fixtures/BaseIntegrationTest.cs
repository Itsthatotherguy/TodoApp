using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Database.Contexts;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.IntegrationTests.Fixtures
{
    public class BaseIntegrationTest : IClassFixture<ApiWebApplicationFactory>, IDisposable
    {
        protected readonly ApiWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        protected readonly TodoDbContext _dbContext;

        private readonly IConfiguration _configuration;

        public BaseIntegrationTest(ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("integrationtestsettings.json")
                .Build();

            var dbContextOptions = new DbContextOptionsBuilder<TodoDbContext>()
                .UseSqlServer(_configuration.GetConnectionString("Application"))
                .Options;

            _dbContext = new TodoDbContext(dbContextOptions);
            _dbContext.Database.Migrate();

            _dbContext.Todos.AddRange(new Todo[]
            {
                new Todo { Title = "Walk the dogs", Description = "The dogs like to take a walk around the block", DueDate = DateTime.Today },
                new Todo { Title = "Wash the dishes", Description = "The dishes are starting to pile up", DueDate = DateTime.Today.AddDays(1) }
            });
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
