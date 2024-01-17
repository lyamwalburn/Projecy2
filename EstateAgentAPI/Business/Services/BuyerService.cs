using AutoMapper;
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;

namespace EstateAgentAPI.Business.Services
{
    public class BuyerService : IBuyerService
    {
        IBuyerRepository _buyersRepository;
        IBookingRepository _bookingRepository;
        private IMapper _mapper;

        public BuyerService(IBuyerRepository buyersRepository, IBookingRepository bookingsRepository, IMapper mapper)
        {
            _buyersRepository = buyersRepository; 
            _bookingRepository = bookingsRepository;
            _mapper = mapper;
        }

        public BuyerDTO Create(BuyerDTO dtoBuyer)
        {
            Buyer buyerData = _mapper.Map<Buyer>(dtoBuyer);
            buyerData = _buyersRepository.Create(buyerData);
            dtoBuyer = _mapper.Map<BuyerDTO>(buyerData);
            return dtoBuyer;
        }

        public void Delete(BuyerDTO dtoBuyer)
        {
            Buyer buyer = _mapper.Map<Buyer>(dtoBuyer);

            //remove all bookings which have a matching buyerId from DB
            var bookings = _bookingRepository.FindAll().ToList();
            foreach (Booking booking in bookings)
            {
                if (booking.BuyerId == buyer.Id)
                {
                    _bookingRepository.Delete(booking);
                }
            }

            _buyersRepository.Delete(buyer);
        }

        public IQueryable<BuyerDTO> FindAll()
        {
            var buyers = _buyersRepository.FindAll().ToList();
            List<BuyerDTO> dtoBuyers = new List<BuyerDTO>();
            foreach(Buyer buyer in buyers)
            {
                dtoBuyers.Add(_mapper.Map<BuyerDTO>(buyer));
            }
            return dtoBuyers.AsQueryable();
        }

        public BuyerDTO FindById(int id)
        {
            Buyer buyer = _buyersRepository.FindById(id);
            BuyerDTO dtoBuyer = _mapper.Map<BuyerDTO>(buyer);
            return dtoBuyer;
        }

        public BuyerDTO Update(BuyerDTO buyer)
        {
            Buyer buyerData = _mapper.Map<Buyer>(buyer);
            var b = _buyersRepository.FindById(buyerData.Id);
            if (b == null)
                return null;

            b.FirstName = buyerData.FirstName;
            b.Surname = buyerData.Surname;
            b.Address = buyerData.Address;
            b.PostCode = buyerData.PostCode;
            b.Phone = buyerData.Phone;

            Buyer buy = _buyersRepository.Update(b);
            BuyerDTO dtoBuyer = _mapper.Map<BuyerDTO>(buy);
            return dtoBuyer;
        }
    }
}
