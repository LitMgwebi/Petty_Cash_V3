using Backend.Services.VaultService;

namespace Backend.Services.TransactionService.CreateHandler
{
    public class DepositState(string userId, int requisitionId = 0) : ICreateTransaction
    {
        private readonly int requisitionId = requisitionId;
        private string userId = userId;

        public async Task<ServerResponse<Transaction>> CreateTransaction(BackendContext _db, IVault _vault, Vault vault, Transaction transaction, decimal cashAmount)
        {
            if (cashAmount < 0)
                return new ServerResponse<Transaction>
                {
                    Success = true,
                    Message = "Error, you cannot deposit an amount smaller than R1"
                };

            //transaction.RequisitionId = null;
            transaction.DepositorId = userId;
            transaction.Amount = cashAmount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = Transaction.Deposit;
            transaction.VaultId = 1;

            vault.CurrentAmount += transaction.Amount;
            ServerResponse<Vault> vaultResponse = await _vault.EditVault(vault);

            if (vaultResponse.Success == false)
            {
                return new ServerResponse<Transaction>
                {
                    Success = false,
                    Message = "Vault could not handle deposit request."
                };
            }

            await _db.Transactions.AddAsync(transaction);

            return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Transaction>
                    {
                        Success = false,
                        Message = "System could not record the new deposit."
                    } : new ServerResponse<Transaction>
                    {
                        Success = true,
                        Message = "System has successfully recorded the deposit."
                    };
        }
    }
}
