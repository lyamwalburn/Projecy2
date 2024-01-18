using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EstateAgentAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropertyController : Controller
    {
        private IPropertyService _propertyService;
        private EstateAgentContext _dbContext;
        public PropertyController(IPropertyService propertyService, EstateAgentContext dbContext)
        {
            _propertyService = propertyService;
            _dbContext= dbContext;
        }

        [HttpGet]
        public IEnumerable<PropertyDTO> Index()
        {
            var properties = _propertyService.FindAll().ToList();
            return properties;
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDTO> GetById(int id)
        {
            var property = _propertyService.FindById(id);
            return property == null ? NotFound() : property;
        }

        [Authorize]
        [HttpPost]
        public ActionResult<PropertyDTO> AddProperty(PropertyDTO property)
        {
            checkIfBuyerIdReturns(property);
            property = _propertyService.Create(property);
            return property;
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDTO> UpdateProperty(PropertyDTO property)
        {
            property = _propertyService.Update(property);
            if (property == null)
                return NotFound();
           
            return property;
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public HttpStatusCode DeleteProperty(int id)
        {
            var property = _propertyService.FindById(id);
            if (property == null)
                return HttpStatusCode.NotFound;
            _propertyService.Delete(property);
            return HttpStatusCode.NoContent;
        }
        private bool checkIfBuyerIdReturns(PropertyDTO property)
        {
            var checkBuyerIdExists = _dbContext.Buyers.Any(b => b.Id == property.BuyerId);
            return property.Status == "SOLD" ? checkBuyerIdExists : false;
            
               
            }


        [Authorize]
        // functionality based on property status
        [Route("sell/{id}")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDTO> SellProperty(PropertyDTO property)
        {
            property = _propertyService.SellProperty(property);
            if (property == null) return NotFound();
            return property;
        }

        [Authorize]
        [Route("withdraw/{id}")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDTO> WithdrawProperty(int id)
        {
            PropertyDTO property = _propertyService.WithdrawProperty(id);
            if (property == null) return NotFound();
            return property;
        }


        [Authorize]
        [Route("relist/{id}")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDTO> RelistProperty(int id)
        {
            PropertyDTO property = _propertyService.RelistWithdrawnProperty(id);
            if (property == null) return NotFound();
            return property;
        }
    }
}
