using Microsoft.EntityFrameworkCore;
using vCore.Domain.Tenants;

namespace vCore.Infrastructure.Persistence;

public sealed class VCoreDbContext : DbContext
{
    public VCoreDbContext(DbContextOptions<VCoreDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TenantEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
