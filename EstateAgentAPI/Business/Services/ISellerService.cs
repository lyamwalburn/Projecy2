using EstateAgentAPI.Business.DTO;

namespace EstateAgentAPI.Business.Services
{
    public interface ISellerService
    {
        IQueryable<SellerDTO> FindAll();
        SellerDTO FindById(int id);
        SellerDTO Create(SellerDTO entity);
        SellerDTO Update(SellerDTO entity);
        void Delete(SellerDTO entity);
    }
}
