using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class OfficeSeeds : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasData(
                new Office
                {
                    OfficeId = 1,
                    Name = "JHB",
                    Description = "Johannesburg",
                    IsActive = true,
                },
                new Office
                {
                    OfficeId = 2,
                    Name = "KIM",
                    Description = "Kimberely",
                    IsActive = true,
                },
                new Office
                {
                    OfficeId = 3,
                    Name = "CPT",
                    Description = "Cape Town",
                    IsActive = true,
                },
                new Office
                {
                    OfficeId = 4,
                    Name = "DBN",
                    Description = "Durban",
                    IsActive = true,
                }
                );
        }
    }
}
