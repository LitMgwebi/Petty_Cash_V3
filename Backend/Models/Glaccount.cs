namespace Backend.Models
{
    public partial class Glaccount
    {
        public int GlaccountId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int MainAccountId { get; set; }

        public int SubAccountId { get; set; }

        public int BranchId { get; set; }

        public int DivisionId { get; set; }

        public int OfficeId { get; set; }

        public bool IsActive { get; set; } = true;

        public bool NeedsMotivation { get; set; }

        public virtual AccountSet? MainAccount { get; set; }

        public virtual Branch? Branch { get; set; }

        public virtual Division? Division { get; set; }

        public virtual Office? Office { get; set; }

        [JsonIgnore]
        public virtual ICollection<Requisition> Requisitions { get; set; } = new List<Requisition>();

        public virtual AccountSet? SubAccount { get; set; }
    }
}
