using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Buisness.Services;
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


    }
}
