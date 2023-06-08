using Microsoft.EntityFrameworkCore;
using xunit_autofixture.services.Interfaces;

namespace xunit_autofixture.services;

public class AppDbContext : DbContext, IAppDbContext
{
    public virtual DbSet<User> Users { get; set; }
}