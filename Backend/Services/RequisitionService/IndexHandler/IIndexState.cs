namespace Backend.Services.RequisitionService.IndexHandler
{
    public interface IIndexState
    {
        Task<ServerResponse<List<Requisition>>> GetRequisitions(BackendContext db);
    }
}
