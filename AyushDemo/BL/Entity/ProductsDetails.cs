namespace ProductBusiness
{
	public class ProductsDetails
	{
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
		public List<string>? Sizes { get; set; }
		public List<string>? CommonWords { get; set; }
	}
}