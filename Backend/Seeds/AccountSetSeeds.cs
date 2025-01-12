using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class AccountSetSeeds : IEntityTypeConfiguration<AccountSet>
    {
        public void Configure(EntityTypeBuilder<AccountSet> builder)
        {
            builder.HasDiscriminator<string>("AccountType")
                .HasValue<MainAccount>("MainAccount")
                .HasValue<SubAccount>("SubAccount");

            //builder.HasData(
            //    new MainAccount
            //    {
            //        AccountSetId = 1,
            //        AccountNumber = "2013",
            //        Name = "Insurance",
            //        IsActive = true,
            //    },
            //    new SubAccount
            //    {
            //        AccountSetId = 10,
            //        AccountNumber = "0206",
            //        Name = "IT Audit",
            //        IsActive = true,
            //    }
            //);
        }
    }
}
