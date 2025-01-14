namespace Backend.Services.RequisitionService
{
    public interface IRequisition
    {
        public Task<ServerResponse<List<Requisition>>> GetRequisitions(string command, int divisionId = 0, int jobTitleId = 0, int status = 0, string issuedState = "");
        public Task<ServerResponse<Requisition>> GetRequisition(int id);
        public Task<ServerResponse<Requisition>> CreateRequisition(Requisition requisition);
        public Task<ServerResponse<Requisition>> EditRequisition(Requisition requisition, string command, string userId = "", int attemptCode = 0, bool forDoc = false, bool addDoc = false, bool deleteDoc = false);
        public Task<ServerResponse<Requisition>> DeleteRequisition(Requisition requisition);
    }
}
