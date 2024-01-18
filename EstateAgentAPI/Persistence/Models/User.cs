using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace EstateAgentAPI.Persistence.Models
{
    public class User : EntityBase, IEquatable<User>, ICloneable
    {
        public User()
        {

        }

        [Column("USER_ID")]
        [Key]
        public override int Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public object Clone()
        {
            return new User
            {
                Id = this.Id,
                Password = this.Password,
                UserName = this.UserName
            };
        }

        public bool Equals(User? other)
        {
            return Id == other.Id;
        }

    }
}