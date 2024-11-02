using Dogs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dogs.DataAccess;

public class DogsDbContext : DbContext
{
    public DogsDbContext(DbContextOptions<DogsDbContext> options) : base(options)
    {
    }

    public DbSet<DogEntity> Dogs { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DogsDbContext).Assembly);
    }
}
