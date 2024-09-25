using ProductBusiness;
using Microsoft.AspNetCore.Mvc;

namespace Apis.Controllers
{
	/// <summary>
	/// Controller for managing products.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{

		private readonly ILogger<ProductController> _logger;
		private readonly ProductBL _business;

		public ProductController(ILogger<ProductController> logger, ProductBL business)
		{
			_logger = logger;
			_business = business;
		}


		/// <summary>
		/// Retrieves a list of products, optionally filtered by price and size, 
		/// and highlights specified words in the product descriptions.
		/// </summary>
		/// <param name="minprice">Optional minimum price to filter products.</param>
		/// <param name="maxprice">Optional maximum price to filter products.</param>
		/// <param name="size">Optional comma-separated string of sizes to filter products.</param>
		/// <param name="highlight">Optional comma-separated string of words to highlight in product descriptions.</param>
		/// <returns>A list of filtered products.</returns>
		[HttpGet()]
		public async Task<IActionResult> GetFilterProductsAsync(
					[FromQuery] decimal? minprice = null,
					[FromQuery] decimal? maxprice = null,
					[FromQuery] string? size = null,
					[FromQuery] string? highlight = null)
		{
			var products = await _business.GetProductsAsync("https://pastebin.com/raw/JucRNpWs");

			products = _business.FilterProduct(minprice, maxprice, size!, highlight!, products!);	
			var details  = _business.GetProductDetails(products);
			products = _business.Highlight(highlight!, products);
			return Ok (new {products,details});
		}

	}
}