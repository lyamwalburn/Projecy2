using EstateAgentAPI.Buisness.DTO;

namespace EstateAgentAPI.Buisness.Services
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
