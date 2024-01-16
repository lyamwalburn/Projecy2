using EstateAgentAPI.Business.DTO;

namespace EstateAgentAPI.Business.Services
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
