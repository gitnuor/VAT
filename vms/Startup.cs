using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Recaptcha.Web.Configuration;
using System;
using System.IO;
using vms.ActionFilter;
using vms.entity.viewModels;
using vms.ioc;
using vms.Utility;

namespace vms;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		// services.Configure<CookiePolicyOptions>(options =>
		// {
		// 	// This lambda determines whether user consent for non-essential cookies is needed for a given request.
		// 	options.CheckConsentNeeded = _ => true;
		// 	options.MinimumSameSitePolicy = SameSiteMode.Strict;
		// });
		services.AddScoped<Logged>();
		services.RegisterVmsServiceInstance(Configuration);
		services.AddMvc(options => { options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); });
		services.AddScoped<ControllerBaseParamModel, ControllerBaseParamModel>();
		services.AddHttpContextAccessor();
		services.AddMvc()
			.AddJsonOptions(_ => { });
		RecaptchaConfigurationManager.SetConfiguration(Configuration);
		// services.AddHsts(options =>
		// {
		// 	options.IncludeSubDomains = true;
		// 	options.Preload = true;
		// 	options.MaxAge = TimeSpan.FromHours(1500);
		// 	options.ExcludedHosts.Add("bracits.com");
		// });

		//todo: for https redirection
		// services.AddHttpsRedirection(options =>
		// {
		// 	options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
		// 	options.HttpsPort = 443;
		// });

		services.AddDataProtection()
			.PersistKeysToFileSystem(
				new DirectoryInfo(Configuration.GetSection("ProtectionDirectory").ToString() ?? @"c:\iVAT\temp-keys\"));
		services.AddHttpContextAccessor();
		services.Configure<PrivateDataModel>(Configuration.GetSection("PrivateData"));
		services.AddSession(options =>
		{
			options.IdleTimeout = TimeSpan.FromMinutes(30); //You can set Time
		});
		// services.AddHostedService<TimedHostedService>();
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Home/Error");
		}

		// app.UseHsts();
		// app.UseMiddleware(typeof(CustomResponseHeaderMiddleware));
		app.UseSession();
		// app.UseCookiePolicy(
		// 	new CookiePolicyOptions
		// 	{
		// 		Secure = CookieSecurePolicy.Always,
		// 		MinimumSameSitePolicy = SameSiteMode.Strict
		// 	});
		// app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthorization();

		app.UseEndpoints(routes =>
		{
			routes.MapControllerRoute(
				"default",
				"{controller=Authentication}/{action=Index}/{id?}");
		});
	}
}