namespace Backend.Services.RequisitionService.IndexHandler.FinanceApprovalService
{
    public interface IFinanceApproval
    {
        Task<ServerResponse<List<Requisition>>> GetRequisitionsForApproval(User user);
        void SetNext(IFinanceApproval financeOfficer);
    }
}
