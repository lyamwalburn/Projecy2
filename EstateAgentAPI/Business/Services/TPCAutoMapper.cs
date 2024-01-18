using AutoMapper;
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;

namespace EstateAgentAPI.Business.Services
{
    public class TPCAutoMapper:Profile
    {
        public TPCAutoMapper() {
            CreateMap<Buyer,BuyerDTO>();
            CreateMap<BuyerDTO,Buyer>();
            CreateMap<Seller, SellerDTO>();
            CreateMap<SellerDTO,Seller>();
            CreateMap<Property, PropertyDTO>();
            CreateMap<PropertyDTO, Property>();
            CreateMap<Booking, BookingDTO>();
            CreateMap<BookingDTO, Booking>();
        }
    }
}
