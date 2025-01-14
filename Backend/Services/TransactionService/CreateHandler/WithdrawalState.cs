using Backend.Services.VaultService;

namespace Backend.Services.TransactionService.CreateHandler
{
    public class WithdrawalState(int requisitionId) : ICreateTransaction
    {
        private readonly int requisitionId = requisitionId;

        public async Task<ServerResponse<Transaction>> CreateTransaction(BackendContext _db, IVault _vault, Vault vault, Transaction transaction, decimal cashAmount)
        {
            if (cashAmount > vault.CurrentAmount)
            {
                return new ServerResponse<Transaction>
                {
                    Success = true,
                    Message = $"System cannot process the withdrawal. Amount withdrawed is larger than the current amount left in the vault. Please replenish. Currently R{vault.CurrentAmount} left."
                };
            }

            transaction.RequisitionId = requisitionId;
            transaction.Amount = cashAmount * -1;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = Transaction.Withdrawal;
            transaction.VaultId = 1;

            vault.CurrentAmount += transaction.Amount;
            ServerResponse<Vault> vaultResponse = await _vault.EditVault(vault);

            if (vaultResponse.Success == false)
            {
                return new ServerResponse<Transaction>
                {
                    Success = false,
                    Message = "Vault could not handle withdrawal request."
                };
            }

            await _db.Transactions.AddAsync(transaction);

            return await _db.SaveChangesAsync() == 0 ?
                    new ServerResponse<Transaction>
                    {
                        Success = false,
                        Message = "System could not record the new withdrawal."
                    } : new ServerResponse<Transaction>
                    {
                        Success = true,
                        Message = "System has successfully recorded the withdrawal."
                    };
        }
    }
}
