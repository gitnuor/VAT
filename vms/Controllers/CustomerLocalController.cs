using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.Utility;
using vms.entity.viewModels;
using vms.utility.StaticData;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;
using AutoMapper;
using vms.entity.viewModels.CustomerViewModel;
using vms.utility;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;
using vms.service.Services.UploadService;

namespace vms.Controllers;

public class CustomerLocalController : ControllerBase
{
	private readonly ICustomerService _service;
	private readonly ICustomerBranchService _customerBranchService;
	private readonly ICountryService _countryService;
	private readonly ICustomsAndVatcommissionarateService _cusAndVatService;
	private readonly IOrgBranchService _branchService;
	private readonly IBusinessCategoryService _businessCategoryService;
	private readonly IBusinessNatureService _businessNatureService;
	private readonly IDivisionOrStateService _divisionOrStateService;
	private readonly IDistrictOrCityService _districtOrCityService;
	private readonly IBankService _bankService;
	private readonly IDocumentTypeService _documentType;
	private readonly IMapper _mapper;

	public CustomerLocalController(ControllerBaseParamModel controllerBaseParamModel, ICustomerService service,
		ICountryService countryService, ICustomsAndVatcommissionarateService cusAndVatService,
		IBusinessCategoryService businessCategoryService, IBusinessNatureService businessNatureService,
		IDivisionOrStateService divisionOrStateService, IDistrictOrCityService districtOrCityService,
		IBankService bankService, IMapper mapper,
		IOrgBranchService branchService, ICustomerBranchService customerBranchService,
		IDocumentTypeService documentType) : base(controllerBaseParamModel)
	{
		_service = service;
		_countryService = countryService;
		_cusAndVatService = cusAndVatService;
		_businessCategoryService = businessCategoryService;
		_businessNatureService = businessNatureService;
		_divisionOrStateService = divisionOrStateService;
		_districtOrCityService = districtOrCityService;
		_bankService = bankService;
		_mapper = mapper;
		_branchService = branchService;
		_customerBranchService = customerBranchService;
		_documentType = documentType;
	}


	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	public async Task<IActionResult> Index()
	{
		return View(await _service.GetCustomerLocalListByOrg(UserSession.ProtectedOrganizationId));
	}


	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	public  IActionResult Details(string id)
	{
		return View();
	}

	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_ADD)]
	public async Task<IActionResult> Create()
	{
		var model = await GetListValuesForCustomerLocalViewModel();
		return View(model);
	}


	[HttpPost]
	[VmsAuthorize(FeatureList.THIRD_PARTY)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_VIEW)]
	[VmsAuthorize(FeatureList.THIRD_PARTY_CUSTOMER_CAN_ADD)]
	public async Task<IActionResult> Create(CustomerLocalCreatePostViewModel model)
	{
		var saveStatus = new SaveStatusViewModel();

		if (!ModelState.IsValid)
		{
			saveStatus.Message = "Please provide valid data";
			saveStatus.IsSaved = false;
			return new JsonResult(saveStatus);
		}

		try
		{
			var operatingUser = GetOperatingUserInformation();
			var customer = await _service.InsertLocalCustomer(model, operatingUser);
			await UnitOfWork.SaveChangesAsync();
			await _service.InsertCustomerDocuments(model.Documents, customer.CustomerId, operatingUser);
			if (customer.IsRequireBranch == true)
			{
				await _service.InsertCustomerBranches(model.CustomerBranchList, customer.CustomerId, operatingUser);
			}

			await UnitOfWork.SaveChangesAsync();

			saveStatus = new SaveStatusViewModel
			{
				Id = VmsDataProtectionManager.EncryptInt(customer.CustomerId),
				Message = ControllerStaticData.SavedSuccessMessage,
				IsSaved = true
			};
		}
		catch (Exception ex)
		{
			saveStatus.Message = ex.Message;
			saveStatus.IsSaved = false;
		}

		return new JsonResult(saveStatus);
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
		model = GetDropdownValuesForCustomerViewModel(model);
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

	private vmCustomer GetDropdownValuesForCustomerViewModel(vmCustomer model)
	{
		throw new NotImplementedException();
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


	private async Task<CustomerLocalCreateViewModel> GetListValuesForCustomerLocalViewModel()
	{
		var model = new CustomerLocalCreateViewModel
		{
			CustomsAndVatCommissionarates = await _cusAndVatService.GetCustomsAndVatcommissionarateSelectList(),
			BusinessCategoryList = await _businessCategoryService.BusinessCategorySelectList(),
			BusinessNatureList = await _businessNatureService.BusinessNatureSelectList(),
			BankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId),
			DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(UserSession.ProtectedOrganizationId)
		};
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