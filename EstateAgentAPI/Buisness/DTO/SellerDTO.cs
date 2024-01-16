
using EstateAgentAPI.Buisness.Helpers.SellerValidationAttributes;

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

       
        public int SellerId { get { return Id; } set { Id = value; }}
        [SellerNameValidationAttribute]
        [SellerNameSpecialCharValidationAttribute]
      
       
        public string? FirstName { get; set; }
        [SellerNameValidationAttribute]
        [SellerNameSpecialCharValidationAttribute]
      
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        [SellerPhoneSpecialCharValidationAttribute]
        [MaxLength(10)]
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