
using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EstateAgentAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SellerController : ControllerBase
    {
        ISellerService _sellerService;
        public SellerController(ISellerService service)
        {
            _sellerService = service;
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
            var seller = _sellerService.FindById(id);
            return seller == null ? NotFound() : seller;
        }
        [HttpPost]
        public SellerDTO AddSeller(SellerDTO seller)
        {
            seller = _sellerService.Create(seller);
            return seller;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SellerDTO> UpdateSeller(SellerDTO seller)
        {
            seller = _sellerService.Update(seller);
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
