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
        public int? BuyerId { get; set; }
        public int? PropertyId { get; set; }
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
