using AutoMapper;
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using EstateAgentAPI.Controllers;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace EstateAgentUnitTests.ControllerTests
{
    public class SellerControllerUnitTests
    {
        private Mapper _mapper;
        private ISellerRepository _repo;
        private IPropertyRepository _repo2;
        private SellerService _service;
        private EstateAgentContext _context;
        private SellerController _controller;
        public SellerControllerUnitTests()
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
            _controller = new SellerController(_service,_context);

        }

        private IServiceProvider GetSellerServiceProivder()
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

        private SellerDTO GetMockSeller()
        {
            return new SellerDTO
            {
                Id = 1,
                FirstName = "Steve",
                Surname = "Smith",
                Address = "1 Main Street",
                PostCode = "TE 5T",
                Phone = "12345"
            };
        }

        [Fact]
        public void TestAddSeller()
        {
            var services = GetSellerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();

                var sellerDTO = new SellerDTO
                {
                    Id = 34,
                    FirstName = "testname",
                    Surname = "surnametest",
                    Address = "123 test street",
                    PostCode = "TE 5T",
                    Phone = "123456789"
                };

                _controller.AddSeller(sellerDTO);
                var seller = _context.Sellers.SingleOrDefault(predicate => predicate.Id == 34);

                Assert.Equal(34, seller.Id);
                Assert.Equal("testname", seller.FirstName);
            }
        }


        [Fact]
        public void TestGetSeller()
        {
            var services = GetSellerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                //Add seller to db
                _controller.AddSeller(GetMockSeller());

                var sellerFromDb = _controller.Index().FirstOrDefault();

                Assert.Equal(1, sellerFromDb.Id);
                Assert.Equal("Smith", sellerFromDb.Surname);
                Assert.Equal("Steve", sellerFromDb.FirstName);
                Assert.Equal("1 Main Street", sellerFromDb.Address);
                Assert.Equal("TE 5T", sellerFromDb.PostCode);
                Assert.Equal("12345", sellerFromDb.Phone);
            }
        }

        [Fact]
        public void TestUpdateSeller()
        {
            var services = GetSellerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                _controller.AddSeller(GetMockSeller());

                var sellerToUpdate = new SellerDTO
                {
                    Id = 1,
                    FirstName = "updated firstname",
                    Surname = "Got Married",
                    Address = "123 moved road",
                    PostCode = "TT 45 9",
                    Phone = "0987654321"
                };

                _controller.UpdateSeller(sellerToUpdate);

                var sellerFromDb = _repo.FindById(1);

                Assert.Equal("updated firstname", sellerFromDb.FirstName);
                Assert.Equal("Got Married", sellerFromDb.Surname);
                Assert.Equal("123 moved road", sellerFromDb.Address);
                Assert.Equal("TT 45 9", sellerFromDb.PostCode);
                Assert.Equal("0987654321", sellerFromDb.Phone);
            }

        }

        [Fact]
        public void TestDeleteSeller()
        {
            var services = GetSellerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                _controller.AddSeller(GetMockSeller());

                var expectedRecordsCount = 0;

                _controller.DeleteSeller(1);

                var sellersFromDb = _controller.Index();

                Assert.Equal(expectedRecordsCount, sellersFromDb.Count());
            }
        }

        [Fact]
        public void TestGetById()
        {
            var services = GetSellerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                _controller.AddSeller(GetMockSeller());

                var secondSeller = new SellerDTO
                {
                    Id = 2,
                    FirstName = "updated firstname",
                    Surname = "Got Married",
                    Address = "123 moved road",
                    PostCode = "TT 45 9",
                    Phone = "0987654321"
                };
                _controller.AddSeller(secondSeller);

                ActionResult<SellerDTO> sellerFromId = _controller.GetById(2);

                Assert.Equal(2, sellerFromId.Value.Id);
                Assert.Equal("updated firstname", sellerFromId.Value.FirstName);
                Assert.Equal("Got Married", sellerFromId.Value.Surname);
                Assert.Equal("123 moved road", sellerFromId.Value.Address);
                Assert.Equal("TT 45 9", sellerFromId.Value.PostCode);
                Assert.Equal("0987654321", sellerFromId.Value.Phone);
            }
        }

        [Fact]
        public void Test404ResponseGetSellerById()
        {
            var services = GetSellerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                _context.Database.EnsureDeleted();
                _controller.AddSeller(GetMockSeller());

                var actionResult = _controller.GetById(99);

                var result = actionResult.Result as NotFoundObjectResult;
                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }

        [Fact]
        public void Test404ResponseUpdateSeller()
        {
            var services = GetSellerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                _context.Database.EnsureDeleted();
                _controller.AddSeller(GetMockSeller());

                var sellerToUpdate = new SellerDTO
                {
                    Id = 99,
                    FirstName = "updated firstname",
                    Surname = "Got Married",
                    Address = "123 moved road",
                    PostCode = "TT 45 9",
                    Phone = "0987654321"
                };

                var actionResult = _controller.UpdateSeller(sellerToUpdate);

                var result = actionResult.Result as NotFoundResult;
                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }

        [Fact]
        public void Test404ResponseDeleteSeller()
        {
            var services = GetSellerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                _context.Database.EnsureDeleted();
                _controller.AddSeller(GetMockSeller());

                var actionResult = _controller.DeleteSeller(99);
                ;
                Assert.NotNull(actionResult);
                Assert.Equal(HttpStatusCode.NotFound, actionResult);
            }
        }


    }
}