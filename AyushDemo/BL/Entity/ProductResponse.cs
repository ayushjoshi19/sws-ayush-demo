using System.Text.Json.Serialization;


namespace ProductBusiness
{
		public class ProductResponse
		{
			[JsonPropertyName("products")]
			public List<Product>? Products { get; set; }
		}
}
