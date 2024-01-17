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

namespace EstateAgentUnitTests.ServiceTests
{
    public class PropertyServiceUnitTests
    {
        private Mapper _mapper;
        private IPropertyRepository _repo;
        private IBookingRepository _repo2;
        private PropertyService _service;
        private EstateAgentContext _context;
        private PropertyController _controller;

        public PropertyServiceUnitTests()
        {
            TPCAutoMapper myProfile = new TPCAutoMapper();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
        }


        private void Setup(IServiceScope scope)
        {
            _repo = scope.ServiceProvider.GetService<IPropertyRepository>();
            _repo2 = scope.ServiceProvider.GetService<IBookingRepository>();
            _service = new PropertyService(_repo, _repo2, _mapper);
            _context = scope.ServiceProvider.GetService<EstateAgentContext>();
            _controller = new PropertyController(_service);
        }

        private IServiceProvider GetPropertyServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<PropertyController>();
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            return services.BuildServiceProvider();
        }

        private PropertyDTO CreateMockPropertyDTO()
        {
            return new PropertyDTO
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
                BuyerId = 4,
                SellerId = 11
            };
        }


        [Fact]
        public void TestFindAll()
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add 2 properties to db
                var mock1 = CreateMockPropertyDTO();
                _controller.AddProperty(mock1);
                var mock2 = CreateMockPropertyDTO();
                mock2.Id = 2;
                _controller.AddProperty(mock2);
                //do FindAll() to get from db
                var propertiesFromDb = _service.FindAll().AsEnumerable();
                var p1FromDb = propertiesFromDb.First();
                var p2FromDb = propertiesFromDb.Last();
                //compare the local to the db-pulled
                Assert.Equal(mock1.Id, p1FromDb.Id);
                Assert.Equal(mock2.Id, p2FromDb.Id);
            }
        }

        [Fact]
        public void TestFindById() 
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add properties to db
                var mock1 = CreateMockPropertyDTO();
                mock1.Id = 100;
                _controller.AddProperty(mock1);
                //do FindById(100) to get from db
                var propertyFromDb = _service.FindById(100);
                //compare the local to the db-pulled
                Assert.Equal(100, propertyFromDb.Id);
            }
        }

        [Fact]
        public void TestCreate()
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //create property 
                var mock = CreateMockPropertyDTO();
                //do create(mock1) to create it in the db
                _service.Create(mock);
                //get it from db
                var propertyFromDb = _service.FindById(1);
                //compare the local to the db-pulled
                Assert.Equal(mock.Id, propertyFromDb.Id);
            }
        }

        [Fact]
        public void TestUpdate()
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock property to db
                var mock = CreateMockPropertyDTO();
                _controller.AddProperty(mock);
                //change local and update db-entry
                mock.NumberOfBathrooms = 5; // instead of the base 2
                _service.Update(mock);
                //compare the local to the db-pulled
                var propertyFromDb = _service.FindById(1);
                Assert.Equal(mock.NumberOfBathrooms, propertyFromDb.NumberOfBathrooms);
            }
        }

        [Fact]
        public void TestDelete() 
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock property to db
                var mock = CreateMockPropertyDTO();
                _controller.AddProperty(mock);
                //delete property from db
                _service.Delete(mock);
                //check db is empty again
                int dbCount = _service.FindAll().Count();
                Assert.Equal(0, dbCount);
            }
        }

        [Fact]
        public void TestSellPropertyStatusChange() 
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock property to db with status="FOR SALE"
                var mock = CreateMockPropertyDTO();
                mock.Status = "FOR SALE";
                _controller.AddProperty(mock);
                //sell property
                _service.SellProperty(mock);
                //check it updated in the db
                var propertyFromDb = _service.FindById(1);
                Assert.Equal("SOLD", propertyFromDb.Status);
            }
        }

        [Fact]
        public void TestSellPropertyRemovesBookings()
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock property to db with status="FOR SALE"
                var mockProperty = CreateMockPropertyDTO();
                mockProperty.Status = "FOR SALE";
                _controller.AddProperty(mockProperty);
                //add mock booking to db with propertyId=1
                Booking mockBooking = new Booking
                {
                    Id = 1,
                    BuyerId = 1,
                    PropertyId = 1,
                    Time = new DateTime(2000, 01, 30)
                };
                _context.Bookings.Add(mockBooking);
                //sell property
                _service.SellProperty(mockProperty);
                //check the booking got deleted from the db
                var bookingsCountFromDb = _context.Bookings.Count();    
                Assert.Equal(0, bookingsCountFromDb);
            }
        }

        [Fact]
        public void TestWithdrawPropertyStatusChange()
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock property to db with status="FOR SALE"
                var mock = CreateMockPropertyDTO();
                mock.Status = "FOR SALE";
                _controller.AddProperty(mock);
                //withdraw property
                _service.WithdrawProperty(mock.Id);
                //check it updated in the db
                var propertyFromDb = _service.FindById(1);
                Assert.Equal("WITHDRAWN", propertyFromDb.Status);
            }
        }

        [Fact]
        public void TestWithdrawPropertyRemovesBookings()
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock property to db with status="FOR SALE"
                var mockProperty = CreateMockPropertyDTO();
                mockProperty.Status = "FOR SALE";
                _controller.AddProperty(mockProperty);
                //add mock booking to db with propertyId=1
                Booking mockBooking = new Booking
                {
                    Id = 1,
                    BuyerId = 1,
                    PropertyId = 1,
                    Time = new DateTime(2000, 01, 30)
                };
                _context.Bookings.Add(mockBooking);
                //withdraw property
                _service.WithdrawProperty(mockProperty.Id);
                //check the booking got deleted from the db
                var bookingsCountFromDb = _context.Bookings.Count();
                Assert.Equal(0, bookingsCountFromDb);
            }
        }

        [Fact]
        public void TestRelistWithdrawnProperty()
        {
            var services = GetPropertyServiceProvider();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //empty db
                _context.Database.EnsureDeleted();
                //add mock property to db with status="WITHDRAWN"
                var mock = CreateMockPropertyDTO();
                mock.Status = "WITHDRAWN";
                _controller.AddProperty(mock);
                //relist property
                _service.RelistWithdrawnProperty(mock.Id);
                //check it updated in the db
                var propertyFromDb = _service.FindById(1);
                Assert.Equal("FOR SALE", propertyFromDb.Status);
            }
        }
    }
}
