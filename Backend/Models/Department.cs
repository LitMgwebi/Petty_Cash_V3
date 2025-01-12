using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Department")]
    public class Department
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();
    }
}
