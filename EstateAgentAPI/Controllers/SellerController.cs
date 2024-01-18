
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EstateAgentAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SellerController : ControllerBase
    {
        ISellerService _sellerService;
        EstateAgentContext _dbContext;
        public SellerController(ISellerService service, EstateAgentContext dbContext)
        {
            _sellerService = service;
            _dbContext = dbContext;

        }

        [HttpGet]
        public IEnumerable<SellerDTO> Index()
        {
            var sellers = _sellerService.FindAll().ToList();
            return sellers;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SellerDTO> GetById(int id)
        {
            var IsSellerIdExists = _dbContext.Sellers.Any(b => b.Id == id);
            if (!IsSellerIdExists) { ModelState.AddModelError("SellerId", "Seller not found"); return NotFound(ModelState); }
        
            var seller = _sellerService.FindById(id);
            return seller;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<SellerDTO> AddSeller(SellerDTO seller)
        {
            var IsFirstNameExists= _dbContext.Sellers.Any(b => b.FirstName == seller.FirstName);

            var IsLastNameExists = _dbContext.Sellers.Any(b => b.Surname == seller.Surname);
            if (IsFirstNameExists && IsLastNameExists) { ModelState.AddModelError("Seller", "Seller already exists"); return BadRequest(ModelState); }

            seller = _sellerService.Create(seller);
            return seller;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SellerDTO> UpdateSeller(SellerDTO seller)
        {
            seller = _sellerService.Update(seller);
            if (seller == null) return NotFound();
            return seller;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public HttpStatusCode DeleteSeller(int id)
        {
            var seller = _sellerService.FindById(id);
            if (seller == null)
                return HttpStatusCode.NotFound;
            _sellerService.Delete(seller);
            return HttpStatusCode.NoContent;
        }
    }
}
