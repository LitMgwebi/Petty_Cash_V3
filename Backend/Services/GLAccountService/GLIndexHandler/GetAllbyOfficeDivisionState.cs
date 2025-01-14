namespace Backend.Services.GLAccountService.GLIndexHandler
{
    public class GetAllbyOfficeDivisionState : IIndexGL
    {
        public async Task<ServerResponse<IEnumerable<Glaccount>>> GetGlAccounts(BackendContext db, User user)
        {
            IEnumerable<Glaccount> glAccounts = await db.Glaccounts
                .Where(d => d.DivisionId == user.DivisionId)
                .Where(o => o.OfficeId == user.OfficeId)
                .Where(x => x.IsActive == true)
                .AsNoTracking()
                .ToListAsync();

            return glAccounts == null ?
                new ServerResponse<IEnumerable<Glaccount>>
                {
                    Success = false,
                    Message = "System could not find any GL accounts in your department."
                } :
                new ServerResponse<IEnumerable<Glaccount>>
                {
                    Success = true,
                    Message = "GL Accounts retrieved in your department successfully."
                };
        }
    }
}
