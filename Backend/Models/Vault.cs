using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Vault")]
    public class Vault
    {
        public int VaultId { get; set; }

        public decimal CurrentAmount { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Note { get; set; }

        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
