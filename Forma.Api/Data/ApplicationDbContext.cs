using Microsoft.EntityFrameworkCore;
using Forma.Api.Models;
using Forma.Api.Seeding;
using Forma.Api.Interfaces;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<FieldType> FieldTypes { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Seed();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var timestampedEntities = ChangeTracker.Entries<ITimestampedEntity>();

        var now = DateTime.UtcNow;

        foreach (var entry in timestampedEntities)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
