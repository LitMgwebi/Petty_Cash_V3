namespace Backend.Services.RequisitionService.IndexHandler
{
    public class GetAllState : IIndexState
    {
        public async Task<ServerResponse<List<Requisition>>> GetRequisitions(BackendContext db)
        {
            List<Requisition> requisitions = await db.Requisitions
                    .Include(gl => gl.Glaccount)
                    .Include(a => a.Applicant)
                    .Include(mr => mr.ManagerRecommendation)
                    .Include(i => i.Issuer)
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

            return requisitions == null ?
                new ServerResponse<List<Requisition>>
                {
                    Success = false,
                    Message = "Could not retrieve all of the requisitions."
                } :
                new ServerResponse<List<Requisition>>
                {
                    Success = true,
                    Data = requisitions,
                    Message = "Requsitions retrieved successfully."
                };
        }
    }
}
