using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        public int TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; } = null!;

        public DateTime TransactionDate { get; set; }

        public int? RequisitionId { get; set; }

        public string? DepositorId { get; set; }

        public int VaultId { get; set; }

        public string? Note { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(RequisitionId))]
        public virtual Requisition? Requisition { get; set; }

        [ForeignKey(nameof(DepositorId))]
        public virtual User? Depositor { get; set; }

        [ForeignKey(nameof(VaultId))]
        public virtual Vault? Vault { get; set; }

        public static readonly string Withdrawal = "Withdrawal";
        public static readonly string All = "All";
        public static readonly string Change = "Change";
        public static readonly string Deposit = "Deposit";
        public static readonly string Reimbursement = "Reimbursement";
    }
}
