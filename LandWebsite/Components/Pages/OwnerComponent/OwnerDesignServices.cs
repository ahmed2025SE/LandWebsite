using LandWebsite.Components.Account.Pages.Manage;
using LandWebsite.Data.Entites;

namespace LandWebsite.Components.Pages.OwnerComponents
{
    public class OwnerDesignServices
    {
        public List<Owner> GetOwners()
        {
            return
              [

                new Owner
                {
                    ownerId = Guid.NewGuid(),
                    ownerName = "ahmed",
                    email = "ahmed@gmail.com",
                    phone = 092888333
                },

                 new Owner
                {
                    ownerId = Guid.NewGuid(),
                    ownerName = "ali",
                    email = "ali@gmail.com",
                    phone = 09127773
                } ,
                 
                 new Owner
                {
                    ownerId = Guid.NewGuid(),
                    ownerName = "krim",
                    email = "krim@gmail.com",
                    phone = 0934466
                },
                
                 new Owner
                {
                    ownerId = Guid.NewGuid(),
                    ownerName = "hmade",
                    email = "hmade@gmail.com",
                    phone = 02777778
                }
            ];


        }

    
    }

}