namespace Backend.Models
{
    public class AccountSet
    {
        public int AccountSetId { get; set; }

        public string AccountNumber { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Glaccount> MainAccounts { get; set; } = new List<Glaccount>();
        [JsonIgnore]
        public virtual ICollection<Glaccount> SubAccounts { get; set; } = new List<Glaccount>();

        public static readonly string MainAccount = "MainAccount";
        public static readonly string SubAccount = "SubAccount";
    }
}
