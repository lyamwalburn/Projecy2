using EstateAgentAPI.Buisness.Helpers.BuyerValidationAttributes;
using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.DTO
{
    public class BuyerDTO : EntityBase, IEquatable<BuyerDTO>
    {
        public BuyerDTO() { }

        [Key]
        public override int Id { get; set; }
        public int BuyerId { get { return Id; } set { Id = value; } }
        [Required(ErrorMessage = "FirstName Is Required")]
        [BuyerNameValidation]
        [BuyerNameSpecialCharValidation]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "LastName Is Required")]
        [BuyerNameValidation]
        [BuyerNameSpecialCharValidation]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "Address Is Required")]
        [BuyerNameSpecialCharValidation]
        public string? Address { get; set; }
        [Required(ErrorMessage = "PostCode Is Required")]
        [BuyerNameSpecialCharValidation]
        public string? PostCode { get; set; }
        [Required(ErrorMessage = "Phone Is Required")]

        [BuyerPhoneSpecialCharValidation]
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
