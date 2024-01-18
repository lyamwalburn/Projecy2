using AutoMapper;
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using EstateAgentAPI.Controllers;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace EstateAgentUnitTests.ControllerTests
{
    public class BookingControllerUnitTests
    {
        private Mapper _mapper;

        public BookingControllerUnitTests()
        {
            TPCAutoMapper myProfile = new TPCAutoMapper();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
        }

        private IServiceProvider GetBookingServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<BookingController>();
            services.AddAutoMapper(typeof(Program));

            return services.BuildServiceProvider();
        }

        private BookingDTO GetMockBookingDTO(int id)
        {
            return new BookingDTO
            {
                Id = id,
                BuyerId = 100,
                PropertyId = 100,
                Time = new DateTime(2000, 01, 30)
            };
        }
        private BookingDTO GetMockBookingDTO2(int id)
        {
            return new BookingDTO
            {
                Id = id,
                BuyerId = 101,
                PropertyId = 101,
                Time = new DateTime(2024, 01, 30)
            };
        }

        [Fact]
        public void TestAddBooking()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBookingRepository>();
                var service = new BookingService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BookingController(service,context);

                context.Database.EnsureDeleted();            // ensure database is empty
                controller.AddBooking(GetMockBookingDTO(100));
                var booking = context.Bookings.Single();

                Assert.Equal(100, booking.Id);
                Assert.Equal(100, booking.BuyerId);
                Assert.Equal(100, booking.PropertyId);
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
                var controller = new BookingController(service, context);

                context.Database.EnsureDeleted();
                controller.AddBooking(GetMockBookingDTO(10));    // TestAddBooking() must pass for this one to pass
                controller.AddBooking(GetMockBookingDTO2(100));

                var bookingsFromDb = controller.Index();
                var booking1 = bookingsFromDb.First();
                var booking2 = bookingsFromDb.Last();

                Assert.Equal(10, booking1.Id);
                Assert.Equal(100, booking2.Id);
            }
        }


        [Fact]
        public void TestGetById()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBookingRepository>();
                var service = new BookingService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BookingController(service, context);

                context.Database.EnsureDeleted();
                var mockBookingDTO = GetMockBookingDTO(100);
                controller.AddBooking(mockBookingDTO);     // TestAddBooking() must pass for this one to pass

                BookingDTO booking = controller.GetById(100).Value;

                Assert.True(booking.Equals(mockBookingDTO));
            }
        }

        [Fact]
        public void TestUpdateBooking()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBookingRepository>();
                var service = new BookingService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BookingController(service, context);

                context.Database.EnsureDeleted();
                BookingDTO mockBookingDTO = GetMockBookingDTO(100);
                controller.AddBooking(mockBookingDTO);     // TestAddBooking() must pass for this one to pass

                mockBookingDTO.BuyerId = 300;  // instead of 100
                mockBookingDTO.PropertyId = 300;
                mockBookingDTO.Time = new DateTime(2024, 01, 30);
                controller.UpdateBooking(mockBookingDTO);

                BookingDTO updatedBookingFromDb = controller.GetById(100).Value; // TestGetById() must pass for this one to pass

                Assert.True(updatedBookingFromDb.Equals(mockBookingDTO));   // checks they have the same ID
                Assert.Equal(300, updatedBookingFromDb.BuyerId);
                Assert.Equal(300, updatedBookingFromDb.PropertyId);
                Assert.Equal(new DateTime(2024, 01, 30), updatedBookingFromDb.Time);
            }
        }

        [Fact]
        public void TestDeleteBooking()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBookingRepository>();
                var service = new BookingService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BookingController(service, context);

                context.Database.EnsureDeleted();
                BookingDTO mockBookingDTO = GetMockBookingDTO(200);
                controller.AddBooking(mockBookingDTO);     // TestAddBooking() must pass for this one to pass


                controller.DeleteBooking(200);
                var check = controller.GetById(200).Result as NotFoundResult;

                Assert.Equal(404, check.StatusCode);
            }
        }


        [Fact]
        public void Test404ResponseGetBookingById()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBookingRepository>();
                var service = new BookingService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BookingController(service, context);

                context.Database.EnsureDeleted();
                controller.AddBooking(GetMockBookingDTO(100));

                var actionResult = controller.GetById(99);

                var result = actionResult.Result as NotFoundResult;
                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }

        [Fact]
        public void Test404ResponseUpdateBooking()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBookingRepository>();
                var service = new BookingService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BookingController(service, context);

                context.Database.EnsureDeleted();
                var mockBookingDTO = GetMockBookingDTO(100);
                controller.AddBooking(mockBookingDTO);

                var bookingToUpdate = new BookingDTO
                {
                    Id = 99,
                    BuyerId = 300,
                    PropertyId = 300,
                    Time = new DateTime(2024, 01, 30)
                };

                var actionResult = controller.UpdateBooking(bookingToUpdate);

                var result = actionResult.Result as NotFoundResult;
                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }

        [Fact]
        public void Test404ResponseDeleteBuyer()
        {
            var services = GetBookingServiceProvider();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IBookingRepository>();
                var service = new BookingService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new BookingController(service,context);

                context.Database.EnsureDeleted();
                controller.AddBooking(GetMockBookingDTO(100));

                var actionResult = controller.DeleteBooking(99);
                ;
                Assert.NotNull(actionResult);
                Assert.Equal(HttpStatusCode.NotFound, actionResult);
            }
        }
    }
}
