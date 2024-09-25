using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ProductApi
{
	public class ValidateHeaderFilter : ActionFilterAttribute
	{
		private readonly string _headerKey;
		private readonly string _expectedValue;

		public ValidateHeaderFilter(string headerKey, string expectedValue)
		{
			_headerKey = headerKey;
			_expectedValue = expectedValue;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.HttpContext.Request.Headers.TryGetValue(_headerKey, out var headerValue) ||
				!headerValue.ToString().Contains(_expectedValue))
			{
				context.Result = new BadRequestObjectResult("Missing or invalid header value.");
			}

			base.OnActionExecuting(context);
		}
	
	}
}
