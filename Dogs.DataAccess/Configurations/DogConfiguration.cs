using Dogs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dogs.DataAccess.Configurations;
public class DogConfiguration : IEntityTypeConfiguration<DogEntity>
{
    public void Configure(EntityTypeBuilder<DogEntity> builder)
    {
        builder.HasKey(d => d.Name);
        builder.Property(d => d.Name).IsRequired();
        builder.Property(d => d.Color).IsRequired();
        builder.Property(d => d.TailLength).IsRequired();
        builder.Property(d => d.Weight).IsRequired();
    }
}
