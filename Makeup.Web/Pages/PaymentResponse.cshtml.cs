using Makeup.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace Makeup.Web.Pages
{
    [Authorize]
    public class PaymentResponseModel : PageModel
    {
        private readonly OrdersService _os;
        public PaymentResponseModel(OrdersService os)
        {
            _os = os;
        }
        public async Task OnGetAsync(string? payment_intent)
        {
            if (string.IsNullOrEmpty(payment_intent))
            {
                RedirectToPage("Payment");
            }

            StripeConfiguration.ApiKey = "sk_test_51OtqA2EShR09FPvo3stfdGTJwMi9PDojuIe8asp32EB2PleRQ368znZaWlqSqnh94Ss2b4xDQM9pY1V52RdDgo1500Hxngq7Ti";
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(payment_intent);
            if (paymentIntent.Status == "succeeded")
            {
            await _os.SetOrderPaid(User!.Identity!.Name!);
            }
        }
    }

}
