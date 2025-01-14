using Backend.Services.AuthService;
using Backend.Services.RequisitionService.IndexHandler.FinanceApprovalService;

namespace Backend.Services.RequisitionService.IndexHandler
{
    public class GetForApprovalState(IAuth auth, User user) : IIndexState
    {
        private readonly User user = user;
        private readonly IAuth _auth = auth;
        public async Task<ServerResponse<List<Requisition>>> GetRequisitions(BackendContext db)
        {
            ServerResponse<List<Requisition>> response = new();
            if (user.DivisionId == Division.FIN)
            {

                IFinanceApproval Deputy = new Deputy(db, _auth);
                IFinanceApproval Manager = new Manager(db);
                IFinanceApproval CFO = new CFO(db);

                CFO.SetNext(Manager);
                Manager.SetNext(Deputy);

                response = await CFO.GetRequisitionsForApproval(user);
            }
            else
            {
                return new ServerResponse<List<Requisition>>
                {
                    Success = false,
                    Message = "You have to be in the Finance Department to approve of this requisitions."
                };
            }
            return response;
        }
    }
}

