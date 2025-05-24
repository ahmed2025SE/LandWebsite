using LandWebsite.Components.Account.Pages.Manage;
using LandWebsite.Data;
using LandWebsite.Data.Entites;
using Microsoft.EntityFrameworkCore;

namespace LandWebsite.Components.Pages.OwnerComponents
{
    public class OwnerServices : IOwnerServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public OwnerServices(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {

            _dbContextFactory = dbContextFactory;

        }



        //-------------احذف صاحب العقار-------------------
        public async Task DeleteAsync(Owner owner)
        {
            var _dbContext = _dbContextFactory.CreateDbContext();
            // Find the existing owner by ID
            var existingOwner = _dbContext.Owners.Find(owner.ownerId);
            // If the owner exists, remove it from the database
            if (existingOwner != null)
            {
                // Remove the owner from the DbSet
                _dbContext.Owners.Remove(existingOwner);
                // Save the changes to the database
                await _dbContext.SaveChangesAsync();
            }
        }



        //------------- idاحصل على صاحب العقار من خلال -------------------
        public Task<Owner?> GetOwnerByownerId(Guid ownerId)
        {
            var _dbContext = _dbContextFactory.CreateDbContext();
            return _dbContext.Owners.FirstOrDefaultAsync(a => a.ownerId == ownerId);


        }



        //-------------احصل على جميع اصحاب العقار-------------------
        public Task<List<Owner>> GetOwners()
        {
            var _dbContext = _dbContextFactory.CreateDbContext();
            return _dbContext.Owners.ToListAsync();

        }



        //-------------   اضافه صاحب العقار------------------
        public async Task<Owner> Upsert(Owner owner)
        {
            var _dbContext = _dbContextFactory.CreateDbContext();
            var existingOwner = await _dbContext.Owners.FirstOrDefaultAsync(a => a.ownerId == owner.ownerId);
            if (existingOwner != null)
            {
                existingOwner.ownerName = owner.ownerName;
                existingOwner.email = owner.email;
                existingOwner.phone = owner.phone;
                _dbContext.Owners.Update(existingOwner);
            }
            else
            {
                await _dbContext.Owners.AddAsync(owner);
            }
            await _dbContext.SaveChangesAsync();
            return owner;
        }
    }

}