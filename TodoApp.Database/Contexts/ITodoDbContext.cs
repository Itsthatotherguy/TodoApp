using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Database.Contexts
{
    public interface ITodoDbContext
    {
        DbSet<Todo> Todos { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}