using AutoMapper;
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using EstateAgentAPI.Controllers;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace EstateAgentUnitTests.ServiceTests
{
    public class SellerServiceUnitTests
    {
        private Mapper _mapper;
        private ISellerRepository _repo;
        private IPropertyRepository _repo2;
        private SellerService _service;
        private EstateAgentContext _context;
        private SellerController _controller;

        public SellerServiceUnitTests()
        {
            TPCAutoMapper myProfile = new TPCAutoMapper();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
        }


        private void Setup(IServiceScope scope)
        {
            _repo = scope.ServiceProvider.GetService<ISellerRepository>();
            _repo2 = scope.ServiceProvider.GetService<IPropertyRepository>();
            _service = new SellerService(_repo, _repo2, _mapper);
            _context = scope.ServiceProvider.GetService<EstateAgentContext>();
            _controller = new SellerController(_service);
        }

        private IServiceProvider GetSellerServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<SellerController>();
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            return services.BuildServiceProvider();
        }

        private SellerDTO CreateMockSellerDTO()
        {
            return new SellerDTO
            {
                Id = 1,
                FirstName = "Christian",
                Surname = "Savage",
                Address = "1 Dundee Road",
                PostCode = "ABC 123",
                Phone = "00440000000000"
            };
        }


        [Fact]
        public void TestFindAll()
        {
            var services = GetSellerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add 2 sellers to db
                var mock1 = CreateMockSellerDTO();
                _controller.AddSeller(mock1);
                var mock2 = CreateMockSellerDTO();
                mock2.Id = 2;
                _controller.AddSeller(mock2);
                //do FindAll() to get from db
                var sellersFromDb = _service.FindAll().AsEnumerable();
                var s1FromDb = sellersFromDb.First();
                var s2FromDb = sellersFromDb.Last();
                //compare the local to the db-pulled
                Assert.Equal(mock1.Id, s1FromDb.Id);
                Assert.Equal(mock2.Id, s2FromDb.Id);
            }
        }

        [Fact]
        public void TestFindById()
        {
            var services = GetSellerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add sellers to db
                var mock = CreateMockSellerDTO();
                mock.Id = 100;
                _controller.AddSeller(mock);
                //do FindById(100) to get from db
                var sellerFromDb = _service.FindById(100);
                //compare the local to the db-pulled
                Assert.Equal(100, sellerFromDb.Id);
            }
        }

        [Fact]
        public void TestCreate()
        {
            var services = GetSellerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //create seller 
                var mock = CreateMockSellerDTO();
                //do create(mock1) to create it in the db
                _service.Create(mock);
                //get it from db
                var sellerFromDb = _service.FindById(1);
                //compare the local to the db-pulled
                Assert.Equal(mock.Id, sellerFromDb.Id);
            }
        }

        [Fact]
        public void TestUpdate()
        {
            var services = GetSellerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock seller to db
                var mock = CreateMockSellerDTO();
                _controller.AddSeller(mock);
                //change local and update db-entry
                mock.FirstName = "Peter";
                _service.Update(mock);
                //compare the local to the db-pulled
                var sellerFromDb = _service.FindById(1);
                Assert.Equal(mock.FirstName, sellerFromDb.FirstName);
            }
        }

        [Fact]
        public void TestDeleteSeller()
        {
            var services = GetSellerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock seller to db
                var mock = CreateMockSellerDTO();
                _controller.AddSeller(mock);
                //delete seller from db
                _service.Delete(mock);
                //check db is empty again
                int dbCount = _service.FindAll().Count();
                Assert.Equal(0, dbCount);
            }
        }

        [Fact]
        public void TestDeleteRemovesProperties()
        {
            var services = GetSellerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock seller to db with sellerId=1
                var mockSeller = CreateMockSellerDTO();
                mockSeller.Id = 1;
                _controller.AddSeller(mockSeller);
                //add mock property to db with sellerId=1
                Property mockProperty = new Property
                {
                    Id = 1,
                    Address = "testaddress",
                    PostCode = "testpostcode",
                    Type = "apartment",
                    NumberOfBedrooms = 1,
                    NumberOfBathrooms = 2,
                    Garden = true,
                    Price = 1000,
                    Status = "SOLD",
                    SellerId = 1,
                    BuyerId = 4
                };
                _context.Properties.Add(mockProperty);
                //delete seller
                _service.Delete(mockSeller);
                //check the property also got deleted from the db
                var propertiesCountFromDb = _context.Properties.Count();
                Assert.Equal(0, propertiesCountFromDb);
            }
        }
    }
}
