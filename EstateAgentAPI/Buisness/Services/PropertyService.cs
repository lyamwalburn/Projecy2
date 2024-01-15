using AutoMapper;
using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;

namespace EstateAgentAPI.Buisness.Services
{
    public class PropertyService : IPropertyService
    {
        IPropertyRepository _propertiesRepository;
        private IMapper _mapper;

        public PropertyService(IPropertyRepository propertiesRepository, IMapper mapper)
        {
            _propertiesRepository = propertiesRepository;
            _mapper = mapper;
        }

        public PropertyDTO Create(PropertyDTO dtoProperty)
        {
            Property propertyData = _mapper.Map<Property>(dtoProperty);
            propertyData = _propertiesRepository.Create(propertyData);
            dtoProperty = _mapper.Map<PropertyDTO>(propertyData);
            return dtoProperty;
        }

        public void Delete(PropertyDTO dtoProperty)
        {
            Property property = _mapper.Map<Property>(dtoProperty);
            _propertiesRepository.Delete(property);
        }

        public IQueryable<PropertyDTO> FindAll()
        {
            var properties = _propertiesRepository.FindAll().ToList();
            List<PropertyDTO> dtoProperties = new List<PropertyDTO>();
            foreach (Property property in properties)
            {
                dtoProperties.Add(_mapper.Map<PropertyDTO>(property));
            }
            return dtoProperties.AsQueryable();
        }

        public PropertyDTO FindById(int id)
        {
            Property property = _propertiesRepository.FindById(id);
            PropertyDTO dtoProperty = _mapper.Map<PropertyDTO>(property);
            return dtoProperty;
        }

        public PropertyDTO Update(PropertyDTO property)
        {
            Property propertyData = _mapper.Map<Property>(property);
            var p = _propertiesRepository.FindById(propertyData.Id);
            if (p == null)
                return null;

            p.Address = propertyData.Address;
            p.PostCode = propertyData.PostCode;
            p.Type = propertyData.Type;
            p.NumberOfBedrooms = propertyData.NumberOfBedrooms;
            p.NumberOfBathrooms = propertyData.NumberOfBathrooms;
            p.Garden = propertyData.Garden;
            p.Price = propertyData.Price;
            p.Status = propertyData.Status;

            Property prop = _propertiesRepository.Update(p);
            PropertyDTO dtoProperty = _mapper.Map<PropertyDTO>(prop);
            return dtoProperty;
        }
    }
}