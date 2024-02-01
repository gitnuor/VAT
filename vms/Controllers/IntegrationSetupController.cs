using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.service.Services.PaymentService;
using vms.service.Services.SettingService;
using vms.service.Services.UploadService;
using vms.Utility;
// using X.PagedList;

namespace vms.Controllers;

public class IntegrationSetupController : ControllerBase
{
    private readonly IAdjustmentTypeService _adjustmentTypeService;
    private readonly IBankService _bankService;
    private readonly IBankBranchService _bankBranchService;
    private readonly IContractTypeService _contractTypeService;
    private readonly ICustomsAndVatcommissionarateService _cusAndVatComService;
    private readonly IDeliveryMethodService _deliveryMethodService;
    private readonly IDistrictService _districtService;
    private readonly IExportTypeService _exportTypeService;
    private readonly IMushakReturnPaymentTypeService _mushakReturnPaymentTypeService;
    private readonly INbrEconomicCodeService _nbrEconomicCodeService;
    private readonly INbrEconomicCodeTypeService _nbrEconomicCodeTypeService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IProductVatTypeService _productVatTypeService;
    private readonly IPurchaseTypeService _purchaseTypeService;
    private readonly ISalesDeliveryTypeService _salesDeliveryTypeService;
    private readonly ISalesTypeService _salesTypeService;
    private readonly IOrgStaticDataService _orgStaticDataService;
    private readonly IOrgStaticDataTypeService _orgStaticDataTypeService;
    private readonly ICountryService _countryService;
    private readonly IPurchaseReasonService _purchaseReasonService;

    //private int organizationId;

    public IntegrationSetupController(IAdjustmentTypeService adjustmentTypeService,
        IBankService bankService,
        IBankBranchService bankBranchService,
        IContractTypeService contractTypeService,
        ICustomsAndVatcommissionarateService cusAndVatComService,
        IDeliveryMethodService deliveryMethodService,
        IDistrictService districtService,
        IExportTypeService exportTypeService,
        IMushakReturnPaymentTypeService mushakReturnPaymentTypeService,
        INbrEconomicCodeService nbrEconomicCodeService,
        INbrEconomicCodeTypeService nbrEconomicCodeTypeService,
        IPaymentMethodService paymentMethodService,
        IProductVatTypeService productVatTypeService,
        IPurchaseTypeService purchaseTypeService,
        ISalesDeliveryTypeService salesDeliveryTypeService,
        ISalesTypeService salesTypeService,
        IOrgStaticDataService orgStaticDataService,
        IOrgStaticDataTypeService orgStaticDataTypeService,
        ICountryService countryService,
        IPurchaseReasonService purchaseReasonService,

        ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
    {
        _adjustmentTypeService = adjustmentTypeService;
        _bankService = bankService;
        _bankBranchService = bankBranchService;
        _contractTypeService = contractTypeService;
        _cusAndVatComService = cusAndVatComService;
        _deliveryMethodService = deliveryMethodService;
        _districtService = districtService;
        _exportTypeService = exportTypeService;
        _mushakReturnPaymentTypeService = mushakReturnPaymentTypeService;
        _nbrEconomicCodeService = nbrEconomicCodeService;
        _nbrEconomicCodeTypeService = nbrEconomicCodeTypeService;
        _paymentMethodService = paymentMethodService;
        _productVatTypeService = productVatTypeService;
        _purchaseTypeService = purchaseTypeService;
        _salesDeliveryTypeService = salesDeliveryTypeService;
        _salesTypeService = salesTypeService;
        _orgStaticDataService = orgStaticDataService;
        _orgStaticDataTypeService = orgStaticDataTypeService;
        _countryService = countryService;
        _purchaseReasonService = purchaseReasonService;

        //organizationId = _session.OrganizationId;
    }

    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_ADJUSTMENT_TYPE_CAN_VIEW)]
    // public async Task<IActionResult> AdjustmentTypeIndex(int? page, string search = null)
    // {
    //
    //
    //     var adjustmentTypeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.AdjustmentType && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var adjustmentTypeList = await _adjustmentTypeService.Query().SelectAsync();
    //
    //     var orgAdjustmentTypeList = from adjustmentType in adjustmentTypeList
    //         join adjustmentTypeData in adjustmentTypeDataList on adjustmentType.AdjustmentTypeId equals
    //             adjustmentTypeData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = adjustmentTypeData.OrganizationId,
    //             OrgStaticDataId = adjustmentTypeData.OrgStaticDataId,
    //             OrgStaticDataTypeId = adjustmentTypeData.OrgStaticDataTypeId,
    //             DataKey = adjustmentTypeData.DataKey,
    //             DataTypeName = adjustmentTypeData.OrgStaticDataType.Name,
    //             DataName = adjustmentType.Name,
    //             ReferenceKey = adjustmentTypeData.ReferenceKey,
    //             IsActive = adjustmentTypeData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgAdjustmentTypeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgAdjustmentTypeList = orgAdjustmentTypeList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                                  || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfAdjustmentType = orgAdjustmentTypeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfAdjustmentType);
    // }

        
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_ADJUSTMENT_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_ADJUSTMENT_TYPE_CAN_ADD)]
    public async Task<IActionResult> AdjustmentTypeCreate()
    {
        ViewData["AdjustmentTypeId"] = new SelectList(await _adjustmentTypeService.Query().SelectAsync(), "AdjustmentTypeId", "Name");
        ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");

        return await Task.FromResult(View());
    }

    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_ADJUSTMENT_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_ADJUSTMENT_TYPE_CAN_ADD)]
    // public async Task<IActionResult> AdjustmentTypeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.AdjustmentType;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.AdjustmentType;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(AdjustmentTypeIndex));
    // }

        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_BANK_CAN_VIEW)]
    // public async Task<IActionResult> BankIndex(int? page, string search = null)
    // {
    //
    //
    //
    //     var bankDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.Bank && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var bankList = await _bankService.Query().SelectAsync();
    //
    //     var orgBankList = from bank in bankList
    //         join bankData in bankDataList on bank.BankId equals
    //             bankData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = bankData.OrganizationId,
    //             OrgStaticDataId = bankData.OrgStaticDataId,
    //             OrgStaticDataTypeId = bankData.OrgStaticDataTypeId,
    //             DataKey = bankData.DataKey,
    //             DataTypeName = bankData.OrgStaticDataType.Name,
    //             DataName = bank.Name,
    //             ReferenceKey = bankData.ReferenceKey,
    //             IsActive = bankData.IsActive
    //         };
    //
    //
    //
    //
    //
    //     ViewBag.PageCount = orgBankList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgBankList = orgBankList.Where(c => c.DataName.ToLower().Contains(search)
    //                                              || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfBank = orgBankList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfBank);
    // }

        
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_BANK_CAN_VIEW)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_BANK_CAN_ADD)]
    public async Task<IActionResult> BankCreate()
    {
        ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
        ViewData["BankId"] = new SelectList(await _bankService.Query().SelectAsync(), "BankId", "Name");

        return await Task.FromResult(View());
    }

    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_BANK_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_BANK_CAN_ADD)]
    // public async Task<IActionResult> BankCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.Bank;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.Bank;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(BankIndex));
    // }


        

    [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_BANK_BRANCH_CAN_VIEW)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_BANK_BRANCH_CAN_ADD)]

    public async Task<IActionResult> BankBranchCreate()
    {
        ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
        ViewData["BankBranchId"] = new SelectList(await _bankBranchService.Query().SelectAsync(), "BankBranchId", "Name");

        return await Task.FromResult(View());
    }

        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CONTRACT_TYPE_CAN_VIEW)]
    //
    // public async Task<IActionResult> ContractTypeIndex(int? page, string search = null)
    // {
    //
    //
    //     var contractTypeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.ContractType && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var contractTypeList = await _contractTypeService.Query().SelectAsync();
    //
    //     var orgContractTypeList = from contractType in contractTypeList
    //         join contractTypeData in contractTypeDataList on contractType.ContractTypeId equals
    //             contractTypeData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = contractTypeData.OrganizationId,
    //             OrgStaticDataId = contractTypeData.OrgStaticDataId,
    //             OrgStaticDataTypeId = contractTypeData.OrgStaticDataTypeId,
    //             DataKey = contractTypeData.DataKey,
    //             DataTypeName = contractTypeData.OrgStaticDataType.Name,
    //             DataName = contractType.Name,
    //             ReferenceKey = contractTypeData.ReferenceKey,
    //             IsActive = contractTypeData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgContractTypeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgContractTypeList = orgContractTypeList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                              || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfContractType = orgContractTypeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfContractType);
    // }


        
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CONTRACT_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CONTRACT_TYPE_CAN_ADD)]

    public async Task<IActionResult> ContractTypeCreate()
    {
        ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
        ViewData["ContractTypeId"] = new SelectList(await _contractTypeService.Query().SelectAsync(), "ContractTypeId", "Name");

        return await Task.FromResult(View());
    }

    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CONTRACT_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CONTRACT_TYPE_CAN_ADD)]
    // public async Task<IActionResult> ContractTypeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.ContractType;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.ContractType;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(ContractTypeIndex));
    // }

        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_COUNTRY_CAN_VIEW)]
    //
    // public async Task<IActionResult> CountryIndex(int? page, string search = null)
    // {
    //
    //     var countryDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.Country && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var countryList = await _countryService.Query().SelectAsync();
    //
    //     var orgCountryList = from country in countryList
    //         join countryData in countryDataList on country.CountryId equals
    //             countryData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = countryData.OrganizationId,
    //             OrgStaticDataId = countryData.OrgStaticDataId,
    //             OrgStaticDataTypeId = countryData.OrgStaticDataTypeId,
    //             DataKey = countryData.DataKey,
    //             DataTypeName = countryData.OrgStaticDataType.Name,
    //             DataName = country.Name,
    //             ReferenceKey = countryData.ReferenceKey,
    //             IsActive = countryData.IsActive
    //         };
    //
    //
    //
    //     ViewBag.PageCount = orgCountryList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgCountryList = orgCountryList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                    || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfCountry = orgCountryList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfCountry);
    // }

        
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_COUNTRY_CAN_VIEW)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_COUNTRY_CAN_ADD)]
    public async Task<IActionResult> CountryCreate()
    {
        ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
        ViewData["CountryId"] = new SelectList(await _countryService.Query().SelectAsync(), "CountryId", "Name");

        return await Task.FromResult(View());
    }

    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_COUNTRY_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_COUNTRY_CAN_ADD)]
    // public async Task<IActionResult> CountryCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.Country;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.Country;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(CountryIndex));
    // }

        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CUSTOMS_AND_VAT_COMMISSIONARATE_CAN_VIEW)]
    // public async Task<IActionResult> CusAndVatIndex(int? page, string search = null)
    // {
    //
    //     var cusAndVatDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.CustomsAndVATCommissionarate && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var cusAndVatList = await _cusAndVatComService.Query().SelectAsync();
    //
    //     var orgcusAndVatList = from cusAndVat in cusAndVatList
    //         join cusAndVatData in cusAndVatDataList on cusAndVat.CustomsAndVatcommissionarateId equals
    //             cusAndVatData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = cusAndVatData.OrganizationId,
    //             OrgStaticDataId = cusAndVatData.OrgStaticDataId,
    //             OrgStaticDataTypeId = cusAndVatData.OrgStaticDataTypeId,
    //             DataKey = cusAndVatData.DataKey,
    //             DataTypeName = cusAndVatData.OrgStaticDataType.Name,
    //             DataName = cusAndVat.Name,
    //             ReferenceKey = cusAndVatData.ReferenceKey,
    //             IsActive = cusAndVatData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgcusAndVatList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         ViewBag.searchText = search;
    //         search = search.ToLower().Trim();
    //         orgcusAndVatList = orgcusAndVatList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                        || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfCusAndVat = orgcusAndVatList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfCusAndVat);
    // }

        
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CUSTOMS_AND_VAT_COMMISSIONARATE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CUSTOMS_AND_VAT_COMMISSIONARATE_CAN_ADD)]
    public async Task<IActionResult> CusAndVatCreate()
    {
        ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
        ViewData["CustomsAndVATCommissionarateId"] = new SelectList(await _cusAndVatComService.Query().SelectAsync(), "CustomsAndVatcommissionarateId", "Name");

        return await Task.FromResult(View());
    }

    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CUSTOMS_AND_VAT_COMMISSIONARATE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_CUSTOMS_AND_VAT_COMMISSIONARATE_CAN_ADD)]
    // public async Task<IActionResult> CusAndVatCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.CustomsAndVATCommissionarate;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.CustomsAndVatcommissionarate;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(CusAndVatIndex));
    // }


        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DELIVERY_METHOD_CAN_VIEW)]
    // public async Task<IActionResult> DeliveryMethodIndex(int? page, string search = null)
    // {
    //
    //     var deliveryMethodDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.DeliveryMethod && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var deliveryMethodList = await _deliveryMethodService.Query().SelectAsync();
    //
    //     var orgDeliveryMethodList = from deliveryMethod in deliveryMethodList
    //         join deliveryMethodData in deliveryMethodDataList on deliveryMethod.DeliveryMethodId equals
    //             deliveryMethodData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = deliveryMethodData.OrganizationId,
    //             OrgStaticDataId = deliveryMethodData.OrgStaticDataId,
    //             OrgStaticDataTypeId = deliveryMethodData.OrgStaticDataTypeId,
    //             DataKey = deliveryMethodData.DataKey,
    //             DataTypeName = deliveryMethodData.OrgStaticDataType.Name,
    //             DataName = deliveryMethod.Name,
    //             ReferenceKey = deliveryMethodData.ReferenceKey,
    //             IsActive = deliveryMethodData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgDeliveryMethodList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgDeliveryMethodList = orgDeliveryMethodList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                                  || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfDeliveryMethod = orgDeliveryMethodList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfDeliveryMethod);
    // }

        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DELIVERY_METHOD_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DELIVERY_METHOD_CAN_ADD)]
    // public async Task<IActionResult> DeliveryMethodCreate()
    // {
    //     ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
    //     ViewData["DeliveryMethodId"] = new SelectList(await _deliveryMethodService.Query().SelectAsync(), "DeliveryMethodId", "Name");
    //
    //     return await Task.FromResult(View());
    // }
    //
    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DELIVERY_METHOD_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DELIVERY_METHOD_CAN_ADD)]
    // public async Task<IActionResult> DeliveryMethodCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.DeliveryMethod;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.DeliveryMethod;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(DeliveryMethodIndex));
    // }

        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DISTRICT_CAN_VIEW)]
    // public async Task<IActionResult> DistrictIndex(int? page, string search = null)
    // {
    //
    //
    //     var districtDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.Discrict && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var districtList = await _districtService.Query().SelectAsync();
    //
    //     var orgDistrictList = from district in districtList
    //         join districtData in districtDataList on district.DistrictId equals
    //             districtData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = districtData.OrganizationId,
    //             OrgStaticDataId = districtData.OrgStaticDataId,
    //             OrgStaticDataTypeId = districtData.OrgStaticDataTypeId,
    //             DataKey = districtData.DataKey,
    //             DataTypeName = districtData.OrgStaticDataType.Name,
    //             DataName = district.Name,
    //             ReferenceKey = districtData.ReferenceKey,
    //             IsActive = districtData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgDistrictList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgDistrictList = orgDistrictList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                      || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfDistrict = orgDistrictList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfDistrict);
    // }

        
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DISTRICT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DISTRICT_CAN_ADD)]

    public async Task<IActionResult> DistrictCreate()
    {
        ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
        ViewData["DistrictId"] = new SelectList(await _districtService.Query().SelectAsync(), "DistrictId", "Name");

        return await Task.FromResult(View());
    }

    // [HttpPost]
    //
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DISTRICT_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_DISTRICT_CAN_ADD)]
    // public async Task<IActionResult> DistrictCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.Discrict;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.District;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(DistrictIndex));
    // }

        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_EXPORT_TYPE_CAN_VIEW)]
    //
    // public async Task<IActionResult> ExportTypeIndex(int? page, string search = null)
    // {
    //
    //
    //     var exportTypeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.ExportType && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var exportTypeList = await _exportTypeService.Query().SelectAsync();
    //
    //     var orgExportTypeList = from exportType in exportTypeList
    //         join districtData in exportTypeDataList on exportType.ExportTypeId equals
    //             districtData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = districtData.OrganizationId,
    //             OrgStaticDataId = districtData.OrgStaticDataId,
    //             OrgStaticDataTypeId = districtData.OrgStaticDataTypeId,
    //             DataKey = districtData.DataKey,
    //             DataTypeName = districtData.OrgStaticDataType.Name,
    //             DataName = exportType.ExportTypeName,
    //             ReferenceKey = districtData.ReferenceKey,
    //             IsActive = districtData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgExportTypeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgExportTypeList = orgExportTypeList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                          || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfExportType = orgExportTypeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfExportType);
    // }


        
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_EXPORT_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_EXPORT_TYPE_CAN_ADD)]

    public async Task<IActionResult> ExportTypeCreate()
    {
        ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
        ViewData["ExportTypeId"] = new SelectList(await _exportTypeService.Query().SelectAsync(), "ExportTypeId", "ExportTypeName");

        return await Task.FromResult(View());
    }

    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_EXPORT_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_EXPORT_TYPE_CAN_ADD)]
    // public async Task<IActionResult> ExportTypeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.ExportType;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.ExportType;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(ExportTypeIndex));
    // }

        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_MUSHAK_RETURN_PAYMENT_TYPE_CAN_VIEW)]
    //
    // public async Task<IActionResult> MushakReturnPaymentTypeIndex(int? page, string search = null)
    // {
    //
    //     var mushakReturnPaymentTypeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.MushakReturnPaymentType && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var mushakReturnPaymentTypeList = await _mushakReturnPaymentTypeService.Query().SelectAsync();
    //
    //     var orgMushakReturnPaymentTypeList = from mushakReturnPaymentType in mushakReturnPaymentTypeList
    //         join mushakReturnPaymentTypeData in mushakReturnPaymentTypeDataList on mushakReturnPaymentType.MushakReturnPaymentTypeId equals
    //             mushakReturnPaymentTypeData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = mushakReturnPaymentTypeData.OrganizationId,
    //             OrgStaticDataId = mushakReturnPaymentTypeData.OrgStaticDataId,
    //             OrgStaticDataTypeId = mushakReturnPaymentTypeData.OrgStaticDataTypeId,
    //             DataKey = mushakReturnPaymentTypeData.DataKey,
    //             DataTypeName = mushakReturnPaymentTypeData.OrgStaticDataType.Name,
    //             DataName = mushakReturnPaymentType.SubFormName,
    //             ReferenceKey = mushakReturnPaymentTypeData.ReferenceKey,
    //             IsActive = mushakReturnPaymentTypeData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgMushakReturnPaymentTypeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         ViewBag.searchText = search;
    //         search = search.ToLower().Trim();
    //         orgMushakReturnPaymentTypeList = orgMushakReturnPaymentTypeList.Where(c => c.DataName.ToLower().Contains(search)
    //             || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfMushakReturnPaymentType = orgMushakReturnPaymentTypeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfMushakReturnPaymentType);
    // }


        
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_MUSHAK_RETURN_PAYMENT_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.INTEGRATION_SETUP_MUSHAK_RETURN_PAYMENT_TYPE_CAN_ADD)]

    public async Task<IActionResult> MushakReturnPaymentTypeCreate()
    {
        ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
        ViewData["MushakReturnPaymentTypeId"] = new SelectList(await _mushakReturnPaymentTypeService.Query().SelectAsync(), "MushakReturnPaymentTypeId", "SubFormName");

        return await Task.FromResult(View());
    }

    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_MUSHAK_RETURN_PAYMENT_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_MUSHAK_RETURN_PAYMENT_TYPE_CAN_ADD)]
    // public async Task<IActionResult> MushakReturnPaymentTypeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.MushakReturnPaymentType;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.MushakReturnPaymentType;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(MushakReturnPaymentTypeIndex));
    // }


        
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_NBR_ECONOMIC_CODE_TYPE_CAN_VIEW)]
    //
    // public async Task<IActionResult> NbrEconomicCodeTypeIndex(int? page, string search = null)
    // {
    //
    //
    //     var nbrEconomicCodeTypeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.NBREconomicCodeType && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var nbrEconomicCodeTypeList = await _nbrEconomicCodeTypeService.Query().SelectAsync();
    //
    //     var orgNbrEconomicCodeTypeList = from nbrEconomicCodeType in nbrEconomicCodeTypeList
    //         join nbrEconomicCodeTypeData in nbrEconomicCodeTypeDataList on nbrEconomicCodeType.NbrEconomicCodeTypeId equals
    //             nbrEconomicCodeTypeData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = nbrEconomicCodeTypeData.OrganizationId,
    //             OrgStaticDataId = nbrEconomicCodeTypeData.OrgStaticDataId,
    //             OrgStaticDataTypeId = nbrEconomicCodeTypeData.OrgStaticDataTypeId,
    //             DataKey = nbrEconomicCodeTypeData.DataKey,
    //             DataTypeName = nbrEconomicCodeTypeData.OrgStaticDataType.Name,
    //             DataName = nbrEconomicCodeType.CodeTypeName,
    //             ReferenceKey = nbrEconomicCodeTypeData.ReferenceKey,
    //             IsActive = nbrEconomicCodeTypeData.IsActive
    //         };
    //
    //
    //     ViewBag.PageCount = orgNbrEconomicCodeTypeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgNbrEconomicCodeTypeList = orgNbrEconomicCodeTypeList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                                            || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfNbrEconomicCodeType = orgNbrEconomicCodeTypeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfNbrEconomicCodeType);
    // }
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_NBR_ECONOMIC_CODE_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_MUSHAK_RETURN_PAYMENT_TYPE_CAN_ADD)]
    // public async Task<IActionResult> NbrEconomicCodeTypeCreate()
    // {
    //     ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
    //     ViewData["NbrEconomicCodeTypeId"] = new SelectList(await _nbrEconomicCodeTypeService.Query().SelectAsync(), "NbrEconomicCodeTypeId", "CodeTypeName");
    //
    //     return await Task.FromResult(View());
    // }
    //
    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_NBR_ECONOMIC_CODE_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_NBR_ECONOMIC_CODE_TYPE_CAN_ADD)]
    // public async Task<IActionResult> NbrEconomicCodeTypeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.NBREconomicCodeType;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.NbrEconomicCodeType;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(NbrEconomicCodeTypeIndex));
    // }
    //
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_NBR_ECONOMIC_CODE_CAN_VIEW)]
    // public async Task<IActionResult> NbrEconomicCodeIndex(int? page, string search = null)
    // {
    //
    //
    //     var nbrEconomicCodeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.NBREconomicCode && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var nbrEconomicCodeList = await _nbrEconomicCodeService.Query().SelectAsync();
    //
    //     var orgNbrEconomicCodeList = from nbrEconomicCode in nbrEconomicCodeList
    //         join nbrEconomicCodeData in nbrEconomicCodeDataList on nbrEconomicCode.NbrEconomicCodeId equals
    //             nbrEconomicCodeData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = nbrEconomicCodeData.OrganizationId,
    //             OrgStaticDataId = nbrEconomicCodeData.OrgStaticDataId,
    //             OrgStaticDataTypeId = nbrEconomicCodeData.OrgStaticDataTypeId,
    //             DataKey = nbrEconomicCodeData.DataKey,
    //             DataTypeName = nbrEconomicCodeData.OrgStaticDataType.Name,
    //             DataName = nbrEconomicCode.EconomicTitle,
    //             ReferenceKey = nbrEconomicCodeData.ReferenceKey,
    //             IsActive = nbrEconomicCodeData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgNbrEconomicCodeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgNbrEconomicCodeList = orgNbrEconomicCodeList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                                    || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfNbrEconomicCode = orgNbrEconomicCodeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfNbrEconomicCode);
    // }
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_NBR_ECONOMIC_CODE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_NBR_ECONOMIC_CODE_CAN_ADD)]
    // public async Task<IActionResult> NbrEconomicCodeCreate()
    // {
    //     ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
    //     ViewData["NbrEconomicCodeId"] = new SelectList(await _nbrEconomicCodeService.Query().SelectAsync(), "NbrEconomicCodeId", "EconomicTitle");
    //
    //     return await Task.FromResult(View());
    // }
    //
    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_NBR_ECONOMIC_CODE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_NBR_ECONOMIC_CODE_CAN_ADD)]
    // public async Task<IActionResult> NbrEconomicCodeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.NBREconomicCode;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.NbrEconomicCode;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(NbrEconomicCodeIndex));
    // }
    //
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PAYMENT_METHOD_CAN_VIEW)]
    //
    // public async Task<IActionResult> PaymentMethodIndex(int? page, string search = null)
    // {
    //
    //
    //     var paymentMethodDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.PaymentMethod && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var paymentMethodList = await _paymentMethodService.Query().SelectAsync();
    //
    //     var orgPaymentMethodList = from paymentMethod in paymentMethodList
    //         join paymentMethodData in paymentMethodDataList on paymentMethod.PaymentMethodId equals
    //             paymentMethodData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = paymentMethodData.OrganizationId,
    //             OrgStaticDataId = paymentMethodData.OrgStaticDataId,
    //             OrgStaticDataTypeId = paymentMethodData.OrgStaticDataTypeId,
    //             DataKey = paymentMethodData.DataKey,
    //             DataTypeName = paymentMethodData.OrgStaticDataType.Name,
    //             DataName = paymentMethod.Name,
    //             ReferenceKey = paymentMethodData.ReferenceKey,
    //             IsActive = paymentMethodData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgPaymentMethodList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgPaymentMethodList = orgPaymentMethodList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                                || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfPaymentMethod = orgPaymentMethodList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfPaymentMethod);
    // }
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PAYMENT_METHOD_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PAYMENT_METHOD_CAN_ADD)]
    // public async Task<IActionResult> PaymentMethodCreate()
    // {
    //     ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
    //     ViewData["PaymentMethodId"] = new SelectList(await _paymentMethodService.Query().SelectAsync(), "PaymentMethodId", "Name");
    //
    //     return await Task.FromResult(View());
    // }
    //
    // [HttpPost]
    //
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PAYMENT_METHOD_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PAYMENT_METHOD_CAN_ADD)]
    // public async Task<IActionResult> PaymentMethodCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.PaymentMethod;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.PaymentMethod;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(PaymentMethodIndex));
    // }
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PRODUCT_VAT_TYPE_CAN_VIEW)]
    // public async Task<IActionResult> ProductVatTypeIndex(int? page, string search = null)
    // {
    //
    //
    //     var productVatTypeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.ProductVATType && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var productVatTypeList = await _productVatTypeService.Query().SelectAsync();
    //
    //     var orgProductVatTypeList = from productVatType in productVatTypeList
    //         join productVatTypeData in productVatTypeDataList on productVatType.ProductVattypeId equals
    //             productVatTypeData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = productVatTypeData.OrganizationId,
    //             OrgStaticDataId = productVatTypeData.OrgStaticDataId,
    //             OrgStaticDataTypeId = productVatTypeData.OrgStaticDataTypeId,
    //             DataKey = productVatTypeData.DataKey,
    //             DataTypeName = productVatTypeData.OrgStaticDataType.Name,
    //             DataName = productVatType.Name,
    //             ReferenceKey = productVatTypeData.ReferenceKey,
    //             IsActive = productVatTypeData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgProductVatTypeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgProductVatTypeList = orgProductVatTypeList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                                  || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfProductVatType = orgProductVatTypeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfProductVatType);
    // }
    //
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PRODUCT_VAT_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PRODUCT_VAT_TYPE_CAN_ADD)]
    // public async Task<IActionResult> ProductVatTypeCreate()
    // {
    //     ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
    //     ViewData["ProductVattypeId"] = new SelectList(await _productVatTypeService.Query().SelectAsync(), "ProductVattypeId", "Name");
    //
    //     return await Task.FromResult(View());
    // }
    //
    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PRODUCT_VAT_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PRODUCT_VAT_TYPE_CAN_ADD)]
    // public async Task<IActionResult> ProductVatTypeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.ProductVATType;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.ProductVATType;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(ProductVatTypeIndex));
    // }
    //
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_TYPE_CAN_VIEW)]
    // public async Task<IActionResult> PurchaseTypeIndex(int? page, string search = null)
    // {
    //
    //
    //     var purchaseTypeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.PurchaseType && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var purchaseTypeList = await _purchaseTypeService.Query().SelectAsync();
    //
    //     var orgPurchaseTypeList = from purchaseType in purchaseTypeList
    //         join purchaseTypeData in purchaseTypeDataList on purchaseType.PurchaseTypeId equals
    //             purchaseTypeData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = purchaseTypeData.OrganizationId,
    //             OrgStaticDataId = purchaseTypeData.OrgStaticDataId,
    //             OrgStaticDataTypeId = purchaseTypeData.OrgStaticDataTypeId,
    //             DataKey = purchaseTypeData.DataKey,
    //             DataTypeName = purchaseTypeData.OrgStaticDataType.Name,
    //             DataName = purchaseType.Name,
    //             ReferenceKey = purchaseTypeData.ReferenceKey,
    //             IsActive = purchaseTypeData.IsActive
    //         };
    //
    //
    //
    //     ViewBag.PageCount = orgPurchaseTypeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgPurchaseTypeList = orgPurchaseTypeList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                              || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfPurchaseType = orgPurchaseTypeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfPurchaseType);
    // }
    //
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_TYPE_CAN_ADD)]
    // public async Task<IActionResult> PurchaseTypeCreate()
    // {
    //     ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
    //     ViewData["PurchaseTypeId"] = new SelectList(await _purchaseTypeService.Query().SelectAsync(), "PurchaseTypeId", "Name");
    //
    //     return await Task.FromResult(View());
    // }
    //
    // [HttpPost]
    //
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_TYPE_CAN_ADD)]
    // public async Task<IActionResult> PurchaseTypeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.PurchaseType;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.PurchaseType;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(PurchaseTypeIndex));
    // }
    //
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_DELIVERY_TYPE_CAN_VIEW)]
    // public async Task<IActionResult> SalesDeliveryTypeIndex(int? page, string search = null)
    // {
    //
    //
    //     var salesDeliveryTypeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.SalesDeliveryType && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var salesDeliveryTypeList = await _salesDeliveryTypeService.Query().SelectAsync();
    //
    //     var orgSalesDeliveryTypeList = from salesDeliveryType in salesDeliveryTypeList
    //         join salesDeliveryTypeData in salesDeliveryTypeDataList on salesDeliveryType.SalesDeliveryTypeId equals
    //             salesDeliveryTypeData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = salesDeliveryTypeData.OrganizationId,
    //             OrgStaticDataId = salesDeliveryTypeData.OrgStaticDataId,
    //             OrgStaticDataTypeId = salesDeliveryTypeData.OrgStaticDataTypeId,
    //             DataKey = salesDeliveryTypeData.DataKey,
    //             DataTypeName = salesDeliveryTypeData.OrgStaticDataType.Name,
    //             DataName = salesDeliveryType.Name,
    //             ReferenceKey = salesDeliveryTypeData.ReferenceKey,
    //             IsActive = salesDeliveryTypeData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgSalesDeliveryTypeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgSalesDeliveryTypeList = orgSalesDeliveryTypeList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                                        || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfSalesDeliveryType = orgSalesDeliveryTypeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfSalesDeliveryType);
    // }
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_DELIVERY_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_DELIVERY_TYPE_CAN_ADD)]
    // public async Task<IActionResult> SalesDeliveryTypeCreate()
    // {
    //     ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
    //     ViewData["SalesDeliveryTypeId"] = new SelectList(await _salesDeliveryTypeService.Query().SelectAsync(), "SalesDeliveryTypeId", "Name");
    //
    //     return await Task.FromResult(View());
    // }
    //
    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_DELIVERY_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_DELIVERY_TYPE_CAN_ADD)]
    // public async Task<IActionResult> SalesDeliveryTypeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.SalesDeliveryType;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.SalesDeliveryType;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(SalesDeliveryTypeIndex));
    // }
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_TYPE_CAN_VIEW)]
    //
    // public async Task<IActionResult> SalesTypeIndex(int? page, string search = null)
    // {
    //
    //
    //     var salesTypeDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.SalesType && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var salesTypeList = await _salesTypeService.Query().SelectAsync();
    //
    //     var orgSalesTypeList = from salesType in salesTypeList
    //         join salesTypeData in salesTypeDataList on salesType.SalesTypeId equals
    //             salesTypeData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = salesTypeData.OrganizationId,
    //             OrgStaticDataId = salesTypeData.OrgStaticDataId,
    //             OrgStaticDataTypeId = salesTypeData.OrgStaticDataTypeId,
    //             DataKey = salesTypeData.DataKey,
    //             DataTypeName = salesTypeData.OrgStaticDataType.Name,
    //             DataName = salesType.SalesTypeName,
    //             ReferenceKey = salesTypeData.ReferenceKey,
    //             IsActive = salesTypeData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgSalesTypeList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgSalesTypeList = orgSalesTypeList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                        || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfSalesType = orgSalesTypeList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfSalesType);
    // }
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_TYPE_CAN_ADD)]
    // public async Task<IActionResult> SalesTypeCreate()
    // {
    //     ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
    //     ViewData["SalesTypeId"] = new SelectList(await _salesTypeService.Query().SelectAsync(), "SalesTypeId", "SalesTypeName");
    //
    //     return await Task.FromResult(View());
    // }
    //
    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_TYPE_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_SALES_TYPE_CAN_ADD)]
    // public async Task<IActionResult> SalesTypeCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.SalesType;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.SalesType;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(SalesTypeIndex));
    // }
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_REASON_CAN_VIEW)]
    // public async Task<IActionResult> PurchaseReasonIndex(int? page, string search = null)
    // {
    //
    //
    //     var purchaseReasonDataList =
    //         await _orgStaticDataService.Query().Where(sd => sd.OrgStaticDataTypeId == (int)EnumOrgStaticDataType.PurchaseReason && sd.OrganizationId == UserSession.OrganizationId).Include(sd => sd.OrgStaticDataType).SelectAsync();
    //
    //     var purchaseReasonList = await _purchaseReasonService.Query().SelectAsync();
    //
    //     var orgPurchaseReasonList = from purchaseReason in purchaseReasonList
    //         join purchaseReasonData in purchaseReasonDataList on purchaseReason.PurchaseReasonId equals
    //             purchaseReasonData.DataKey
    //         select new vmIntegrationSetup
    //         {
    //             OrganizationId = purchaseReasonData.OrganizationId,
    //             OrgStaticDataId = purchaseReasonData.OrgStaticDataId,
    //             OrgStaticDataTypeId = purchaseReasonData.OrgStaticDataTypeId,
    //             DataKey = purchaseReasonData.DataKey,
    //             DataTypeName = purchaseReasonData.OrgStaticDataType.Name,
    //             DataName = purchaseReason.Reason,
    //             ReferenceKey = purchaseReasonData.ReferenceKey,
    //             IsActive = purchaseReasonData.IsActive
    //         };
    //
    //     ViewBag.PageCount = orgPurchaseReasonList.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         orgPurchaseReasonList = orgPurchaseReasonList.Where(c => c.DataName.ToLower().Contains(search)
    //                                                                  || c.ReferenceKey.ToString().Contains(search));
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listPurchaseReason = orgPurchaseReasonList.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listPurchaseReason);
    // }
    //
    //     
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_REASON_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_REASON_CAN_ADD)]
    // public async Task<IActionResult> PurchaseReasonCreate()
    // {
    //     ViewData["OrgStaticDataTypeId"] = new SelectList(await _orgStaticDataTypeService.Query().SelectAsync(), "OrgStaticDataTypeId", "Name");
    //     ViewData["PurchaseReasonId"] = new SelectList(await _purchaseReasonService.Query().SelectAsync(), "PurchaseReasonId", "Reason");
    //
    //     return await Task.FromResult(View());
    // }
    //
    // [HttpPost]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_REASON_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.INTEGRATION_SETUP_PURCHASE_REASON_CAN_ADD)]
    // public async Task<IActionResult> PurchaseReasonCreate(OrgStaticDatum orgStatData)
    // {
    //     try
    //     {
    //         orgStatData.OrganizationId = UserSession.OrganizationId;
    //         orgStatData.IsActive = true;
    //         orgStatData.OrgStaticDataTypeId = (int)EnumOrgStaticDataType.PurchaseReason;
    //         orgStatData.EffectiveFrom = DateTime.Now;
    //         orgStatData.EffectiveTo = null;
    //         orgStatData.CreatedBy = UserSession.UserId;
    //         orgStatData.CreatedTime = DateTime.Now;
    //         orgStatData.ModifiedBy = null;
    //         orgStatData.ModifiedTime = null;
    //
    //         _orgStaticDataService.Insert(orgStatData);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         var jObj = JsonConvert.SerializeObject(orgStatData, Formatting.None,
    //             new JsonSerializerSettings()
    //             {
    //                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //             });
    //
    //         AuditLog au = new AuditLog();
    //         au.ObjectTypeId = (int)EnumObjectType.PurchaseReason;
    //         au.PrimaryKey = orgStatData.OrgStaticDataId;
    //         au.AuditOperationId = (int)EnumOperations.Add;
    //         au.UserId = UserSession.UserId;
    //         au.Datetime = DateTime.Now;
    //         au.Descriptions = jObj.ToString();
    //         au.IsActive = true;
    //         au.CreatedBy = UserSession.UserId;
    //         au.CreatedTime = DateTime.Now;
    //         au.OrganizationId = UserSession.OrganizationId;
    //         await AuditLogCreate(au);
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //
    //     return RedirectToAction(nameof(PurchaseReasonIndex));
    // }
}