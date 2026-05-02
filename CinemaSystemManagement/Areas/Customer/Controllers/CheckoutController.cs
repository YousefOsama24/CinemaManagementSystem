using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace CinemaSystemManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Pay()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 5000,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Movie Ticket"
                            }
                        },
                        Quantity = 1
                    }
                },

                Mode = "payment",
                SuccessUrl = "https://localhost:7228/Customer/Checkout/Success",
                CancelUrl = "https://localhost:7228/Customer/Cart"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
