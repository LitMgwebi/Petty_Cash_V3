using Backend.Services.TransactionService.CreateHandler;
using Backend.Services.VaultService;

namespace Backend.Services.TransactionService
{
    public class TransactionService(BackendContext db, IVault vault) : ITransaction
    {
        private BackendContext _db = db;
        private IVault _vault = vault;


        public async Task<ServerResponse<IEnumerable<Transaction>>> GetTransactions(string type = "")
        {
            try
            {
                IEnumerable<Transaction> transactions = new List<Transaction>();

                transactions = await _db.Transactions
                .Include(d => d.Depositor)
                .Include(r => r.Requisition)
                .ThenInclude(a => a!.Applicant)
                .Include(v => v.Vault)
                .Where(x => x.IsActive == true)
                .AsNoTracking()
                .ToListAsync();

                return transactions == null ?
                    new ServerResponse<IEnumerable<Transaction>>
                    {
                        Success = false,
                        Message = "System could not find any transactions."
                    } : new ServerResponse<IEnumerable<Transaction>>
                    {
                        Success = true,
                        Data = transactions,
                        Message = "Transactions retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Transaction>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Transaction>> GetTransaction(int transactionId)
        {
            try
            {
                Transaction? transaction = await _db.Transactions
                    .Include(d => d.Depositor)
                    .Include(r => r.Requisition)
                    .ThenInclude(a => a!.Applicant)
                    .Include(v => v.Vault)
                    .Where(x => x.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.TransactionId == transactionId);

                return transaction == null ?
                    new ServerResponse<Transaction>
                    {
                        Success = false,
                        Message = "System could not retrieve the transaction."
                    } : new ServerResponse<Transaction>
                    {
                        Success = true,
                        Data = transaction,
                        Message = $"Transaction #{transaction.TransactionId} retrieved successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Transaction>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Transaction>> CreateTransaction(decimal cashAmount, string type, int requisitionId, string note, string userId)
        {
            try
            {
                TransactionCreator creator = new TransactionCreator();
                ServerResponse<Transaction> response = new();
                Transaction transaction = new Transaction();
                ServerResponse<Vault> vaultResponse = await _vault.GetVault(1);

                Vault vault = vaultResponse.Data!;
                transaction.Note = note;

                if (type == Transaction.Withdrawal)
                {
                    creator.setState(new WithdrawalState(requisitionId));
                    response = await creator.request(_db, _vault, vault, transaction, cashAmount);
                }
                else if (type == Transaction.Deposit)
                {
                    creator.setState(new DepositState(userId, requisitionId));
                    response = await creator.request(_db, _vault, vault, transaction, cashAmount);
                }
                else if (type == Transaction.Change)
                {
                    creator.setState(new ChangeState(requisitionId));
                    response = await creator.request(_db, _vault, vault, transaction, cashAmount);
                }
                else if (type == Transaction.Reimbursement)
                {
                    creator.setState(new ReimbursementState(requisitionId));
                    response = await creator.request(_db, _vault, vault, transaction, cashAmount);
                }
                else
                    return new ServerResponse<Transaction>
                    {
                        Message = "System was unable to resolve type of transaction. Please contact ICT.",
                        Success = false
                    };
                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse<Transaction>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Transaction>> EditTransaction(Transaction transaction)
        {
            try
            {
                _db.Transactions.Update(transaction);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Transaction>
                    {
                        Success = false,
                        Message = $"System could not add edit transaction #{transaction.TransactionId}."
                    } : new ServerResponse<Transaction>
                    {
                        Success = true,
                        Message = $"Transaction #{transaction.TransactionId} edited successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Transaction>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Transaction>> DeleteTransaction(Transaction transaction)
        {
            try
            {
                transaction.IsActive = false;
                _db.Transactions.Update(transaction);

                return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Transaction>
                    {
                        Success = false,
                        Message = $"System could not add delete transaction #{transaction.TransactionId}."
                    } : new ServerResponse<Transaction>
                    {
                        Success = true,
                        Message = $"Transaction #{transaction.TransactionId} deleted successfully."
                    };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Transaction>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
