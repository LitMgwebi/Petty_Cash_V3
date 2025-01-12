using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class BranchSeeds : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasData(
                new Branch
                {
                    BranchId = 1,
                    Name = "ADM",
                    Description = "Administration",
                    IsActive = true,
                },
                new Branch
                {
                    BranchId = 2,
                    Name = "RGC",
                    Description = "Regulatory Compliance",
                    IsActive = true,
                },
                new Branch
                {
                    BranchId = 3,
                    Name = "DMT",
                    Description = "Diamond Trade",
                    IsActive = true,
                },
                new Branch
                {
                    BranchId = 4,
                    Name = "ZZZ",
                    IsActive = true,
                }
            );
        }
    }
}
