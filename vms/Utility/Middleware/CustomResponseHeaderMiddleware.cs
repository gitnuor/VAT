using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace vms.Utility.Middleware;

public class CustomResponseHeaderMiddleware
{
	private readonly RequestDelegate _next;

	public CustomResponseHeaderMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		//To add Headers AFTER everything you need to do this
		context.Response.OnStarting(state =>
		{
			var httpContext = (HttpContext)state;
			// httpContext.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
			httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
			httpContext.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
			httpContext.Response.Headers.Remove("X-Frame-Options");
			httpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN ");
			httpContext.Response.Headers.Add("Content-Security-Policy",
				"default-src 'self' cdn.jsdelivr.net google.com www.google.com gstatic.com www.gstatic.com; script-src 'self' 'unsafe-inline' cdn.jsdelivr.net google.com www.google.com gstatic.com www.gstatic.com; style-src 'self' 'unsafe-inline' cdn.jsdelivr.net google.com www.google.com gstatic.com www.gstatic.com; font-src 'self' cdn.jsdelivr.net google.com www.google.com gstatic.com www.gstatic.com; img-src 'self' data:; frame-src 'self' google.com www.google.com gstatic.com www.gstatic.com");
			httpContext.Response.Headers.Add("Referrer-Policy", "no-referrer");
			httpContext.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
			httpContext.Response.Headers.Add("Permissions-Policy",
				"accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
			httpContext.Response.Headers.Remove("X-Powered-By");
			return Task.CompletedTask;
		}, context);

		await _next(context);
	}
}