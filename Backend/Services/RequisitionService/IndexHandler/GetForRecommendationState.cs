using Backend.Services.AuthService;
using Backend.Services.RequisitionService.IndexHandler.RecommendationStructureService;
using Microsoft.CodeAnalysis.Recommendations;

namespace Backend.Services.RequisitionService.IndexHandler
{
    public class GetForRecommendationState(IAuth auth, User user) : IIndexState
    {
        private readonly User user = user;
        private readonly IAuth _auth = auth;
        public async Task<ServerResponse<List<Requisition>>> GetRequisitions(BackendContext db)
        {
            ServerResponse<List<Requisition>> response = new();

            IRecommender GM = new GeneralManager(db);
            IRecommender Manager = new Manager(db);
            IRecommender Deputy = new Deputy(db, _auth);

            GM.SetNext(Manager);
            Manager.SetNext(Deputy);

            response = await GM.GetRequisitionsForRecommendation(user);

            return response;
        }
    }
}
