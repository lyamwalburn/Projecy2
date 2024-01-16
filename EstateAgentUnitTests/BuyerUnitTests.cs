using AutoMapper;
using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Buisness.Services;
using EstateAgentAPI.Controllers;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace EstateAgentUnitTests
{
    public class BuyerUnitTests
    {
        private Mapper _mapper;

        public BuyerUnitTests()
        {
            TPCAutoMapper myProfile = new TPCAutoMapper();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

        }

        private IServiceProvider GetBuyerServiceProivder()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase("MockDBData"));
            services.AddScoped<IBuyerService, BuyerService>();
            services.AddScoped<IBuyerRepository, BuyerRepository>();
            services.AddScoped<BuyerController>();
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            return services.BuildServiceProvider();
        }

        private BuyerDTO GetMockBuyer()
        {
            return new BuyerDTO
            {
                Id = 1,
                FirstName = "Steve",
                Surname = "Smith",
                Address = "1 Main Street",
                Postcode = "TE 5T",
                Phone = "12345"
            };
        }

        [Fact]
        public void TestAddBuyer()
        {
            var services = GetBuyerServiceProivder();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBuyerRepository>();
                var service = new BuyerService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BuyerController(service);
                //Clear database
                context.Database.EnsureDeleted();

                var buyerDTO = new BuyerDTO
                {
                    FirstName = "testname",
                    Surname = "surnametest",
                    Address = "123 test street",
                    Postcode = "TE 5T",
                    Phone = "123456789"
                };

                controller.AddBuyer(buyerDTO);
                var buyer = context.Buyers.Single();

                Assert.Equal(1, buyer.Id);
                Assert.Equal("testname", buyer.FirstName);
            }
        }


        [Fact]
        public void TestGetBuyer()
        {
            var services = GetBuyerServiceProivder();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBuyerRepository>();
                var service = new BuyerService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BuyerController(service);
                //Clear database
                context.Database.EnsureDeleted();
                //Add buyer to db
                controller.AddBuyer(GetMockBuyer());

                var buyerFromDb = controller.Index().FirstOrDefault();
                
                Assert.Equal(1, buyerFromDb.Id);
                Assert.Equal("Smith", buyerFromDb.Surname);
                Assert.Equal("Steve", buyerFromDb.FirstName);
                Assert.Equal("1 Main Street", buyerFromDb.Address);
                Assert.Equal("TE 5T", buyerFromDb.Postcode);
                Assert.Equal("12345", buyerFromDb.Phone);
            }
        }

        [Fact]
        public void TestPutBuyer()
        {
            var services = GetBuyerServiceProivder();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBuyerRepository>();
                var service = new BuyerService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BuyerController(service);
                //Clear database
                context.Database.EnsureDeleted();
                controller.AddBuyer(GetMockBuyer());


            }
            
        }
    }
}