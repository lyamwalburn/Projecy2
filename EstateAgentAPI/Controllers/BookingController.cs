using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Buisness.Services;
using EstateAgentAPI.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EstateAgentAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : Controller
    {
        private IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public IEnumerable<BookingDTO> Index()
        {
            var bookings = _bookingService.FindAll().ToList();
            return bookings;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BookingDTO> GetById(int id)
        {
            var booking = _bookingService.FindById(id);
            return booking == null ? NotFound() : booking;
        }

        [HttpPost]
        public BookingDTO AddBooking(BookingDTO booking)
        {
            booking = _bookingService.Create(booking);
            return booking;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BookingDTO> UpdateBuyer(BookingDTO booking)
        {
            booking = _bookingService.Update(booking);
            return booking;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public HttpStatusCode DeleteBooking(int id)
        {
            var booking = _bookingService.FindById(id);
            if (booking == null)
                return HttpStatusCode.NotFound;
            _bookingService.Delete(booking);
            return HttpStatusCode.NoContent;
        }


    }
}
