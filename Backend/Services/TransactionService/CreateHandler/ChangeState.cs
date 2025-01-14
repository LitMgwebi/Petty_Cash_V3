using Backend.Services.VaultService;

namespace Backend.Services.TransactionService.CreateHandler
{
    public class ChangeState(int requisitionId) : ICreateTransaction
    {
        private readonly int requisitionId = requisitionId;

        public async Task<ServerResponse<Transaction>> CreateTransaction(BackendContext _db, IVault _vault, Vault vault, Transaction transaction, decimal cashAmount)
        {
            transaction.RequisitionId = requisitionId;
            transaction.Amount = cashAmount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = Transaction.Change;
            transaction.VaultId = 1;

            vault.CurrentAmount += transaction.Amount;
            ServerResponse<Vault> vaultResponse = await _vault.EditVault(vault);

            if (vaultResponse.Success == false)
            {
                return new ServerResponse<Transaction>
                {
                    Success = false,
                    Message = "Vault could not handle change request."
                };
            }

            await _db.Transactions.AddAsync(transaction);

            return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Transaction>
                    {
                        Success = false,
                        Message = "System could not register your change. Please contact ICT."
                    } : new ServerResponse<Transaction>
                    {
                        Success = true,
                        Message = "System has successfully recorded the deposit of change."
                    };
        }
    }
}