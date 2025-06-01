using LandWebsite.Data.Entites;

namespace LandWebsite.Components.Pages.LandComponent
{
    public interface ILandServices
    {
        Task DeleteAsync(Land land);
        Task<Land?> GetLandBylandId(Guid landId);
        Task<List<Land>> GetLands();
        Task<List<Land>> GetLandsByOwnerId(Guid ownerId);
        Task<Land> Upsert(Land land);
    }
}
