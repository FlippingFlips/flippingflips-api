using FF.Core.Models;
using FF.Domain.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FF.Infrastructure.Data.Config
{
    public class ScoreConfig : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Points).IsRequired();
            builder.Property(b => b.GamePlayedId).IsRequired();
        }
    }
}
