using LandWebsite.Data.Entites;

namespace LandWebsite.Components.Pages.OwnerComponents
{
    public interface IOwnerServices
    {
        Task DeleteAsync(Owner owner);
        Task<Owner?> GetOwnerByownerId(Guid ownerId);
        Task<List<Owner>> GetOwners();
        Task<Owner> Upsert(Owner owner);
    }
}