using Backend.Services.AuthService;

namespace Backend.Services.RequisitionService.IndexHandler.RecommendationStructureService
{
    public class Deputy(BackendContext db, IAuth auth) : IRecommender
    {
        private IRecommender? nextRecommender;
        private BackendContext _db = db;
        private IAuth _auth = auth;

        public void SetNext(IRecommender? nextRecommender) => this.nextRecommender = nextRecommender;

        public async Task<ServerResponse<List<Requisition>>> GetRequisitionsForRecommendation(User loggedInUser)
        {
            ServerResponse<List<Requisition>> response = new();
            List<Requisition> requisitions = new();

            if (_auth.ValidateUserRole("Deputy") && loggedInUser.DivisionId == Division.FIN)
            {
                requisitions = await _db.Requisitions
                .Include(a => a.Applicant)
                .Include(m => m.Documents)
                .Include(gl => gl.Glaccount)
                .Where(nm => (nm.NeedsMotivation == true && nm.Documents.Where(g => g.IsActive).Any()) || nm.NeedsMotivation == false)
                .Where(a => a.IsActive == true)
                .Where(u => u.Applicant!.Id != loggedInUser.Id)
                .Where(d => d.Applicant!.DivisionId == loggedInUser.DivisionId)
                    .Where(a => a.ManagerRecommendation == null && a.FinanceApproval == null)
                    .AsNoTracking()
                    .ToListAsync();

                return requisitions == null ?
                    new ServerResponse<List<Requisition>>
                    {
                        Success = false,
                        Message = "System could not retrieve all of the requisitions for Finance to recommend. Please contact ICT."
                    } :
                    new ServerResponse<List<Requisition>>
                    {
                        Success = true,
                        Data = requisitions,
                        Message = "All of the requisitions have been retrieved for Deputy to recommend."
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
