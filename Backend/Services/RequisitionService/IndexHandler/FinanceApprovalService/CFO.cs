namespace Backend.Services.RequisitionService.IndexHandler.FinanceApprovalService
{
    public class CFO(BackendContext db) : IFinanceApproval
    {
        private IFinanceApproval? nextOfficer;
        private BackendContext _db = db;

        public void SetNext(IFinanceApproval nextOfficer) => this.nextOfficer = nextOfficer;


        public async Task<ServerResponse<List<Requisition>>> GetRequisitionsForApproval(User loggedInUser)
        {
            List<Requisition> requisitions = new();
            if (loggedInUser.JobTitleId == JobTitle.GeneralManager && loggedInUser.DivisionId == Division.FIN)
            {
                requisitions = await _db.Requisitions
                           .Include(a => a.Applicant)
                           .Include(m => m.Manager)
                           .Include(mr => mr.ManagerRecommendation)
                            .Include(gl => gl.Glaccount)
                           .Where(a => a.IsActive == true)
                           .Where(m => m.ManagerRecommendation != null && m.ManagerRecommendation.StatusId == Status.Recommended)
                           .Where(a => a.FinanceApproval == null)
                           .Where(am => am.AmountRequested >= 1000)
                            .AsNoTracking()
                           .ToListAsync();

                return requisitions == null ?
                    new ServerResponse<List<Requisition>>
                    {
                        Success = false,
                        Message = "System could not retrieve all of the requisitions for CFO to process. Please contact ICT."
                    } :
                    new ServerResponse<List<Requisition>>
                    {
                        Success = true,
                        Data = requisitions,
                        Message = "All of the requisitions have been retrieved for CFO to process."
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
                        Message = "Error on CFO level"
                    };
                }
            }
        }
    }
}

