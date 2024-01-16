using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.DTO
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
        public string? PostCode { get; set; }
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
                PostCode = this.PostCode,
                Phone = this.Phone
            };
        }
    }
}
