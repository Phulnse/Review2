using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.ModelBuilderConfig
{
    public class NotifyConfiguration : IEntityTypeConfiguration<Notify>
    {
        public void Configure(EntityTypeBuilder<Notify> builder)
        {
            builder.Property(x => x.State)
                    .HasConversion(new EnumToStringConverter<TopicStateEnum>());
            builder.Property(x => x.Progress)
                    .HasConversion(new EnumToStringConverter<TopicProgressEnum>());
        }
    }
}
