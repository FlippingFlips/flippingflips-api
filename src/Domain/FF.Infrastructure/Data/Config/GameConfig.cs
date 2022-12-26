using FF.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FF.Infrastructure.Data.Config
{
    public class GameConfig : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.Property(b => b.Id).HasMaxLength(40);
            builder.Property(b => b.Author).IsRequired();
            builder.Property(b => b.FileUrl).IsRequired();
            builder.Property(b => b.Title).IsRequired();
            builder.Property(b => b.Version).IsRequired();
            builder.Property(b => b.Year).IsRequired();            
            builder.HasIndex(x => new { x.Title, x.FileName, x.Version}).IsUnique();
            builder.Property(b => b.Description).HasMaxLength(250);
            builder.Property(b => b.FileUrl).HasMaxLength(150);
            builder.Property(b => b.FileName).HasMaxLength(150);
            builder.Property(b => b.FileNamePatched).HasMaxLength(150);
            builder.Property(b => b.FilePatchUrl).HasMaxLength(200);
            builder.Property(b => b.CRC).HasMaxLength(16);
            builder.Property(b => b.CRCPatched).HasMaxLength(16);            
        }
    }
}