using FF.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FF.Infrastructure.Data.Config
{
    public class GamePlayedConfig : IEntityTypeConfiguration<GamePlayed>
    {
        public void Configure(EntityTypeBuilder<GamePlayed> builder)
        {
            builder.Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Property(s => s.ApplicationUserId).IsRequired().HasMaxLength(36);
            builder.Property(s => s.GameId).IsRequired().HasMaxLength(40);
            builder.Property(b => b.CRC).HasMaxLength(16);
            builder.Property(b => b.SystemVersion).HasMaxLength(16);
        }
    }
}