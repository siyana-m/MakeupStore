using Makeup.Services.DTO.Orders;
using Makeup.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Makeup.Web.Pages
{
    [Authorize]
    public class CartModel : PageModel
    {
        private readonly OrdersService _os;
        public CartModel(OrdersService os)
        {
            _os = os;
        }
        public OrderDto? Order { get; set; }
        public async Task OnGet()
        {
            this.Order = await _os.GetLatestOrderByUser(User!.Identity!.Name!);
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            string username = User!.Identity!.Name!;
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("User is not authenticated.");
            }

            try
            {
                await _os.AddProduct(username, productId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding product to cart: {ex.Message}");
            }
        }

    }

}
