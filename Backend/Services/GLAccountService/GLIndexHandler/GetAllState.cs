namespace Backend.Services.GLAccountService.GLIndexHandler
{
    public class GetAllState(Division division): IIndexGL
    {
        private readonly Division division = division;

        public async Task<ServerResponse<IEnumerable<Glaccount>>> GetGlAccounts(BackendContext db, User user)
        {
            IEnumerable<Glaccount> glAccounts = await db.Glaccounts
                    .Include(d => d.Division)
                    .Where(x => x.Division == division)
                    .Where(x => x.IsActive == true)
                    .AsNoTracking()
                    .ToListAsync();

            return glAccounts == null ?
                new ServerResponse<IEnumerable<Glaccount>>
                {
                    Success = false,
                    Message = "System could not find any GL accounts."
                } :
                new ServerResponse<IEnumerable<Glaccount>>
                {
                    Success = true,
                    Message = "GL Accounts retrieved by division successfully."
                };
        }
    }
}
