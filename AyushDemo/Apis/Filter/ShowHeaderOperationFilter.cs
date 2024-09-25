using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductApi
{
	public class ShowHeaderOperationFilter: IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			operation.Parameters ??= new List<OpenApiParameter>();

			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "X-SWS-Header",
				In = ParameterLocation.Header,
				Required = true, // Set to true if the header is required
				Schema = new OpenApiSchema
				{
					Type = "string"
				},
				Description = "This header is required for authentication to validate the request."
			});
		}
	}
}
