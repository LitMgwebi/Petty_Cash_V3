using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class SubAccountSeeds : IEntityTypeConfiguration<SubAccount>
    {
        public void Configure(EntityTypeBuilder<SubAccount> builder)
        {
            builder.HasData(
                new SubAccount
                {
                    AccountSetId = 10,
                    AccountNumber = "0206",
                    Name = "IT Audit",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 11,
                    AccountNumber = "0045",
                    Name = "Meeting Fees",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 12,
                    AccountNumber = "0001",
                    Name = "Accomodation",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 13,
                    AccountNumber = "0006",
                    Name = "Basic Salaries",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 14,
                    AccountNumber = "0034",
                    Name = "Housing",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 15,
                    AccountNumber = "0101",
                    Name = "Membership Fees",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 16,
                    AccountNumber = "0094",
                    Name = "System Support",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 17,
                    AccountNumber = "0002",
                    Name = "Air travel",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 18,
                    AccountNumber = "0066",
                    Name = "Shuttle and Taxi Service",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 19,
                    AccountNumber = "0044",
                    Name = "Medical Aid",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 20,
                    AccountNumber = "0010",
                    Name = "Cellphones and Data",
                    IsActive = true,
                },
                new SubAccount
                {
                    AccountSetId = 21,
                    AccountNumber = "0086",
                    Name = "Vehicle Rental",
                    IsActive = true,
                }
                );
        }
    }
}
