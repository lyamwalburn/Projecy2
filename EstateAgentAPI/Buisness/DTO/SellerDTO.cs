
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
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? PostCode { get; set; }
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
                PostCode = this.PostCode,
                Phone = this.Phone

            };
        }



       

        public bool Equals(SellerDTO? other)
        {
            return Id == other.Id;
        }
        }
}