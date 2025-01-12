namespace Backend.Models
{
    public class Branch
    {
        public int BranchId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Glaccount> Glaccounts { get; set; } = new List<Glaccount>();
    }
}
