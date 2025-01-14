namespace Backend.Services.GLAccountService
{
    public interface IGLAccount
    {
        public Task<ServerResponse<IEnumerable<Glaccount>>> GetGlAccounts(string command, string userId = "", int division = 0);
        public Task<ServerResponse<Glaccount>> GetGlAccount(int id);
        public Task<ServerResponse<Glaccount>> CreateGlAccount(Glaccount glAccount);
        public Task<ServerResponse<Glaccount>> EditGlAccount(Glaccount glAccount);
        public Task<ServerResponse<Glaccount>> DeleteGlAccount(Glaccount glAccount);
    }
}
