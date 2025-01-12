namespace Backend.Models
{
    public partial class Status
    {
        public int StatusId { get; set; }

        public string? Option { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsRecommended { get; set; }

        public bool IsState { get; set; }

        public bool IsApproved { get; set; }

        [JsonIgnore]
        public virtual ICollection<Requisition> FinanceApprovals { get; set; } = new List<Requisition>();

        [JsonIgnore]
        public virtual ICollection<Requisition> ManagerRecommendations { get; set; } = new List<Requisition>();

        [JsonIgnore]
        public virtual ICollection<Requisition> StatesofRequisition { get; set; } = new List<Requisition>();

        //[JsonIgnore]
        //public virtual ICollection<Requisition> Statuses { get; set; } = new List<Requisition>();

        public static int Approved = 1;
        public static int Declined = 2;
        public static int Recommended = 3;
        public static int Rejected = 4;
        public static int InProcess = 5;
        public static int Open = 6;
        public static int Issued = 7;
        public static int Returned = 8;
        public static int Closed = 9;
        public static int Deleted = 10;
        public static int Reimbursed = 11;
    }
}
