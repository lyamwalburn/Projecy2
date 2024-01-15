using EstateAgentAPI.Buisness.DTO;

namespace EstateAgentAPI.Buisness.Services
{
    public interface IPropertyService
    {
        IQueryable<PropertyDTO> FindAll();
        PropertyDTO FindById(int id);
        PropertyDTO Create(PropertyDTO entity);
        PropertyDTO Update(PropertyDTO entity);
        void Delete(PropertyDTO entity);
    }
}
