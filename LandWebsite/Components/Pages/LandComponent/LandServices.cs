using LandWebsite.Data.Entites;
using LandWebsite.Data;
using Microsoft.EntityFrameworkCore;
using static MudBlazor.Icons.Custom;

namespace LandWebsite.Components.Pages.LandComponent
{
    public class LandServices : ILandServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public LandServices(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {

            _dbContextFactory = dbContextFactory;

        }



        //-------------احذف الارض-------------------
        public async Task DeleteAsync(Land land)
        {
            var _dbContext = _dbContextFactory.CreateDbContext();
            var existingLand = _dbContext.Lands.Find(land.landId);
            if (existingLand != null)
            {
                
                _dbContext.Lands.Remove(existingLand);
               
                await _dbContext.SaveChangesAsync();
            }
        }



        //------------- idاحصل على الارض من خلال -------------------
        public Task<Land?> GetLandBylandId(Guid landId)
        {
            var _dbContext = _dbContextFactory.CreateDbContext();
            return _dbContext.Lands.FirstOrDefaultAsync(a => a.landId == landId);


        }



        //-------------احصل على جميع الاراضي-------------------
        public Task<List<Land>> GetLands()
        {
            var _dbContext = _dbContextFactory.CreateDbContext();
            return _dbContext.Lands.ToListAsync();

        }

        //----------- احصل على جميع الاراضي من خلال معرف صاحب العقار --------------
        public Task<List<Land>> GetLandsByOwnerId(Guid OwnerId)
        {
            var _dbContext = _dbContextFactory.CreateDbContext();
            return _dbContext.Lands.Where(a => a.OwnerId ==OwnerId).ToListAsync();

        }



        //-------------  اضافه او تعديل ارض------------------
        public async Task<Land> Upsert(Land land)
        {
            var _dbContext = _dbContextFactory.CreateDbContext();
            var existingLand = await _dbContext.Lands.FirstOrDefaultAsync(a => a.landId == land.landId);
            if (existingLand != null)
            {
                existingLand.area = land.area;
                existingLand.price = land.price;
                existingLand.location = land.location;
               
                _dbContext.Lands.Update(existingLand);
            }
            else
            {
                await _dbContext.Lands.AddAsync(land);
            }
            await _dbContext.SaveChangesAsync();
            return land;
        }
    }
}
