using Backend.Seeds;

namespace Backend.Models
{
    public class BackendContext : IdentityDbContext<User>
    {
        public BackendContext()
        {
        }

        public BackendContext(DbContextOptions<BackendContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Division> Divisions { get; set; }

        public virtual DbSet<JobTitle> JobTitles { get; set; }

        public virtual DbSet<Glaccount> Glaccounts { get; set; }

        public virtual DbSet<AccountSet> AccountSets { get; set; }

        public virtual DbSet<Office> Offices { get; set; }

        public virtual DbSet<Branch> Branches { get; set; }

        public virtual DbSet<Requisition> Requisitions { get; set; }

        public virtual DbSet<Status> Statuses { get; set; }

        public virtual DbSet<Document> Documents { get; set; }

        public virtual DbSet<Vault> Vaults { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Motivation>();
            modelBuilder.Entity<Receipt>();
            //modelBuilder.Entity<MainAccount>();
            //modelBuilder.Entity<SubAccount>();
            //modelBuilder.Entity<AccountSet>()
            //    .UseTphMappingStrategy();
            modelBuilder.Entity<Glaccount>(entity =>
            {
                entity.ToTable("GLAccount");

                entity.Property(e => e.GlaccountId).HasColumnName("GLAccountID");
                entity.Property(e => e.IsActive).HasColumnName("isActive");
                entity.Property(e => e.MainAccountId).HasColumnName("MainAccountID");
                entity.Property(e => e.BranchId).HasColumnName("BranchID");
                entity.Property(e => e.SubAccountId).HasColumnName("SubAccountID");

                entity.HasOne(d => d.MainAccount).WithMany(p => p.MainAccounts)
                    .HasForeignKey(d => d.MainAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLAccount_MainAccount");

                entity.HasOne(d => d.Branch).WithMany(p => p.Glaccounts)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLAccount_Purpose");

                entity.HasOne(d => d.SubAccount).WithMany(p => p.SubAccounts)
                    .HasForeignKey(d => d.SubAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLAccount_SubAccount");
            });

            modelBuilder.Entity<Office>(entity =>
            {
                entity.ToTable("Office");

                entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
                entity.Property(e => e.IsActive).HasColumnName("isActive");
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.ToTable("Purpose");

                entity.Property(e => e.BranchId).HasColumnName("BranchID");
                entity.Property(e => e.IsActive).HasColumnName("isActive");
            });

            modelBuilder.Entity<Requisition>(entity =>
            {
                entity.ToTable("Requisition");

                entity.Property(e => e.RequisitionId).HasColumnName("RequisitionID");
                entity.Property(e => e.AmountRequested).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ApplicantId).HasColumnName("ApplicantID");
                entity.Property(e => e.CashIssued).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Change).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.FinanceApprovalId).HasColumnName("FinanceApprovalID");
                entity.Property(e => e.FinanceOfficerId).HasColumnName("FinanceOfficerID");
                entity.Property(e => e.GlaccountId).HasColumnName("GLAccountID");
                entity.Property(e => e.IsActive).HasColumnName("isActive");
                entity.Property(e => e.IssuerId).HasColumnName("IssuerID");
                entity.Property(e => e.ManagerRecommendationId).HasColumnName("ManagerApprovalID");
                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
                entity.Property(e => e.TotalExpenses).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Applicant).WithMany(p => p.Applicants)
                    .HasForeignKey(d => d.ApplicantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Requisition_User");

                entity.HasOne(d => d.FinanceApproval).WithMany(p => p.FinanceApprovals)
                    .HasForeignKey(d => d.FinanceApprovalId)
                    .HasConstraintName("FK_Requisition_Status1");

                entity.HasOne(d => d.FinanceOfficer).WithMany(p => p.FinanceOfficers)
                    .HasForeignKey(d => d.FinanceOfficerId)
                    .HasConstraintName("FK_Requisition_User1");

                entity.HasOne(d => d.Glaccount).WithMany(p => p.Requisitions)
                    .HasForeignKey(d => d.GlaccountId)
                    .HasConstraintName("FK_Requisition_GLAccount");

                entity.HasOne(d => d.Issuer).WithMany(p => p.Issuers)
                    .HasForeignKey(d => d.IssuerId)
                    .HasConstraintName("FK_Requisition_User3");

                entity.HasOne(d => d.ManagerRecommendation).WithMany(p => p.ManagerRecommendations)
                    .HasForeignKey(d => d.ManagerRecommendationId)
                    .HasConstraintName("FK_Requisition_Status");

                entity.HasOne(d => d.State).WithMany(p => p.StatesofRequisition)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK_Requisition_Status2");

                entity.HasOne(d => d.Manager).WithMany(p => p.Managers)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK_Requisition_User2");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
                entity.Property(e => e.Description).HasMaxLength(50);
                entity.Property(e => e.IsActive).HasColumnName("isActive");
            });

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BranchSeeds());
            modelBuilder.ApplyConfiguration(new DivisionSeeds());
            modelBuilder.ApplyConfiguration(new DepartmentSeeds());
            modelBuilder.ApplyConfiguration(new OfficeSeeds());
            modelBuilder.ApplyConfiguration(new AccountSetSeeds());
            modelBuilder.ApplyConfiguration(new MainAccountSeeds());
            modelBuilder.ApplyConfiguration(new SubAccountSeeds());
            modelBuilder.ApplyConfiguration(new RolesSeeds());
            modelBuilder.ApplyConfiguration(new StatusSeeds());
            modelBuilder.ApplyConfiguration(new JobTitleSeeds());
            modelBuilder.ApplyConfiguration(new DocumentSeeds());
            modelBuilder.ApplyConfiguration(new VaultSeeds());
        }
    }
}
