using AutoMapper;
using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;

namespace EstateAgentAPI.Buisness.Services
{
    public class BuyerService : IBuyerService
    {
        IBuyerRepository _buyersRepository;
        private IMapper _mapper;

        public BuyerService(IBuyerRepository buyersRepository, IMapper mapper)
        {
            _buyersRepository = buyersRepository;
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
            b.Postcode = buyerData.Postcode;
            b.Phone = buyerData.Phone;

            Buyer buy = _buyersRepository.Update(b);
            BuyerDTO dtoBuyer = _mapper.Map<BuyerDTO>(buy);
            return dtoBuyer;
        }
    }
}
