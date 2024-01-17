using AutoMapper;
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;

namespace EstateAgentAPI.Business.Services
{
    public class PropertyService : IPropertyService
    {
        IPropertyRepository _propertiesRepository;
        IBookingRepository _bookingRepository;
        private IMapper _mapper;

        public PropertyService(IPropertyRepository propertiesRepository, IBookingRepository bookingsRepository, IMapper mapper)
        {
            _propertiesRepository = propertiesRepository;
            _bookingRepository = bookingsRepository;
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

            //remove all bookings which have a matching PropertyId from DB
            var bookings = _bookingRepository.FindAll().ToList();
            foreach (Booking booking in bookings)
            {
                if (booking.PropertyId == property.Id)
                {
                    _bookingRepository.Delete(booking);
                }
            }

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
            if (p == null) return null;

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

        public PropertyDTO SellProperty(PropertyDTO property)
        {
            Property propertyData = _mapper.Map<Property>(property);
            var p = _propertiesRepository.FindById(propertyData.Id);
            if (p == null) return null;
        
            p.Status = "SOLD";
            p.BuyerId = propertyData.BuyerId;
        
            Property prop = _propertiesRepository.Update(p);
            PropertyDTO dtoProperty = _mapper.Map<PropertyDTO>(prop);

            //remove all bookings which have a matching PropertyId from DB
            var bookings = _bookingRepository.FindAll().ToList();
            foreach (Booking booking in bookings)
            {
                if (booking.PropertyId == propertyData.Id)
                {
                    _bookingRepository.Delete(booking);
                }
            }

            return dtoProperty;
        }

        public PropertyDTO WithdrawProperty(int propertyId)
        {
            var p = _propertiesRepository.FindById(propertyId);
            if (p == null) return null;

            p.Status = "WITHDRAWN";

            Property prop = _propertiesRepository.Update(p);
            PropertyDTO dtoProperty = _mapper.Map<PropertyDTO>(prop);

            //remove all bookings which have a matching PropertyId from DB
            var bookings = _bookingRepository.FindAll().ToList();
            foreach (Booking booking in bookings)
            {
                if (booking.PropertyId == propertyId)
                {
                    _bookingRepository.Delete(booking);
                }
            }

            return dtoProperty;
        }

        public PropertyDTO RelistWithdrawnProperty(int propertyId)
        {
            var p = _propertiesRepository.FindById(propertyId);
            if (p == null) return null;

            p.Status = "FOR SALE";

            Property prop = _propertiesRepository.Update(p);
            PropertyDTO dtoProperty = _mapper.Map<PropertyDTO>(prop);

            return dtoProperty;
        }
    }
}