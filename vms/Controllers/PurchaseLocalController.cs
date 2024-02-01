using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.ReportsViewModel;
using vms.service.Services.MushakService;
using vms.service.Services.PaymentService;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.TransactionService;
using vms.service.Services.UploadService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class PurchaseLocalController : ControllerBase
{

    #region readonly field

    private readonly IPurchaseService _purchaseService;
    private readonly IDebitNoteService _debitNoteService;
    private readonly IPurchaseOrderService _purchaseOrderService;
    private readonly IMeasurementUnitService _measurementUnitService;
    private readonly IPurchaseTypeService _purchaseTypeService;
    private readonly IVendorService _vendorService;
    private readonly IPurchaseReasonService _purchaseReasonService;
    private readonly IPurchaseOrderDetailService _purchaseOdService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IOrganizationService _organizationService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IDocumentTypeService _documentType;
    private readonly IProductVatTypeService _vatTypeService;
    private readonly IBankService _bankService;
    private readonly INbrEconomicCodeService _nbrEconomicCodeService;
    private readonly ICustomsAndVatcommissionarateService _customsAndVatcommissionarateService;
    private readonly IAutocompleteService _autocompleteService;
    private readonly ISaleService _saleService;
    //private readonly ISalesDetailService _salesDetailService;
    private readonly IVehicleTypeService _vType;
    private readonly IDamageTypeService _damageTypeService;
    private readonly IDamageService _damageService;
    private readonly IPurchaseDetailService _purchaseDetailService;
    private readonly IProductStoredProcedureService _productStoredProcedureService;
    private readonly IOrgBranchService _branchService;
    private readonly IMushakGenerationService _mushakGenerationService;

    private readonly IPurchaseImportTaxPaymentTypeService _importTaxPaymentTypeService;
    private readonly IDistrictService _districtService;

    private readonly IVmsExcelService _excelService;
    private readonly IExcelDataUploadService _excelDataUploadService;

    private readonly IExcelSimplifiedPurchaseService _excelSimplifiedPurchaseService;
    private readonly IExcelSimplifiedLocalPurchaseService _excelSimplifiedLocalPurchaseService;

    private readonly IFileOperationService _fileOperationService;

    #endregion

    #region constructor

    public PurchaseLocalController(ControllerBaseParamModel controllerBaseParamModel, IPurchaseService purchaseService,
        //SalesDetailService salesDetailService,
        ISaleService saleService,
        IAutocompleteService autocompleteService,
        ICustomsAndVatcommissionarateService customsAndVatcommissionarateService,
        IBankService bankService,
        INbrEconomicCodeService nbrEconomicCodeService,
        IProductVatTypeService vatTypeService,
        IDocumentTypeService documentType, IDebitNoteService debitNoteService, IOrganizationService organizationService, IWebHostEnvironment hostingEnvironment, IPurchaseReasonService purchaseReasonService, IPurchaseOrderService purchaseOrderService, IPurchaseOrderDetailService purchaseOrderDetailService,
        IMeasurementUnitService measurementUnitService, IPurchaseTypeService purchaseTypeService, IVendorService vendorService, IPaymentMethodService paymentMethodService
        , IVehicleTypeService vType, IDamageTypeService damageTypeService, IDamageService damageService, IPurchaseDetailService purchaseDetailService
        , IDamageDetailService damageDetailService, IProductStoredProcedureService productStoredProcedureService, IMushakGenerationService mushakGenerationService, IPurchaseImportTaxPaymentTypeService importTaxPaymentTypeService, IDistrictService districtService, IOrgBranchService branchService, IVmsExcelService excelService, IExcelSimplifiedPurchaseService excelSimplifiedPurchaseService, IExcelDataUploadService excelDataUploadService, IFileOperationService fileOperationService, IExcelSimplifiedLocalPurchaseService excelSimplifiedLocalPurchaseService)
        : base(controllerBaseParamModel)
    {
        _purchaseService = purchaseService;
        _productStoredProcedureService = productStoredProcedureService;
        _mushakGenerationService = mushakGenerationService;
        _importTaxPaymentTypeService = importTaxPaymentTypeService;
        _districtService = districtService;
        _branchService = branchService;
        _excelService = excelService;
        _excelSimplifiedPurchaseService = excelSimplifiedPurchaseService;
        _excelDataUploadService = excelDataUploadService;
        _fileOperationService = fileOperationService;
        _excelSimplifiedLocalPurchaseService = excelSimplifiedLocalPurchaseService;
        _damageService = damageService;
        _purchaseDetailService = purchaseDetailService;
        _damageTypeService = damageTypeService;
        //_salesDetailService = salesDetailService;
        _saleService = saleService;
        _autocompleteService = autocompleteService;
        _customsAndVatcommissionarateService = customsAndVatcommissionarateService;
        _bankService = bankService;
        _nbrEconomicCodeService = nbrEconomicCodeService;
        _vatTypeService = vatTypeService;
        _documentType = documentType;
        _debitNoteService = debitNoteService;
        _purchaseOrderService = purchaseOrderService;
        _purchaseTypeService = purchaseTypeService;
        _purchaseOdService = purchaseOrderDetailService;
        _measurementUnitService = measurementUnitService;
        _vendorService = vendorService;
        _purchaseReasonService = purchaseReasonService;
        _hostingEnvironment = hostingEnvironment;
        _organizationService = organizationService;
        _paymentMethodService = paymentMethodService;
        _vType = vType;
    }

    #endregion

    #region ListData

    [VmsAuthorize(FeatureList.PURCHASE)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        //return View(await _purchaseService.GetPurchaseLocalListByOrganization(UserSession.ProtectedOrganizationId));
        return View(await _purchaseService.GetPurchaseLocalListByOrganizationAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch));
    }

    #endregion

    #region Purchase Local Save

    [VmsAuthorize(FeatureList.PURCHASE)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
    public async Task<IActionResult> PurchaseLocal()
    {
        var pOrgId = UserSession.ProtectedOrganizationId;
        var model = new VmPurchaseLocal
        {
            PurchaseDate = DateTime.Today,
            PurchaseReasonSelectList = await _purchaseReasonService.GetSelectList(),
            VendorSelectList = await _vendorService.GetLocalVendorSelectList(pOrgId),
            ProductList = _productStoredProcedureService.GetProductForPurchase(pOrgId),
            ProductVatTypes = await _vatTypeService.GetLocalPurchaseProductVatTypes(),
            PaymentMethodList = await _paymentMethodService.GetPaymentMethods(),
            BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId),
            PaymentDate = DateTime.Today,
            PaymentDocumentOrTransDate = DateTime.Today,
            DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(pOrgId),
            OrgBranchList = await _branchService.GetOrgBranchSelectListByUser(pOrgId, UserSession.BranchIds, UserSession.IsRequireBranch),
            IsRequireGoodsId = UserSession.IsRequireGoodsId,
            IsRequireSkuId = UserSession.IsRequireSkuId,
            IsRequireSkuNo = UserSession.IsRequireSkuNo,
            IsRequirePartNo = UserSession.IsRequirePartNo
        };
        return View(model);
    }

    [VmsAuthorize(FeatureList.PURCHASE)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]

    // move to Purchase local
    [HttpPost]
    public async Task<JsonResult> PurchaseLocal(VmPurchaseLocalPost model)
    {
        model.OrganizationId = UserSession.OrganizationId;
        model.CreatedBy = UserSession.UserId;
        model.PurchaseDate = DateTime.Now;
        var id = IvatDataProtector.Protect((await _purchaseService.InsertLocalPurchase(model)).ToString());
        await UnitOfWork.SaveChangesAsync();
        return Json(new { id = id });
    }

    [VmsAuthorize(FeatureList.PURCHASE)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
    [HttpPost]
    public async Task<JsonResult> PurchaseLocalDraft(VmPurchaseLocalPost model)
    {
        model.OrganizationId = UserSession.OrganizationId;
        model.CreatedBy = UserSession.UserId;
        model.PurchaseDate = DateTime.Now;
        var id = IvatDataProtector.Protect((await _purchaseService.InsertLocalPurchaseDraft(model)).ToString());
        await UnitOfWork.SaveChangesAsync();
        return Json(new { id = id });
    }

    #endregion

    #region damage 

    public async Task<IActionResult> Damage(string id)
	{
		var purchaseDetails = await _purchaseDetailService.GetPurchaseDetails(id);
		var damageDetails = purchaseDetails.Select(s => new DamageDetail
		{
			DamageDetailId = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.DamageDetailId).FirstOrDefault()),
			DamageDescription = s.DamageDetails?.Select(s1 => s1.DamageDescription).FirstOrDefault(),
			DamageQty = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.DamageQty).FirstOrDefault()),
			SuggestedNewUnitPrice = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.SuggestedNewUnitPrice).FirstOrDefault()),
			ApprovedNewUnitPrice = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.ApprovedNewUnitPrice).FirstOrDefault()),
			ApprovedUsableQty = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.ApprovedUsableQty).FirstOrDefault()),
			MeasurementUnitId = Convert.ToInt32(s.Product.MeasurementUnitId),
			UsablePercent = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.UsablePercent).FirstOrDefault()),
			UsableQty = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.DamageDetailId).FirstOrDefault()),
			Product = s.Product,
			ProductId = s.Product.ProductId,
			PurchaseDetailId = s.PurchaseDetailId,
			PurchaseDetail = purchaseDetails.FirstOrDefault()
		}).ToList();

		Damage damage = new Damage();
		VmPurchaseDamage vm = new VmPurchaseDamage();
		vm.DamageDetails = damageDetails;
		vm.Damage = damage;
		vm.PurchaseId = purchaseDetails.Select(s => s.PurchaseId).FirstOrDefault();
		ViewData["damageType"] = new SelectList(await _damageTypeService.Query().SelectAsync(), "DamageTypeId", "Name");
		return View("DamageNew", vm);
	}
	[HttpPost]
	[VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD_DEBIT_NOTE)]
	public async Task<JsonResult> Damage(VmPurchaseDamagePost vm)
	{
		_damageService.InsertPurchaseDamage(vm, UserSession);
		await UnitOfWork.SaveChangesAsync();
		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		return Json(new { id = 1 });
	}

	public async Task<IActionResult> UpdateDamage(string id)
	{
		var purchaseDetails = await _purchaseDetailService.GetPurchaseDetails(id);
		var damageDetails = purchaseDetails.Select(s => new DamageDetail
		{
			DamageDetailId = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.DamageDetailId).FirstOrDefault()),
			DamageDescription = s.DamageDetails?.Select(s1 => s1.DamageDescription).FirstOrDefault(),
			DamageQty = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.DamageQty).FirstOrDefault()),
			SuggestedNewUnitPrice = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.SuggestedNewUnitPrice).FirstOrDefault()),
			ApprovedNewUnitPrice = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.ApprovedNewUnitPrice).FirstOrDefault()),
			ApprovedUsableQty = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.ApprovedUsableQty).FirstOrDefault()),
			MeasurementUnitId = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.MeasurementUnitId).FirstOrDefault()),
			UsablePercent = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.UsablePercent).FirstOrDefault()),
			UsableQty = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.DamageDetailId).FirstOrDefault()),
			Product = s.Product,
			PurchaseDetail = purchaseDetails.FirstOrDefault()
		}).ToList();

		Damage damage = new Damage();

		var vm = new vmDamageDetails { DamageDetails = damageDetails, Damage = damage };
		ViewData["damageType"] = new SelectList(await _damageTypeService.Query().SelectAsync(), "DamageTypeId", "Name");

		return View(vm);
	}

	#endregion

	#region Loacal purchase details data

	public async Task<IActionResult> Details(string id)
	{
		var purchase = await _purchaseOrderService.GetPurchaseDetails(id);
		var purchaseDetails = await _purchaseOdService.GetPurchaseDetails(id);

		var getNotes = await _debitNoteService.Query().Include(c => c.Purchase).Where(c => c.Purchase.OrganizationId == UserSession.OrganizationId && c.PurchaseId == int.Parse(IvatDataProtector.Unprotect(id))).SelectAsync();
		var vm = new VmPurchaseDetail { PurchaseDetails = purchaseDetails, Purchase = purchase, DebitNotes = getNotes };
		return View(vm);
	}

    #endregion

    #region Debit Note

    [VmsAuthorize(FeatureList.PURCHASE)]

    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD_DEBIT_NOTE)]
    public async Task<IActionResult> DebitNote(string id)
    {
        //var idAfterDecrypt = VmsDataProtectionManager.DecryptInt(id);
        var purchase = await _purchaseOrderService.GetPurchaseDetails(id);
        var purchaseDetails = await _purchaseOdService.GetPurchaseDetails(id);
        VmPurchaseDebitNote vmPurchaseDebitNote = new VmPurchaseDebitNote();
        vmPurchaseDebitNote.Purchase = purchase;
        vmPurchaseDebitNote.PurchaseDetails = purchaseDetails;
        ViewData["VType"] = new SelectList(await _vType.Query().SelectAsync(), "VehicleTypeId", "VehicleTypeName");

        return View(vmPurchaseDebitNote);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.PURCHASE)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD_DEBIT_NOTE)]
    public async Task<JsonResult> DebitNoteSave(VmPurchaseDebitNotePost vm)
    {
        var createdBy = UserSession.UserId;
        var organizationId = UserSession.OrganizationId;
        bool status = false;

        if (vm.vmPurchaseDebitNoteDetials.Count > 0)
        {
            vm.CreatedBy = createdBy;
            vm.CreatedTime = DateTime.Now;
            status = await _purchaseOrderService.InsertDebitNote(vm);
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        return Json(new { id = 1 });
    }

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_8)]
	public async Task<IActionResult> DebitNoteViewById(int? id, int? language = 0)
	{
		if (id == null)
		{
			return BadRequest("Invalid request");
		}

		var model = new vmDebitMushak
		{
			Language = language ?? 0,
			DebitMushakList = await _mushakGenerationService.Mushak6P8(id.Value),
			DebitNoteId = id.Value
		};
		return View(model);
	}

	#endregion

	#region Upload purchase

	[VmsAuthorize(FeatureList.PURCHASE)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_VDS_LIST_CAN_VIEW)]

    public async Task<IActionResult> UploadSimplifiedLocalPurchaseList()
    {
        var model = new VmViewSimplifiedPurchase
        {
            Purchases = await _excelDataUploadService.GetSimplifiedLocalPurchaseListAsync()
        };
        return View(model);
    }


    [VmsAuthorize(FeatureList.PURCHASE)]
    [VmsAuthorize(FeatureList.PURCHASE_PURCHASE_VDS_LIST_CAN_VIEW)]
    [HttpPost]
    public async Task<IActionResult> UploadSimplifiedLocalPurchase(VmSingleFileUpload model)
    {

        if (ModelState.IsValid)
        {
            var msg = ExcelFileValidationLocalPurchase(model);
            if (!string.IsNullOrEmpty(msg))
            {
                TempData["ErrorMessage"] = msg;
            }
            else
            {
                try
                {
                    var simplifiedRawPurchaseList = _excelService.ReadExcel<VmExcelSimplifiedLocalPurchase>(model.UploadedFile);
                    msg = ExcelFileValidationLocalPurchaseDupChk(simplifiedRawPurchaseList);
                    if (string.IsNullOrEmpty(msg))
                    {
                        var fileSaveDto = new FileSaveDto
                        {
                            FileRootPath = ControllerStaticData.FileRootPath,
                            FileModulePath = ControllerStaticData.FileUploadedSimplifiedLocalPurchasePath,
                            OrganizationId = UserSession.OrganizationId
                        };

                        var fsf = await _fileOperationService.SaveFile(model.UploadedFile, fileSaveDto);

                        var excelDataUpload = new ExcelDataUpload
                        {
                            ExcelUploadedDataTypeId = (int)EnumUploadedExcelDataType.SimplifiedLocalPurchase,
                            UploadedFileName = model.UploadedFile.FileName,
                            StoredFilePath = fsf.FileUrl,
                            CreatedBy = UserSession.UserId,
                            OrganizationId = UserSession.OrganizationId,
                            CreatedTime = DateTime.Now,
                            UploadTime = DateTime.Now,
                        };
                        _excelDataUploadService.Insert(excelDataUpload);
                        await UnitOfWork.SaveChangesAsync();

                        await _excelSimplifiedLocalPurchaseService.SaveSimplifiedPurchaseList(simplifiedRawPurchaseList,
                            excelDataUpload.ExcelDataUploadId);
                        await UnitOfWork.SaveChangesAsync();
                        var process = await _purchaseService.ProcessUploadedSimplifiedLocalPurchase(excelDataUpload.ExcelDataUploadId,
                            UserSession.OrganizationId);
                        TempData["SuccessMessage"] = "Uploaded successfully!!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = msg;
                    }
                }
                catch (Exception exception)
                {
                    TempData["ErrorMessage"] = exception.Message;
                }

            }

        }
        else
        {
            var errorMessage = "";
            foreach (var modelStateValue in ModelState.Values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    errorMessage += error.ErrorMessage + "<br>";
                }
            }
            TempData["ErrorMessage"] = errorMessage;
        }

        return RedirectToAction(nameof(UploadSimplifiedLocalPurchaseList));
    }

    #endregion

    #region private method


    private string ExcelFileValidationLocalPurchase(VmSingleFileUpload model)
    {
        //File type validation
        long FILE_SIZE = 20971520;
        //5242880-5mb/20971520-20mb
        string[] FILE_EXTENSION = { ".xls", ".xlsx" };

        int CELL_NUMBER = 42;

        string fileExtension = Path.GetExtension(model.UploadedFile.FileName).ToLower();

        if (!fileExtension.Equals(FILE_EXTENSION[0]) && !fileExtension.Equals(FILE_EXTENSION[1]))
        {
            return "Not a valid file. File extension must be *.xls or *.xlsx";
        }
        //File Size in byte

        if (model.UploadedFile.Length > FILE_SIZE)
        {
            return "File size exceed";
        }
        //Header Column Count

        using var stream = model.UploadedFile.OpenReadStream();
        stream.Position = 0;
        var xssWorkbook = new XSSFWorkbook(stream);
        var sheet = xssWorkbook.GetSheetAt(0);
        var firstRow = sheet.GetRow(0);

        if (firstRow.Cells.Count != CELL_NUMBER)
        {
            return "Excel column mismatch";
        }

        var obj = new VmExcelSimplifiedLocalPurchase();
        var processingRow = 0;
        foreach (var columnHeader in firstRow.Cells)
        {
            var colName = columnHeader.StringCellValue.ToLower().Replace("zone", "branch").Replace("office", "branch");
            if (!colName.Equals(obj.GetType().GetProperties()[processingRow++].Name.ToLower()))
                return "Column " + processingRow + " or row sequence not matched";
        }
        //Row count
        return sheet.LastRowNum == 0 ? "Blank excel file" : "";
    }

    private string ExcelFileValidationLocalPurchaseDupChk(List<VmExcelSimplifiedLocalPurchase> simplifiedRawPurchaseList)
    {
        var dupes = simplifiedRawPurchaseList.GroupBy(x => new { x.PurchaseId, x.PurchaseDetailId, x.PaymentId }).Where(x => x.Skip(1).Any()).ToArray();
        if (dupes.Any())
            return "Duplicate records found on excel";

        //Data Type and Length validation
        var i = 1;
        var message = string.Empty;
        var sb = new StringBuilder();
        foreach (var model in simplifiedRawPurchaseList)
        {
            if (!Validate(model, out var results))
            {
                var j = 1;
                foreach (var validationResult in results)
                {
                    sb.Append($"{i}.{j}. Row {i} {validationResult.ErrorMessage} </br>");
                    j++;
                }

                i++;
            }
        }

        if (sb.Length > 0)
        {
            message = sb.ToString();
        }

        return message;
    }


    static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
    {
        results = new List<ValidationResult>();

        return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
    }

    #endregion


}