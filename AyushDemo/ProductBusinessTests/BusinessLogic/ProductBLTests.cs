using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace ProductBusiness.Tests
{
	[TestClass]
	public class ProductBLTests
	{
		private Mock<HttpMessageHandler>? _handlerMock;
		private HttpClient? _httpClient;
		private Mock<ILogger<ProductBL>>? _loggerMock;
		private ProductBL? _productBL;

		[TestInitialize]
		public void Setup()
		{
			_handlerMock = new Mock<HttpMessageHandler>();
			_httpClient = new HttpClient(_handlerMock.Object);
			_loggerMock = new Mock<ILogger<ProductBL>>();
			_productBL = new ProductBL(_httpClient, _loggerMock.Object);
		}


		/// <summary>
		/// Tests the FilterProduct method with valid filter parameters.
		/// </summary>
		[TestMethod]
		public void FilterProduct_WithValidFilters_ReturnsFilteredProducts()
		{
			// Arrange
			var products = new List<Product>
		{
			new Product { Title = "Red Trouser", Price = 15.0m, Sizes = new List<string> { "medium" } },
			new Product { Title = "Blue Trouser", Price = 20.0m, Sizes = new List<string> { "small" } }
		};

			// Act
			var result = _productBL.FilterProduct(10m, 20m, "medium", null, products);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("Red Trouser", result[0].Title);
		}

		/// <summary>
		/// Tests the FilterProduct method when minPrice is null.
		/// </summary>
		[TestMethod]
		public void FilterProduct_MinPriceNull_ReturnsAllProducts()
		{
			// Arrange
			var products = new List<Product>
		{
			new Product { Title = "Red Trouser", Price = 15.0m, Sizes = new List<string> { "medium" } },
			new Product { Title = "Blue Trouser", Price = 20.0m, Sizes = new List<string> { "small" } }
		};

			// Act
			var result = _productBL.FilterProduct(null, 20m, null, null, products);

			// Assert
			Assert.AreEqual(2, result.Count);
		}

		/// <summary>
		/// Tests the FilterProduct method when maxPrice is null.
		/// </summary>
		[TestMethod]
		public void FilterProduct_MaxPriceNull_ReturnsAllProducts()
		{
			// Arrange
			var products = new List<Product>
		{
			new Product { Title = "Red Trouser", Price = 15.0m, Sizes = new List<string> { "medium" } },
			new Product { Title = "Blue Trouser", Price = 20.0m, Sizes = new List<string> { "small" } }
		};

			// Act
			var result = _productBL.FilterProduct(10m, null, null, null, products);

			// Assert
			Assert.AreEqual(2, result.Count);
		}

		/// <summary>
		/// Tests the FilterProduct method when no products match the filter criteria.
		/// </summary>
		[TestMethod]
		public void FilterProduct_NoMatchingProducts_ReturnsEmptyList()
		{
			// Arrange
			var products = new List<Product>
		{
			new Product { Title = "Red Trouser", Price = 25.0m, Sizes = new List<string> { "medium" } }
		};

			// Act
			var result = _productBL.FilterProduct(10m, 20m, null, null, products);

			// Assert
			Assert.AreEqual(0, result.Count);
		}

		/// <summary>
		/// Tests the Highlight method to ensure words are highlighted in product descriptions.
		/// </summary>
		[TestMethod]
		public void Highlight_WithValidWords_ReturnsHighlightedDescriptions()
		{
			// Arrange
			var products = new List<Product>
		{
			new Product { Title = "Test Product", Price = 10.0m, Description = "This is a green product." }
		};

			// Act
			var result = _productBL.Highlight("green", products);

			// Assert
			Assert.AreEqual("This is a <em>green</em> product.", result[0].Description);
		}

		/// <summary>
		/// Tests the Highlight method when no highlight words are provided.
		/// </summary>
		[TestMethod]
		public void Highlight_NoWords_ReturnsOriginalDescriptions()
		{
			// Arrange
			var products = new List<Product>
		{
			new Product { Title = "Test Product", Price = 10.0m, Description = "This is a red product." }
		};

			// Act
			var result = _productBL.Highlight("", products);

			// Assert
			Assert.AreEqual("This is a red product.", result[0].Description);
		}

		/// <summary>
		/// Tests the GetProductDetails method to ensure it returns correct min/max prices and sizes.
		/// </summary>
		[TestMethod]
		public void GetProductDetails_WithProducts_ReturnsCorrectDetails()
		{
			// Arrange
			var products = new List<Product>
		{
			new Product { Price = 10.0m, Sizes = new List<string> { "small", "medium" },Description = "This is a red product." },
			new Product { Price = 20.0m, Sizes = new List<string> { "large" },Description = "This is a red product." }
		};

			// Act
			var result = _productBL.GetProductDetails(products);

			// Assert
			Assert.AreEqual(10.0m, result.MinPrice);
			Assert.AreEqual(20.0m, result.MaxPrice);
			Assert.AreEqual(3, result.Sizes.Count); // small, medium, large
		}

		/// <summary>
		/// Tests the GetProductDetails method when there are no products.
		/// </summary>
		[TestMethod]
		public void GetProductDetails_NoProducts_ReturnsDefaultDetails()
		{
			// Arrange
			var products = new List<Product>();

			// Act
			var result = _productBL.GetProductDetails(products);

			// Assert
			Assert.AreEqual(0, result.MinPrice);
			Assert.AreEqual(0, result.MaxPrice);
		}

		/// <summary>
		/// Tests the HighlightWords method for case insensitivity.
		/// </summary>
		[TestMethod]
		public void HighlightWords_CaseInsensitive_ReturnsHighlightedDescription()
		{
			// Arrange
			var description = "This is a green product.";
			var wordToHighlight = "Green";

			// Act
			var result = _productBL.HighlightWords(description, wordToHighlight);

			// Assert
			Assert.AreEqual("This is a <em>Green</em> product.", result);
		}
	}
}