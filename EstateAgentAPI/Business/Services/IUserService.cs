using EstateAgentAPI.Business.DTO;

namespace EstateAgentAPI.Business.Services
{
    public interface IUserService
    {
        IQueryable<UserDTO> FindAll();
        UserDTO FindById(int id);
        UserDTO Create(UserDTO entity);
        UserDTO Update(UserDTO entity);
        void Delete(UserDTO entity);



    }
}
