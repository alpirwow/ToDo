using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configuration
{
    internal class PriorityConfiguration : IEntityTypeConfiguration<PriorityEntity>
    {
        public void Configure(EntityTypeBuilder<PriorityEntity> builder)
        {
            builder.ToTable("priorities");

            builder.HasKey(x => x.Level);

            builder.HasMany(x => x.ToDoItems)
                .WithOne(x => x.Priority)
                .HasForeignKey(x => x.PriorityLevel);
        }
    }
}
