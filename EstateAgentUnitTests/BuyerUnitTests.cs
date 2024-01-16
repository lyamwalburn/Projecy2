using AutoMapper;
using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Buisness.Services;
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

namespace EstateAgentUnitTests
{
    public class BuyerUnitTests
    {
        private Mapper _mapper;
        private IBuyerRepository _repo;
        private BuyerService _service;
        private EstateAgentContext _context;
        private BuyerController _controller;

        public BuyerUnitTests()
        {
            TPCAutoMapper myProfile = new TPCAutoMapper();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
        }

        private void Setup(IServiceScope scope)
        {
            _repo= scope.ServiceProvider.GetService<IBuyerRepository>();
            _service = new BuyerService(_repo, _mapper);
            _context = scope.ServiceProvider.GetService<EstateAgentContext>();
            _controller = new BuyerController(_service);
        }

        private IServiceProvider GetBuyerServiceProivder()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<EstateAgentContext>(options => options.UseInMemoryDatabase("MockDBData"));
            services.AddScoped<IBuyerService, BuyerService>();
            services.AddScoped<IBuyerRepository, BuyerRepository>();
            services.AddScoped<BuyerController>();
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            return services.BuildServiceProvider();
        }

        private BuyerDTO GetMockBuyer()
        {
            return new BuyerDTO
            {
                Id = 1,
                FirstName = "Steve",
                Surname = "Smith",
                Address = "1 Main Street",
                Postcode = "TE 5T",
                Phone = "12345"
            };
        }

        [Fact]
        public void TestAddBuyer()
        {
            var services = GetBuyerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();

                var buyerDTO = new BuyerDTO
                {
                    FirstName = "testname",
                    Surname = "surnametest",
                    Address = "123 test street",
                    Postcode = "TE 5T",
                    Phone = "123456789"
                };

                _controller.AddBuyer(buyerDTO);
                var buyer = _context.Buyers.Single();

                Assert.Equal(1, buyer.Id);
                Assert.Equal("testname", buyer.FirstName);
            }
        }


        [Fact]
        public void TestGetBuyer()
        {
            var services = GetBuyerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                //Add buyer to db
                _controller.AddBuyer(GetMockBuyer());

                var buyerFromDb = _controller.Index().FirstOrDefault();
                
                Assert.Equal(1, buyerFromDb.Id);
                Assert.Equal("Smith", buyerFromDb.Surname);
                Assert.Equal("Steve", buyerFromDb.FirstName);
                Assert.Equal("1 Main Street", buyerFromDb.Address);
                Assert.Equal("TE 5T", buyerFromDb.Postcode);
                Assert.Equal("12345", buyerFromDb.Phone);
            }
        }

        [Fact]
        public void TestPutBuyer()
        {
            var services = GetBuyerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                _controller.AddBuyer(GetMockBuyer());

                var buyerToUpdate = new BuyerDTO
                {
                    Id = 1,
                    FirstName = "updated firstname",
                    Surname = "Got Married",
                    Address = "123 moved road",
                    Postcode = "TT 45 9",
                    Phone = "0987654321"
                };

                _controller.UpdateBuyer(buyerToUpdate);

                var buyerFromDb = _repo.FindById(1);

                Assert.Equal("updated firstname", buyerFromDb.FirstName);
                Assert.Equal("Got Married", buyerFromDb.Surname);
                Assert.Equal("123 moved road", buyerFromDb.Address);
                Assert.Equal("TT 45 9", buyerFromDb.Postcode);
                Assert.Equal("0987654321", buyerFromDb.Phone);
            }
            
        }

        [Fact]
        public void TestDeleteBuyer()
        {
            var services = GetBuyerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                _controller.AddBuyer(GetMockBuyer());

                var expectedRecordsCount = 0;

                _controller.DeleteBuyer(1);

                var buyersFromDb = _controller.Index();

                Assert.Equal(expectedRecordsCount, buyersFromDb.Count());
            }
        }

        [Fact]
        public void TestGetById()
        {
            var services = GetBuyerServiceProivder();
            using (var scope = services.CreateScope())
            {
                Setup(scope);
                //Clear database
                _context.Database.EnsureDeleted();
                _controller.AddBuyer(GetMockBuyer());

                var secondBuyer = new BuyerDTO
                {
                    Id = 2,
                    FirstName = "updated firstname",
                    Surname = "Got Married",
                    Address = "123 moved road",
                    Postcode = "TT 45 9",
                    Phone = "0987654321"
                };
                _controller.AddBuyer(secondBuyer);

                ActionResult<BuyerDTO> buyerFromId = _controller.GetById(2);

                Assert.Equal(2,buyerFromId.Value.Id);
                Assert.Equal("updated firstname", buyerFromId.Value.FirstName);
                Assert.Equal("Got Married", buyerFromId.Value.Surname);
                Assert.Equal("123 moved road", buyerFromId.Value.Address);
                Assert.Equal("TT 45 9", buyerFromId.Value.Postcode);
                Assert.Equal("0987654321", buyerFromId.Value.Phone);
            }
        }

        [Fact]
        public void Test404ResponseGetBuyerById()
        {
            var services = GetBuyerServiceProivder();
            using(var scope = services.CreateScope())
            {
                Setup(scope);
                _context.Database.EnsureDeleted();
                _controller.AddBuyer(GetMockBuyer());

                var actionResult = _controller.GetById(99);

                var result = actionResult.Result as NotFoundResult;
                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }
    }
}