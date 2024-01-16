using EstateAgentAPI.Persistence.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgentAPI.Persistence.Models
{
    public class Booking : EntityBase, IEquatable<Booking>, ICloneable
    {
        [Column("BOOKING_ID")]
        [Key]
        public override int Id { get; set; }
        //public int BookingId { get; set; }
        public int BuyerId { get; set; }
        public int PropertyId { get;set; }
        public DateTime Time { get; set; }

        public object Clone()
        {
            return new Booking {
                Id = this.Id,
                BuyerId = this.BuyerId,
                PropertyId = this.PropertyId,
                Time = this.Time
            };
        }
            
        public bool Equals(Booking? other)
        {
            return Id == other.Id;
        }
    }
}
