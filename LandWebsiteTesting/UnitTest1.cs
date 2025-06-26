using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using LandWebsite.Data;
using LandWebsite.Data.Entites;
using LandWebsite.Components.Pages.OwnerComponents;
using System;
using System.Threading.Tasks;

namespace LandWebsiteTesting
{
    public class OwnerServices_GetTests //test1
    {
        private IDbContextFactory<ApplicationDbContext> GetDbContextFactory(DbContextOptions<ApplicationDbContext> options)
        {
            var mockFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();
            mockFactory.Setup(f => f.CreateDbContext()).Returns( value:new ApplicationDbContext(options));
            return mockFactory.Object;
        }


        private static DbContextOptions<ApplicationDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }


         //    اختبار حذف صاحب العقار
        [Fact]
        public async Task DeleteAsync_ShouldRemovesOwner_WhenOwnerExists()
        {
            
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);
            using var dbContext = new ApplicationDbContext(options);
            // Arrange
            var owner = new Owner
            {
                ownerId = Guid.NewGuid(),
                ownerName = "Ahmed",
                email = "ahmed@",
                phone = 1234567890
            };

            // نحفظ صاحب العقار في قاعدة البيانات
            using (var cf = factory.CreateDbContext())
            {
                cf.Owners.Add(owner);
                await cf.SaveChangesAsync();
            }
            options = GetDbContextOptions();
            factory = GetDbContextFactory(options);
            var service = new OwnerServices(factory);

            // Act
            await service.DeleteAsync(owner);


            // Assert
            options = GetDbContextOptions();
            factory = GetDbContextFactory(options);
            using var Context = factory.CreateDbContext();

            var deleted = await Context.Owners.FindAsync(owner.ownerId);
            Assert.Null(deleted);
        }

        // اختبار الحصول على صاحب العقار من خلال id
        [Fact]
        public async Task GetOwnerByownerId_ShouldReturnsOwner_WhenOwnerExist()
        {
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);

            // Arrange  
            var ownerId = Guid.NewGuid();   

            var owner = new Owner
            {
                ownerId = ownerId,
                ownerName = " ahmed",
                email = "ahmed badr@",
                phone = 0923030
            };

            using var dbContext = new ApplicationDbContext(options);
            dbContext.Owners.Add(owner); // اضافة صاحب العقار إلى قاعدة البيانات  
            await dbContext.SaveChangesAsync();

            var service = new OwnerServices(factory);

            // Act  
            var result = await service.GetOwnerByownerId(ownerId);

            // Assert  
            var addedOwner = await dbContext.Owners.FindAsync(result.ownerId);
            Assert.NotNull(addedOwner);
            Assert.Equal(ownerId, result.ownerId);
        }

        //اختبار الحصول على جميع اصحاب العقار
        [Fact]
        public async Task GetOwners_ShouldReturnsAllOwners_WhenOwnerExists()
        {
            // Arrange
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);
            using var dbContext = new ApplicationDbContext(options);

            dbContext.Owners.AddRange(

                new Owner { ownerId = Guid.NewGuid(), ownerName = "Owner 1", email = "owner1@", phone = 1234567890 },
                new Owner { ownerId = Guid.NewGuid(), ownerName = "Owner 2", email = "owner2@", phone = 9876543 }
            );
            await dbContext.SaveChangesAsync();
            var service = new OwnerServices(factory);

            // Act
            var result = await service.GetOwners();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }


        //اختبار تعديل صاحب عقار موجود بالفعل
        [Fact]
        public async Task UpsertOwner__ShouldUpdateExistingOwner_WhenOwnerExist()
        {
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);

            // Arrange
            var existingOwner = new Owner
            {
                ownerId = Guid.NewGuid(),
                ownerName = "Existing Owner",
                email = "existingowner@",
                phone = 1234567890
            };

            var cf = factory.CreateDbContext();
            cf.Owners.Add(existingOwner);
             await cf.SaveChangesAsync();

            // تعديل بيانات صاحب العقار
            var owner = new Owner
            {
                ownerId = existingOwner.ownerId,
                ownerName = " ahmed",
                email = "ahmed badr@",
                phone = 0923030
            };

            var service = new OwnerServices(factory);

            // Act
             var result = await service.Upsert(owner);

            // Assert
            var dbContext = factory.CreateDbContext();
            var updatedOwner = await dbContext.Owners.FindAsync(result.ownerId);
                Assert.NotNull(updatedOwner);
                Assert.Equal(owner.ownerName, updatedOwner.ownerName);
                Assert.Equal(owner.email, updatedOwner.email);
                Assert.Equal(owner.phone, updatedOwner.phone);
            

        }

        //                          اختبار اضافه صاحب عقار جديد

        [Fact]
        public async Task UpsertOwner__ShouldAddNewOwner_WhenDoesnotExist()
        {
            var options = GetDbContextOptions();
            var factory = GetDbContextFactory(options);
            // Arrange
            var owner = new Owner
            {
                ownerId = Guid.NewGuid(), 
                ownerName = "New Owner",
                email = "newowner@",
                phone = 987654321
            };

            using var dbContext = new ApplicationDbContext(options);
            var service = new OwnerServices(factory);
            // Act
            var result = await service.Upsert(owner);
            // Assert
            var addedOwner = await dbContext.Owners.FindAsync(result.ownerId);
            Assert.NotNull(addedOwner);
            Assert.Equal(owner.ownerId, result.ownerId);



        }


    


    }
}