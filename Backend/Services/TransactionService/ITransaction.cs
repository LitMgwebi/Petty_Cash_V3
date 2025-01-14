namespace Backend.Services.TransactionService
{
    public interface ITransaction
    {
        public Task<ServerResponse<IEnumerable<Transaction>>> GetTransactions(string type = "");
        public Task<ServerResponse<Transaction>> GetTransaction(int transactionId);
        public Task<ServerResponse<Transaction>> CreateTransaction(decimal cashIssued, string type, int requisitionId = 0, string note = "", string userId = "");
        public Task<ServerResponse<Transaction>> EditTransaction(Transaction transaction);
        public Task<ServerResponse<Transaction>> DeleteTransaction(Transaction transaction);
    }
}
