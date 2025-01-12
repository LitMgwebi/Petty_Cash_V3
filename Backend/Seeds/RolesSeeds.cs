using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Seeds
{
    public class RolesSeeds : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "c303538f-3fd6-4fc1-974c-d94c07ba1391",
                    Name = "Super_User",
                    NormalizedName = "Super_User"
                },
                new IdentityRole
                {
                    Id = "37ce7a5a-9260-405c-9dd0-b8f4a32156fd",
                    Name = "Manager",
                    NormalizedName = "Manager",
                },
                new IdentityRole
                {
                    Id = "68d5c727-9ae8-401a-8c2c-1cebb5e78735",
                    Name = "GM_Manager",
                    NormalizedName = "GM_Manager"
                },
                new IdentityRole
                {
                    Id = "1a69126a-3658-44b3-9b2b-1732d0ce9e1a",
                    Name = "ICT_Admin",
                    NormalizedName = "ICT_Admin"
                },
                new IdentityRole
                {
                    Id = "24e9d163-c600-42db-92ca-594fdc639e58",
                    Name = "Cashier",
                    NormalizedName = "Cashier",
                },
                new IdentityRole
                {
                    Id = "f50b76c7-3bba-4edb-93d4-eef4af92a9ab",
                    Name = "HR_Admin",
                    NormalizedName = "HR_Admin",
                },
                new IdentityRole
                {
                    Id = "fd1d6d8f-9e0f-49e1-a569-746fc8eaa6f6",
                    Name = "Finance_Admin",
                    NormalizedName = "Finance_Admin",
                },
                new IdentityRole
                {
                    Id = "b139cc03-eb14-45a2-a560-8415006211a1",
                    Name = "PA_Admin",
                    NormalizedName = "PA_Admin",
                },
                new IdentityRole
                {
                    Id = "bd88b1a9-2e95-4167-88d2-7c0d6b204f44",
                    Name = "CEO_Admin",
                    NormalizedName = "CEO_Admin",
                },
                new IdentityRole
                {
                    Id = "6bd427b1-62c9-425b-86ed-a1f69d2d570b",
                    Name = "SCM_Admin",
                    NormalizedName = "SCM_Admin",
                },
                new IdentityRole
                {
                    Id = "b69328a6-ad18-4ae3-bc96-a69816cd3a1d",
                    Name = "Employee",
                    NormalizedName = "Employee",
                },
                new IdentityRole
                {
                    Id = "3531888a-9e52-4f49-aca7-e85fe0705c33",
                    Name = "DEEC_Admin",
                    NormalizedName = "DEEC_Admin",
                },
                new IdentityRole
                {
                    Id = "50b0ecd5-fb64-4724-9190-bc9953ccd7b5",
                    Name = "SRM_Admin",
                    NormalizedName = "SRM_Admin",
                },
                new IdentityRole
                {
                    Id = "o36538f-4lk6-4fc1-974c-d94c07ba1391",
                    Name = "Executive",
                    NormalizedName = "Executive",
                },
                new IdentityRole
                {
                    Id = "3yt427c9-62c9-425b-86ed-a1f69d2d603k",
                    Name = "Senior Employee",
                    NormalizedName = "Senior Employee",
                }

                );
        }
    }
}
