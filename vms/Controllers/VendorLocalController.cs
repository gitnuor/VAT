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
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using AutoMapper;
using vms.entity.viewModels.VendorViewModel;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class VendorLocalController : ControllerBase

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


    public VendorLocalController(ControllerBaseParamModel controllerBaseParamModel, IVendorService service, ICountryService countryService, ICustomsAndVatcommissionarateService cusAndVatService, IBusinessCategoryService businessCategoryService, IBusinessNatureService businessNatureService, IDivisionOrStateService divisionOrStateService, IDistrictOrCityService districtOrCityService, IBankService bankService, IMapper mapper) : base(controllerBaseParamModel)
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
        return View(await _service.GetVendorLocalListByOrg(UserSession.ProtectedOrganizationId));
	}

    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_ADD)]
    public async Task<IActionResult> Create()
    {

	    vmVendor model = new vmVendor();
	    model = await GetDropdownValuesForVendorViewModel(model);
	    return View(model);
    }



    [HttpPost]
    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_ADD)]
    public async Task<IActionResult> Create(vmVendor model)
    {
	    var vendor = _mapper.Map<vmVendor, Vendor>(model);
	    try
	    {
		    vendor.CreatedTime = DateTime.Now;
		    vendor.CreatedBy = UserSession.UserId;
		    vendor.OrganizationId = UserSession.OrganizationId;
		    vendor = SetVendorLocationField(vendor);
		    _service.Insert(vendor);
		    await UnitOfWork.SaveChangesAsync();


		    var jObj = JObject.Parse(JsonConvert.SerializeObject(vendor));

		    AuditLog au = new AuditLog();
		    au.ObjectTypeId = (int)EnumObjectType.Vendor;
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
		    return RedirectToAction("Index", "Vendor");


	    }
	    catch (Exception ex)
	    {
		    TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			return RedirectToAction("Index", "Vendor");
		}
    }

	[VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    public async Task<ActionResult> FileDownload()
    {
        var sWebRootFolder = Environment.WebRootPath;
        sWebRootFolder = Path.Combine(sWebRootFolder, "ExportExcel");
        var sFileName = @"Vendors.xlsx";
        var memory = new MemoryStream();
        using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
        {

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            var excelSheet = workbook.CreateSheet("Vendors");

            var style = workbook.CreateCellStyle();
            var styleHeading = workbook.CreateCellStyle();
            var fontHeading = workbook.CreateFont();
            fontHeading.FontHeightInPoints = 14;
            styleHeading.Alignment = HorizontalAlignment.Center;
            styleHeading.VerticalAlignment = VerticalAlignment.Center;
            styleHeading.SetFont(fontHeading);
            //styleHeading.WrapText = true;

            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            //style.WrapText = true;

            var row = excelSheet.CreateRow(0);
            row.CreateCell(0).CellStyle = styleHeading;
            row.GetCell(0).SetCellValue("Name");
            excelSheet.DefaultRowHeightInPoints = 18;
            row.CreateCell(1).CellStyle = styleHeading;
            row.GetCell(1).SetCellValue("Address");
            excelSheet.DefaultRowHeightInPoints = 18;
            row.CreateCell(2).CellStyle = styleHeading;
            row.GetCell(2).SetCellValue("BIN");
            excelSheet.DefaultRowHeightInPoints = 18;
            row.CreateCell(3).CellStyle = styleHeading;
            row.GetCell(3).SetCellValue("Phone No.");
            excelSheet.DefaultRowHeightInPoints = 18;
            row.CreateCell(4).CellStyle = styleHeading;
            row.GetCell(4).SetCellValue("Status");
            excelSheet.DefaultRowHeightInPoints = 18;
               

            var vendors = await _service.GetVendors(UserSession.OrganizationId);
            var rowCounter = 1;
            foreach (var vendor in vendors)
            {
                row = excelSheet.CreateRow(rowCounter);
                row.CreateCell(0).CellStyle = style;
                row.GetCell(0).SetCellValue(vendor.Name);
                row.CreateCell(1).CellStyle = style;
                row.GetCell(1).SetCellValue(vendor.Address);
                row.CreateCell(2).CellStyle = style;
                row.GetCell(2).SetCellValue(vendor.BinNo);
                row.CreateCell(3).CellStyle = style;
                row.GetCell(3).SetCellValue(vendor.ContactNo);
                row.CreateCell(4).CellStyle = style;
                row.GetCell(4).SetCellValue(vendor.IsRegistered ? "Registered" : "Not Registered");
                                      
                rowCounter++;
            }

            for (var i = 0; i <= 4; i++)
            {
                excelSheet.AutoSizeColumn(i, false);
            }

			workbook.Write(fs, false);
		}

        using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
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
    public async Task<IActionResult> CreateLocal()
    {

	    var model = new VendorLocalCreateViewModel();
	    model = await GetDropdownValuesForVendorViewModel(model);
	    return View(model);
	}

    [VmsAuthorize(FeatureList.THIRD_PARTY)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_VIEW)]
    [VmsAuthorize(FeatureList.THIRD_PARTY_VENDOR_CAN_ADD)]
    public async Task<IActionResult> CreateForeign()
    {
	    var model = new VendorForeignCreateViewModel();
	    model = await GetDropdownValuesForVendorViewModel(model);
	    return View(model);
    }

	private Vendor SetVendorLocationField(Vendor vendor)
    {
        if (vendor.IsForeignVendor)
        {
            vendor.IsRegistered = false;
            vendor.IsRegisteredAsTurnOverOrg = false;
            vendor.BinNo = null;
            vendor.CustomsAndVatcommissionarateId = null;
        }
        else
        {
            vendor.IsForeignVendor = false;
            vendor.CountryId = 14;
        }

        return vendor;
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
                vendorData = SetVendorLocationField(vendorData);
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
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
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
        model.BusinessCategoryList = await _businessCategoryService.GetAllCategorySelectListItem();
        model.BusinessNatureList = await _businessNatureService.BusinessNatureSelectList();
        model.BankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
        return model;
    }

    private async Task<VendorLocalCreateViewModel> GetDropdownValuesForVendorViewModel(VendorLocalCreateViewModel model)
    {
        
        model.CustomsAndVatCommissionarates = await _cusAndVatService.GetCustomsAndVatcommissionarateSelectList();
        model.BusinessCategoryList = await _businessCategoryService.GetAllCategorySelectListItem();
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