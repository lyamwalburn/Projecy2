using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using EstateAgentAPI.EF;
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
        private EstateAgentContext _dbContext;
        public BookingController(IBookingService bookingService, EstateAgentContext dbContext)
        {
            _bookingService = bookingService;
            _dbContext = dbContext;
            
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
        public ActionResult<BookingDTO> AddBooking(BookingDTO booking)
        {
            var IsPropertyIdExists = _dbContext.Bookings.Any(b => b.PropertyId == booking.PropertyId);
              var IsBuyerIdExists= _dbContext.Bookings.Any(b=> b.BuyerId == booking.BuyerId);
              var dateTimeCheck = _dbContext.Bookings.Any(b => b.Time == booking.Time);
            if(booking == null) { return BadRequest(); }
            if (IsPropertyIdExists && IsBuyerIdExists && dateTimeCheck) { ModelState.AddModelError("PropertyId", "time slot already booked"); return BadRequest(ModelState);  }
            
            booking = _bookingService.Create(booking);
            return booking;
        
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BookingDTO> UpdateBooking(BookingDTO booking)
        {
            booking = _bookingService.Update(booking);
            if (booking == null) return NotFound();
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
