using Backend.Services.AuthService;

namespace Backend.Services.RequisitionService.IndexHandler.FinanceApprovalService
{
    public class Deputy(BackendContext db, IAuth auth) : IFinanceApproval
    {
        //private IFinanceApproval nextOfficer = null!;
        private BackendContext _db = db;
        private IAuth _auth = auth;

        public void SetNext(IFinanceApproval nextOfficer) { }


        public async Task<ServerResponse<List<Requisition>>> GetRequisitionsForApproval(User loggedInUser)
        {
            List<Requisition> requisitions = new();
            if (_auth.ValidateUserRole("Deputy"))
            {
                requisitions = await _db.Requisitions
                    .Include(a => a.Applicant)
                    .Include(m => m.Manager)
                    .Include(mr => mr.ManagerRecommendation)
                    .Include(gl => gl.Glaccount)
                    .Where(a => a.IsActive == true)
                    .Where(m => m.ManagerRecommendation != null && m.ManagerRecommendation.StatusId == Status.Recommended)
                    .Where(a => a.FinanceApproval == null)
                    .Where(ar => ar.AmountRequested < 500)
                    .Where(m => m.Manager!.Id != loggedInUser.Id)
                    .AsNoTracking()
                    .ToListAsync();

                return requisitions == null ?
                       new ServerResponse<List<Requisition>>
                       {
                           Success = false,
                           Message = "System could not retrieve all of the requisitions for approval processing. Please contact ICT."
                       } :
                       new ServerResponse<List<Requisition>>
                       {
                           Success = true,
                           Data = requisitions,
                           Message = "All of the requisitions have been retrieved for Deputy to process."
                       };
            }
            else
            {
                return new ServerResponse<List<Requisition>>
                {
                    Success = false,
                    Message = "Error when retrieving requisitions to recommend on Deputy level."
                };
            }
        }
    }
}
