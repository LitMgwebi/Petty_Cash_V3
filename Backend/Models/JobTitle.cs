using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("JobTitle")]
    public class JobTitle
    {
        public int JobTitleId { get; set; }
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; } = true;


        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        public static readonly int CEO = 1;
        public static readonly int GeneralManager = 2;
        public static readonly int Manager = 3;
        public static readonly int Staff = 4;
        public static readonly int Consultant = 5;
        public static readonly int ChairPerson = 6;
        public static readonly int BoardMember = 7;
    }
}
