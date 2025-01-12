using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Document")]
    public class Document
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public DateTime DateUploaded { get; set; }
        public int RequisitionId { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(RequisitionId))]
        public virtual Requisition? Requisition { get; set; }

        public static readonly string Motivation = "motivation";
        public static readonly string Receipt = "receipt";
    }
}
