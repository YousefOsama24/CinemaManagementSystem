using CinemaSystemManagement.Data;
using CinemaSystemManagement.Models;
using CinemaSystemManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace CinemaSystemManagement.Areas.Customer.Controllers
{
    [Area(SD.CUSTOMER_AREA)]
    public class BookingController : Controller
    {
        private readonly AppDbContext _db;

        public BookingController(AppDbContext db)
        {
            _db = db;
        }

        // ================= SELECT SEATS =================
        public async Task<IActionResult> SelectSeat(int id)
        {
            var show = await _db.ShowTimes
                .Include(s => s.Seats)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (show == null)
                return NotFound();

            return View(show);
        }

        // ================= CHECKOUT =================
        [HttpPost]
        public async Task<IActionResult> Checkout(int showId, List<int> seatIds)
        {
            if (seatIds == null || !seatIds.Any())
                return BadRequest("No seats selected");

            var show = await _db.ShowTimes
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(x => x.Id == showId);

            if (show == null)
                return NotFound();

            var seats = await _db.Seats
                .Where(s => seatIds.Contains(s.Id))
                .ToListAsync();

            if (seats.Any(s => s.IsBooked))
                return BadRequest("Some seats already booked");

            var domain = "https://localhost:7228/";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(show.Price * 100),

                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = show.Movie.Name
                            }
                        },
                        Quantity = seatIds.Count
                    }
                },

                Mode = "payment",

                SuccessUrl = domain + $"Customer/Booking/Success?showId={showId}&seatIds={string.Join(",", seatIds)}",
                CancelUrl = domain + "Customer/Booking/Cancel"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }

        // ================= SUCCESS =================
        public async Task<IActionResult> Success(int showId, string seatIds)
        {
            var ids = seatIds.Split(',').Select(int.Parse).ToList();

            var seats = await _db.Seats
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            foreach (var seat in seats)
            {
                seat.IsBooked = true;

                _db.Tickets.Add(new Ticket
                {
                    ShowTimeId = showId,
                    SeatId = seat.Id,
                    PurchaseDate = DateTime.Now
                });
            }

            await _db.SaveChangesAsync();

            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}