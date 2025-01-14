using Backend.Services.UserService;

namespace Backend.Services.RequisitionService.IndexHandler
{
    public class GetForCloseState(IUser user, string userId) : IIndexState
    {
        private readonly IUser _user = user;
        private readonly string userId = userId;

        public async Task<ServerResponse<List<Requisition>>> GetRequisitions(BackendContext db)
        {
            User user = await _user.GetUserById(userId);
            List<Requisition> requisitions = new List<Requisition>();
            if (user.JobTitle!.JobTitleId == 16)
            {
                requisitions = await db.Requisitions
                    .Include(a => a.Applicant)
                    .Include(m => m.Manager)
                    .Include(f => f.FinanceOfficer)
                    .Include(i => i.Issuer)
                    .Include(gl => gl.Glaccount)
                    .Where(a => a.IsActive == true)
                    .Where(a => a.StateId == 8)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                return new ServerResponse<List<Requisition>>
                {
                    Success = false,
                    Message = "You have to be an Accounts Payable to view the requisitions that require closing."
                };
            }

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

