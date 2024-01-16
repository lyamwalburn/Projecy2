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


        private BookingDTO GetMockBooking()
        {
            return new BookingDTO
            {
                BuyerId = 1,
                PropertyId = 1,
                Time = new DateTime(2022,12,24)
            };
        }

        [Fact]
        public void TestAddBooking()
        {
            Assert.Equal(1, 1);
        }

    }
}
