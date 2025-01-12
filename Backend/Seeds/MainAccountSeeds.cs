using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class MainAccountSeeds : IEntityTypeConfiguration<MainAccount>
    {
        public void Configure(EntityTypeBuilder<MainAccount> builder)
        {
            builder.HasData(
                new MainAccount
                {
                    AccountSetId = 1,
                    AccountNumber = "2013",
                    Name = "Insurance",
                    IsActive = true,
                },
                new MainAccount
                {
                    AccountSetId = 2,
                    AccountNumber = "2012",
                    Name = "Inspection",
                    IsActive = true,
                },
                new MainAccount
                {
                    AccountSetId = 3,
                    AccountNumber = "2007",
                    Name = "Domestic Travel",
                    IsActive = true,
                },
                new MainAccount
                {
                    AccountSetId = 4,
                    AccountNumber = "2031",
                    Name = "Staff Renumeration",
                    IsActive = true,
                },
                new MainAccount
                {
                    AccountSetId = 5,
                    AccountNumber = "2017",
                    Name = "Legal Fees",
                    IsActive = true,
                },
                new MainAccount
                {
                    AccountSetId = 6,
                    AccountNumber = "2080",
                    Name = "Support Services",
                    IsActive = true,
                },
                new MainAccount
                {
                    AccountSetId = 7,
                    AccountNumber = "2038",
                    Name = "Training and Development",
                    IsActive = true,
                },
                new MainAccount
                {
                    AccountSetId = 8,
                    AccountNumber = "2035",
                    Name = "Telecommunication",
                    IsActive = true,
                },
                new MainAccount
                {
                    AccountSetId = 9,
                    AccountNumber = "2011",
                    Name = "Hospitality",
                    IsActive = true,
                }
                );
        }
    }
}
