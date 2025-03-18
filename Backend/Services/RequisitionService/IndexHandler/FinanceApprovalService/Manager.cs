using Backend.Services.AuthService;

namespace Backend.Services.RequisitionService.IndexHandler.FinanceApprovalService
{
    public class Manager(BackendContext db, IAuth auth) : IFinanceApproval
    {
        private IFinanceApproval? nextOfficer;
        private BackendContext _db = db;
        private IAuth _auth = auth;

        public void SetNext(IFinanceApproval nextOfficer) => this.nextOfficer = nextOfficer;


        public async Task<ServerResponse<List<Requisition>>> GetRequisitionsForApproval(User loggedInUser)
        {
            List<Requisition> requisitions = new();
            if (_auth.ValidateUserRole("Manager"))
            {
                requisitions = await _db.Requisitions
                   .Include(a => a.Applicant)
                   .Include(gl => gl.Glaccount)
                   .Include(m => m.Manager)
                   .Include(mr => mr.ManagerRecommendation)
                   .Where(a => a.IsActive == true)
                   .Where(m => m.ManagerRecommendation != null && m.ManagerRecommendation.StatusId == Status.Recommended)
                   .Where(a => a.FinanceApproval == null)
                   .Where(ar => ar.AmountRequested < 1000 && ar.AmountRequested >= 500)
                   .AsNoTracking()
                   .ToListAsync();

                return requisitions == null ?
                    new ServerResponse<List<Requisition>>
                    {
                        Success = false,
                        Message = "System could not retrieve all of the requisitions for Finance manager to process. Please contact ICT."
                    } :
                    new ServerResponse<List<Requisition>>
                    {
                        Success = true,
                        Data = requisitions,
                        Message = "All of the requisitions have been retrieved for Finance manager to process."
                    };
            }
            else
            {
                if (nextOfficer != null)
                    return await nextOfficer!.GetRequisitionsForApproval(loggedInUser);
                else
                {
                    return new ServerResponse<List<Requisition>>
                    {
                        Success = false,
                        Message = "Error on Manager level"
                    };
                }
            }
        }
    }
}

