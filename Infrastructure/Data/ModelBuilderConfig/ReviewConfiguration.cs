using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.ModelBuilderConfig
{
    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.State)
                .HasConversion(new EnumToStringConverter<ReviewStateEnum>());
            builder.HasKey(r => new
            {
                r.TopicId,
                r.State,
                r.ReportNumber,
            });
            builder.HasMany(r => r.Councils)
                .WithOne(c => c.Review)
                .HasPrincipalKey(r => r.Id);
            builder.HasMany(r => r.Documents)
                .WithOne(d => d.Review)
                .HasPrincipalKey(r => r.Id);
            builder.Property(r => r.DecisionOfCouncil)
                .HasConversion(new EnumToStringConverter<CouncilDecisionEnum>());
            builder.HasIndex(p => p.Id).IsUnique();
        }
    }
}
