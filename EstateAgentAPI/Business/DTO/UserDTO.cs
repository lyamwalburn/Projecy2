using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.DTO
{
    public class UserDTO : EntityBase, IEquatable<UserDTO>
    {
        public UserDTO() { }

        [Key]
        public override int Id { get; set; }
        //public int UserId { get { return Id; } set { Id = value; } }
        public string? Password { get; set; }
        public string? UserName { get; set; }
   

        public bool Equals(UserDTO? other)
        {
            return Id == other.Id;
        }

        public object Clone()
        {
            return new UserDTO
            {
                Id = this.Id,
                Password = this.Password,
                UserName = this.UserName
            };
        }
    }
}
