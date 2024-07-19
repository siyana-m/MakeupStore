using Makeup.Services;
using Makeup.Services.DTO.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Climate;
using static System.Reflection.Metadata.BlobBuilder;

namespace Makeup.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ProductsService _productsService;
        private readonly OrdersService _ordersService; 
        public IndexModel(ProductsService productsService, OrdersService ordersService)
        {
            _productsService = productsService;
            _ordersService = ordersService; 

        }

        public string? SearchTerm { get; set; }
        public IList<ProductDto> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                Products = await _productsService.GetAll();
            }
            else
            {
                Products = await _productsService.Search(searchTerm);
            }
            return Page();
        }
        
        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {

            if (User!.Identity!.IsAuthenticated)
            {
                await _ordersService.AddProduct(User.Identity.Name!, productId);
                return RedirectToPage("/Cart");
            }
            else
            {
                return RedirectToPage("/Login");
            }
        }


    }
}
