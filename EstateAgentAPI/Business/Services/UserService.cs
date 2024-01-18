using AutoMapper;
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;

namespace EstateAgentAPI.Business.Services
{
    public class UserService : IUserService
    {
        IUserRepository _usersRepository;
        private IMapper _mapper;

        public UserService(IUserRepository UsersRepository, IMapper mapper)
        {
            _usersRepository = UsersRepository;
            _mapper = mapper;
       
        }

        public UserDTO Create(UserDTO dtoUser)
        {
            User UserData = _mapper.Map<User>(dtoUser);
            UserData = _usersRepository.Create(UserData);
            dtoUser = _mapper.Map<UserDTO>(UserData);
            return dtoUser;
        }

        public void Delete(UserDTO dtoUser)
        {
            User user = _mapper.Map<User>(dtoUser);
            _usersRepository.Delete(user);
        }

        public IQueryable<UserDTO> FindAll()
        {
            var Users = _usersRepository.FindAll().ToList();
            List<UserDTO> dtoUsers = new List<UserDTO>();
            foreach (User User in Users)
            {
                dtoUsers.Add(_mapper.Map<UserDTO>(User));
            }
            return dtoUsers.AsQueryable();
        }

        public UserDTO FindById(int id)
        {
            User User = _usersRepository.FindById(id);
            UserDTO dtoUser = _mapper.Map<UserDTO>(User);
            return dtoUser;
        }

        public UserDTO Update(UserDTO User)
        {
            User UserData = _mapper.Map<User>(User);
            var b = _usersRepository.FindById(UserData.Id);
            if (b == null)
                return null;

            b.UserName = UserData.UserName;
            b.Password = UserData.Password;
           

            User us = _usersRepository.Update(b);
            UserDTO dtoUser = _mapper.Map<UserDTO>(us);
            return dtoUser;
        }
    }
}
