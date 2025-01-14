namespace Backend.Services.RequisitionService.EditHandler
{
    public interface IEditState
    {
        Task<ServerResponse<Requisition>> EditRequisition(BackendContext db, Requisition requisition);
    }
}
