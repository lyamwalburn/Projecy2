using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Buisness.DTO
{
    public class BuyerDTO :EntityBase, IEquatable<BuyerDTO>
    {
        public BuyerDTO() { }

        [Key]
        public override int Id { get; set; }
        public int BuyerId { get { return Id; } set { Id = value; } }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public string? Phone { get; set; }

        public bool Equals(BuyerDTO? other)
        {
            return Id == other.Id;
        }

        public object Clone()
        {
            return new BuyerDTO
            {
                Id = this.Id,
                FirstName = this.FirstName,
                Surname = this.Surname,
                Address = this.Address,
                Postcode = this.Postcode,
                Phone = this.Phone
            };
        }
    }
}
