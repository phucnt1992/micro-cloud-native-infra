using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MicroTodo.Domain.Entities;

namespace MicroTodo.Infra.Persistence.Configurations;

abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedOn)
            .HasColumnName("created_on")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("timezone('utc', now())");

        builder.Property(x => x.ModifiedOn)
            .HasColumnName("modified_on")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("timezone('utc', now())");

        builder.Property(x => x.Version)
            .HasColumnName("version")
            .IsConcurrencyToken();
    }
}
