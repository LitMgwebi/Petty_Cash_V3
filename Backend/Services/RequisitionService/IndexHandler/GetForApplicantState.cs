namespace Backend.Services.RequisitionService.IndexHandler
{
    public class GetForApplicantState(string userId) : IIndexState
    {
        private readonly string userId = userId;
        public async Task<ServerResponse<List<Requisition>>> GetRequisitions(BackendContext db)
        {
            List<Requisition> requisitions = await db.Requisitions
                    .Include(gl => gl.Glaccount)
                    .Include(a => a.Applicant)
                    .Include(mr => mr.ManagerRecommendation)
                    .Include(mr => mr.FinanceApproval)
                    .Include(i => i.Issuer)
                    .Where(a => a.ApplicantId == userId)
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

            return requisitions == null ?
                new ServerResponse<List<Requisition>>
                {
                    Success = false,
                    Message = "Could not retrieve all of your requisitions."
                } :
                new ServerResponse<List<Requisition>>
                {
                    Success = true,
                    Data = requisitions,
                    Message = "Your requsitions retrieved successfully."
                };
        }
    }
}
