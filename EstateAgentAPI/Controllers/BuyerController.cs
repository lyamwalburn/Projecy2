using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EstateAgentAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuyerController : Controller
    {
        private IBuyerService _buyerService;
        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
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
            var buyer = _buyerService.FindById(id);
            return buyer == null ? NotFound() : buyer;
        }

        [HttpPost]
        public BuyerDTO AddBuyer(BuyerDTO buyer) {
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
