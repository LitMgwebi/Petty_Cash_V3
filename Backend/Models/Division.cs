using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Division")]
    public partial class Division
    {
        public int DivisionId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int DepartmentId { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        [JsonIgnore]
        public virtual ICollection<Glaccount> Glaccounts { get; set; } = new List<Glaccount>();

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        public static readonly int INS = 1;
        public static readonly int ICT = 2;
        public static readonly int LEG = 3;
        public static readonly int HRE = 4;
        public static readonly int FIN = 5;
    }
}
