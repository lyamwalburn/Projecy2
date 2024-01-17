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
    public class BookingServiceUnitTests
    {
        private Mapper _mapper;
        private IBookingRepository _repo;
        private BookingService _service;
        private EstateAgentContext _context;
        private BookingController _controller;

        public BookingServiceUnitTests()
        {
            TPCAutoMapper myProfile = new TPCAutoMapper();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
        }


        private void Setup(IServiceScope scope)
        {
            _repo = scope.ServiceProvider.GetService<IBookingRepository>();
            _service = new BookingService(_repo,  _mapper);
            _context = scope.ServiceProvider.GetService<EstateAgentContext>();
            _controller = new BookingController(_service,_context);
        }

        private IServiceProvider GetBookingServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<BookingController>();
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            return services.BuildServiceProvider();
        }

        private BookingDTO CreateMockBookingDTO()
        {
            return new BookingDTO
            {
                Id = 1,
                BookingId = 1,
                PropertyId = 1,
                Time = new DateTime(2000, 01, 30)
            };
        }



        [Fact]
        public void TestFindAll()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add 2 bookings to db
                var mock1 = CreateMockBookingDTO();
                _controller.AddBooking(mock1);
                var mock2 = CreateMockBookingDTO();
                mock2.Id = 2;
                _controller.AddBooking(mock2);
                //do FindAll() to get from db
                var bookingsFromDb = _service.FindAll().AsEnumerable();
                var b1FromDb = bookingsFromDb.First();
                var b2FromDb = bookingsFromDb.Last();
                //compare the local to the db-pulled
                Assert.Equal(mock1.Id, b1FromDb.Id);
                Assert.Equal(mock2.Id, b2FromDb.Id);
            }
        }

        [Fact]
        public void TestFindById()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add booking to db
                var mock = CreateMockBookingDTO();
                mock.Id = 100;
                _controller.AddBooking(mock);
                //do FindById(100) to get from db
                var bookingFromDb = _service.FindById(100);
                //compare the local to the db-pulled
                Assert.Equal(100, bookingFromDb.Id);
            }
        }

        [Fact]
        public void TestCreate()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //create booking 
                var mock = CreateMockBookingDTO();
                //do create(mock) to create it in the db
                _service.Create(mock);
                //get it from db
                var bookingFromDb = _service.FindById(1);
                //compare the local to the db-pulled
                Assert.Equal(mock.Id, bookingFromDb.Id);
            }
        }

        [Fact]
        public void TestUpdate()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock booking to db
                var mock = CreateMockBookingDTO();
                _controller.AddBooking(mock);
                //change local and update db-entry
                mock.BuyerId = 5;
                _service.Update(mock);
                //compare the local to the db-pulled
                var bookingFromDb = _service.FindById(1);
                Assert.Equal(5, bookingFromDb.BuyerId);
            }
        }

        [Fact]
        public void TestDelete()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock booking to db
                var mock = CreateMockBookingDTO();
                _controller.AddBooking(mock);
                //delete booking from db
                _service.Delete(mock);
                //check db is empty again
                int dbCount = _service.FindAll().Count();
                Assert.Equal(0, dbCount);
            }
        }
    }
}
