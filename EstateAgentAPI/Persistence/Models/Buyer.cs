using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgentAPI.Persistence.Models
{
    public class Buyer :EntityBase, IEquatable<Buyer>, ICloneable
    {
        [Column("BUYER_ID")]
        [Key]
        public override int Id { get; set; }
        //  public int BuyerId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }

        public object Clone()
        {
            return new Buyer
            {
                Id = this.Id,
                FirstName = this.FirstName,
                Surname = this.Surname,
                Address = this.Address,
                Postcode = this.Postcode,
                Phone = this.Phone
            };
        }

        public bool Equals(Buyer? other)
        {
            return Id == other.Id;
        }
    }
}
