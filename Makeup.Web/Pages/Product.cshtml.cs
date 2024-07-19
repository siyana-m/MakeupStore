using Makeup.Services;
using Makeup.Services.DTO.Products;
using Makeup.Services.External;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Climate;

namespace Makeup.Web.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ProductsService _productsService;

        private readonly YoutubeService _youtubeService;

        private readonly OrdersService _ordersService;


        public ProductDto? Product { get; set; }
        public YoutubeProductDto? Youtube { get; set; }
        public bool IsAddedToCart { get; set; }

        public ProductModel(ProductsService productsService, YoutubeService youtubeService, OrdersService ordersService)
        {
            _productsService = productsService;
            _youtubeService = youtubeService;
            _ordersService = ordersService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productsService.GetById(id);
            if (Product == null)
            {
                return NotFound();
            }

            this.Youtube = await _youtubeService.GetVideosAsync(Product.Name!);


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int productId)
        {
            await _ordersService.AddProduct(User!.Identity!.Name!, productId);
            this.IsAddedToCart = true;
            return RedirectToPage("/Cart");
        }

    }
}
