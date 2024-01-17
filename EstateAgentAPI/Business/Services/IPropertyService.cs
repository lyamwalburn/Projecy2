using EstateAgentAPI.Business.DTO;

namespace EstateAgentAPI.Business.Services
{
    public interface IPropertyService
    {
        IQueryable<PropertyDTO> FindAll();
        PropertyDTO FindById(int id);
        PropertyDTO Create(PropertyDTO entity);
        PropertyDTO Update(PropertyDTO entity);
        void Delete(PropertyDTO entity);
        PropertyDTO SellProperty(int propertyId);
        PropertyDTO WithdrawProperty(int propertyId);
        PropertyDTO RelistWithdrawnProperty(int propertyId);
    }
}
