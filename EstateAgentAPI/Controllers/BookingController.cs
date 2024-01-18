using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Models;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.Serialization;

namespace EstateAgentAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    //Authorisation Scheme
    [Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BookingDTO> AddBooking(BookingDTO booking)
        {
            var IsPropertyIdExists = _dbContext.Bookings.Any(b => b.PropertyId == booking.PropertyId);
              var IsBuyerIdExists= _dbContext.Bookings.Any(b=> b.BuyerId == booking.BuyerId);
            var IfBuyerNotExists = _dbContext.Bookings.Any(b=> b.BuyerId != booking.BuyerId);

           //DateTime bookingTime = (DateTime)booking.Time;
        //   int bookingHour= bookingTime.Hour;
           // var dateTimeCheck = _dbContext.Bookings.Any(b => b.Time == booking.Time);
           
           
          
           if(booking == null) { return BadRequest(); }
            if (IsPropertyIdExists)
            {
                var dateTimeCheck = _dbContext.Bookings.Any(b => b.Time == booking.Time);
                if (dateTimeCheck)
                {
                    ModelState.AddModelError("PropertyId", "property time slot already booked"); return BadRequest(ModelState);
                } 
            }
            if (IsBuyerIdExists)
            {
                var dateTimeCheckforBuyer = _dbContext.Bookings.Any(b => b.Time == booking.Time);
                if (dateTimeCheckforBuyer)
                {
                    ModelState.AddModelError("PropertyId", "Buyer time slot already booked"); return BadRequest(ModelState);
                }
            }


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
