namespace Backend.Models
{
    public partial class Office
    {
        public int OfficeId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Glaccount> Glaccounts { get; set; } = new List<Glaccount>();
    }
}
