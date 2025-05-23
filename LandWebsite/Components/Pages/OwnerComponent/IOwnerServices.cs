using LandWebsite.Data.Entites;

namespace LandWebsite.Components.Pages.OwnerComponents
{
    public interface IOwnerServices
    {
        void Delete(Owner owner);
        Owner GetOwnerById(Guid ownerid);
        List<Owner> GetOwners();
        Owner Save(Owner owner);
        Owner Update(Owner owner);
    }
}