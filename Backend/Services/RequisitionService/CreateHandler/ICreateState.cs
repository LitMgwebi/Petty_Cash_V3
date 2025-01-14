namespace Backend.Services.RequisitionService.CreateHandler
{
    public interface ICreateState
    {
        public Task<ServerResponse<Requisition>> CreateRequisition(BackendContext db, Requisition requisition, string userId);
    }
}
