using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.ModelBuilderConfig
{
    public class ContractTypeConfiguration : IEntityTypeConfiguration<ContractType>
    {
        public void Configure(EntityTypeBuilder<ContractType> builder)
        {
            builder.HasKey(x => new
            {
                x.TypeName,
                x.State
            });
            builder.HasIndex(p => p.Id).IsUnique();
            builder.Property(x => x.State).HasConversion(new EnumToStringConverter<ContractStateEnum>());
        }
    }
}
