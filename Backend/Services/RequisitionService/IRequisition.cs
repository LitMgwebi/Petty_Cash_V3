namespace Backend.Services.RequisitionService
{
    public interface IRequisition
    {
        public Task<ServerResponse<List<Requisition>>> GetRequisitions(string command);
        public Task<ServerResponse<Requisition>> GetRequisition(int id);
        public Task<ServerResponse<Requisition>> CreateRequisition(Requisition requisition);
        public Task<ServerResponse<Requisition>> EditRequisition(Requisition requisition, string command, int attemptCode = 0);
        public Task<ServerResponse<Requisition>> DeleteRequisition(Requisition requisition);
    }
}
