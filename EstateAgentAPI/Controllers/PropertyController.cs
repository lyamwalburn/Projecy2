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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDTO> GetById(int id)
        {
            var property = _propertyService.FindById(id);
            return property == null ? NotFound() : property;
        }

        [HttpPost]
        public ActionResult<PropertyDTO> AddProperty(PropertyDTO property)
        {
            checkIfBuyerIdReturns(property);
            property = _propertyService.Create(property);
            return property;
        }

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


    }
}
