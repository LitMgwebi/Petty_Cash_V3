namespace Backend.Services.GLAccountService.GLIndexHandler
{
    public class GlIndexer
    {
        private IIndexGL state = null!;

        public void setState(IIndexGL state) => this.state = state;

        public async Task<ServerResponse<IEnumerable<Glaccount>>> request(BackendContext db, User? user = null) => await state.GetGlAccounts(db, user!);
    }
}
