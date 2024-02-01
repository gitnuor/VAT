using vms.service.Services.SettingService;

namespace vms.api.Authentication;

public class ApiKeyAuthMiddleware
{
	private readonly RequestDelegate _next;

	public ApiKeyAuthMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IIntegratedApplicationService integratedApplicationService)
	{
		if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
		{
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync("API Key is missing!");
			return;
		}

		if (!(await integratedApplicationService.IsIntegratedApplicationExists(extractedApiKey)))
		{
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync("Invalid API Key!");
			return;
		}
		await _next(context);
	}
}