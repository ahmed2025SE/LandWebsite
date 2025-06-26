using LandWebsite.Components.Pages.LandComponent;
using LandWebsite.Components.Pages.OwnerComponents;
using LandWebsite.Data;
using LandWebsite.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandWebsiteTesting
{
    public class LandServices_Tests
    {
        private IDbContextFactory<ApplicationDbContext> GetDbContextFactory(DbContextOptions<ApplicationDbContext> options)
        {
            var mockFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();
            mockFactory.Setup(f => f.CreateDbContext()).Returns(value: new ApplicationDbContext(options));
            return mockFactory.Object;
        }


        private static DbContextOptions<ApplicationDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }



        //    اختبار حذف الارض
        [Fact]
        public async Task DeleteAsync_ShouldRemovesLand_WhenLandExists()
        {
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);
            using var dbContext = new ApplicationDbContext(options);
            // Arrange
            var land = new Land
            {
                landId = Guid.NewGuid(),
                location = "Farm",
                area = 1000,
                price = 50000,
                OwnerId = Guid.NewGuid()
            };
            // نحفظ الارض في قاعدة البيانات
            using (var cf = factory.CreateDbContext())
            {
                cf.Lands.Add(land);
                await cf.SaveChangesAsync();
            }
            options = GetDbContextOptions();// إعادة تعيين خيارات قاعدة البيانات
            factory = GetDbContextFactory(options);// إنشاء مصنع جديد باستخدام الخيارات الجديدة
            var service = new LandServices(factory);
            // Act
            await service.DeleteAsync(land);
            // Assert
            using (var cf = factory.CreateDbContext())
            {
                var deletedLand = await cf.Lands.FindAsync(land.landId);
                Assert.Null(deletedLand); // يجب أن تكون الارض محذوفة
            }
        }



        //    اختبار الحصول على الارض من خلال معرف الارض
        [Fact]
        public async Task GetLandBylandId_ShouldReturnLand_WhenLandExists()
        {
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);
            using var dbContext = new ApplicationDbContext(options);
            // Arrange
            var land = new Land
            {
                landId = Guid.NewGuid(),
                location = "Farm",
                area = 1000,
                price = 50000,
                OwnerId = Guid.NewGuid()
            };
            // نحفظ الارض في قاعدة البيانات
            dbContext.Lands.Add(land); // اضافة ارض إلى قاعدة البيانات  
            await dbContext.SaveChangesAsync();

            var service = new LandServices(factory);
            // Act
            var result = await service.GetLandBylandId(land.landId);
            // Assert
            var addedLand = await dbContext.Lands.FindAsync(result.landId);
            Assert.NotNull(addedLand); // يجب أن تكون النتيجة غير فارغة
            Assert.Equal(land.landId, result.landId); // يجب أن يكون معرف الارض مطابقًا
        }


        


        //    اختبار الحصول على جميع الاراضي
        [Fact]
        public async Task GetLands_ShouldReturnAllLands_WhenLandsExist()
        {
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);
            using var dbContext = new ApplicationDbContext(options);
            // Arrange
     

            dbContext.Lands.AddRange(

               new Land
               {
                   landId = Guid.NewGuid(),
                   location = "Houari",
                   area = 1000,
                   price = 50000,
                   OwnerId = Guid.NewGuid()
               },
               new Land
               {
                   landId = Guid.NewGuid(),
                   location = "Leithy",
                   area = 2000,
                   price = 100000,
                   OwnerId = Guid.NewGuid()
               }
           );
            await dbContext.SaveChangesAsync();
          
            
            var service = new LandServices(factory);
            // Act
            var result = await service.GetLands();
            // Assert
            Assert.NotNull(result); // يجب أن تكون النتيجة غير فارغة
            Assert.Equal(2, result.Count); // يجب أن يكون هناك أرضين
        }




        //    اختبار الحصول على جميع الاراضي من خلال معرف صاحب العقار
        [Fact]
        public async Task GetLandsByOwnerId_ShouldReturnLands_WhenOwnerHasLands()
        {
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);
            using var dbContext = new ApplicationDbContext(options);
            // Arrange
            var ownerId = Guid.NewGuid();
            dbContext.Lands.AddRange(

               new Land
               {
                   landId = Guid.NewGuid(),
                   location = "Houari",
                   area = 1000,
                   price = 50000,
                   OwnerId = ownerId
               },
               new Land
               {
                   landId = Guid.NewGuid(),
                   location = "Leithy",
                   area = 2000,
                   price = 100000,
                   OwnerId = ownerId
               }
           );
            await dbContext.SaveChangesAsync();
           
            var service = new LandServices(factory);
            // Act
            var result = await service.GetLandsByOwnerId(ownerId);
            // Assert
            Assert.NotNull(result); // يجب أن تكون النتيجة غير فارغة
            Assert.Equal(2, result.Count); // يجب أن يكون هناك أرضين لصاحب العقار المحدد
        }










        //    اختبار اضافة ارض جديده
        [Fact]
        public async Task AddLand_ShouldAddLand_WhenDoesnotExist()
        {
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);
            using var dbContext = new ApplicationDbContext(options);
            // Arrange
            var land = new Land
            {
                landId = Guid.NewGuid(),
                location = "Houari",
                area = 1000,
                price = 50000,
                OwnerId = Guid.NewGuid()
            };
            var service = new LandServices(factory);
            // Act
            await service.Upsert(land);
            // Assert
            using (var cf = factory.CreateDbContext())
            {
                var addedLand = await cf.Lands.FindAsync(land.landId);
                Assert.NotNull(addedLand); // يجب أن تكون الارض مضافة
                Assert.Equal(land.location, addedLand.location); // يجب أن يكون الموقع مطابقًا
                Assert.Equal(land.area, addedLand.area); // يجب أن تكون المساحة مطابقة
                Assert.Equal(land.price, addedLand.price); // يجب أن يكون السعر مطابقًا
                Assert.Equal(land.OwnerId, addedLand.OwnerId); // يجب أن يكون معرف صاحب العقار مطابقًا
            }
        }


       //      اختبار تعديل بيانات الارض
    [Fact]
        public async Task UpdateLand_ShouldUpdateLand_WhenLandExists()
        {
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);
            using var dbContext = new ApplicationDbContext(options);
            // Arrange
            var existingLand = new Land
            {
                landId = Guid.NewGuid(),
                location = "Houari",
                area = 1000,
                price = 50000,
                OwnerId = Guid.NewGuid()
            };
            // نحفظ الارض في قاعدة البيانات
            using (var cf = factory.CreateDbContext())
            {
                cf.Lands.Add(existingLand);
                await cf.SaveChangesAsync();
            }
            options = GetDbContextOptions();
            factory = GetDbContextFactory(options);
            var service = new LandServices(factory);

            // تعديل الارض
            var land = new Land
            {
                landId = existingLand.landId,
                location = "Leithy",
                area = 1500,
                price = 60000,
                OwnerId = existingLand.OwnerId // يجب أن يكون معرف صاحب العقار مطابقًا
            };
           
            // Act
            await service.Upsert(land);
            // Assert
            using (var cf = factory.CreateDbContext())
            {
                var updatedLand = await cf.Lands.FindAsync(land.landId);
                Assert.NotNull(updatedLand); // يجب أن تكون الارض موجودة
                Assert.Equal(land.location, updatedLand.location); // يجب أن يكون الموقع محدثًا
                Assert.Equal(land.area, updatedLand.area); // يجب أن تكون المساحة محدثة
                Assert.Equal(land.price, updatedLand.price); // يجب أن يكون السعر محدثًا
                Assert.Equal(land.OwnerId, updatedLand.OwnerId); // يجب أن يكون معرف صاحب العقار مطابقًا
            }


           

        }


    }
}
