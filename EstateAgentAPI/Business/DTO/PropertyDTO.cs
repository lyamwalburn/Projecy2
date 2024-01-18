
using EstateAgentAPI.Business.Helpers.SellerValidationAttributes;
using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.DTO
{
    public class PropertyDTO : EntityBase, IEquatable<PropertyDTO>
    {
        public PropertyDTO() { }

        [Key]
        public override int Id { get; set; }
        public int PropertyId { get { return Id; } set { Id = value; } }
        [Required(ErrorMessage = "Address  Required")]
        [BuyerAddressSpecialCharValidation]
        public string? Address { get; set; }
        [Required(ErrorMessage = "PostCode Required")]
        [BuyerPostCodeSpecialCharValidation]
        public string? PostCode { get; set; }
        [Required(ErrorMessage = "Property Type Required")]
        [PropertyTypeValidation]
        public string? Type { get; set; }
        [Required(ErrorMessage = "Number of Bedrooms  Required")]
        [PropertyNumberValidation]
        public int? NumberOfBedrooms { get; set; }
        [Required(ErrorMessage = "Number of Bathrooms  Required")]
        [PropertyNumberValidation]
        public int? NumberOfBathrooms { get; set; }
        [Required(ErrorMessage = "Garden option must be selected")]
        public bool? Garden { get; set; }
        [Required(ErrorMessage = "Price Required")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Status Required")]
        public string? Status { get; set; }
        [Required(ErrorMessage = "SellerId Is Required")]
        public int? SellerId { get; set; }
       // [Required(ErrorMessage = "BuyerId Is Required")]
        public int? BuyerId { get; set; }

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
                SellerId = this.SellerId,
                BuyerId = this.BuyerId
            };
        }
    }
}