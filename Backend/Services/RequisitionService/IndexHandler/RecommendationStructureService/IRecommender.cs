using Backend.Services.AuthService;

namespace Backend.Services.RequisitionService.IndexHandler.RecommendationStructureService
{
    public interface IRecommender
    {
        Task<ServerResponse<List<Requisition>>> GetRequisitionsForRecommendation(User user, BackendContext db, IAuth auth);

        void SetNext(IRecommender next);
    }
}
