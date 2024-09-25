using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace ProductBusiness
{
	public class ProductBL
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<ProductBL> _logger;

		public ProductBL(HttpClient httpClient, ILogger<ProductBL> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		/// <summary>
		/// Asynchronously fetches products from the specified URL.
		/// </summary>
		/// <param name="url">The URL to fetch products from.</param>
		/// <returns>A list of products, or null if the fetch fails.</returns>
		public async Task<List<Product>?> GetProductsAsync(string url)
		{
			try
			{
				var response = await _httpClient.GetAsync(url);
				response.EnsureSuccessStatusCode();
				_logger.LogInformation("Product fetched successfully at {Time}", DateTime.UtcNow);
				string jsonResponse = await response.Content.ReadAsStringAsync();
				_logger.LogInformation(jsonResponse);
				var productResponse = JsonSerializer.Deserialize<ProductResponse>(jsonResponse);
				_logger.LogInformation("Fetched {Count} products", productResponse?.Products!.Count);
				return productResponse?.Products;
			}
			catch (Exception)
			{
				_logger.LogError("Unable to fetch the product.");
				throw;
			}
		}

		/// <summary>
		/// Filters the provided list of products based on price and size criteria.
		/// </summary>
		/// <param name="minPrice">The minimum price to filter by (optional).</param>
		/// <param name="maxPrice">The maximum price to filter by (optional).</param>
		/// <param name="size">A comma-separated string of sizes to filter by (optional).</param>
		/// <param name="highlight">Words to highlight in the product descriptions (not used in this method).</param>
		/// <param name="product">The list of products to filter.</param>
		/// <returns>A list of products that meet the filter criteria.</returns>
		public List<Product> FilterProduct(decimal? minPrice, decimal? maxPrice, string size, string highlight, List<Product> product)
		{
			var products = new List<Product>();
			products.AddRange(product);

			// Filtering by price
			if (minPrice.HasValue)
			{
				products = products.Where(p => p.Price >= minPrice.Value).ToList();
			}

			if (maxPrice.HasValue)
			{
				products = products.Where(p => p.Price <= maxPrice.Value).ToList();
			}

			// Filtering by size
			if (!string.IsNullOrEmpty(size))
			{
				var sizeArray = size.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
							.Select(s => s.Trim())
							.ToList();

				products = products.Where(p => p.Sizes!.Any(ps => sizeArray.Contains(ps, StringComparer.OrdinalIgnoreCase))).ToList();
			}
			return products;
		}

		/// <summary>
		/// Highlights specified words in the product descriptions.
		/// </summary>
		/// <param name="highlight">A comma-separated string of words to highlight.</param>
		/// <param name="products">The list of products whose descriptions will be modified.</param>
		/// <returns>A list of products with highlighted descriptions.</returns>
		public List<Product> Highlight(string highlight, List<Product> products)
		{
			if (!string.IsNullOrEmpty(highlight))
			{
				var highlightWords = highlight.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var word in highlightWords)
				{
					products = products.Select(p =>
						new Product
						{
							Title = p.Title,
							Price = p.Price,
							Sizes = p.Sizes,
							Description = HighlightWords(p.Description!, word.Trim())
						}).ToList();
				}
			}

			return products;
		}

		/// <summary>
		/// Gets details about the filtered products including min/max price and available sizes.
		/// </summary>
		/// <param name="products">The list of products to analyze.</param>
		/// <returns>A <see cref="ProductsDetails"/> object containing filtering information.</returns>
		public ProductsDetails GetProductDetails(List<Product> products)
		{
			if (products.Count == 0) return new ProductsDetails();
			var filterDetails = new ProductsDetails
			{
				MinPrice = products.Min(p => p.Price),
				MaxPrice = products.Max(p => p.Price),
				Sizes = products.SelectMany(p => p.Sizes!).Distinct().ToList(),
				CommonWords = GetCommonWords(products)
			};
			return filterDetails;
		}

		/// <summary>
		/// Highlights a specific word in a product description.
		/// </summary>
		/// <param name="description">The product description to modify.</param>
		/// <param name="word">The word to highlight.</param>
		/// <returns>The modified description with the word highlighted.</returns>
		public string HighlightWords(string description, string word)
		{
			return description.Replace(word, $"<em>{word}</em>", StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Retrieves the most common words in the product descriptions, excluding the top five.
		/// </summary>
		/// <param name="products">The list of products to analyze.</param>
		/// <returns>A list of the ten most common words in the product descriptions.</returns>
		private List<string> GetCommonWords(List<Product> products)
		{
			var wordCounts = new Dictionary<string, int>();

			foreach (var product in products)
			{
				var words = product.Description!.Split(new[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var word in words)
				{
					var lowerWord = word.ToLower();
					if (wordCounts.ContainsKey(lowerWord))
					{
						wordCounts[lowerWord]++;
					}
					else
					{
						wordCounts[lowerWord] = 1;
					}
				}
			}

			return wordCounts.OrderByDescending(kvp => kvp.Value)
							 .Select(kvp => kvp.Key)
							 .Skip(5)
							 .Take(10)
							 .ToList();
		}
	}

}
