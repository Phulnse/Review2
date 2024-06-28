using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.ModelBuilderConfig
{
    internal class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.Property(t => t.Progress)
                .HasConversion(new EnumToStringConverter<TopicProgressEnum>());
            builder.Property(t => t.State)
                .HasConversion(new EnumToStringConverter<TopicStateEnum>());
            builder.HasIndex(t => t.Code).IsUnique();
            builder.HasOne(t => t.Creator)
                    .WithMany(u => u.Topics)
                    .HasForeignKey(t => t.CreatorId);
            builder.HasOne(t => t.Decider)
                    .WithMany(d => d.DecidedTopics)
                    .HasForeignKey(t => t.DeciderId);
            builder.Property(t => t.TopicName).HasMaxLength(100);
            builder.Property(t => t.Description).HasMaxLength(1000);
            builder.Property(t => t.ReasonOfDecision).HasMaxLength(500);
            builder.ToTable(t => t.HasTrigger("TR_Notify"));
        }
    }
}
