using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using vms.api.Authentication;
using vms.api.VmsApiUtility;
using vms.ioc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddControllers(x => x.Filters.Add<ApiKeyAuthFilter>());
builder.Services.RegisterVmsServiceInstance(builder.Configuration);
builder.Services.AddScoped<ControllerBaseParamModel, ControllerBaseParamModel>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
	x.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
	{
		Description = "The API Key to access the API",
		Type = SecuritySchemeType.ApiKey,
		Name = "X-API-Key",
		In = ParameterLocation.Header,
		Scheme = "ApiKeyScheme"
	});

	var scheme = new OpenApiSecurityScheme
	{
		Reference = new OpenApiReference
		{
			Type = ReferenceType.SecurityScheme,
			Id = "ApiKey"
		},
		In = ParameterLocation.Header
	};
	var requirement = new OpenApiSecurityRequirement
	{
		{scheme, new List<string>()}
	};
	x.AddSecurityRequirement(requirement);
});

builder.Services.AddDataProtection()
	.PersistKeysToFileSystem(new DirectoryInfo(@"c:\iVAT_API\temp-keys\"));

var app = builder.Build();
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
	app.UseSwagger();
	app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();