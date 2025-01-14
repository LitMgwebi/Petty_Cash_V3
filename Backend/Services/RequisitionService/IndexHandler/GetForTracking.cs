namespace Backend.Services.RequisitionService.IndexHandler
{
    public class GetForTracking : IIndexState
    {
        public async Task<ServerResponse<List<Requisition>>> GetRequisitions(BackendContext db)
        {
            List<Requisition> requisitions = await db.Requisitions
                    .Include(gl => gl.Glaccount)
                    .Include(a => a.Applicant)
                    .Include(fa => fa.FinanceApproval)
                    .Include(fo => fo.FinanceOfficer)
                    .Include(i => i.Issuer)
                    .Include(mr => mr.ManagerRecommendation)
                    .Include(m => m.Manager)
                    .Include(i => i.Issuer)
                    .Where(a => a.IsActive == true)
                    .Where(c => c.ManagerRecommendation != null && c.FinanceApproval != null && c.Issuer != null && c.ConfirmChangeReceived != true)
                    .AsNoTracking()
                    .ToListAsync();

            return requisitions == null ?
                    new ServerResponse<List<Requisition>>
                    {
                        Success = false,
                        Message = "System could not retrieve all of the requisitions for tracking. Please contact ICT."
                    } :
                    new ServerResponse<List<Requisition>>
                    {
                        Success = true,
                        Data = requisitions,
                        Message = "All of the requisitions for tracking have been retrieved."
                    };
        }
    }
}
