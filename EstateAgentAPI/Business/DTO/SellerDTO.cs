
using EstateAgentAPI.Buisness.Helpers.SellerValidationAttributes;
using EstateAgentAPI.Business.Helpers.SellerValidationAttributes;
using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.DTO
{
    public class SellerDTO : EntityBase, IEquatable<SellerDTO>
    {
        public SellerDTO()
        {

        }
        [Key]
        public override int Id { get; set; }

        public int SellerId { get { return Id; } set { Id = value; } }
        [Required(ErrorMessage = "First Name Is Required")]
        [SellerNameValidationAttribute]
        [SellerNameSpecialCharValidationAttribute]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        [SellerNameValidationAttribute]
        [SellerNameSpecialCharValidationAttribute]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Address Is Required")]
        [SellerPhoneSpecialCharValidation]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Postcode Is Required")]
        [SellerPostCodeSpecialCharValidation]
        public string? Postcode { get; set; }
        [Required(ErrorMessage = "Phone number Is Required")]
        [SellerPhoneSpecialCharValidationAttribute]
        public string? Phone { get; set; }

        public object Clone()
        {
            return new SellerDTO
            {
                Id = this.Id,
                // SellerId = this.SellerId,
                FirstName = this.FirstName,
                Surname = this.Surname,
                Address = this.Address,
                Postcode = this.Postcode,
                Phone = this.Phone

            };
        }





        public bool Equals(SellerDTO? other)
        {
            return Id == other.Id;
        }
    }
}