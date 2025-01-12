using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSets",
                columns: table => new
                {
                    AccountSetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSets", x => x.AccountSetId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "JobTitle",
                columns: table => new
                {
                    JobTitleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitle", x => x.JobTitleId);
                });

            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    OfficeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.OfficeID);
                });

            migrationBuilder.CreateTable(
                name: "Purpose",
                columns: table => new
                {
                    BranchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purpose", x => x.BranchID);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Option = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    IsRecommended = table.Column<bool>(type: "bit", nullable: false),
                    IsState = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Vault",
                columns: table => new
                {
                    VaultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vault", x => x.VaultId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Division",
                columns: table => new
                {
                    DivisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Division", x => x.DivisionId);
                    table.ForeignKey(
                        name: "FK_Division_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Idnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    JobTitleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Division_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Division",
                        principalColumn: "DivisionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_JobTitle_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitle",
                        principalColumn: "JobTitleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Office_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Office",
                        principalColumn: "OfficeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GLAccount",
                columns: table => new
                {
                    GLAccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainAccountID = table.Column<int>(type: "int", nullable: false),
                    SubAccountID = table.Column<int>(type: "int", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    NeedsMotivation = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLAccount", x => x.GLAccountID);
                    table.ForeignKey(
                        name: "FK_GLAccount_Division_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Division",
                        principalColumn: "DivisionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GLAccount_MainAccount",
                        column: x => x.MainAccountID,
                        principalTable: "AccountSets",
                        principalColumn: "AccountSetId");
                    table.ForeignKey(
                        name: "FK_GLAccount_Office_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Office",
                        principalColumn: "OfficeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GLAccount_Purpose",
                        column: x => x.BranchID,
                        principalTable: "Purpose",
                        principalColumn: "BranchID");
                    table.ForeignKey(
                        name: "FK_GLAccount_SubAccount",
                        column: x => x.SubAccountID,
                        principalTable: "AccountSets",
                        principalColumn: "AccountSetId");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requisition",
                columns: table => new
                {
                    RequisitionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountRequested = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashIssued = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicantID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GLAccountID = table.Column<int>(type: "int", nullable: false),
                    FinanceOfficerID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ManagerID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ManagerRecommendationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ManagerApprovalID = table.Column<int>(type: "int", nullable: true),
                    FinanceApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinanceApprovalID = table.Column<int>(type: "int", nullable: true),
                    NeedsMotivation = table.Column<bool>(type: "bit", nullable: false),
                    IssuerID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApplicantCode = table.Column<int>(type: "int", nullable: false),
                    ConfirmApplicantCode = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmChangeReceived = table.Column<bool>(type: "bit", nullable: false),
                    ReceiptReceived = table.Column<bool>(type: "bit", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Stage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: true),
                    ManagerComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinanceComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicantComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmReimbursement = table.Column<bool>(type: "bit", nullable: false),
                    ReimbursementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReimburserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisition", x => x.RequisitionID);
                    table.ForeignKey(
                        name: "FK_Requisition_GLAccount",
                        column: x => x.GLAccountID,
                        principalTable: "GLAccount",
                        principalColumn: "GLAccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requisition_Status",
                        column: x => x.ManagerApprovalID,
                        principalTable: "Status",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK_Requisition_Status1",
                        column: x => x.FinanceApprovalID,
                        principalTable: "Status",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK_Requisition_Status2",
                        column: x => x.StateId,
                        principalTable: "Status",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK_Requisition_User",
                        column: x => x.ApplicantID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requisition_User1",
                        column: x => x.FinanceOfficerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requisition_User2",
                        column: x => x.ManagerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requisition_User3",
                        column: x => x.IssuerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequisitionId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Document_Requisition_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "Requisition",
                        principalColumn: "RequisitionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequisitionId = table.Column<int>(type: "int", nullable: true),
                    DepositorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VaultId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_AspNetUsers_DepositorId",
                        column: x => x.DepositorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_Requisition_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "Requisition",
                        principalColumn: "RequisitionID");
                    table.ForeignKey(
                        name: "FK_Transaction_Vault_VaultId",
                        column: x => x.VaultId,
                        principalTable: "Vault",
                        principalColumn: "VaultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountSets",
                columns: new[] { "AccountSetId", "AccountNumber", "AccountType", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "2013", "MainAccount", null, true, "Insurance" },
                    { 2, "2012", "MainAccount", null, true, "Inspection" },
                    { 3, "2007", "MainAccount", null, true, "Domestic Travel" },
                    { 4, "2031", "MainAccount", null, true, "Staff Renumeration" },
                    { 5, "2017", "MainAccount", null, true, "Legal Fees" },
                    { 6, "2080", "MainAccount", null, true, "Support Services" },
                    { 7, "2038", "MainAccount", null, true, "Training and Development" },
                    { 8, "2035", "MainAccount", null, true, "Telecommunication" },
                    { 9, "2011", "MainAccount", null, true, "Hospitality" },
                    { 10, "0206", "SubAccount", null, true, "IT Audit" },
                    { 11, "0045", "SubAccount", null, true, "Meeting Fees" },
                    { 12, "0001", "SubAccount", null, true, "Accomodation" },
                    { 13, "0006", "SubAccount", null, true, "Basic Salaries" },
                    { 14, "0034", "SubAccount", null, true, "Housing" },
                    { 15, "0101", "SubAccount", null, true, "Membership Fees" },
                    { 16, "0094", "SubAccount", null, true, "System Support" },
                    { 17, "0002", "SubAccount", null, true, "Air travel" },
                    { 18, "0066", "SubAccount", null, true, "Shuttle and Taxi Service" },
                    { 19, "0044", "SubAccount", null, true, "Medical Aid" },
                    { 20, "0010", "SubAccount", null, true, "Cellphones and Data" },
                    { 21, "0086", "SubAccount", null, true, "Vehicle Rental" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a69126a-3658-44b3-9b2b-1732d0ce9e1a", null, "ICT_Admin", "ICT_Admin" },
                    { "24e9d163-c600-42db-92ca-594fdc639e58", null, "Cashier", "Cashier" },
                    { "3531888a-9e52-4f49-aca7-e85fe0705c33", null, "DEEC_Admin", "DEEC_Admin" },
                    { "37ce7a5a-9260-405c-9dd0-b8f4a32156fd", null, "Manager", "Manager" },
                    { "3yt427c9-62c9-425b-86ed-a1f69d2d603k", null, "Senior Employee", "Senior Employee" },
                    { "50b0ecd5-fb64-4724-9190-bc9953ccd7b5", null, "SRM_Admin", "SRM_Admin" },
                    { "68d5c727-9ae8-401a-8c2c-1cebb5e78735", null, "GM_Manager", "GM_Manager" },
                    { "6bd427b1-62c9-425b-86ed-a1f69d2d570b", null, "SCM_Admin", "SCM_Admin" },
                    { "b139cc03-eb14-45a2-a560-8415006211a1", null, "PA_Admin", "PA_Admin" },
                    { "b69328a6-ad18-4ae3-bc96-a69816cd3a1d", null, "Employee", "Employee" },
                    { "bd88b1a9-2e95-4167-88d2-7c0d6b204f44", null, "CEO_Admin", "CEO_Admin" },
                    { "c303538f-3fd6-4fc1-974c-d94c07ba1391", null, "Super_User", "Super_User" },
                    { "f50b76c7-3bba-4edb-93d4-eef4af92a9ab", null, "HR_Admin", "HR_Admin" },
                    { "fd1d6d8f-9e0f-49e1-a569-746fc8eaa6f6", null, "Finance_Admin", "Finance_Admin" },
                    { "o36538f-4lk6-4fc1-974c-d94c07ba1391", null, "Executive", "Executive" }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DepartmentId", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, null, true, "CEO Office" },
                    { 2, null, true, "CFO Office" },
                    { 3, null, true, "Governance" },
                    { 4, null, true, "Regulatory Compliance" },
                    { 5, null, true, "Corporate Services" },
                    { 6, null, true, "Trade" }
                });

            migrationBuilder.InsertData(
                table: "JobTitle",
                columns: new[] { "JobTitleId", "Description", "IsActive" },
                values: new object[,]
                {
                    { 1, "CEO", true },
                    { 2, "General Manager", true },
                    { 3, "Manager", true },
                    { 4, "Staff", true },
                    { 5, "Consultant", true },
                    { 6, "Chair Person", true },
                    { 7, "Board Member", true }
                });

            migrationBuilder.InsertData(
                table: "Office",
                columns: new[] { "OfficeID", "Description", "isActive", "Name" },
                values: new object[,]
                {
                    { 1, "Johannesburg", true, "JHB" },
                    { 2, "Kimberely", true, "KIM" },
                    { 3, "Cape Town", true, "CPT" },
                    { 4, "Durban", true, "DBN" }
                });

            migrationBuilder.InsertData(
                table: "Purpose",
                columns: new[] { "BranchID", "Description", "isActive", "Name" },
                values: new object[,]
                {
                    { 1, "Administration", true, "ADM" },
                    { 2, "Regulatory Compliance", true, "RGC" },
                    { 3, "Diamond Trade", true, "DMT" },
                    { 4, null, true, "ZZZ" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "StatusID", "Description", "isActive", "IsApproved", "IsRecommended", "IsState", "Option" },
                values: new object[,]
                {
                    { 1, "Approved", true, true, false, false, "Approve" },
                    { 2, "Declined", true, true, false, false, "Decline" },
                    { 3, "Recommended", true, false, true, false, "Recommend" },
                    { 4, "Rejected", true, false, true, false, "Reject" },
                    { 5, "In Process", true, false, false, true, "Process" },
                    { 6, "Open", true, false, false, true, "Open" },
                    { 7, "Issued", true, false, false, true, "Issue" },
                    { 8, "Returned", true, false, false, true, "Return" },
                    { 9, "Closed", true, false, false, true, "Close" },
                    { 10, "Deleted", true, false, false, true, "Delete" },
                    { 11, "Reimbursed", true, false, false, true, "Reimburse" }
                });

            migrationBuilder.InsertData(
                table: "Vault",
                columns: new[] { "VaultId", "CurrentAmount", "IsActive", "Note" },
                values: new object[] { 1, 10000m, true, null });

            migrationBuilder.InsertData(
                table: "Division",
                columns: new[] { "DivisionId", "DepartmentId", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 4, "Inspectorate", true, "INS" },
                    { 2, 5, "Information Communication Technology", true, "ICT" },
                    { 3, 3, "Legal", true, "LEG" },
                    { 4, 5, "Human Resources", true, "HRE" },
                    { 6, 5, "Finance", true, "FIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DivisionId",
                table: "AspNetUsers",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobTitleId",
                table: "AspNetUsers",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OfficeId",
                table: "AspNetUsers",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Division_DepartmentId",
                table: "Division",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_RequisitionId",
                table: "Document",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccount_BranchID",
                table: "GLAccount",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccount_DivisionId",
                table: "GLAccount",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccount_MainAccountID",
                table: "GLAccount",
                column: "MainAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccount_OfficeId",
                table: "GLAccount",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccount_SubAccountID",
                table: "GLAccount",
                column: "SubAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Requisition_ApplicantID",
                table: "Requisition",
                column: "ApplicantID");

            migrationBuilder.CreateIndex(
                name: "IX_Requisition_FinanceApprovalID",
                table: "Requisition",
                column: "FinanceApprovalID");

            migrationBuilder.CreateIndex(
                name: "IX_Requisition_FinanceOfficerID",
                table: "Requisition",
                column: "FinanceOfficerID");

            migrationBuilder.CreateIndex(
                name: "IX_Requisition_GLAccountID",
                table: "Requisition",
                column: "GLAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Requisition_IssuerID",
                table: "Requisition",
                column: "IssuerID");

            migrationBuilder.CreateIndex(
                name: "IX_Requisition_ManagerApprovalID",
                table: "Requisition",
                column: "ManagerApprovalID");

            migrationBuilder.CreateIndex(
                name: "IX_Requisition_ManagerID",
                table: "Requisition",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Requisition_StateId",
                table: "Requisition",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DepositorId",
                table: "Transaction",
                column: "DepositorId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_RequisitionId",
                table: "Transaction",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_VaultId",
                table: "Transaction",
                column: "VaultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Requisition");

            migrationBuilder.DropTable(
                name: "Vault");

            migrationBuilder.DropTable(
                name: "GLAccount");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AccountSets");

            migrationBuilder.DropTable(
                name: "Purpose");

            migrationBuilder.DropTable(
                name: "Division");

            migrationBuilder.DropTable(
                name: "JobTitle");

            migrationBuilder.DropTable(
                name: "Office");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
