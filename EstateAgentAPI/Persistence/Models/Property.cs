using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgentAPI.Persistence.Models
{
    public class Property : EntityBase, IEquatable<Property>, ICloneable
    {

        [Column("PROPERTY_ID")]
        [Key]

        public override int Id { get; set; }
        //public int PropertyId { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Type { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public bool Garden { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int SellerId { get; set; }
        public int? BuyerId { get; set; }

        public object Clone()
        {
            return new Property
            {
                Id = this.Id,
                Address = this.Address,
                PostCode = this.PostCode,
                Type = this.Type,
                NumberOfBedrooms = this.NumberOfBedrooms,
                NumberOfBathrooms = this.NumberOfBathrooms,
                Garden = this.Garden,
                Price = this.Price,
                Status = this.Status,
                SellerId = this.SellerId,
                BuyerId = this.BuyerId
            };
        }

        public bool Equals(Property? other)
        {
            return Id == other.Id;
        }
    }
}
