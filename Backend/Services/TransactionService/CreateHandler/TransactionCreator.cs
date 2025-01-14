using Backend.Services.VaultService;

namespace Backend.Services.TransactionService.CreateHandler
{
    public class TransactionCreator
    {
        private ICreateTransaction state = null!;

        public void setState(ICreateTransaction state) => this.state = state;

        public async Task<ServerResponse<Transaction>> request(BackendContext _db, IVault _vault, Vault vault, Transaction transaction, decimal cashAmount) => await state.CreateTransaction(_db, _vault, vault, transaction, cashAmount);
    }
}
