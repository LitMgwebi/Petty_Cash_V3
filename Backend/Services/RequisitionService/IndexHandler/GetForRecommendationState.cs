using Backend.Services.AuthService;
using Backend.Services.RequisitionService.IndexHandler.RecommendationStructureService;

namespace Backend.Services.RequisitionService.IndexHandler
{
    public class GetForRecommendationState(IAuth auth, User user) : IIndexState
    {
        private readonly User user = user;
        private readonly IAuth _auth = auth;
        public async Task<ServerResponse<List<Requisition>>> GetRequisitions(BackendContext db)
        {
            ServerResponse<List<Requisition>> response = new();

            IRecommender GM = new GeneralManager();
            IRecommender Manager = new Manager();
            IRecommender Deputy = new Deputy();

            GM.SetNext(Manager);
            Manager.SetNext(Deputy);

            response = await GM.GetRequisitionsForRecommendation(user, db, _auth);

            return response;
        }
    }
}
