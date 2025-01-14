using Backend.Services.AuthService;
using Backend.Services.GLAccountService;
using Backend.Services.JobTitleService;
using Backend.Services.StatusService;
using Backend.Services.TransactionService;
using Backend.Services.UserService;
using System.Security.Claims;
using Backend.Services.RequisitionService.IndexHandler;
using Backend.Services.RequisitionService.EditHandler;
using Backend.Services.RequisitionService.CreateHandler;

namespace Backend.Services.RequisitionService
{
    public class RequisitionService(BackendContext db, IUser user, IGLAccount gLAccount, IJobTitle jobTitle, ITransaction transaction, IStatus status, IHttpContextAccessor httpContextAccessor, IAuth auth) : IRequisition
    {
        private BackendContext _db = db;
        private readonly IAuth _auth = auth;
        private readonly IUser _user = user;
        private readonly ITransaction _transaction = transaction;
        private readonly IGLAccount _glAccount = gLAccount;
        private readonly IJobTitle _jobTitle = jobTitle;
        private readonly IStatus _status = status;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private string GetUserId() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        private string GetUserRole() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role)!;

        public async Task<ServerResponse<List<Requisition>>> GetRequisitions(string command, int divisionId, int jobTitleId, int statusId, string issuedState)
        {
            try
            {
                ServerResponse<List<Requisition>> response = new();
                Indexer indexer = new();

                string userId = GetUserId();
                string role = GetUserRole();
                User user = await _user.GetUserById(userId);

                if (command == Requisition.getStates.All)
                {
                    indexer.setState(new GetAllState());
                    response = await indexer.request(_db);
                }
                else if (command == Requisition.getStates.ForOne)
                {
                    indexer.setState(new GetForApplicantState(userId));
                    response = await indexer.request(_db);
                }
                else if (command == Requisition.getStates.Recommendation)
                {
                    indexer.setState(new GetForRecommendationState(_auth, user));
                    response = await indexer.request(_db);
                }
                else if (command == Requisition.getStates.Approval)
                {
                    indexer.setState(new GetForApprovalState(_auth, user));
                    response = await indexer.request(_db);
                }
                else if (command == Requisition.getStates.Opened)
                {
                    indexer.setState(new GetAllOpenState(_auth));
                    response = await indexer.request(_db);
                }
                else if (command == Requisition.getStates.Issued)
                {
                    indexer.setState(new GetAllIssuedState(_auth));
                    response = await indexer.request(_db);
                }
                else if (command == Requisition.getStates.Returned)
                {
                    indexer.setState(new GetAllReturnedState(_auth));
                    response = await indexer.request(_db);
                }
                else if (command == Requisition.getStates.Receiving)
                {
                    indexer.setState(new GetForReimbursementState(_auth));
                    response = await indexer.request(_db);
                }
                else if (command == Requisition.getStates.Tracking)
                {
                    indexer.setState(new GetForTracking());
                    response = await indexer.request(_db);
                }
                else
                {
                    return new ServerResponse<List<Requisition>>
                    {
                        Success = false,
                        Message = "System could not resolve command when configuring Indexer."
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse<List<Requisition>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Requisition>> GetRequisition(int id)
        {
            try
            {
                Requisition requisition = await _db.Requisitions
                    .Include(m => m.ManagerRecommendation)
                    .Include(f => f.FinanceApproval)
                    .Include(m => m.Manager)
                    .Include(f => f.FinanceOfficer)
                    .Include(f => f.Issuer)
                    .Include(f => f.State)
                    .Include(z => z.Applicant)
                    .Include(gl => gl.Glaccount)
                    .Where(a => a.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.RequisitionId == id);

                return requisition == null ?
                    new ServerResponse<Requisition>
                    {
                        Success = false,
                        Message = "System could not retrieve the Requisition requested."
                    } :
                    new ServerResponse<Requisition>
                    {
                        Success = true,
                        Data = requisition,
                        Message = "System has retrieved requisition successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Requisition>> CreateRequisition(Requisition requisition)
        {
            ServerResponse<Requisition> response = new();
            try
            {
                ServerResponse<Glaccount> glaccountResponse = await _glAccount.GetGlAccount(requisition.GlaccountId);
                Glaccount glaccount = glaccountResponse.Data!;
                Creator creator = new();
                string userId = GetUserId();

                if (glaccount.NeedsMotivation == true || requisition.AmountRequested >= 2000)
                {
                    creator.setState(new RequireMotivationState());
                    response = await creator.request(_db, requisition, userId);
                }
                else if (glaccount.NeedsMotivation == false)
                {
                    creator.setState(new StandardCreateState());
                    response = await creator.request(_db, requisition, userId);
                }
                else
                {
                    return new ServerResponse<Requisition>
                    {
                        Success = false,
                        Message = "System could not resolve error within requisition creation."
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Requisition>> EditRequisition(Requisition requisition, string command, string userId, int attemptCode, bool forDoc, bool addDoc, bool deleteDoc)
        {
            try
            {
                ServerResponse<Requisition> response = new();
                Editor editor = new();

                ServerResponse<Requisition> requisitionResponse = await GetRequisition(requisition.RequisitionId);
                Requisition reviewRequisition = requisitionResponse.Data!;

                if (command == Requisition.editStates.Recommendation)
                {
                    editor.setState(new RecommendationState(reviewRequisition, userId));
                    response = await editor.request(_db, requisition);
                }
                else if (command == Requisition.editStates.Approval)
                {
                    editor.setState(new ApprovalState(reviewRequisition, userId));
                    response = await editor.request(_db, requisition);
                }
                else if (command == Requisition.editStates.Edit)
                {
                    editor.setState(new WholeRequisitionState(_glAccount, reviewRequisition));
                    response = await editor.request(_db, requisition);
                }
                else if (command == Requisition.editStates.Issuing)
                {
                    editor.setState(new IssuingState(_transaction, userId, attemptCode));
                    response = await editor.request(_db, requisition);
                }
                else if (command == Requisition.editStates.Expenses)
                {
                    editor.setState(new ExpensesState());
                    response = await editor.request(_db, requisition);
                }
                else if (command == Requisition.editStates.Return)
                {
                    editor.setState(new ReturnedState(reviewRequisition));
                    response = await editor.request(_db, requisition);
                }
                else if (command == Requisition.editStates.Close)
                {
                    editor.setState(new CloseState(_transaction));
                    response = await editor.request(_db, requisition);
                }
                else if (forDoc == true && addDoc == true)
                {
                    editor.setState(new AddDocumentState(command));
                    response = await editor.request(_db, requisition);
                }
                else if (forDoc == true && deleteDoc == true)
                {
                    editor.setState(new DeleteDocumentState(command));
                    response = await editor.request(_db, reviewRequisition);
                }
                else
                    return new ServerResponse<Requisition>
                    {
                        Success = false,
                        Message = "System could not resolve error within requisition editing."
                    };

                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Requisition>> DeleteRequisition(Requisition requisition)
        {
            try
            {
                if (requisition.FinanceApproval == null)
                {
                    requisition.CloseDate = DateTime.Now;
                    requisition.StateId = Status.Deleted;
                    requisition.IsActive = false;
                    _db.Requisitions.Update(requisition);
                    int result = await _db.SaveChangesAsync();

                    return await _db.SaveChangesAsync() == 0 ?
                        new ServerResponse<Requisition>
                        {
                            Success = false,
                            Message = "System could not delete the requested requisition."
                        } :
                        new ServerResponse<Requisition>
                        {
                            Success = true,
                            Message = "The requisition has been deleted from the system."
                        };
                }
                else
                {
                    return new ServerResponse<Requisition>
                    {
                        Success = false,
                        Message = "You cannot delete a requisition once finance has approved."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServerResponse<Requisition>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
