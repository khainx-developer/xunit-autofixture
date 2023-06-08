using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace xunit_autofixture.services.Interfaces;

public interface IAppDbContext
{
    EntityEntry<T> Entry<T>(T entity) where T : class;

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

    DbSet<T> Set<T>() where T : class;

    DbSet<User> Users { get; set; }
}