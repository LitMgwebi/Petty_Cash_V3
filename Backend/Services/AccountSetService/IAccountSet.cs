namespace Backend.Services.AccountSetService
{
    public interface IAccountSet
    {
        Task<ServerResponse<IEnumerable<AccountSet>>> GetAllAccountSets(string command);
        Task<ServerResponse<AccountSet>> GetOneAccountSet(string command, int id);
        Task<ServerResponse<AccountSet>> CreateAccountSet(AccountSet account, string command);
        Task<ServerResponse<AccountSet>> EditAccountSet(AccountSet accountSet);
        Task<ServerResponse<AccountSet>> DeleteAccountSet(AccountSet accountSet);
    }
}
