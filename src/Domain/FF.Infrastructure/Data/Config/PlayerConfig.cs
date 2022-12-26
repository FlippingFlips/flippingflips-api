using FF.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FF.Infrastructure.Data.Config
{
    public class PlayerConfig : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Initials).IsRequired().HasMaxLength(4);
            builder.Property(b => b.Name).HasMaxLength(20);
            builder.Property(b => b.ApplicationUserId).HasMaxLength(40);
        }
    }
}
