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
    [Authorize]
    public class BuyerController : Controller
    {
        private IBuyerService _buyerService;
        EstateAgentContext _dbContext;
        public BuyerController(IBuyerService buyerService, EstateAgentContext dbContext)
        {
            _buyerService = buyerService;
            _dbContext= dbContext;
        }

        [HttpGet]
        public IEnumerable<BuyerDTO> Index()
        {
            var buyers = _buyerService.FindAll().ToList();
            return buyers;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BuyerDTO> GetById(int id)
        {
            var IsBuyerIdExists = _dbContext.Buyers.Any(b => b.Id == id);
            if (!IsBuyerIdExists) { ModelState.AddModelError("BuyerId", "Buyer not found"); return NotFound(ModelState); }

            var buyer = _buyerService.FindById(id);
            return buyer;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BuyerDTO> AddBuyer(BuyerDTO buyer) {
            var IsFirstNameExists = _dbContext.Buyers.Any(b => b.FirstName == buyer.FirstName);

            var IsLastNameExists = _dbContext.Buyers.Any(b => b.Surname == buyer.Surname);
            if (IsFirstNameExists && IsLastNameExists) { ModelState.AddModelError("Buyer", "Buyer already exists"); return BadRequest(ModelState); }

            buyer = _buyerService.Create(buyer);
            return buyer;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BuyerDTO> UpdateBuyer(BuyerDTO buyer)
        {
            buyer = _buyerService.Update(buyer);
            if (buyer == null)
                return NotFound();
            return buyer;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public HttpStatusCode DeleteBuyer(int id)
        {
            var buyer = _buyerService.FindById(id);
            if (buyer == null)
                return HttpStatusCode.NotFound;
            _buyerService.Delete(buyer);
            return HttpStatusCode.NoContent;
        }


    }
}
