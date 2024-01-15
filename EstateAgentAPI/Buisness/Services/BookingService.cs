using AutoMapper;
using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;

namespace EstateAgentAPI.Buisness.Services
{
    public class BookingService : IBookingService
    {
        IBookingRepository _bookingsRepository;
        private IMapper _mapper;

        public BookingService(IBookingRepository bookingsRepository, IMapper mapper)
        {
            _bookingsRepository = bookingsRepository;
            _mapper = mapper;
        }

        public BookingDTO Create(BookingDTO dtoBooking)
        {
            Booking bookingData = _mapper.Map<Booking>(dtoBooking);
            bookingData = _bookingsRepository.Create(bookingData);
            dtoBooking = _mapper.Map<BookingDTO>(bookingData);
            return dtoBooking;
        }

        public void Delete(BookingDTO dtoBooking)
        {
            Booking booking = _mapper.Map<Booking>(dtoBooking);
            _bookingsRepository.Delete(booking);
        }

        public IQueryable<BookingDTO> FindAll() 
        {
            var bookings = _bookingsRepository.FindAll().ToList();
            List<BookingDTO> dtoBookings = new List<BookingDTO>();
            foreach(Booking booking in bookings)
            {
                dtoBookings.Add(_mapper.Map<BookingDTO>(booking));
            }
            return dtoBookings.AsQueryable();
        }

        public BookingDTO FindById(int id)
        {
            Booking booking= _bookingsRepository.FindById(id);
            BookingDTO dtoBooking = _mapper.Map<BookingDTO>(booking);
            return dtoBooking;
        }

        public BookingDTO Update(BookingDTO booking)
        {
            Booking bookingData = _mapper.Map<Booking>(booking);
            var b = _bookingsRepository.FindById(bookingData.Id);
            if (b == null) return null;

            b.BuyerId = bookingData.BuyerId;
            b.PropertyId = bookingData.PropertyId;
            b.Time = bookingData.Time;

            Booking book= _bookingsRepository.Update(b);
            BookingDTO dtoBooking= _mapper.Map<BookingDTO>(book);
            return dtoBooking;
        }
    }
}
