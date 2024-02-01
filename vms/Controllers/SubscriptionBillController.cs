using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.Utility;
using vms.entity.viewModels;
using vms.entity.viewModels.SubscriptionAndBilling;
using vms.service.Services.BillingService;
using vms.service.Services.ReportService;
using vms.service.Services.SettingService;
using vms.service.Services.ThirdPartyService;

namespace vms.Controllers;

public class SubscriptionBillController(ControllerBaseParamModel controllerBaseParamModel, ISubscriptionBillService service, IOrgBranchService branchService, IReportOptionService reportOptionService, ICustomerSubscriptionService customerSubscriptionService)
	: ControllerBase(controllerBaseParamModel)
{
	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	public async Task<IActionResult> Index()
	{
		return View(await service.GetSubscriptionBill(UserSession.OrganizationId));
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	public async Task<IActionResult> InitializeBill()
	{
		var currentTime = DateTime.Now;
		var model = new SubscriptionBillInitiationViewModel
		{
			OrgBranchList = await branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId,
				UserSession.BranchIds, UserSession.IsRequireBranch),
			MonthList = reportOptionService.GetMonthSelectList(),
			BillYear = currentTime.Year,
			BillMonth = currentTime.Month
		};
		return View(model);
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	[HttpPost]
	public async Task<IActionResult> GenerateBill(SubscriptionBillCreateViewModel model)
	{
		if (model == null)
			throw new ArgumentNullException();
		var branches = (await branchService.GetOrgBranchByOrganization(UserSession.ProtectedOrganizationId)).ToList();
		model.SubscriptionBillDetails = await customerSubscriptionService.GetSubscriptionBillDetailCreateViewModel(
			UserSession.OrganizationId, model.BillingOfficeId, model.CollectionOfficeId, model.BillYear,
			model.BillMonth);
		model.BillingOfficeName = branches.First(b => b.OrgBranchId == model.BillingOfficeId).Name;
		model.CollectionOfficeName = branches.First(b => b.OrgBranchId == model.CollectionOfficeId).Name;
		model.BillMonthName = ((EnumMonth)model.BillMonth).ToString();
		return View(model);
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
    [HttpPost]
    public async Task<IActionResult> SaveBill(SubscriptionBillCreateViewModel model)
	{
		try
		{
			await service.GenerateSubscriptionBill(model, UserSession.OrganizationId, UserSession.UserId, DateTime.Now);
			return RedirectToAction(nameof(Index));
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	public async Task<IActionResult> InitializeSendingBillToVat()
	{
		var currentTime = DateTime.Now;
		var model = new SubscriptionBillInitiationViewModel
		{
			OrgBranchList = await branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId,
				UserSession.BranchIds, UserSession.IsRequireBranch),
			MonthList = reportOptionService.GetMonthSelectList(),
			BillYear = currentTime.Year,
			BillMonth = currentTime.Month
		};
		return View(model);
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	[HttpPost]
	public async Task<IActionResult> SendBillToVat(SubscriptionBillCreateViewModel model)
	{
		if (model == null)
			throw new ArgumentNullException();
		var branches = (await branchService.GetOrgBranchByOrganization(UserSession.ProtectedOrganizationId)).ToList();
		model.SubscriptionBillDetails = await customerSubscriptionService.GetSubscriptionBillDetailCreateViewModel(
			UserSession.OrganizationId, model.BillingOfficeId, model.CollectionOfficeId, model.BillYear,
			model.BillMonth);
		model.BillingOfficeName = branches.First(b => b.OrgBranchId == model.BillingOfficeId).Name;
		model.CollectionOfficeName = branches.First(b => b.OrgBranchId == model.CollectionOfficeId).Name;
		model.BillMonthName = ((EnumMonth)model.BillMonth).ToString();
		return View(model);
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
    [HttpPost]
    public async Task<IActionResult> SaveSendingBillToVat(SubscriptionBillCreateViewModel model)
	{
		try
		{
			await service.GenerateSubscriptionBill(model, UserSession.OrganizationId, UserSession.UserId, DateTime.Now);
			return RedirectToAction(nameof(Index));
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
}