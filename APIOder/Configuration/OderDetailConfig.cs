using APIOder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIOder.Configuration
{
    public class OderDetailConfig : IEntityTypeConfiguration<OderDetail>
    {
        public void Configure(EntityTypeBuilder<OderDetail> builder)
        {
            builder.ToTable("OderDetail");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OderId).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.UnitPrice).IsRequired();
            builder.Property(x => x.ProductName).IsRequired();
            builder.HasOne(p => p.Oder).WithMany(p => p.OderDetails).HasForeignKey(p => p.OderId);
        }
    }
}
