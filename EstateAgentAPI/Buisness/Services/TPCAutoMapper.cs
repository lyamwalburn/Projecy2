using AutoMapper;
using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Persistence.Models;

namespace EstateAgentAPI.Buisness.Services
{
    public class TPCAutoMapper:Profile
    {
        public TPCAutoMapper() {
            CreateMap<Buyer,BuyerDTO>();
            CreateMap<BuyerDTO,Buyer>();
        }
    }
}
