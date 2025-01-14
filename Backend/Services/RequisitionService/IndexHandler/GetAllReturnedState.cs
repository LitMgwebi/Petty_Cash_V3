using Backend.Services.AuthService;

namespace Backend.Services.RequisitionService.IndexHandler
{
    public class GetAllReturnedState(IAuth auth) : IIndexState
    {
        private readonly IAuth _auth = auth;

        public async Task<ServerResponse<List<Requisition>>> GetRequisitions(BackendContext db)
        {
            if (_auth.ValidateUserRole("Accounts_Payable"))
            {
                List<Requisition> requisitions = await db.Requisitions
                    .Include(a => a.Applicant)
                    .Include(m => m.Manager)
                    .Include(f => f.FinanceOfficer)
                    .Include(i => i.Issuer)
                    .Include(gl => gl.Glaccount)
                    .Where(a => a.IsActive == true)
                    .Where(a => a.StateId == Status.Returned)
                    .AsNoTracking()
                    .ToListAsync();

                return requisitions == null ?
                    new ServerResponse<List<Requisition>>
                    {
                        Success = false,
                        Message = "System could not retrieve all of the returned requisitions. Please contact ICT."
                    } :
                    new ServerResponse<List<Requisition>>
                    {
                        Success = true,
                        Data = requisitions,
                        Message = "All of the returned requisitions have been retrieved."
                    };
            }
            else
            {
                return new ServerResponse<List<Requisition>>
                {
                    Success = false,
                    Message = "You have to be an Accounts Payable to view the returned requisitions."
                };
            }
        }
    }
}
