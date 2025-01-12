using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class DepartmentSeeds : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(
                new Department
                {
                    DepartmentId = 1,
                    Name = "CEO Office"
                },
                new Department
                {
                    DepartmentId = 2,
                    Name = "CFO Office"
                },
                new Department
                {
                    DepartmentId = 3,
                    Name = "Governance"
                },
                new Department
                {
                    DepartmentId = 4,
                    Name = "Regulatory Compliance"
                },
                new Department
                {
                    DepartmentId = 5,
                    Name = "Corporate Services"
                },
                new Department
                {
                    DepartmentId = 6,
                    Name = "Trade"
                }
                );
        }
    }
}
