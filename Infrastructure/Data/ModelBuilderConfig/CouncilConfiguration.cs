using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelBuilderConfig
{
    internal class CouncilConfiguration : IEntityTypeConfiguration<Council>
    {
        public void Configure(EntityTypeBuilder<Council> builder)
        {
            builder.HasKey(c => new { c.UserId, c.ReviewId });
            builder.HasIndex(c => c.Id).IsUnique();
        }
    }
}
