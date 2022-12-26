using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FF.Core.Models;

namespace FF.Infrastructure.Data.Config
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(b => b.Id).HasMaxLength(32);
            builder.Property(b => b.Email).IsRequired().HasMaxLength(64);
            builder.Property(b => b.UserName).IsRequired();
            builder.Property(b => b.MachName).HasMaxLength(32);
            builder.Property(b => b.MachDesc).HasMaxLength(250);
        }
    }
}
