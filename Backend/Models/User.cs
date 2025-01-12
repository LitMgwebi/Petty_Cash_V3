namespace Backend.Models
{
    public partial class User : IdentityUser
    {
        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Idnumber { get; set; } = null!;

        public int DivisionId { get; set; }

        public int OfficeId { get; set; }

        public int JobTitleId { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual Division? Division { get; set; }

        public virtual Office? Office { get; set; }

        public virtual JobTitle? JobTitle { get; set; }

        [JsonIgnore]
        public virtual ICollection<Requisition> Applicants { get; set; } = new List<Requisition>();

        [JsonIgnore]
        public virtual ICollection<Requisition> FinanceOfficers { get; set; } = new List<Requisition>();

        [JsonIgnore]
        public virtual ICollection<Requisition> Issuers { get; set; } = new List<Requisition>();

        [JsonIgnore]
        public virtual ICollection<Requisition> Managers { get; set; } = new List<Requisition>();

        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        //[JsonIgnore]
        //public IList<UserRole>? UserRoles { get; set; }

        public string FullName
        {
            get
            {
                return $"{Firstname} {Lastname}";
            }
        }
    }
}
