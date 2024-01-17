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

namespace EstateAgentUnitTests
{
    public class PropertyUnitTests
    {
        private Mapper _mapper;
        private IPropertyRepository _repo;
        private IBookingRepository _repo2;
        private PropertyService _service;
        private EstateAgentContext _context;
        private PropertyController _controller;

        public PropertyUnitTests()
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

        private IServiceProvider GetPropertyServiceProivder()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<PropertyController>();
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            return services.BuildServiceProvider();
        }

        private PropertyDTO GetMockProperty()
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
        public void TestAddProperty()
        {
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();

                var propertyDTO = new PropertyDTO
                {
                    Address = "test1address",
                    PostCode = "test1postcode",
                    Type = "apartment",
                    NumberOfBedrooms = 1,
                    NumberOfBathrooms = 2,
                    Garden = true,
                    Price = 1000,
                    Status = "SOLD",
                    BuyerId = 4,
                    SellerId = 11
                };

                _controller.AddProperty(propertyDTO);
                var property = _context.Properties.Single();

                Assert.Equal(1, property.Id);
                Assert.Equal("test1address", property.Address);
            }
        }

        [Fact]
        public void TestGetProperty()
        {
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                //Add property to db
                _controller.AddProperty(GetMockProperty());

                var propertyFromDb = _controller.Index().FirstOrDefault();

                Assert.Equal(1, propertyFromDb.Id);
                Assert.Equal("testaddress", propertyFromDb.Address);
                Assert.Equal("testpostcode", propertyFromDb.PostCode);
                Assert.Equal("apartment", propertyFromDb.Type);
                Assert.Equal(1, propertyFromDb.NumberOfBedrooms);
                Assert.Equal(2, propertyFromDb.NumberOfBathrooms);
                Assert.Equal(true, propertyFromDb.Garden);
                Assert.Equal(1000, propertyFromDb.Price);
                Assert.Equal("SOLD", propertyFromDb.Status);
                Assert.Equal(4, propertyFromDb.BuyerId);
                Assert.Equal(11, propertyFromDb.SellerId);
            }
        }

        [Fact]
        public void TestGetById()
        {
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                _controller.AddProperty(GetMockProperty());

                var secondProperty = new PropertyDTO
                {
                    Id = 2,
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
                _controller.AddProperty(secondProperty);

                ActionResult<PropertyDTO> propertyFromId = _controller.GetById(2);

                Assert.Equal(2, propertyFromId.Value.Id);
                Assert.Equal("testaddress", propertyFromId.Value.Address);
                Assert.Equal("testpostcode", propertyFromId.Value.PostCode);
                Assert.Equal("apartment", propertyFromId.Value.Type);
                Assert.Equal(1, propertyFromId.Value.NumberOfBedrooms);
                Assert.Equal(2, propertyFromId.Value.NumberOfBathrooms);
                Assert.True(propertyFromId.Value.Garden);
                Assert.Equal(1000, propertyFromId.Value.Price);
                Assert.Equal("SOLD", propertyFromId.Value.Status);
                Assert.Equal(4, propertyFromId.Value.BuyerId);
                Assert.Equal(11, propertyFromId.Value.SellerId);
            }

        }

        [Fact]
        public void TestPutProperty()
        {
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                _controller.AddProperty(GetMockProperty());

                var propertyToUpdate = new PropertyDTO
                {
                    Id = 1,
                    Address = "updated testaddress",
                    PostCode = "updated testpostcode",
                    Type = "detached",
                    NumberOfBedrooms = 2,
                    NumberOfBathrooms = 1,
                    Garden = false,
                    Price = 1500,
                    Status = "SOLD",
                    BuyerId = 4,
                    SellerId = 11
                };

                _controller.UpdateProperty(propertyToUpdate);

                var propertyFromDb = _repo.FindById(1);

                Assert.Equal(1, propertyFromDb.Id);
                Assert.Equal("updated testaddress", propertyFromDb.Address);
                Assert.Equal("updated testpostcode", propertyFromDb.PostCode);
                Assert.Equal("detached", propertyFromDb.Type);
                Assert.Equal(2, propertyFromDb.NumberOfBedrooms);
                Assert.Equal(1, propertyFromDb.NumberOfBathrooms);
                Assert.False(propertyFromDb.Garden);
                Assert.Equal(1500, propertyFromDb.Price);
                Assert.Equal("SOLD", propertyFromDb.Status);
                Assert.Equal(4, propertyFromDb.BuyerId);
                Assert.Equal(11, propertyFromDb.SellerId);
            }

        }

        [Fact]
        public void TestDeleteProperty()
        {
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                _controller.AddProperty(GetMockProperty());

                var expectedRecordsCount = 0;

                _controller.DeleteProperty(1);

                var propertiesFromDb = _controller.Index();

                Assert.Equal(expectedRecordsCount, propertiesFromDb.Count());
            }
        }

        [Fact]
        public void Test404ResponseGetPropertyById()
        {
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                _context.Database.EnsureDeleted();
                _controller.AddProperty(GetMockProperty());

                var actionResult = _controller.GetById(99);

                var result = actionResult.Result as NotFoundResult;
                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }

        [Fact]
        public void Test404ResponseUpdateProperty()
        {
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                _context.Database.EnsureDeleted();
                _controller.AddProperty(GetMockProperty());

                var propertyToUpdate = new PropertyDTO
                {
                    Id = 100,
                    Address = "updated testaddress",
                    PostCode = "updated testpostcode",
                    Type = "detached",
                    NumberOfBedrooms = 2,
                    NumberOfBathrooms = 1,
                    Garden = false,
                    Price = 1500,
                    Status = "SOLD",
                    BuyerId = 4,
                    SellerId = 11
                };

                var actionResult = _controller.UpdateProperty(propertyToUpdate);

                var result = actionResult.Result as NotFoundResult;
                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }

        [Fact]
        public void Test404ResponseDeleteBuyer()
        {
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                _context.Database.EnsureDeleted();
                _controller.AddProperty(GetMockProperty());

                var actionResult = _controller.DeleteProperty(100);
                ;
                Assert.NotNull(actionResult);
                Assert.Equal(HttpStatusCode.NotFound, actionResult);
            }
        }

    }
}
