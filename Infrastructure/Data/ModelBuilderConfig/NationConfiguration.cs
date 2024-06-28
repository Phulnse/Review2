using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelBuilderConfig
{
    public class NationConfiguration : IEntityTypeConfiguration<Nation>
    {
        public void Configure(EntityTypeBuilder<Nation> builder)
        {
            builder.HasKey(n => n.NationName);
        }
    }
}
