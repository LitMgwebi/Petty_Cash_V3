namespace Backend.Services.RequisitionService.IndexHandler.RecommendationStructureService
{
    public class Manager(BackendContext db) : IRecommender
    {
        private IRecommender nextRecommender = null!;
        private BackendContext _db = db;

        public void SetNext(IRecommender nextRecommender) => this.nextRecommender = nextRecommender;

        public async Task<ServerResponse<List<Requisition>>> GetRequisitionsForRecommendation(User loggedInUser)
        {
            ServerResponse<List<Requisition>> response = new();
            List<Requisition> requisitions = new();

            if (loggedInUser.JobTitleId == JobTitle.Manager)
            {
                requisitions = await _db.Requisitions
                   .Include(a => a.Applicant)
                   .Include(m => m.Documents)
                   .Include(gl => gl.Glaccount)
                   .Where(a => a.IsActive == true)
                   .Where(u => u.Applicant!.Id != loggedInUser.Id)
                   .Where(nm => (nm.NeedsMotivation == true && nm.Documents.Where(g => g.IsActive).Any()) || nm.NeedsMotivation == false)
                   .Where(d => d.Applicant!.DivisionId == loggedInUser.DivisionId && d.Applicant!.JobTitleId != JobTitle.Manager)
                   .Where(a => a.ManagerRecommendation == null && a.FinanceApproval == null)
                   .AsNoTracking()
                   .ToListAsync();

                return requisitions == null ?
                    new ServerResponse<List<Requisition>>
                    {
                        Success = false,
                        Message = "System could not retrieve all of the requisitions for Manager to recommend. Please contact ICT."
                    } :
                    new ServerResponse<List<Requisition>>
                    {
                        Success = true,
                        Data = requisitions,
                        Message = "All of the requisitions have been retrieved for Manager to recommend."
                    };
            }
            else
            {
                if (nextRecommender != null)
                    return await nextRecommender.GetRequisitionsForRecommendation(loggedInUser);
                else
                {
                    return new ServerResponse<List<Requisition>>
                    {
                        Success = false,
                        Message = "Error when retrieving requisitions to recommend on Manager level."
                    };
                }
            }
        }
    }
}
