using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Buisness.DTO
{
    public class PropertyDTO : EntityBase, IEquatable<PropertyDTO>
    {
        public PropertyDTO() { }

        [Key]
        public override int Id { get; set; }
        public int PropertyId { get { return Id; } set { Id = value; } }
        public string? Address { get; set; }
        public string? PostCode { get; set; }
        public string? Type { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public bool? Garden { get; set; }
        public decimal? Price { get; set; }
        public string? Status { get; set; }
        public int? SellerID { get; set; }
        public int? BuyerID { get; set; }
       

        public bool Equals(PropertyDTO? other)
        {
            return Id == other.Id;
        }

        public object Clone()
        {
            return new PropertyDTO
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
                SellerID = this.SellerID,
                BuyerID = this.BuyerID

            };
        }
    }
}