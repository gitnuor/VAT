using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace vms.api.Authentication;

public class ApiKeyAuthFilter : IAsyncAuthorizationFilter
{
	private readonly IConfiguration _configuration;

	public ApiKeyAuthFilter(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
		{
			context.Result = new UnauthorizedObjectResult("API Key is missing!");
			return Task.CompletedTask;
		}

		var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName) ?? "a5bf0b89-b092-4641-9083-f415088a1541";

		if (apiKey.Equals(extractedApiKey)) return Task.CompletedTask;

		context.Result = new UnauthorizedObjectResult("Invalid API Key!");
		return Task.CompletedTask;

	}
}