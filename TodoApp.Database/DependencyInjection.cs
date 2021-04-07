using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Database.Contexts;

namespace TodoApp.Database
{
    public static class DependencyInjection
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Application");

            services.AddDbContext<TodoDbContext>(opt => opt.UseSqlServer(connectionString));

            services.AddScoped<ITodoDbContext, TodoDbContext>();
        }
    }
}
