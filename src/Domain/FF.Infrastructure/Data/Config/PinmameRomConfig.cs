using FF.Domain.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FF.Infrastructure.Data.Config
{
    public class PinmameRomConfig : IEntityTypeConfiguration<PinmameRom>
    {
        public void Configure(EntityTypeBuilder<PinmameRom> builder)
        {
            builder.HasIndex(x => new {x.Id}).IsUnique();
            builder.Property(b => b.Id).HasMaxLength(64);
            builder.Property(b => b.ParentRom).HasMaxLength(64);
        }
    }
}
