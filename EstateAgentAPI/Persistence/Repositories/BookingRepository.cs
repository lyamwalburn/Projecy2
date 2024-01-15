using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;

namespace EstateAgentAPI.Persistence.Repositories
{
    public class BookingRepository : RepositoryBase<Booking>, IBookingRepository
    {
        public BookingRepository(EstateAgentContext repositoryContext) : base(repositoryContext) 
        { 
        }
    }
}
