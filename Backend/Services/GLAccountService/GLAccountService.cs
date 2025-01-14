using Backend.Models;
using Backend.Services.AccountSetService;
using Backend.Services.BranchService;
using Backend.Services.DivisonService;
using Backend.Services.GLAccountService.GLIndexHandler;
using Backend.Services.OfficeService;
using Backend.Services.UserService;

namespace Backend.Services.GLAccountService
{
    public class GLAccountService(BackendContext db, IDivision department, IAccountSet account, IBranch branch, IOffice office, IUser user, IDivision division): IGLAccount
    {
        private BackendContext _db = db;
        private readonly IDivision _department = department;
        private readonly IUser _user = user;
        private readonly IAccountSet _account = account;
        private readonly IBranch _branch = branch;
        private readonly IOffice _office = office;
        private readonly IDivision _division = division;

        public async Task<ServerResponse<IEnumerable<Glaccount>>> GetGlAccounts(string command, string userId = "", int divisionId = 0)
        {
            try
            {
                User user = new User();
                Division division = new Division();

                ServerResponse<IEnumerable<Glaccount>> glAccounts = new();
                GlIndexer indexer = new GlIndexer();

                if (command == "all")
                {
                    ServerResponse<Division> divisionResponse = await _division.GetDivision(divisionId);
                    division = divisionResponse.Data!;

                    indexer.setState(new GetAllState(division));
                    glAccounts = await indexer.request(_db);
                }
                else if (command == "division")
                {
                    user = await _user.GetUserById(userId);

                    indexer.setState(new GetAllbyDivisionState());
                    glAccounts = await indexer.request(_db, user);
                }
                else if (command == "office")
                {
                    indexer.setState(new GetAllbyOfficeDivisionState());
                    glAccounts = await indexer.request(_db);
                }
                else
                {
                    return new ServerResponse<IEnumerable<Glaccount>>
                    {
                        Success = false,
                        Message = "System could not resolve command."
                    };
                }

                return glAccounts;
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Glaccount>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Glaccount>> GetGlAccount(int id)
        {
            try
            {
                Glaccount glAccount = await _db.Glaccounts
                    .Where(a => a.IsActive == true)
                    .Include(x => x.Branch)
                    .Include(x => x.MainAccount)
                    .Include(x => x.SubAccount)
                    .Include(x => x.Division)
                    .Include(x => x.Office)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.GlaccountId == id);

                return glAccount == null ?
                    new ServerResponse<Glaccount>
                    {
                        Success = false,
                        Message = "System could not retrieve the GL account."
                    } :
                    new ServerResponse<Glaccount>
                    {
                        Success = true,
                        Message = "GL Account retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Glaccount>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Glaccount>> CreateGlAccount(Glaccount glAccount)
        {
            try
            {
                ServerResponse<AccountSet> mainAccountResponse = await _account.GetOneAccountSet(AccountSet.MainAccount, glAccount.MainAccountId);
                ServerResponse<AccountSet> subAccountResponse = await _account.GetOneAccountSet(AccountSet.SubAccount, glAccount.SubAccountId);
                ServerResponse<Division> divisionResponse = await _division.GetDivision(glAccount.DivisionId);
                ServerResponse<Branch> branchResponse = await _branch.GetBranch(glAccount.BranchId);
                ServerResponse<Office> officeResponse = await _office.GetOffice(glAccount.OfficeId);

                MainAccount mainAccount = (MainAccount)mainAccountResponse.Data!;
                SubAccount subAccount = (SubAccount)subAccountResponse.Data!;
                Division division = divisionResponse.Data!;
                Branch branch = branchResponse.Data!;
                Office office = officeResponse.Data!;
                glAccount.Description = $"{mainAccount.AccountNumber}/{subAccount.AccountNumber}/{division.Name}/{branch.Name}/{office.Name}";

                glAccount.Name = $"{division.Description} {mainAccount.Name} ({subAccount.Name})";

                ServerResponse<IEnumerable<Glaccount>> glAccountResponse = await GetGlAccounts(command: "all");
                IEnumerable<Glaccount> glAccounts = glAccountResponse.Data!;

                if (glAccounts.Select(x => x.Name).ToList().Contains(glAccount.Name))
                    new ServerResponse<Glaccount>
                    {
                        Success = false,
                        Message = $"System already contains GL Account with the name: {glAccount.Name}"
                    };

                await _db.Glaccounts.AddAsync(glAccount);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Glaccount>
                    {
                        Success = false,
                        Message = $"System could not add the new GL account: {glAccount.Name}."
                    } :
                    new ServerResponse<Glaccount>
                    {
                        Success = true,
                        Message = $"{glAccount.Name} added to database successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Glaccount>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Glaccount>> EditGlAccount(Glaccount glAccount)
        {
            try
            {
                ServerResponse<AccountSet> mainAccountResponse = await _account.GetOneAccountSet(AccountSet.MainAccount, glAccount.MainAccountId);
                ServerResponse<AccountSet> subAccountResponse = await _account.GetOneAccountSet(AccountSet.SubAccount, glAccount.SubAccountId);
                ServerResponse<Division> divisionResponse = await _division.GetDivision(glAccount.DivisionId);
                ServerResponse<Branch> branchResponse = await _branch.GetBranch(glAccount.BranchId);
                ServerResponse<Office> officeResponse = await _office.GetOffice(glAccount.OfficeId);

                glAccount.MainAccount = (MainAccount)mainAccountResponse.Data!;
                glAccount.SubAccount = (SubAccount)subAccountResponse.Data!;
                glAccount.Division = divisionResponse.Data!;
                glAccount.Branch = branchResponse.Data!;
                glAccount.Office = officeResponse.Data!;

                glAccount.Description = $"{glAccount.MainAccount!.AccountNumber}/{glAccount.SubAccount!.AccountNumber}/{glAccount.Division!.Name}/{glAccount.Branch.Name}/{glAccount.Office.Name}";
                glAccount.Name = $"{glAccount.Division.Description} {glAccount.MainAccount.Name} ({glAccount.SubAccount.Name})";

                _db.Glaccounts.Update(glAccount);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Glaccount>
                    {
                        Success = false,
                        Message = $"System could not edit {glAccount.Name}."
                    } :
                    new ServerResponse<Glaccount>
                    {
                        Success = true,
                        Message = $"{glAccount.Name} has successfully been edited."
                    };
            }
            catch (Exception ex) { return new ServerResponse<Glaccount> { Success = false, Message = ex.Message }; }
        }

        public async Task<ServerResponse<Glaccount>> DeleteGlAccount(Glaccount glAccount)
        {
            try
            {
                glAccount.IsActive = false;
                _db.Glaccounts.Update(glAccount);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Glaccount>
                    {
                        Success = false,
                        Message = $"System could not delete ${glAccount.Name}"
                    } :
                    new ServerResponse<Glaccount>
                    {
                        Success = true,
                        Message = $"{glAccount.Name} has successfully been deleted."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Glaccount>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
