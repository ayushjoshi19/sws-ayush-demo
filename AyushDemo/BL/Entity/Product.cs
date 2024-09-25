using System.Text.Json.Serialization;


namespace ProductBusiness
{
	/// <summary>
	/// Represents an e-commerce product.
	/// </summary>
	public class Product
	{
		/// <summary>
		/// Gets or sets the title of the product.
		/// </summary>
		[JsonPropertyName("title")]
		public string? Title { get; set; }

		/// <summary>
		/// Gets or sets the price of the product.
		/// </summary>
		[JsonPropertyName("price")]
		public decimal Price { get; set; }

		/// <summary>
		/// Gets or sets the available sizes for the product.
		/// </summary>
		[JsonPropertyName("sizes")]
		public List<string>? Sizes { get; set; }

		/// <summary>
		/// Gets or sets the description of the product.
		/// </summary>
		[JsonPropertyName("description")]
		public string? Description { get; set; }
	}
}
