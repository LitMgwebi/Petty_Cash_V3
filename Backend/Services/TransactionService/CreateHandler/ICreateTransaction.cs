using Backend.Services.VaultService;

namespace Backend.Services.TransactionService.CreateHandler
{
    public interface ICreateTransaction
    {
        public Task<ServerResponse<Transaction>> CreateTransaction(BackendContext db, IVault _vault, Vault vault, Transaction transaction, decimal cashAmount);
    }
}
