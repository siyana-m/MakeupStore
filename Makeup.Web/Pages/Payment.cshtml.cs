using Makeup.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace Makeup.Web.Pages
{
    [Authorize]
    public class PaymentModel : PageModel
    {
        private readonly OrdersService _os;
        private readonly ILogger<PaymentModel> _logger;
        public PaymentModel(OrdersService os, ILogger<PaymentModel> logger)
        {
            _os = os;
            _logger = logger;
        }
        [BindProperty]
        public string? StripeToken { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var order = await _os.GetLatestOrderByUser(User!.Identity!.Name!);

                if (order == null)
                {
                    return RedirectToPage("/Error", new { message = "Order not found" });
                }

                if (order.IsPaid)
                {
                    RedirectToPage("/");
                }

                StripeConfiguration.ApiKey = "sk_test_51OtqA2EShR09FPvo3stfdGTJwMi9PDojuIe8asp32EB2PleRQ368znZaWlqSqnh94Ss2b4xDQM9pY1V52RdDgo1500Hxngq7Ti";
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(order.OrderDetails!.Sum(x => x.Quantity * x.UnitPrice)) * 100,
                    Currency = "bgn",
                    Description = "Makeup Order #" + order.Id,
                    AutomaticPaymentMethods = new
               PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                };
                var service = new PaymentIntentService();
                var paymentIntent = service.Create(options);
                this.StripeToken = paymentIntent.ClientSecret;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the payment for user {UserName}", User!.Identity!.Name);
                return RedirectToPage("/Error", new { message = "An error occurred while processing your payment. Please try again." });
            }
        }
    }
}
