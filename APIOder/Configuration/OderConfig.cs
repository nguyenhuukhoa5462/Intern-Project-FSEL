using APIOder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIOder.Configuration
{
    public class OderConfig : IEntityTypeConfiguration<Oder>
    {
        public void Configure(EntityTypeBuilder<Oder> builder)
        {
            builder.ToTable("Oder");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IdCustomer).IsRequired();
            builder.Property(x => x.OderDate).IsRequired();
        }
    }
}
