using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.Utility;
using vms.entity.viewModels;
using vms.utility.StaticData;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;
using AutoMapper;
using vms.entity.viewModels.VendorViewModel;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class VendorForeignController : ControllerBase

{
    private readonly IVendorService _service;
    private readonly ICountryService _countryService;
    private readonly ICustomsAndVatcommissionarateService _cusAndVatService;
    private readonly IBusinessCategoryService _businessCategoryService;
    private readonly IBusinessNatureService _businessNatureService;
    private readonly IDivisionOrStateService _divisionOrStateService;
    private readonly IDistrictOrCityService _districtOrCityService;
    private readonly IBankService _bankService;
    private readonly IMapper _mapper;


    public VendorForeignController(ControllerBaseParamModel controllerBaseParamModel, IVendorService service, ICountryService countryService, ICustomsAndVatcommissionarateService cusAndVatService, IBusinessCategoryService businessCategoryService, IBusinessNatureService businessNatureService, IDivisionOrStateService divisionOrStateService, IDistrictOrCityService districtOrCityService, IBankService bankService, IMapper mapper) : base(controllerBaseParamModel)
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
    }

    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        return View(await _service.GetVendorForeignListByOrg(UserSession.ProtectedOrganizationId));
    }

    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW_DETAILS)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    public ActionResult VendorDetail()
    {
        return View();
	}

    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_ADD)]
    public async Task<IActionResult> Create()
    {

		var model = new VendorForeignCreateViewModel();
		model = await GetDropdownValuesForVendorViewModel(model);
		return View(model);
	}

    [HttpPost]
    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_ADD)]
    public async Task<IActionResult> Create(VendorForeignCreateViewModel model)
    {
        var vendor = _mapper.Map<VendorForeignCreateViewModel, Vendor>(model);
        try
        {                
            vendor.CreatedTime = DateTime.Now;
            vendor.CreatedBy = UserSession.UserId;
            vendor.OrganizationId = UserSession.OrganizationId;
            vendor.IsRegistered = false;
            vendor.IsRegisteredAsTurnOverOrg = false;
            vendor.BinNo = null;
            vendor.CustomsAndVatcommissionarateId = null;
            vendor.VendorTypeId = (int)EnumVendorType.Foreign;
            _service.Insert(vendor);
            await UnitOfWork.SaveChangesAsync();


            var jObj = JObject.Parse(JsonConvert.SerializeObject(vendor));

            AuditLog au = new AuditLog();
            au.ObjectTypeId = (int) EnumObjectType.Vendor;
            au.PrimaryKey = vendor.VendorId;
            au.AuditOperationId = 1;
            au.UserId = UserSession.UserId;
            au.Datetime = DateTime.Now;
            au.Descriptions = jObj.ToString();
            au.IsActive = true;
            au.CreatedBy = UserSession.UserId;
            au.CreatedTime = DateTime.Now;
            au.OrganizationId = UserSession.OrganizationId;

            var vendorStatus = await AuditLogCreate(au);



            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        }
        catch (Exception ex)
        {
	        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
        }
        return RedirectToAction(nameof(Index));
	}

    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW_DETAILS)]
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }
        int vendorId = int.Parse(IvatDataProtector.Unprotect(id));
        var vendor = await _service.Query().Where(w => w.OrganizationId == UserSession.OrganizationId)
            .SingleOrDefaultAsync(p => p.VendorId == vendorId, CancellationToken.None);
        if (vendor == null)
        {
            return NotFound();
        }

        vendor.EncryptedId = IvatDataProtector.Protect(vendor.VendorId.ToString());
           
        return View(vendor);
    }
    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_EDIT)]
    public async Task<ActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vendor = await _service.GetVendor(id);
        if (vendor == null)
        {
            return NotFound();
        }

        vmVendor model = new vmVendor();
        model = _mapper.Map<Vendor, vmVendor>(vendor);
        model = await GetDropdownValuesForVendorViewModel(model);
        model.EncryptedId = id;
        var divisions = await _divisionOrStateService.Query().Where(x => x.OrganizationId == UserSession.OrganizationId && x.CountryId == vendor.CountryId).SelectAsync();
        if (model.CountryId != null)
            model.DivisionList =
                await _divisionOrStateService.GetDivisionsByCountryId(UserSession.OrganizationId,
                    model.CountryId.Value);

        if (model.DivisionOrStateId != null)
            model.DistrictList =
                await _districtOrCityService.GetDistrictsByDivisionId(UserSession.OrganizationId,
                    model.DivisionOrStateId.Value);

        if (model.BankBranchCountryId != null)
            model.BankBranchDistrictList =
                await _districtOrCityService.GetDistrictsByCountryId(UserSession.OrganizationId,
                    model.BankBranchCountryId.Value);

        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_EDIT)]
    public async Task<IActionResult> Edit(vmVendor model)
    {
        if (ModelState.IsValid)
        {               
            int vendorId = int.Parse(IvatDataProtector.Unprotect(model.EncryptedId));
            try
            {                    
                var vendorData = await _service.Query().SingleOrDefaultAsync(p => p.VendorId == vendorId, CancellationToken.None);
                // vendorData = SetVendorLocationField(vendorData);
                var prevObj = JsonConvert.SerializeObject(vendorData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                vendorData = _mapper.Map(model, vendorData);
                    
                vendorData.ModifiedBy = UserSession.UserId;
                vendorData.ModifiedTime = DateTime.Now;
                vendorData.OrganizationId = UserSession.OrganizationId;

                _service.Update(vendorData);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(vendorData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.Vendor,
                    PrimaryKey = vendorData.VendorId,
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

			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
           
        return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.VENDOR);
    }
       
    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
        
    public async Task<ActionResult> Delete(int? id)
    {
        var vendor = await _service.Query().SingleOrDefaultAsync(p => p.VendorId == id, CancellationToken.None);
            

        return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.VENDOR);
    }

       
    public async Task<JsonResult> VendorAutoComplete(string filterText)
    {
        var vendor = await _service.Query().Where(c => c.Name.Contains(filterText)&&c.OrganizationId==UserSession.OrganizationId).SelectAsync(CancellationToken.None);
        return new JsonResult(vendor.Select(x => new
        {
            Id = x.VendorId,
            Name = x.Name
        }).ToList());
    }

    private async Task<vmVendor> GetDropdownValuesForVendorViewModel(vmVendor model)
    {
        model.Countries = await _countryService.CountrySelectList();
        model.CustomsAndVatCommissionarates = await _cusAndVatService.GetCustomsAndVatcommissionarateSelectList();
        model.BusinessCategoryList = await _businessCategoryService.BusinessCategorySelectList();
        model.BusinessNatureList = await _businessNatureService.BusinessNatureSelectList();
        model.BankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
        return model;
    }

    private async Task<VendorLocalCreateViewModel> GetDropdownValuesForVendorViewModel(VendorLocalCreateViewModel model)
    {
        model.CustomsAndVatCommissionarates = await _cusAndVatService.GetCustomsAndVatcommissionarateSelectList();
        model.BusinessCategoryList = await _businessCategoryService.BusinessCategorySelectList();
        model.BusinessNatureList = await _businessNatureService.BusinessNatureSelectList();
        model.BankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
        return model;
    }

    private async Task<VendorForeignCreateViewModel> GetDropdownValuesForVendorViewModel(VendorForeignCreateViewModel model)
    {
        model.Countries = await _countryService.CountrySelectList();
        model.BankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
        return model;
    }

}