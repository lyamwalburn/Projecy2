using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgentAPI.Persistence.Models
{
    public class Seller : EntityBase, IEquatable<Buyer>, ICloneable
    {
        public Seller()
        {

        }

        [Column("SELLER_ID")]
        [Key]
        public override int Id { get; set; }
        public string FirstName {  get; set; }
        public string Surname { get; set; }
        public string Address {  get; set; }   
        public string PostCode { get; set; }
        public string Phone { get; set; }

        public object Clone()
        {
            return new Seller
            {
                Id = this.Id,

                FirstName = this.FirstName,
                Surname = this.Surname,
                Address = this.Address,
                PostCode = this.PostCode,
                Phone = this.Phone

            };
        }

        public bool Equals(Buyer? other)
        {
            return Id == other.Id;
        }

    }
}
