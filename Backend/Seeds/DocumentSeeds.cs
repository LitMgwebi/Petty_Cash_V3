using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class DocumentSeeds : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasDiscriminator<string>("DocumentType")
                .HasValue<Motivation>("Motivation")
                .HasValue<Receipt>("Receipt");
        }
    }
}
