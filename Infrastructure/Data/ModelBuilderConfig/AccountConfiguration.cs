using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelBuilderConfig
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Email);
            builder.HasOne(a => a.User)
                    .WithOne(u => u.Account)
                    .HasForeignKey<User>(u => u.AccountEmail)
                    .IsRequired();
            builder.HasOne(a => a.Staff)
                    .WithOne(s => s.Account)
                    .HasForeignKey<Staff>(s => s.AccountEmail)
                    .IsRequired();
            builder.HasOne(a => a.Role)
                    .WithMany(r => r.Accounts)
                    .HasForeignKey(a => a.RoleName);
        }
    }
}
