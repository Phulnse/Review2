using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelBuilderConfig
{
    internal class MemberReviewConfiguration : IEntityTypeConfiguration<MemberReview>
    {
        public void Configure(EntityTypeBuilder<MemberReview> builder)
        {
            builder.HasKey(m => new { m.UserId, m.TopicId });
            builder.HasIndex(m => m.Id).IsUnique();
        }
    }
}
