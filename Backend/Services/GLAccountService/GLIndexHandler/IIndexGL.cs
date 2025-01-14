namespace Backend.Services.GLAccountService.GLIndexHandler
{
    public interface IIndexGL
    {
        Task<ServerResponse<IEnumerable<Glaccount>>> GetGlAccounts(BackendContext db, User user);
    }
}
