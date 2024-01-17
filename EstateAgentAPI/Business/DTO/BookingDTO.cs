using EstateAgentAPI.Business.Helpers.BookingValidationAttributes;
using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.DTO
{
    public class BookingDTO : EntityBase, IEquatable<BookingDTO>
    {
        public BookingDTO() { }

        [Key]
        public override int Id { get; set; }
        public int BookingId { get { return Id; } set { Id = value; } }
        [Required(ErrorMessage = "BuyerId Required")]
        public int? BuyerId { get; set; }
        [Required(ErrorMessage = "PropertyId Required")]
        public int? PropertyId { get; set; }
        [Required(ErrorMessage = "BookingTime Required")]
        [BookingTimeValidation]
        public DateTime? Time { get; set; }

        public bool Equals(BookingDTO? other)
        {
            return Id == other.Id;
        }

        public object Clone()
        {
            return new BookingDTO
            {
                Id = this.Id,
                BuyerId = this.BuyerId,
                PropertyId = this.PropertyId,
                Time = this.Time
            };
        }
    }
}
