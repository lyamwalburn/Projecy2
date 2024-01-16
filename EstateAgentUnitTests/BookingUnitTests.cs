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
    public class BookingUnitTests
    {
        private Mapper _mapper;

        public BookingUnitTests()
        {
            TPCAutoMapper myProfile = new TPCAutoMapper();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
        }

        private IServiceProvider GetBookingServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase("MockDBData"));
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<BookingController>();
            services.AddAutoMapper(typeof(Program));

            return services.BuildServiceProvider();
        }


        private BookingDTO GetMockBooking(int id)
        {
            return new BookingDTO
            {
                Id = id,
                BuyerId = 404,
                PropertyId = 404,
                Time = new DateTime(2000,01,30)
            };
        }

        [Fact]
        public void TestAddBooking()
        {
            var services = GetBookingServiceProvider();
            using(var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBookingRepository>();
                var service = new BookingService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BookingController(service);

                context.Database.EnsureDeleted();            // ensure database is empty
                controller.AddBooking(GetMockBooking(404));  
                var booking = context.Bookings.Single();     

                Assert.Equal(404, booking.Id);
                Assert.Equal(404, booking.BuyerId);
                Assert.Equal(404, booking.PropertyId);
                Assert.Equal(new DateTime(2000, 01, 30), booking.Time); 
            }
        }


        [Fact]
        public void TestIndexGetAll()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBookingRepository>();
                var service = new BookingService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BookingController(service);

                context.Database.EnsureDeleted();            // ensure database is empty
                // TestAddBooking() must pass for this one to pass
                controller.AddBooking(GetMockBooking(1));
                controller.AddBooking(GetMockBooking(404));

                var bookingsFromDb = controller.Index();
                var booking1 = bookingsFromDb.First();
                var booking2 = bookingsFromDb.Last();

                Assert.Equal(1, booking1.Id);
                Assert.Equal(404, booking2.Id);
            }
        }

    }
}
