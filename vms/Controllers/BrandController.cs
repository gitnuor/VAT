using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.ProductService;
using vms.utility.StaticData;
using vms.Utility;


namespace vms.Controllers;

public class BrandController : ControllerBase
{
	private readonly IBrandService _brand;

	public BrandController(IBrandService brand,
		ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
	{
		_brand = brand;
	}

	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.AdministrationBrandCanView)]
	public async Task<IActionResult> Index()
	{
		return View(await _brand.GetBrandList(UserSession.ProtectedOrganizationId));
	}


	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.AdministrationBrandCanView)]
	[VmsAuthorize(FeatureList.AdministrationBrandCanAdd)]
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.AdministrationBrandCanView)]
	[VmsAuthorize(FeatureList.AdministrationBrandCanAdd)]
	public async Task<IActionResult> Create(Brand brand)
	{
		brand.CreatedTime = DateTime.Now;
		brand.CreatedBy = UserSession.UserId;
		brand.OrganizationId = UserSession.OrganizationId;

		_brand.Insert(brand);
		await UnitOfWork.SaveChangesAsync();
		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		var jObj = JsonConvert.SerializeObject(brand, Formatting.None,
			new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});


		AuditLog au = new AuditLog();
		au.ObjectTypeId = (int)EnumObjectType.Brand;
		au.PrimaryKey = brand.BrandId;
		au.AuditOperationId = (int)EnumOperations.Add;
		au.UserId = UserSession.UserId;
		au.Datetime = DateTime.Now;
		au.Descriptions = jObj;
		au.IsActive = true;
		au.CreatedBy = UserSession.UserId;
		au.CreatedTime = DateTime.Now;
		au.OrganizationId = UserSession.OrganizationId;
		await AuditLogCreate(au);

		return RedirectToAction(nameof(Index));
	}


	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.AdministrationBrandCanView)]
	[VmsAuthorize(FeatureList.AdministrationBrandCanEdit)]
	public async Task<IActionResult> Edit(string id)
	{
		if (id == null)
		{
			return NotFound();
		}

		int BrandId = int.Parse(IvatDataProtector.Unprotect(id));
		var cost = await _brand.Query()
			.SingleOrDefaultAsync(p => p.BrandId == BrandId, CancellationToken.None);
		if (cost == null)
		{
			return NotFound();
		}

		return View(cost);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.AdministrationBrandCanView)]
	[VmsAuthorize(FeatureList.AdministrationBrandCanEdit)]
	public async Task<IActionResult> Edit(Brand brand)
	{
		if (ModelState.IsValid)
		{
			try
			{
				brand.ModifiedTime = DateTime.Now;
				brand.ModifiedBy = UserSession.UserId;
				brand.OrganizationId = UserSession.OrganizationId;
				_brand.Update(brand);
				await UnitOfWork.SaveChangesAsync();
			}
			catch
			{
				return RedirectToAction(nameof(Index));
			}
		}
		else
		{
			return View(brand);
		}

		return RedirectToAction(nameof(Index));
	}
}