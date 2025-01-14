namespace Backend.ViewModel
{
    public class EditRequisition
    {
        public Requisition Requisition { get; set; } = null!;

        public string command { get; set; } = null!;

        public int attemptCode { get; set; }
    }
}
