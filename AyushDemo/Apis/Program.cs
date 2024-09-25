using Microsoft.OpenApi.Models;
using ProductApi;
using ProductBusiness;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers(options =>
{
	// Add the global filter
	options.Filters.Add(new ValidateHeaderFilter("X-SWS-Header", "123"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "SWS Demo API", Version = "v1" });

	// Add custom header
	c.OperationFilter<ShowHeaderOperationFilter>();
}); ;
builder.Services.AddHttpClient();
builder.Services.AddScoped<ProductBL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
