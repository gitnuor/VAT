using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.Models;
using vms.Utility;
using vms.utility.StaticData;
using vms.service.Services.SecurityService;
using vms.service.Services.SettingService;
using vms.service.Services.ReportService;

namespace vms.Controllers;

public class HomeController : ControllerBase
{
	private readonly IUserService _service;
	private readonly IRoleRightService _roleRightService;
	private readonly IRightService _rightService;
	private readonly IConfiguration _configuration;
	private readonly IOrganizationService _orgcConfiguration;
	private readonly IDashboardService _dashboardervice;

	public HomeController(ControllerBaseParamModel controllerBaseParamModel, IUserService service,
		IRoleRightService roleRightService, IRightService rightService, IOrganizationService orgcConfiguration,
		IDashboardService dashboardervice) : base(controllerBaseParamModel)
	{
		_service = service;
		_roleRightService = roleRightService;
		_configuration = Configuration;
		_rightService = rightService;
		_orgcConfiguration = orgcConfiguration;
		_dashboardervice = dashboardervice;
	}

	[VmsAuthorize(FeatureList.NBR_USER)]
	public IActionResult Index()
	{
		HttpContext.Session.Clear();
		return View();
	}

	public IActionResult Reference()
	{
		return View();
	}

	public async Task<IActionResult> Dashboard()
	{
		var currentDate = DateTime.Now;

		var model = await _dashboardervice.GetDashboardInfo(UserSession.OrganizationId, currentDate.Year,
			currentDate.Month);

		if (UserSession.OrganizationId == 0)
		{
			return RedirectToAction(ControllerStaticData.NBR_USER, ControllerStaticData.HOME);
		}

		return View(model);
	}

	[VmsAuthorize(FeatureList.NBR_USER)]
	public async Task<IActionResult> NbrUser()
	{
		var org = new Organization();


		org = await _orgcConfiguration.Query()
			.SingleOrDefaultAsync(c => c.OrganizationId == UserSession.OrganizationId, CancellationToken.None);


		return View(org);
	}

	[VmsAuthorize(FeatureList.NBR_USER)]
	public async Task<IActionResult> LogOrg(int id)
	{
		var org = new Organization();


		org = await _orgcConfiguration.Query()
			.SingleOrDefaultAsync(c => c.OrganizationId == id, CancellationToken.None);


		if (id >= 0)
		{
			var session = new VmUserSession
			{
				UserId = UserSession.UserId,
				UserName = UserSession.UserName,
				RoleId = UserSession.RoleId,
				RoleName = UserSession.RoleName,
				OrganizationId = id,
				OrganizationName = org.Name,
				Rights = UserSession.Rights
			};
			HttpContext.Session.SetComplexData(ControllerStaticData.SESSION, session);
		}

		return RedirectToAction(ControllerStaticData.DISPLAY_DASHBOARD, ControllerStaticData.HOME);
	}

	[VmsAuthorize(FeatureList.NBR_USER)]
	public async Task<IActionResult> LogOutOrg(int id)
	{
		if (UserSession.OrganizationId == 0)
		{
			return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.AUTHENTICATION);
		}

		else
		{
			var org = new Organization();


			org = await _orgcConfiguration.Query()
				.SingleOrDefaultAsync(c => c.OrganizationId == 0, CancellationToken.None);


			var session = new VmUserSession
			{
				UserId = UserSession.UserId,
				UserName = UserSession.UserName,
				RoleId = UserSession.RoleId,
				RoleName = UserSession.RoleName,
				OrganizationId = 0,
				OrganizationName = org.Name,
				Rights = UserSession.Rights
			};
			HttpContext.Session.SetComplexData(ControllerStaticData.SESSION, session);
		}

		return RedirectToAction(ControllerStaticData.DISPLAY_DASHBOARD, ControllerStaticData.HOME);
	}

	public IActionResult About()
	{
		ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.DASHBOARD_APPLICATION_DISPLAY_MESSAGE;

		return View();
	}

	public IActionResult Contact()
	{
		ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.DASHBOARD_CONTAC_DISPLAY_MESSAGE;

		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}