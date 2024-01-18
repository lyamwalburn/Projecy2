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
    public class BuyerServiceUnitTests
    {
        private Mapper _mapper;
        private IBuyerRepository _repo;
        private IBookingRepository _repo2;
        private BuyerService _service;
        private EstateAgentContext _context;
        private BuyerController _controller;

        public BuyerServiceUnitTests()
        {
            TPCAutoMapper myProfile = new TPCAutoMapper();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
        }


        private void Setup(IServiceScope scope)
        {
            _repo = scope.ServiceProvider.GetService<IBuyerRepository>();
            _repo2 = scope.ServiceProvider.GetService<IBookingRepository>();
            _service = new BuyerService(_repo, _repo2, _mapper);
            _context = scope.ServiceProvider.GetService<EstateAgentContext>();
            _controller = new BuyerController(_service,_context);
        }

        private IServiceProvider GetBuyerServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IBuyerService, BuyerService>();
            services.AddScoped<IBuyerRepository, BuyerRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<BuyerController>();
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            return services.BuildServiceProvider();
        }

        private BuyerDTO CreateMockBuyerDTO()
        {
            return new BuyerDTO
            {
                Id = 1,
                FirstName = "Christian",
                Surname = "Savage",
                Address = "1 Dundee Road",
                PostCode = "ABC 123",
                Phone = "00440000000000"
            };
        }
        private BuyerDTO CreateMockBuyerDTO2()
        {
            return new BuyerDTO
            {
                Id = 2,
                FirstName = "Peter",
                Surname = "Behague",
                Address = "1 Kent Road",
                PostCode = "DEF456",
                Phone = "00440000000001"
            };
        }



        [Fact]
        public void TestFindAll()
        {
            var services = GetBuyerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add 2 buyers to db
                var mock1 = CreateMockBuyerDTO();
                _controller.AddBuyer(mock1);
                var mock2 = CreateMockBuyerDTO2();
                _controller.AddBuyer(mock2);
                //do FindAll() to get from db
                var buyersFromDb = _service.FindAll().AsEnumerable();
                var b1FromDb = buyersFromDb.First();
                var b2FromDb = buyersFromDb.Last();
                //compare the local to the db-pulled
                Assert.Equal(mock1.Id, b1FromDb.Id);
                Assert.Equal(mock2.Id, b2FromDb.Id);
            }
        }

        [Fact]
        public void TestFindById()
        {
            var services = GetBuyerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add buyer to db
                var mock = CreateMockBuyerDTO();
                mock.Id = 100;
                _controller.AddBuyer(mock);
                //do FindById(100) to get from db
                var buyerFromDb = _service.FindById(100);
                //compare the local to the db-pulled
                Assert.Equal(100, buyerFromDb.Id);
            }
        }

        [Fact]  
        public void TestCreate()
        {
            var services = GetBuyerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //create buyer 
                var mock = CreateMockBuyerDTO();
                //do create(mock) to create it in the db
                _service.Create(mock);
                //get it from db
                var buyerFromDb = _service.FindById(1);
                //compare the local to the db-pulled
                Assert.Equal(mock.Id, buyerFromDb.Id);
            }
        }

        [Fact]
        public void TestUpdate()
        {
            var services = GetBuyerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock buyer to db
                var mock = CreateMockBuyerDTO();
                _controller.AddBuyer(mock);
                //change local and update db-entry
                mock.FirstName = "Peter";
                _service.Update(mock);
                //compare the local to the db-pulled
                var buyerFromDb = _service.FindById(1);
                Assert.Equal("Peter", buyerFromDb.FirstName);
            }
        }

        [Fact]
        public void TestDeleteBuyer()
        {
            var services = GetBuyerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock buyer to db
                var mock = CreateMockBuyerDTO();
                _controller.AddBuyer(mock);
                //delete buyer from db
                _service.Delete(mock);
                //check db is empty again
                int dbCount = _service.FindAll().Count();
                Assert.Equal(0, dbCount);
            }
        }

        [Fact]
        public void TestDeleteRemovesBookings()
        {
            var services = GetBuyerServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock buyer to db with buyerId=1
                var mockBuyer = CreateMockBuyerDTO();
                mockBuyer.Id = 1;
                _controller.AddBuyer(mockBuyer);
                //add mock booking to db with buyerId=1
                Booking mockBooking = new Booking
                {
                    Id = 1,
                    BuyerId = 1,
                    PropertyId = 1,
                    Time = new DateTime(2000, 01, 30)
                };
                _context.Bookings.Add(mockBooking);
                //delete buyer
                _service.Delete(mockBuyer);
                //check the booking also got deleted from the db
                var bookingsCountFromDb = _context.Bookings.Count();
                Assert.Equal(0, bookingsCountFromDb);
            }
        }
    }
}
