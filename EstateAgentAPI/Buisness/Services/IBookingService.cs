using EstateAgentAPI.Buisness.DTO;

namespace EstateAgentAPI.Buisness.Services
{
    public interface IBookingService
    {
        IQueryable<BookingDTO> FindAll();
        BookingDTO FindById(int id);
        BookingDTO Create(BookingDTO entity);
        BookingDTO Update(BookingDTO entity);
        void Delete(BookingDTO entity);
    }
}
