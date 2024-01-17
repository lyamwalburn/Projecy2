using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
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
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
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
        public PropertyDTO AddProperty(PropertyDTO property)
        {
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


        // functionality based on property status
        [HttpPatch("sell{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDTO> SellProperty(PropertyDTO property)
        {
            property = _propertyService.SellProperty(property);
            if (property == null) return NotFound();
            return property;
        }

        [HttpPatch("withdraw{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDTO> WithdrawProperty(int propertyId)
        {
            PropertyDTO property = _propertyService.WithdrawProperty(propertyId);
            if (property == null) return NotFound();
            return property;
        }

        [HttpPatch("relist{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDTO> RelistProperty(int propertyId)
        {
            PropertyDTO property = _propertyService.RelistWithdrawnProperty(propertyId);
            if (property == null) return NotFound();
            return property;
        }
    }
}
