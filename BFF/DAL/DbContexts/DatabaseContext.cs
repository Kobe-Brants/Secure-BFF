using Dal.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Dal.DbContexts;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserTypeConfiguration).Assembly, x => x.Namespace != null && x.Namespace.StartsWith("Dal.EntityConfigurations"));
    }
}