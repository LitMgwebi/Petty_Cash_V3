using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class JobTitleSeeds : IEntityTypeConfiguration<JobTitle>
    {
        public void Configure(EntityTypeBuilder<JobTitle> builder)
        {
            builder.HasData(
                new JobTitle
                {
                    JobTitleId = 1,
                    Description = "CEO"
                },
                new JobTitle
                {
                    JobTitleId = 2,
                    Description = "General Manager"
                },
                new JobTitle
                {
                    JobTitleId = 3,
                    Description = "Manager"
                },
                new JobTitle
                {
                    JobTitleId = 4,
                    Description = "Staff"
                },
                new JobTitle
                {
                    JobTitleId = 5,
                    Description = "Consultant"
                },
                new JobTitle
                {
                    JobTitleId = 6,
                    Description = "Chair Person"
                },
                new JobTitle
                {
                    JobTitleId = 7,
                    Description = "Board Member"
                }
                );
        }
    }
}
