using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class DivisionSeeds : IEntityTypeConfiguration<Division>
    {
        public void Configure(EntityTypeBuilder<Division> builder)
        {
            builder.HasData(
                new Division
                {
                    DivisionId = 1,
                    Name = "INS",
                    Description = "Inspectorate",
                    DepartmentId = 4,
                    IsActive = true,
                },
                new Division
                {
                    DivisionId = 2,
                    Name = "ICT",
                    Description = "Information Communication Technology",
                    DepartmentId = 5,
                    IsActive = true,
                },
                new Division
                {
                    DivisionId = 3,
                    Name = "LEG",
                    Description = "Legal",
                    DepartmentId = 3,
                    IsActive = true,
                },
                new Division
                {
                    DivisionId = 4,
                    Name = "HRE",
                    Description = "Human Resources",
                    DepartmentId = 5,
                    IsActive = true,
                },
                new Division
                {
                    DivisionId = 6,
                    Name = "FIN",
                    Description = "Finance",
                    DepartmentId = 5,
                    IsActive = true,
                }
                );
        }
    }
}
