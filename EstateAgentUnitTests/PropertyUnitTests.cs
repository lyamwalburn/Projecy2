using AutoMapper;
using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Buisness.Services;
using EstateAgentAPI.Controllers;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace EstateAgentUnitTests
{
    public class PropertyUnitTests
    {
        private Mapper _mapper;

        public PropertyUnitTests()
        {
            TPCAutoMapper myProfile = new TPCAutoMapper();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
        }
        private IServiceProvider GetPropertyServiceProivder()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase("MockDBData"));
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
                Address = "testaddress",
                PostCode = "testpostcode",
                Type = "apartment",
                NumberOfBedrooms = 1,
                NumerOfBathrooms = 2,
                Garden = true,
                Price = 1000,
                Status = "SOLD",
                BuyerID = 4,
                SellerID = 11
            };
        }

        [Fact]
        public void TestAddProperty()
        {
            //Arrange
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IPropertyRepository>();
                var service = new PropertyService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new PropertyController(service);
                //Clear database
                context.Database.EnsureDeleted();

                var propertyDTO = new PropertyDTO
                {
                    Address = "testaddress",
                    PostCode = "testpostcode",
                    Type = "apartment",
                    NumberOfBedrooms = 1,
                    NumerOfBathrooms = 2,
                    Garden = true,
                    Price = 1000,
                    Status = "SOLD",
                    BuyerID = 4,
                    SellerID = 11
                };

                //Act
                controller.AddProperty(propertyDTO);
                var property = context.Properties.Single();

                //Assert
                Assert.Equal(1, property.Id);
                Assert.Equal("testaddress", property.Address);
            }
        }

        [Fact]
        public void TestGetProperty()
        {
            //Arrange
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IPropertyRepository>();
                var service = new PropertyService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new PropertyController(service);
                //Clear database
                context.Database.EnsureDeleted();

                //Act
                controller.AddProperty(GetMockProperty());

                //Assert
                Assert.Equal(1, context.Properties.Count());
                Assert.Equal("testaddress", context.Properties.FirstOrDefault().Address);
            }
        }

        [Fact]
        public void TestGetById()
        {
            //Arrange
            var services = GetPropertyServiceProivder();
            using (var scope = services.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IPropertyRepository>();
                var service = new PropertyService(repo, _mapper);
                var context = scope.ServiceProvider.GetService<EstateAgentContext>();
                var controller = new PropertyController(service);
                //Clear database
                context.Database.EnsureDeleted();

                //Act
                controller.AddProperty(GetMockProperty());

                ActionResult<PropertyDTO> p = controller.GetById(1);

                //Assert
                //var result = p.Result as OkObjectResult;

                Assert.Equal("testaddress", p.Value.Address);
            }

        }

    }
}
