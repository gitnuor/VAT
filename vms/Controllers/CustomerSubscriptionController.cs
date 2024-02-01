using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vms.Utility;
using vms.entity.viewModels;
using vms.entity.viewModels.CustomerViewModel;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.ThirdPartyService;
using AutoMapper;
using vms.entity.models;

namespace vms.Controllers;

public class CustomerSubscriptionController(ControllerBaseParamModel controllerBaseParamModel, ICustomerSubscriptionService service, ICustomerService customerService, IOrgBranchService branchService, IProductService productService, IMapper mapper)
	: ControllerBase(controllerBaseParamModel)
{
	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	public async Task<IActionResult> Index()
	{
		return View(await service.GetCustomerSubscription(UserSession.OrganizationId));
	}

	// [VmsAuthorize(FeatureList.THIRD_PARTY)]
	// [VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	// public async Task<IActionResult> Manage(string id)
	// {
	// 	// if (id == null)
	// 	// {
	// 	// 	return BadRequest();
	// 	// }
	// 	//
	// 	// var customer = await service.GetCustomerManageModel(id);
	// 	//
	// 	// if (customer == null)
	// 	// {
	// 	// 	return NotFound();
	// 	// }
	//
	// 	// return View(customer);
	// 	return View();
	// }

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	public async Task<IActionResult> Create()
	{
		var subscriptionServices = await productService.GetProductListByOrg(UserSession.ProtectedOrganizationId);
		var modelDetails = mapper.Map<IEnumerable<ViewProduct>, IEnumerable<CustomerSubscriptionDetailCreateViewModel>>(subscriptionServices);
		var model = new CustomerSubscriptionCreateViewModel
		{
			CustomerList = await customerService.GetCustomers(UserSession.OrganizationId),
			OrgBranchList = await branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId,
				UserSession.BranchIds, UserSession.IsRequireBranch),
			CustomerSubscriptionDetails = modelDetails
		};
		return View(model);
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	[HttpPost]
	public async Task<IActionResult> Create(CustomerSubscriptionCreateViewModel model)
	{
		if (!ModelState.IsValid)
		{
			var subscriptionServices = await productService.GetProductListByOrg(UserSession.ProtectedOrganizationId);
			var modelDetails = mapper.Map<IEnumerable<ViewProduct>, IEnumerable<CustomerSubscriptionDetailCreateViewModel>>(subscriptionServices);

			model.CustomerList = await customerService.GetCustomers(UserSession.OrganizationId);
			model.OrgBranchList = await branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId,
				UserSession.BranchIds, UserSession.IsRequireBranch);

			return View(model);
		}

		try
		{

			await service.SaveCustomerSubscription(model);
				return RedirectToAction(nameof(Index));
			
		}
		catch (Exception e)
		{
			ModelState.AddModelError("SaveError", e.Message);
			return View(model);
		}
	}
}