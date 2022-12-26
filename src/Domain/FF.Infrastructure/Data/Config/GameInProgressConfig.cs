using FF.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FF.Infrastructure.Data.Config
{
    public class GameInProgressConfig : IEntityTypeConfiguration<GameInProgress>
    {
        public void Configure(EntityTypeBuilder<GameInProgress> builder)
        {
            builder.Property(s => s.ApplicationUserId).IsRequired()
                .HasMaxLength(36);
            builder.Property(s => s.GameId).IsRequired().HasMaxLength(36);
            builder.Property(b => b.SystemVersion).HasMaxLength(16);
        }
    }
}