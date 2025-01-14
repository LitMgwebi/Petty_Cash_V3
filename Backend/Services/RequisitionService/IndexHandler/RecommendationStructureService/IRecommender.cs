namespace Backend.Services.RequisitionService.IndexHandler.RecommendationStructureService
{
    public interface IRecommender
    {
        Task<ServerResponse<List<Requisition>>> GetRequisitionsForRecommendation(User user);

        void SetNext(IRecommender next);
    }
}
