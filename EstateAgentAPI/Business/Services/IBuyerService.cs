using EstateAgentAPI.Business.DTO;

namespace EstateAgentAPI.Business.Services
{
    public interface IBuyerService
    {
        IQueryable<BuyerDTO> FindAll();
        BuyerDTO FindById(int id);
        BuyerDTO Create(BuyerDTO entity);
        BuyerDTO Update(BuyerDTO entity);
        void Delete (BuyerDTO entity);



    }
}
