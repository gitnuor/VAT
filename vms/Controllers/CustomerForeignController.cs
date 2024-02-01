using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.Utility;
using vms.entity.viewModels;
using vms.utility.StaticData;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using vms.entity.viewModels.CustomerViewModel;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class CustomerForeignController : ControllerBase
{
	private readonly ICustomerService _service;
	private readonly ICustomerBranchService _customerBranchService;
	private readonly ICustomerDeliveryAddressService _customerDeliveryAddressService;
	private readonly ICountryService _countryService;
	private readonly IHostEnvironment _hostingEnvironment;
	private readonly ICustomsAndVatcommissionarateService _cusAndVatService;
	private readonly IOrganizationService _organizationService;
	private readonly IOrgBranchService _branchService;
	private readonly IBusinessCategoryService _businessCategoryService;
	private readonly IBusinessNatureService _businessNatureService;
	private readonly IDivisionOrStateService _divisionOrStateService;
	private readonly IDistrictOrCityService _districtOrCityService;
	private readonly IBankService _bankService;
	private readonly IMapper _mapper;

	public CustomerForeignController(IOrganizationService organizationService,
		ControllerBaseParamModel controllerBaseParamModel, ICustomerService service, ICountryService countryService,
		IHostEnvironment hostingEnvironment, ICustomsAndVatcommissionarateService cusAndVatService,
		IBusinessCategoryService businessCategoryService, IBusinessNatureService businessNatureService,
		IDivisionOrStateService divisionOrStateService, IDistrictOrCityService districtOrCityService,
		IBankService bankService, IMapper mapper, ICustomerDeliveryAddressService customerDeliveryAddressService,
		IOrgBranchService branchService, ICustomerBranchService customerBranchService) : base(controllerBaseParamModel)
	{
		_organizationService = organizationService;
		_service = service;
		_countryService = countryService;
		_hostingEnvironment = hostingEnvironment;
		_cusAndVatService = cusAndVatService;
		_businessCategoryService = businessCategoryService;
		_businessNatureService = businessNatureService;
		_divisionOrStateService = divisionOrStateService;
		_districtOrCityService = districtOrCityService;
		_bankService = bankService;
		_mapper = mapper;
		_customerDeliveryAddressService = customerDeliveryAddressService;
		_branchService = branchService;
		_customerBranchService = customerBranchService;
	}


	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	public async Task<IActionResult> Index()
	{
		return View(await _service.GetCustomerForeignListByOrg(UserSession.ProtectedOrganizationId));
	}


	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	public async Task<IActionResult> Manage(string id)
	{
		if (id == null)
		{
			return BadRequest();
		}

		var customer = await _service.GetCustomerManageModel(id);

		if (customer == null)
		{
			return NotFound();
		}

		return View(customer);
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_ADD)]
	public async Task<IActionResult> Create()
	{
		var model = new CustomerForeignCreateViewModel();
		model = await GetDropdownValuesForForeignCustomerViewModel(model);

		return View(model);
	}

	private Customer SetCustomerLocationField(Customer customer)
	{
		if (customer.IsForeignCustomer)
		{
			customer.IsFullExportOriented = false;
			customer.Bin = null;
			customer.CustomsAndVatcommissionarateId = null;
		}
		else
		{
			customer.IsForeignCustomer = false;
			customer.CountryId = 14;
		}

		return customer;
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_ADD)]
	public async Task<IActionResult> Create(CustomerForeignCreateViewModel model)
	{
		var customer = _mapper.Map<CustomerForeignCreateViewModel, Customer>(model);
		try
		{
			customer.CreatedTime = DateTime.Now;
			customer.CreatedBy = UserSession.UserId;
			customer.IsActive = true;
			customer.OrganizationId = UserSession.OrganizationId;
			customer.IsForeignCustomer = true;
			customer.IsFullExportOriented = false;
			customer.Bin = null;
			customer.CustomsAndVatcommissionarateId = null;
			_service.Insert(customer);
			await UnitOfWork.SaveChangesAsync();


			var cusObj = JsonConvert.SerializeObject(customer, Formatting.None,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});
			;


			AuditLog cu = new AuditLog();

			cu.ObjectTypeId = (int)EnumObjectType.Customer;
			cu.PrimaryKey = customer.CustomerId;
			cu.AuditOperationId = (int)EnumOperations.Add;
			cu.UserId = UserSession.UserId;
			cu.Datetime = DateTime.Now;
			cu.Descriptions = cusObj.ToString();
			cu.IsActive = true;
			cu.CreatedBy = UserSession.UserId;
			cu.CreatedTime = DateTime.Now;
			cu.OrganizationId = UserSession.OrganizationId;


			var cusadd_status = await AuditLogCreate(cu);


			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
			return RedirectToAction(nameof(Index));
		}
		catch (Exception ex)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			return RedirectToAction(nameof(Index));
		}
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_EDIT)]
	public async Task<IActionResult> Edit(string id)
	{
		if (id == null)
		{
			return NotFound();
		}

		int cusId = int.Parse(IvatDataProtector.Unprotect(id));
		var cus = await _service.Query().SingleOrDefaultAsync(p => p.CustomerId == cusId, CancellationToken.None);
		if (cus == null)
		{
			return NotFound();
		}

		cus.Countries = await _countryService.CountrySelectList();

		cus.CustomsAndVatCommissionarates = await _cusAndVatService.GetCustomsAndVatcommissionarateSelectList();

		cus.EncryptedId = IvatDataProtector.Protect(cus.CustomerId.ToString());

		vmCustomer model = new vmCustomer();
		model = _mapper.Map<Customer, vmCustomer>(cus);
		model = await GetDropdownValuesForCustomerViewModel(model);
		if (model.CountryId != null)
			model.DivisionList =
				await _divisionOrStateService.GetDivisionsByCountryId(UserSession.OrganizationId,
					model.CountryId.Value);

		if (model.DivisionOrStateId != null)
			model.DistrictList =
				await _districtOrCityService.GetDistrictsByDivisionId(UserSession.OrganizationId,
					model.DivisionOrStateId.Value);

		if (model.DeliveryCountryId != null)
			model.DeliveryDivisionList =
				await _divisionOrStateService.GetDivisionsByCountryId(UserSession.OrganizationId,
					model.DeliveryCountryId.Value);

		if (model.DeliveryDivisionOrStateId != null)
			model.DeliveryDistrictList =
				await _districtOrCityService.GetDistrictsByDivisionId(UserSession.OrganizationId,
					model.DeliveryDivisionOrStateId.Value);

		if (model.BankBranchCountryId != null)
			model.BankBranchDistrictList =
				await _districtOrCityService.GetDistrictsByCountryId(UserSession.OrganizationId,
					model.BankBranchCountryId.Value);
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_EDIT)]
	public async Task<IActionResult> Edit(vmCustomer customer)
	{
		int cusId = int.Parse(IvatDataProtector.Unprotect(customer.EncryptedId));
		//if (cusId != customer.CustomerId)
		//{
		//    return NotFound();
		//}
		if (ModelState.IsValid)
		{
			try
			{
				var customerData = await _service.Query()
					.SingleOrDefaultAsync(p => p.CustomerId == cusId, CancellationToken.None);
				customerData = SetCustomerLocationField(customerData);
				var prevObj = JsonConvert.SerializeObject(customerData, Formatting.None,
					new JsonSerializerSettings()
					{
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					});

				customerData = _mapper.Map(customer, customerData);

				customerData.ModifiedBy = UserSession.UserId;
				customerData.ModifiedTime = DateTime.Now;
				customerData.OrganizationId = UserSession.OrganizationId;

				_service.Update(customerData);

				await UnitOfWork.SaveChangesAsync();
				var jObj = JsonConvert.SerializeObject(customerData, Formatting.None,
					new JsonSerializerSettings()
					{
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					});


				AuditLog au = new AuditLog
				{
					ObjectTypeId = (int)EnumObjectType.Customer,
					PrimaryKey = customerData.CustomerId,
					AuditOperationId = (int)EnumOperations.Edit,
					UserId = UserSession.UserId,
					Datetime = DateTime.Now,
					Descriptions = GetChangeInformation(prevObj.ToString(), jObj.ToString()),
					IsActive = true,
					CreatedBy = UserSession.UserId,
					CreatedTime = DateTime.Now,
					OrganizationId = UserSession.OrganizationId
				};
				await AuditLogCreate(au);
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
			}
			catch (DbUpdateConcurrencyException)
			{
			}
		}

		return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.CUSTOMER);
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_DELETE)]
	public async Task<ActionResult> ChangeCustomerStatus(string id)
	{
		var customer = await _service.Query()
			.SingleOrDefaultAsync(p => p.CustomerId == int.Parse(IvatDataProtector.Unprotect(id)),
				CancellationToken.None);

		if (customer.IsActive == true)
		{
			customer.IsActive = false;
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;
		}
		else
		{
			customer.IsActive = true;
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;
		}

		_service.Update(customer);

		await UnitOfWork.SaveChangesAsync();


		AuditLog cu_del = new AuditLog();

		cu_del.ObjectTypeId = (int)EnumObjectType.Customer;
		cu_del.PrimaryKey = customer.CustomerId;
		cu_del.AuditOperationId = (int)EnumOperations.Delete;
		cu_del.UserId = UserSession.UserId;
		cu_del.Datetime = DateTime.Now;
		cu_del.Descriptions = "IsActive:0";
		cu_del.IsActive = true;
		cu_del.CreatedBy = UserSession.UserId;
		cu_del.CreatedTime = DateTime.Now;
		cu_del.OrganizationId = UserSession.OrganizationId;


		var cusdel_status = await AuditLogCreate(cu_del);

		return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.CUSTOMER);
	}


	[HttpGet]
	public async Task<JsonResult> GetCustomerDetails(int id)
	{
		var customer = await _service.Query().SingleOrDefaultAsync(c => c.CustomerId == id, CancellationToken.None);
		return Json(customer);
	}


	[HttpGet]
	public async Task<JsonResult> GetOrganizationDetails(int id)
	{
		var org = await _organizationService.Query()
			.SingleOrDefaultAsync(c => c.OrganizationId == id, CancellationToken.None);
		return Json(org);
	}


	public async Task<JsonResult> CustomerAutoComplete(string filterText)
	{
		var Customer = await _service.Query()
			.Where(c => c.Name.Contains(filterText) && c.OrganizationId == UserSession.OrganizationId)
			.SelectAsync(CancellationToken.None);
		return new JsonResult(Customer.Select(x => new
		{
			Id = x.CustomerId,
			Name = x.Name
		}).ToList());
	}

	public async Task<JsonResult> GetFullExportOriented()
	{
		var customers = new List<Customer>
		{
			new Customer
			{
				CustomerId = 0,
				Name = ""
			}
		};

		var objTypes = (List<Customer>)await _service.Query()
			.Where(c => c.IsFullExportOriented == true && c.OrganizationId == UserSession.OrganizationId)
			.SelectAsync(CancellationToken.None);


		if (objTypes.Any())
		{
			customers.AddRange(objTypes);
		}

		return new JsonResult(customers.Select(x => new
		{
			Id = x.CustomerId,
			Name = x.Name
		}).ToList());
	}

	public async Task<JsonResult> GetForeignCustomer()
	{
		var customers = new List<Customer>
		{
			new Customer
			{
				CustomerId = 0,
				Name = ""
			}
		};

		var objTypes = (List<Customer>)await _service.Query()
			.Where(c => c.IsForeignCustomer == true && c.OrganizationId == UserSession.OrganizationId)
			.SelectAsync(CancellationToken.None);


		if (objTypes.Any())
		{
			customers.AddRange(objTypes);
		}

		return new JsonResult(customers.Select(x => new
		{
			Id = x.CustomerId,
			Name = x.Name
		}).ToList());
	}

	private async Task<CustomerForeignCreateViewModel> GetDropdownValuesForForeignCustomerViewModel(
		CustomerForeignCreateViewModel model)
	{
		model.Countries = await _countryService.CountrySelectList();
		model.BankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
		return model;
	}

	private async Task<vmCustomer> GetDropdownValuesForCustomerViewModel(vmCustomer model)
	{
		model.Countries = await _countryService.CountrySelectList();
		model.CustomsAndVatCommissionarates = await _cusAndVatService.GetCustomsAndVatcommissionarateSelectList();
		model.BusinessCategoryList = await _businessCategoryService.GetAllCategorySelectListItem();

		model.BusinessNatureList = await _businessNatureService.BusinessNatureSelectList();
		//model.DivisionList = await _divisionOrStateService.GetDivisionSelectListItem(_session.OrganizationId);
		//model.DistrictList = await _districtOrCityService.GetDistrictSelectListItem(_session.OrganizationId);
		model.BankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);

		return model;
	}

	private async Task<CustomerLocalCreateViewModel> GetListValuesForCustomerLocalViewModel(
		CustomerLocalCreateViewModel model)
	{
		// model.Countries = await _countryService.GetCountrySelectList();
		model.CustomsAndVatCommissionarates = await _cusAndVatService.GetCustomsAndVatcommissionarateSelectList();
		model.BusinessCategoryList = await _businessCategoryService.GetAllCategorySelectListItem();
		model.BusinessNatureList = await _businessNatureService.BusinessNatureSelectList();
		model.BankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
		var branches = await _branchService.GetOrgBranchByOrganization(UserSession.ProtectedOrganizationId);
		foreach (var branch in branches)
		{
			model.CustomerBranchList.Add(new CustomerBranchCreateViewModel
			{
				OrgBranchId = branch.OrgBranchId,
				BranchName = branch.Name
			});
		}

		return model;
	}
}