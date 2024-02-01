using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.Models;
using vms.utility.StaticData;
using vms.Utility;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using vmTransferReceive = vms.Models.vmTransferReceive;
using vms.utility;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.entity.viewModels.ReportsViewModel;
using NPOI.SS.Util;
using vms.entity.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text;
using vms.service.Services.TransactionService;
using vms.service.Services.UploadService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;
using vms.service.Services.PaymentService;
using vms.service.Services.MushakService;
using vms.service.Services.ProductService;

namespace vms.Controllers;

public class PurchaseController : ControllerBase
{
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
	private readonly ISalesDetailService _salesDetailService;
	private readonly IVehicleTypeService _vType;
	private readonly IDamageTypeService _damageTypeService;
	private readonly IDamageService _damageService;
	private readonly IPurchaseDetailService _purchaseDetailService;
	private readonly IProductStoredProcedureService _productStoredProcedureService;
	private readonly IPurchaseService _purchaseService;
	private readonly IOrgBranchService _branchService;
	private readonly IMushakGenerationService _mushakGenerationService;

	private readonly IPurchaseImportTaxPaymentTypeService _importTaxPaymentTypeService;
	private readonly IDistrictService _districtService;

	private readonly IVmsExcelService _excelService;
	private readonly IExcelDataUploadService _excelDataUploadService;

	private readonly IExcelSimplifiedPurchaseService _excelSimplifiedPurchaseService;
	private readonly IExcelSimplifiedLocalPurchaseService _excelSimplifiedLocalPurchaseService;

	private readonly IFileOperationService _fileOperationService;
	//private int organizationId;

	public PurchaseController(ISalesDetailService salesDetailService,
		ISaleService saleService,
		IAutocompleteService autocompleteService,
		ICustomsAndVatcommissionarateService customsAndVatcommissionarateService,
		IBankService bankService,
		INbrEconomicCodeService nbrEconomicCodeService,
		IProductVatTypeService vatTypeService,
		IDocumentTypeService documentType, IDebitNoteService debitNoteService, IOrganizationService organizationService, IWebHostEnvironment hostingEnvironment, ControllerBaseParamModel controllerBaseParamModel,
		IPurchaseReasonService purchaseReasonService, IPurchaseOrderService purchaseOrderService, IPurchaseOrderDetailService purchaseOrderDetailService,
		IMeasurementUnitService measurementUnitService, IPurchaseTypeService purchaseTypeService, IVendorService vendorService, IPaymentMethodService paymentMethodService
		, IVehicleTypeService vType, IDamageTypeService damageTypeService, IDamageService damageService, IPurchaseDetailService purchaseDetailService
		, IDamageDetailService damageDetailService, IProductStoredProcedureService productStoredProcedureService, IPurchaseService purchaseService, IMushakGenerationService mushakGenerationService, IPurchaseImportTaxPaymentTypeService importTaxPaymentTypeService, IDistrictService districtService, IOrgBranchService branchService, IVmsExcelService excelService, IExcelSimplifiedPurchaseService excelSimplifiedPurchaseService, IExcelDataUploadService excelDataUploadService, IFileOperationService fileOperationService, IExcelSimplifiedLocalPurchaseService excelSimplifiedLocalPurchaseService) : base(controllerBaseParamModel)
	{
		_productStoredProcedureService = productStoredProcedureService;
		_purchaseService = purchaseService;
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
		_salesDetailService = salesDetailService;
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

	[VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
	public async Task<IActionResult> Index()
	{
		//return View(await _purchaseService.GetPurchaseListByOrganization(UserSession.ProtectedOrganizationId));
        return View(await _purchaseService.GetPurchaseListByOrganizationAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch));
    }

	[VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
	public async Task<IActionResult> IndexNew()
	{
		var model = new vmPurchaseIndex();
		model.PurchaseList = await _purchaseService.GetPurchaseByOrganization(UserSession.ProtectedOrganizationId);
		return View(model);
	}

	[VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_PRINT_MUSHAK_6_8)]
	public async Task<IActionResult> DebitNoteView(int? id, int? page, string search = null)
	{
		//string txt = search;

		//var getNotes = await _debitNoteService.Query().Include(c => c.Purchase).Where(c => c.Purchase.OrganizationId == UserSession.OrganizationId).SelectAsync();
		var getNotes = await _debitNoteService.GetDebitNoteDataByOrgAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);

  //      if (id != null)
		//{
		//	getNotes = getNotes.Where(c => c.PurchaseId == id);
		//}
		//if (search != null)
		//{
		//	search = search.ToLower().Trim();
		//	getNotes = getNotes.Where(c => c.Purchase.PurchaseId.ToString().Contains(search)
		//								   || c.ReturnDate.ToString().Contains(search)
		//								   || c.ReasonOfReturn.ToLower().Contains(search)
		//	);
		//}
		//if (txt != null)
		//{
		//	ViewData[ViewStaticData.SEARCH_TEXT] = txt;
		//}
		//else
		//{
		//	ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
		//}

		//var pageNumber = page ?? 1;
		var listOfSale = getNotes; //.ToPagedList(pageNumber, 10);
		return View(listOfSale);
	}

	// move to purchase local

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

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_8)]
	public async Task<IActionResult> Mushok6P8ExportToExcel(vmDebitMushak data)
	{
		var model = new List<SpDebitMushak>();

		model = await _mushakGenerationService.Mushak6P8(data.DebitNoteId);
		if (model.Any())
		{

			var firstItem = model.First();

			string sWebRootFolder = _hostingEnvironment.WebRootPath;
			sWebRootFolder = Path.Combine(sWebRootFolder, "ExportExcel");
			if (!Directory.Exists(sWebRootFolder))
			{
				Directory.CreateDirectory(sWebRootFolder);
			}
			string sFileName = @"Mushak_6.8.xlsx";
			string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
			FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
			var memory = new MemoryStream();
			using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
			{

				IWorkbook workbook;
				workbook = new XSSFWorkbook();
				ISheet excelSheet = workbook.CreateSheet("Mushak6.8");
				IRow row = excelSheet.CreateRow(0);

				ICellStyle style = workbook.CreateCellStyle();
				ICellStyle styleHeading = workbook.CreateCellStyle();
				IFont fontHeading = workbook.CreateFont();
				fontHeading.FontHeightInPoints = 18;
				styleHeading.Alignment = HorizontalAlignment.Center;
				styleHeading.VerticalAlignment = VerticalAlignment.Center;
				styleHeading.SetFont(fontHeading);
				styleHeading.WrapText = true;
				IFont font = workbook.CreateFont();
				font.IsBold = true;
				style.Alignment = HorizontalAlignment.Center;
				style.VerticalAlignment = VerticalAlignment.Center;
				style.SetFont(font);
				style.WrapText = true;


				row.CreateCell(0).CellStyle = styleHeading;
				row.GetCell(0).SetCellValue("গণপ্রজাতন্ত্রী বাংলাদেশ সরকার");
				excelSheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));
				excelSheet.DefaultRowHeightInPoints = 24;

				row = excelSheet.CreateRow(1);
				row.CreateCell(0).CellStyle = styleHeading;
				row.GetCell(0).SetCellValue("জাতীয় রাজস্ব বোর্ড");
				excelSheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
				excelSheet.DefaultRowHeightInPoints = 24;

				row = excelSheet.CreateRow(2);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue("ডেবিট নোট");
				excelSheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 5));

				row = excelSheet.CreateRow(3);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue("[বিধি ৪০ এর উপ-বিধি (১) এর দফা (ছ) দ্রষ্টব্য]");
				excelSheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 5));


				row = excelSheet.CreateRow(4);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 5));

				row = excelSheet.CreateRow(5);
				row.CreateCell(0).SetCellValue("ফেরত প্রদানকারী ব্যক্তির - ");
				excelSheet.AddMergedRegion(new CellRangeAddress(5, 5, 0, 2));
				row.CreateCell(3).SetCellValue("ফেরত গ্রহণকারী ব্যক্তির - ");
				excelSheet.AddMergedRegion(new CellRangeAddress(5, 5, 3, 5));

				row = excelSheet.CreateRow(6);
				row.CreateCell(0).SetCellValue("নাম : " + firstItem.OrgName);
				excelSheet.AddMergedRegion(new CellRangeAddress(6, 6, 0, 2));
				row.CreateCell(3).SetCellValue("নাম : " + firstItem.VENName);
				excelSheet.AddMergedRegion(new CellRangeAddress(6, 6, 3, 5));

				row = excelSheet.CreateRow(7);
				row.CreateCell(0).SetCellValue("বিআইএন : " + firstItem.OrgBin);
				excelSheet.AddMergedRegion(new CellRangeAddress(7, 7, 0, 2));
				row.CreateCell(3).SetCellValue("বিআইএন : " + firstItem.VENBin);
				excelSheet.AddMergedRegion(new CellRangeAddress(7, 7, 3, 5));

				row = excelSheet.CreateRow(8);
				row.CreateCell(0).SetCellValue("মূল চালান নম্বর : " + firstItem.PSInvoice);
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 0, 2));
				row.CreateCell(3).SetCellValue("ডেবিট নোট নম্বর : " + firstItem.DebitNo);
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 3, 5));

				row = excelSheet.CreateRow(9);
				row.CreateCell(0).SetCellValue("মূল চালান ইস্যুর তারিখ : " + (firstItem.PSDate == null ? "" : firstItem.PSDate.Value.ToString("dd/MM/yyyy")));
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 0, 2));
				row.CreateCell(3).SetCellValue("ইস্যুর তারিখ : " + (firstItem.DrTime == null ? "" : firstItem.DrTime.Value.ToString("dd/MM/yyyy")));
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 3, 5));

				row = excelSheet.CreateRow(10);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(10, 10, 0, 2));
				row.CreateCell(3).SetCellValue("ইস্যুর সময় : " + (firstItem.DrTime == null ? "" : firstItem.DrTime.Value.ToString("hh:mm tt")));
				excelSheet.AddMergedRegion(new CellRangeAddress(10, 10, 3, 5));

				row = excelSheet.CreateRow(11);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(11, 11, 0, 5));

				row = excelSheet.CreateRow(12);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue("ক্রমিক সংখ্যা");
				excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 0, 0));
				row.CreateCell(1).CellStyle = style;
				row.GetCell(1).SetCellValue("ফেরতপ্রাপ্ত সরবরাহের বিবরণ");
				excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 1, 1));
				row.CreateCell(2).CellStyle = style;
				row.GetCell(2).SetCellValue("সরবরাহের একক");
				excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 2, 2));
				row.CreateCell(3).CellStyle = style;
				row.GetCell(3).SetCellValue("পরিমাণ");
				excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 3, 3));
				row.CreateCell(4).CellStyle = style;
				row.GetCell(4).SetCellValue("একক মূল্য (১) টাকায়");
				excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 4, 4));
				row.CreateCell(5).CellStyle = style;
				row.GetCell(5).SetCellValue("মোট মূল্য (টাকায়)");
				excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 5, 5));


				row = excelSheet.CreateRow(13);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue("(১)");
				row.CreateCell(1).CellStyle = style;
				row.GetCell(1).SetCellValue("(২)");
				row.CreateCell(2).CellStyle = style;
				row.GetCell(2).SetCellValue("(৩)");
				row.CreateCell(3).CellStyle = style;
				row.GetCell(3).SetCellValue("(৪)");
				row.CreateCell(4).CellStyle = style;
				row.GetCell(4).SetCellValue("(৫)");
				row.CreateCell(5).CellStyle = style;
				row.GetCell(5).SetCellValue("(৬)");


				int rowCounter = 14, SlNo = 1;
				decimal TotalSum = 0, vds = 0, vat = 0, sd = 0;
				foreach (var item in model)
				{
					row = excelSheet.CreateRow(rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(SlNo);
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(item.ProductName);
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(item.Quantity);
					row.CreateCell(3).CellStyle = style;
					row.GetCell(3).SetCellValue(item.ReturnQuantity == null ? "" : item.ReturnQuantity.Value.ToString());
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(item.UnitPrice == null ? "" : item.UnitPrice.Value.ToString());
					row.CreateCell(5).CellStyle = style;
					row.GetCell(5).SetCellValue(item.TotalAmount == null ? "" : item.TotalAmount.Value.ToString());

					TotalSum += item.TotalAmount == null ? 0 : item.TotalAmount.Value;
					vds += item.DeductionAmount == null ? 0 : item.DeductionAmount.Value;
					vat += item.VatAmount == null ? 0 : item.VatAmount.Value;
					sd += item.SupplementaryDutyAmount == null ? 0 : item.SupplementaryDutyAmount.Value;

					rowCounter++;
					SlNo++;
				}

				row = excelSheet.CreateRow(rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 3));
				row.CreateCell(4).SetCellValue("মোট মূল্য :");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 4));
				row.CreateCell(5).SetCellValue(TotalSum.ToString());
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 5, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 3));
				row.CreateCell(4).SetCellValue("বাদ কর্তন  : ");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 4));
				row.CreateCell(5).SetCellValue(vds.ToString());
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 5, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 3));
				row.CreateCell(4).SetCellValue("মূসকসহ মূল্য : ");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 4));
				row.CreateCell(5).SetCellValue((TotalSum + vat).ToString());
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 5, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 3));
				row.CreateCell(4).SetCellValue("মূসকের পরিমাণ : ");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 4));
				row.CreateCell(5).SetCellValue(vat.ToString());
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 5, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 3));
				row.CreateCell(4).SetCellValue("সম্পূরক শুল্কের পরিমাণ : ");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 4));
				row.CreateCell(5).SetCellValue(sd.ToString());
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 5, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 3));
				row.CreateCell(4).SetCellValue("মোট কর : ");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 4));
				row.CreateCell(5).SetCellValue((sd + vat).ToString());
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 5, 5));

				for (int i = 0; i < 5; i++)
				{
					excelSheet.AutoSizeColumn(i, true);
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

		return RedirectToAction(nameof(DebitNoteView));

	}

	// move to Purchase local

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
			OrgBranchList = await _branchService.GetOrgBranchSelectList(pOrgId),
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

    // move to Purchase local

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


    // move to Purchase Foreign

    [VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
	public async Task<IActionResult> PurchaseImport()
	{
		var organizationId = UserSession.ProtectedOrganizationId;
		var model = new VmPurchaseImport
		{
			PurchaseReasonSelectList = await _purchaseReasonService.GetSelectList(),
			VendorSelectList = await _vendorService.GetLocalForeignSelectList(organizationId),
			ProductList = _productStoredProcedureService.GetProductForPurchase(organizationId),
			MeasurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(organizationId),
			ProductVatTypes = await _vatTypeService.GetForeignPurchaseProductVatTypes(),
			VatCommissionarateList =
				await _customsAndVatcommissionarateService.GetCustomsAndVatcommissionarateSelectList(),
			// AtpBankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId),
			DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(organizationId),
			PaymentMethodList = await _paymentMethodService.GetPaymentMethods(),
			LcDate = DateTime.Now,
			BillOfEntryDate = DateTime.Now.AddDays(30),
			DueDate = DateTime.Now.AddDays(90),
			PurchaseDate = DateTime.Now,
			PaymentDate = DateTime.Now,
			PaymentDocumentOrTransDate = DateTime.Now,
			PurchaseImportTaxPaymentDocOrChallanDate = DateTime.Now,
			PurchaseImportTaxPaymentDate = DateTime.Now,
			BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId),
			PurchaseImportTaxPaymentTypeList =
				await _importTaxPaymentTypeService.GetPurchaseImportTaxPaymentTypeSelectList(),
			DistrictList = await _districtService.DistrictSelectList(),
			OrgBranchList = await _branchService.GetOrgBranchSelectList(organizationId),
			IsRequireGoodsId = UserSession.IsRequireGoodsId,
			IsRequireSkuId = UserSession.IsRequireSkuId,
			IsRequireSkuNo = UserSession.IsRequireSkuNo,
			IsRequirePartNo = UserSession.IsRequirePartNo
		};
		return View(model);
	}

    // move to Purchase Foreign
    [VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
	[HttpPost]
	public async Task<JsonResult> PurchaseImport(VmPurchaseImportPost model)
	{
		model.OrganizationId = UserSession.OrganizationId;
		model.CreatedBy = UserSession.UserId;
		var id = IvatDataProtector.Protect((await _purchaseService.InsertImportPurchase(model)).ToString());
		await UnitOfWork.SaveChangesAsync();
		return Json(new { id = id });
	}
    // move to Purchase Foreign

    [VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
	[HttpPost]
	public async Task<JsonResult> PurchaseImportDraft(VmPurchaseImportPost model)
	{
		model.OrganizationId = UserSession.OrganizationId;
		model.CreatedBy = UserSession.UserId;
		var id = IvatDataProtector.Protect((await _purchaseService.InsertImportPurchaseDraft(model)).ToString());
		await UnitOfWork.SaveChangesAsync();
		return Json(new { id = id });
	}



	[VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
	public async Task<IActionResult> PurchaseLocalBk()
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		ViewData[ControllerStaticData.PURCHASE_REASON_ID] = new SelectList(await _purchaseReasonService.Query().SelectAsync(), ControllerStaticData.PURCHASE_REASON_ID, ControllerStaticData.REASON);
		ViewData[ControllerStaticData.VENDOR_ID] = new SelectList(await _vendorService.Query().Where(c => c.CreatedBy == createdBy).SelectAsync(), ControllerStaticData.VENDOR_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.PURCHASE_TYPE_ID] = new SelectList(await _purchaseTypeService.Query().SelectAsync(), ControllerStaticData.PURCHASE_TYPE_ID, ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(await _measurementUnitService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId).SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.DOCUMENT_TYPE_ID] = new SelectList(await _documentType.Query().SelectAsync(), ControllerStaticData.DOCUMENT_TYPE_ID, "Name");
		ViewData[ControllerStaticData.PAYMENT_METHOD_ID] = new SelectList(await _paymentMethodService.Query().SelectAsync(), ControllerStaticData.PAYMENT_METHOD_ID, "Name");

		return View();
	}

	[VmsAuthorize(FeatureList.PURCHASE)]
	public async Task<IActionResult> PurchaseForeign()
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		ViewData[ControllerStaticData.PURCHASE_REASON_ID] = new SelectList(await _purchaseReasonService.Query().SelectAsync(), ControllerStaticData.PURCHASE_REASON_ID, ControllerStaticData.REASON);
		ViewData[ControllerStaticData.VENDOR_ID] = new SelectList(await _vendorService.Query().Where(c => c.CreatedBy == createdBy).SelectAsync(), ControllerStaticData.VENDOR_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.PURCHASE_TYPE_ID] = new SelectList(await _purchaseTypeService.Query().SelectAsync(), ControllerStaticData.PURCHASE_TYPE_ID, ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(await _measurementUnitService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId).SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.DOCUMENT_TYPE_ID] = new SelectList(await _documentType.Query().SelectAsync(), ControllerStaticData.DOCUMENT_TYPE_ID, "Name");
		ViewData[ControllerStaticData.PAYMENT_METHOD_ID] = new SelectList(await _paymentMethodService.Query().SelectAsync(), ControllerStaticData.PAYMENT_METHOD_ID, "Name");
		return View();
	}

	[VmsAuthorize(FeatureList.PURCHASE)]
	public async Task<IActionResult> NewPurchase()
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		ViewData[ControllerStaticData.PURCHASE_REASON_ID] = new SelectList(await _purchaseReasonService.Query().SelectAsync(), ControllerStaticData.PURCHASE_REASON_ID, ControllerStaticData.REASON);
		ViewData[ControllerStaticData.VENDOR_ID] = new SelectList(await _vendorService.Query().Where(c => c.CreatedBy == createdBy).SelectAsync(), ControllerStaticData.VENDOR_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.PURCHASE_TYPE_ID] = new SelectList(await _purchaseTypeService.Query().SelectAsync(), ControllerStaticData.PURCHASE_TYPE_ID, ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(await _measurementUnitService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId).SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);

		return View();
	}

	public async Task<FileSaveFeedbackDto> FileSaveAsync(IFormFile File)
	{
		FileSaveFeedbackDto fdto = new FileSaveFeedbackDto();
		var FileExtenstion = Path.GetExtension(File.FileName);

		string FileName = Guid.NewGuid().ToString();

		FileName += FileExtenstion;
		Organization organization = await _organizationService.Query().FirstOrDefaultAsync(c => c.OrganizationId == UserSession.OrganizationId, CancellationToken.None);
		var FolderName = ControllerStaticData.APPLICATION_DOCUMENT + organization.OrganizationId;
		var uploads = Path.Combine(_hostingEnvironment.WebRootPath, FolderName);

		fdto.MimeType = FileExtenstion;
		bool exists = Directory.Exists(uploads);
		if (!exists)
		{
			Directory.CreateDirectory(uploads);
		}
		if (File.Length > 0)
		{
			var filePath = Path.Combine(uploads, File.FileName);
			fdto.FileUrl = filePath;
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await File.CopyToAsync(fileStream);
			}
		}
		return fdto;
	}

	public IActionResult FileUpload(vmPurchaseIndex requestData)
	{
		if (ModelState.IsValid)
		{
			try
			{

				/** This code should be moved to service as per needed. **/
				//IFormFile file = Request.Form.Files[0];
				IFormFile file = requestData.UploadedFile;
				if (file.FileName.IndexOf(".xlsx") > 0 || file.FileName.IndexOf(".xls") > 0)
				{
					string folderName = "UploadedExcel\\Purchase";
					string rootPath = _hostingEnvironment.WebRootPath;
					string newPath = Path.Combine(rootPath, folderName);
					string fullPath = "";
					string FileName = Guid.NewGuid().ToString();

					if (!Directory.Exists(newPath))
					{
						Directory.CreateDirectory(newPath);
					}
					if (file.Length > 0)
					{

						fullPath = Path.Combine(newPath, FileName + "_" + file.FileName);
						using (var stream = new FileStream(fullPath, FileMode.Create))
						{
							file.CopyTo(stream);
						}
					}

					FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
					IWorkbook workbook = null;
					if (file.FileName.IndexOf(".xlsx") > 0)
					{
						workbook = new XSSFWorkbook(fs);
					}
					else if (file.FileName.IndexOf(".xls") > 0)
					{
						workbook = new HSSFWorkbook(fs);
					}
					workbook.MissingCellPolicy = MissingCellPolicy.RETURN_NULL_AND_BLANK;

					// BulkPurchase Sheet Number One Start
					List<Purchase> purchaseList = new List<Purchase>();
					ISheet sheetPurchase = workbook.GetSheetAt(0);
					if (sheetPurchase != null)
					{
						int rowCount = sheetPurchase.LastRowNum;
						for (int i = 1; i <= rowCount; i++)
						{
							var data = new Purchase();
							IRow curRow = sheetPurchase.GetRow(i);
							data.ReferenceKey = curRow.GetCell(0) == null ? "" : curRow.GetCell(0).StringCellValue.Trim();
							data.VendorId = ExcelDataConverter.GetNullableInt(curRow.GetCell(1) == null ? "" : curRow.GetCell(1).ToString());

							data.VatChallanNo = curRow.GetCell(2) == null ? "" : curRow.GetCell(2).StringCellValue.Trim();
							data.VatChallanIssueDate = ExcelDataConverter.GetDatetime(curRow.GetCell(3) == null ? "" : curRow.GetCell(3).StringCellValue.Trim());
							data.VendorInvoiceNo = curRow.GetCell(4) == null ? "" : curRow.GetCell(4).StringCellValue.Trim();
							data.InvoiceNo = curRow.GetCell(5) == null ? "" : curRow.GetCell(5).StringCellValue.Trim();
							data.PurchaseDate = ExcelDataConverter.GetDatetime(curRow.GetCell(6) == null ? "" : curRow.GetCell(6).StringCellValue.Trim());
							data.PurchaseTypeId = Convert.ToInt32(curRow.GetCell(7) == null ? 0 : curRow.GetCell(7).ToString());
							data.PurchaseReasonId = Convert.ToInt32(curRow.GetCell(8) == null ? 0 : curRow.GetCell(8).ToString());
							data.DiscountOnTotalPrice = Convert.ToDecimal(curRow.GetCell(9) == null ? "0" : curRow.GetCell(9).StringCellValue.Trim());
							data.AdvanceTaxPaidAmount = ExcelDataConverter.GetNullableDecimal(curRow.GetCell(10) == null ? "" : curRow.GetCell(10).StringCellValue.Trim());
							data.Atpdate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(11) == null ? "" : curRow.GetCell(11).StringCellValue.Trim());
							data.AtpbankId = ExcelDataConverter.GetNullableInt(curRow.GetCell(12) == null ? "" : curRow.GetCell(12).StringCellValue);
							data.AtpbankBranchName = curRow.GetCell(13) == null ? "" : curRow.GetCell(13).StringCellValue.Trim();
							data.AtpnbrEconomicCodeId = ExcelDataConverter.GetNullableInt(curRow.GetCell(14) == null ? "" : curRow.GetCell(14).StringCellValue.Trim());
							data.AtpchallanNo = curRow.GetCell(15) == null ? "" : curRow.GetCell(15).StringCellValue.Trim();
							data.IsVatDeductedInSource = Convert.ToBoolean(curRow.GetCell(16) == null ? false : curRow.GetCell(16).ToString());
							data.Vdsamount = ExcelDataConverter.GetNullableDecimal(curRow.GetCell(17) == null ? "" : curRow.GetCell(17).StringCellValue.Trim());
							data.IsVdscertificatePrinted = Convert.ToBoolean(curRow.GetCell(18) == null ? false : curRow.GetCell(18).ToString());
							data.VdscertificateNo = curRow.GetCell(19) == null ? "" : curRow.GetCell(19).StringCellValue.Trim();
							data.VdscertificateDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(20) == null ? "" : curRow.GetCell(20).StringCellValue.Trim());
							data.VdspaymentBookTransferNo = curRow.GetCell(21) == null ? "" : curRow.GetCell(21).StringCellValue.Trim();
							data.Vdsnote = curRow.GetCell(22) == null ? "" : curRow.GetCell(22).StringCellValue.Trim();
							data.ExpectedDeliveryDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(23) == null ? "" : curRow.GetCell(23).StringCellValue.Trim());
							data.DeliveryDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(24) == null ? "" : curRow.GetCell(24).StringCellValue.Trim());
							data.LcNo = curRow.GetCell(25) == null ? "" : curRow.GetCell(25).StringCellValue.Trim();
							data.LcDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(26) == null ? "" : curRow.GetCell(26).StringCellValue.Trim());
							data.BillOfEntry = curRow.GetCell(27) == null ? "" : curRow.GetCell(27).StringCellValue.Trim();
							data.BillOfEntryDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(28) == null ? "" : curRow.GetCell(28).StringCellValue.Trim());
							data.CustomsAndVatcommissionarateId = ExcelDataConverter.GetNullableInt(curRow.GetCell(29) == null ? "" : curRow.GetCell(29).StringCellValue.Trim());
							data.DueDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(30) == null ? "" : curRow.GetCell(30).StringCellValue.Trim());
							data.TermsOfLc = curRow.GetCell(31) == null ? "" : curRow.GetCell(31).StringCellValue.Trim();
							data.PoNumber = curRow.GetCell(32) == null ? "" : curRow.GetCell(32).StringCellValue.Trim();
							data.IsComplete = Convert.ToBoolean(curRow.GetCell(33) == null ? false : curRow.GetCell(33).ToString());
							data.CreatedBy = ExcelDataConverter.GetNullableInt(curRow.GetCell(34) == null ? "" : curRow.GetCell(34).StringCellValue.Trim());
							data.CreatedTime = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(35) == null ? "" : curRow.GetCell(35).StringCellValue.Trim());

							purchaseList.Add(data);

						}

					}
					// BulkPurchase Sheet Number One End

					// BulkPurchase Sheet Number Two start
					List<PurchaseDetail> purchaseDetailsList = new List<PurchaseDetail>();
					ISheet sheetPurchaseDetails = workbook.GetSheetAt(1);
					if (sheetPurchaseDetails != null)
					{
						int rowCount = sheetPurchaseDetails.LastRowNum;
						for (int i = 1; i <= rowCount; i++)
						{
							var data = new PurchaseDetail();
							IRow curRow = sheetPurchaseDetails.GetRow(i);
							data.ReferenceKey = curRow.GetCell(0) == null ? "" : curRow.GetCell(0).ToString();
							data.PurchaseIdReference = curRow.GetCell(1) == null ? "0" : curRow.GetCell(1).ToString();

							data.ProductId = Convert.ToInt32(curRow.GetCell(2) == null ? "" : curRow.GetCell(2).ToString());
							data.ProductVattypeId = Convert.ToInt32(curRow.GetCell(3) == null ? "0" : curRow.GetCell(3).ToString());
							data.Quantity = Convert.ToDecimal(curRow.GetCell(4) == null ? "0" : curRow.GetCell(4).ToString());
							data.UnitPrice = Convert.ToDecimal(curRow.GetCell(5) == null ? "" : curRow.GetCell(5).ToString());
							data.DiscountPerItem = Convert.ToDecimal(curRow.GetCell(6) == null ? "0" : curRow.GetCell(6).ToString());
							data.CustomDutyPercent = Convert.ToDecimal(curRow.GetCell(7) == null ? "0" : curRow.GetCell(7).ToString());
							data.ImportDutyPercent = Convert.ToDecimal(curRow.GetCell(8) == null ? 0 : curRow.GetCell(8).ToString());
							data.RegulatoryDutyPercent = Convert.ToDecimal(curRow.GetCell(9) == null ? "0" : curRow.GetCell(9).ToString());
							data.SupplementaryDutyPercent = Convert.ToDecimal(curRow.GetCell(10) == null ? "0" : curRow.GetCell(10).ToString());
							data.Vatpercent = Convert.ToDecimal(curRow.GetCell(11) == null ? "0" : curRow.GetCell(11).ToString());
							data.AdvanceTaxPercent = Convert.ToDecimal(curRow.GetCell(12) == null ? "0" : curRow.GetCell(12).ToString());
							data.AdvanceIncomeTaxPercent = Convert.ToDecimal(curRow.GetCell(13) == null ? "0" : curRow.GetCell(13).ToString());
							data.MeasurementUnitId = Convert.ToInt32(curRow.GetCell(14) == null ? "0" : curRow.GetCell(14).ToString());

							purchaseDetailsList.Add(data);

						}

					}
					// BulkPurchase Sheet Number Two End

					// BulkPurchase Sheet Number Two start
					List<PurchasePayment> purchasePaymentList = new List<PurchasePayment>();
					ISheet sheetPurchasePayment = workbook.GetSheetAt(2);
					if (sheetPurchasePayment != null)
					{
						int rowCount = sheetPurchasePayment.LastRowNum;
						for (int i = 1; i <= rowCount; i++)
						{
							var data = new PurchasePayment();
							IRow curRow = sheetPurchasePayment.GetRow(i);
							data.ReferenceKey = curRow.GetCell(0) == null ? "" : curRow.GetCell(0).ToString();
							data.PurchaseIdReference = curRow.GetCell(1) == null ? "0" : curRow.GetCell(1).ToString();

							data.PaymentMethodId = Convert.ToInt32(curRow.GetCell(2) == null ? "0" : curRow.GetCell(2).ToString());
							data.PaidAmount = Convert.ToDecimal(curRow.GetCell(3) == null ? "0" : curRow.GetCell(3).ToString());
							data.PaymentDate = ExcelDataConverter.GetDatetime(curRow.GetCell(4) == null ? "" : curRow.GetCell(4).ToString());

							purchasePaymentList.Add(data);

						}

					}
					// BulkPurchase Sheet Number Two End
				}
				else
				{
					return RedirectToAction(nameof(Index));
				}

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return RedirectToAction(nameof(Index));
			}
		}

		return RedirectToAction(nameof(Index));
	}

	public async Task<ActionResult> FileDownload()
	{
		string sWebRootFolder = _hostingEnvironment.WebRootPath;
		string sFileName = @"Purchase.xlsx";
		string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
		FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
		var memory = new MemoryStream();
		using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
		{

			IWorkbook workbook;
			workbook = new XSSFWorkbook();
			ISheet excelSheet = workbook.CreateSheet("Purchase");

			ICellStyle style = workbook.CreateCellStyle();
			ICellStyle styleHeading = workbook.CreateCellStyle();
			IFont fontHeading = workbook.CreateFont();
			fontHeading.FontHeightInPoints = 14;
			styleHeading.Alignment = HorizontalAlignment.Center;
			styleHeading.VerticalAlignment = VerticalAlignment.Center;
			styleHeading.SetFont(fontHeading);
			//styleHeading.WrapText = true;

			style.Alignment = HorizontalAlignment.Center;
			style.VerticalAlignment = VerticalAlignment.Center;
			style.WrapText = true;

			IRow row = excelSheet.CreateRow(0);
			row.CreateCell(0).CellStyle = styleHeading;
			row.GetCell(0).SetCellValue("Purchase Type");
			row.CreateCell(1).CellStyle = styleHeading;
			row.GetCell(1).SetCellValue("Challan No.");
			row.CreateCell(2).CellStyle = style;
			row.GetCell(2).SetCellValue("Time");
			row.CreateCell(3).CellStyle = styleHeading;
			row.GetCell(3).SetCellValue("Invoice");
			row.CreateCell(4).CellStyle = styleHeading;
			row.GetCell(4).SetCellValue("Date");
			row.CreateCell(5).CellStyle = styleHeading;
			row.GetCell(5).SetCellValue("Vendor");
			row.CreateCell(6).CellStyle = styleHeading;
			row.GetCell(6).SetCellValue("Price(-VAT)");
			row.CreateCell(7).CellStyle = styleHeading;
			row.GetCell(7).SetCellValue("VAT");
			row.CreateCell(8).CellStyle = styleHeading;
			row.GetCell(8).SetCellValue("SD");
			row.CreateCell(9).CellStyle = styleHeading;
			row.GetCell(9).SetCellValue("VDS?");

			var Purchase = await _purchaseOrderService.GetPurchasesIncludingOtherTables(UserSession.OrganizationId);
			int rowCounter = 1;
			foreach (var purchase in Purchase)
			{
				row = excelSheet.CreateRow(rowCounter);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue(purchase.PurchaseType.Name);
				row.CreateCell(1).CellStyle = style;
				row.GetCell(1).SetCellValue(purchase.VatChallanNo);
				row.CreateCell(2).CellStyle = style;
				row.GetCell(2).SetCellValue(purchase.VatChallanIssueDate.ToString());
				row.CreateCell(3).CellStyle = style;
				row.GetCell(3).SetCellValue(purchase.InvoiceNo);
				row.CreateCell(4).CellStyle = style;
				row.GetCell(4).SetCellValue(purchase.PurchaseDate.ToShortDateString());
				row.CreateCell(5).CellStyle = style;
				row.GetCell(5).SetCellValue(purchase.Vendor.Name);
				row.CreateCell(6).CellStyle = style;
				row.GetCell(6).SetCellValue(purchase.TotalPriceWithoutVat.ToString());
				row.CreateCell(7).CellStyle = style;
				row.GetCell(7).SetCellValue(purchase.TotalVat.ToString());
				row.CreateCell(8).CellStyle = style;
				row.GetCell(8).SetCellValue(purchase.TotalSupplementaryDuty.ToString());
				row.CreateCell(9).CellStyle = style;
				row.GetCell(9).SetCellValue(purchase.IsVatDeductedInSource == true ? "Yes" : "No");
				rowCounter++;
			}

			for (int i = 0; i <= 9; i++)
			{
				excelSheet.AutoSizeColumn(i, true);
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

	[VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
	public async Task<JsonResult> CreateAsync(vmPurchase vm)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		bool status = false;

		if (vm.ContentInfoJson != null)
		{
			Content content;
			foreach (var contentInfo in vm.ContentInfoJson)
			{
				try
				{
					content = new Content();
					vm.ContentInfoJsonTest = new List<Content>();
					var File = contentInfo.UploadFile;
					var FileSaveFeedbackDto = await FileSaveAsync(File);
					content.FileUrl = FileSaveFeedbackDto.FileUrl;
					content.MimeType = FileSaveFeedbackDto.MimeType;
					content.DocumentTypeId = contentInfo.DocumentTypeId;
					vm.ContentInfoJsonTest.Add(content);
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}
		}

		if (vm.PurchaseOrderDetailList.Count > 0)
		{
			VmPurchase vmPurchase = new VmPurchase();
			vmPurchase.PurchaseOrderDetailList = vm.PurchaseOrderDetailList;
			vmPurchase.ContentInfoJson = vm.ContentInfoJsonTest;
			vmPurchase.PurchasePaymenJson = vm.PurchasePaymenJson;
			vmPurchase.OrganizationId = organizationId;
			vmPurchase.VendorId = vm.VendorId;
			vmPurchase.VatChallanNo = vm.VatChallanNo;
			vmPurchase.VatChallanIssueDate = vm.VatChallanIssueDate;
			vmPurchase.VendorInvoiceNo = vm.VendorInvoiceNo;
			vmPurchase.InvoiceNo = vm.InvoiceNo;
			vmPurchase.PurchaseTypeId = vm.PurchaseTypeId;
			vmPurchase.PurchaseReasonId = vm.PurchaseReasonId;
			vmPurchase.DiscountOnTotalPrice = vm.DiscountOnTotalPrice;
			vmPurchase.IsVatDeductedInSource = vm.IsVatDeductedInSource;
			vmPurchase.PaidAmount = vm.PaidAmount;
			vmPurchase.ExpectedDeliveryDate = vm.ExpectedDeliveryDate;
			vmPurchase.DeliveryDate = DateTime.Now;
			vmPurchase.LcNo = vm.LcNo;
			vmPurchase.LcDate = vm.LcDate;
			vmPurchase.BillOfEntry = vm.BillOfEntry;
			vmPurchase.BillOfEntryDate = vm.BillOfEntryDate;
			vmPurchase.DueDate = vm.DueDate;
			vmPurchase.TermsOfLc = vm.TermsOfLc;
			vmPurchase.PoNumber = vm.PoNumber;
			vmPurchase.MushakGenerationId = vm.MushakGenerationId;
			vmPurchase.IsComplete = true;
			vmPurchase.CreatedBy = createdBy;
			vmPurchase.CreatedTime = DateTime.Now;
			vmPurchase.VDSAmount = vm.VDSAmount;
			vmPurchase.AdvanceTaxPaidAmount = vm.AdvanceTaxPaidAmount;
			vmPurchase.ATPDate = vm.ATPDate;
			vmPurchase.ATPBankBranchId = vm.ATPBankBranchId;
			vmPurchase.ATPBankBranchName = vm.ATPBankBranchName;
			vmPurchase.ATPNbrEconomicCodeId = vm.ATPNbrEconomicCodeId;
			vmPurchase.ATPChallanNo = vm.ATPChallanNo;
			vmPurchase.CustomsAndVATCommissionarateId = vm.CustomsAndVATCommissionarateId;
			vmPurchase.ReferenceKey = vm.ReferenceKey;
			status = await _purchaseOrderService.InsertData(vmPurchase);
		}

		if (status == true)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		}
		else
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
		}

		return Json(status);
	}

	[VmsAuthorize(FeatureList.PURCHASE)]
	public async Task<IActionResult> CreateForeignOrder()
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		ViewData[ControllerStaticData.PURCHASE_REASON_ID] = new SelectList(await _purchaseReasonService.Query().SelectAsync(), ControllerStaticData.PURCHASE_REASON_ID, ControllerStaticData.REASON);
		ViewData[ControllerStaticData.VENDOR_ID] = new SelectList(await _vendorService.Query().Where(c => c.CreatedBy == createdBy).SelectAsync(), ControllerStaticData.VENDOR_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.PURCHASE_TYPE_ID] = new SelectList(await _purchaseTypeService.Query().Where(c => c.PurchaseTypeId == 2).SelectAsync(), ControllerStaticData.PURCHASE_TYPE_ID, ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(await _measurementUnitService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId).SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);
		return View();
	}

	public async Task<IActionResult> Cancel(int? id)
	{
		try
		{
			var purchaseData = await _purchaseOrderService.Query().SingleOrDefaultAsync(p => p.PurchaseId == id, CancellationToken.None);
			_purchaseOrderService.Update(purchaseData);
			await UnitOfWork.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
		}

		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<JsonResult> GetPurchaseDetails(int purchaseId)
	{
		var purchaseDetails = await _purchaseOdService.Query().Include(c => c.Product).Include(c => c.MeasurementUnit).Include(c => c.ProductVattype).Include(c => c.Purchase).Where(c => c.PurchaseId == purchaseId).SelectAsync(CancellationToken.None);
		return new JsonResult(purchaseDetails.Select(x => new
		{
			PurchaseDetailId = x.PurchaseDetailId,
			ProductId = x.ProductId,
			PurchaseId = x.PurchaseId,
			ProductVattypeId = x.ProductVattypeId,
			Iteams = x.Quantity,
			Amount = x.UnitPrice,
			Vat = x.Vatpercent,
			Unit = x.MeasurementUnit.Name,
			MeasurementUnitId = x.MeasurementUnitId,
			CreatedBy = x.CreatedBy,
			CreatedTime = x.CreatedTime,
			ProductName = x.Product.Name
		}).ToList());
	}

	public async Task<JsonResult> InvoiceKeyWordSearch(string filterText)
	{
		var product = await _purchaseOrderService.Query().Where(c => c.InvoiceNo.Contains(filterText) && c.OrganizationId == UserSession.OrganizationId).SelectAsync(CancellationToken.None);
		return new JsonResult(product.Select(x => new
		{
			Id = x.PurchaseId,
			InvoiceNo = x.InvoiceNo,
			UnitPrice = x.TotalPriceWithoutVat
		}).ToList());
	}

	public async Task<JsonResult> InvoiceKeyWordSearchByVDS(string filterText)
	{
		var product = await _purchaseOrderService.Query().Where(c => c.InvoiceNo.Contains(filterText) && c.IsVatDeductedInSource == true && c.OrganizationId == UserSession.OrganizationId).SelectAsync(CancellationToken.None);
		return new JsonResult(product.Select(x => new
		{
			Id = x.PurchaseId,
			InvoiceNo = x.InvoiceNo,
			UnitPrice = x.TotalPriceWithoutVat
		}).ToList());
	}

	public async Task<IActionResult> CancelAdd(int id)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		ViewData[ControllerStaticData.PURCHASE_REASON_ID] = new SelectList(await _purchaseReasonService.Query().SelectAsync(), ControllerStaticData.PURCHASE_REASON_ID, "Reason");
		ViewData[ControllerStaticData.VENDOR_ID] = new SelectList(await _vendorService.Query().Where(c => c.CreatedBy == createdBy).SelectAsync(), ControllerStaticData.VENDOR_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.PURCHASE_TYPE_ID] = new SelectList(await _purchaseTypeService.Query().SelectAsync(), ControllerStaticData.PURCHASE_TYPE_ID, ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(await _measurementUnitService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId).SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);

		var purchaseList = await _purchaseOrderService.Query()
			.SingleOrDefaultAsync(c => c.PurchaseId == id, CancellationToken.None);
		return View(purchaseList);
	}

	public IActionResult AccountsView()
	{
		return View();
	}


	public async Task<IActionResult> Details(string id)
	{
		var purchase = await _purchaseOrderService.GetPurchaseDetails(id);
		var purchaseDetails = await _purchaseOdService.GetPurchaseDetails(id);

		var getNotes = await _debitNoteService.Query().Include(c => c.Purchase).Where(c => c.Purchase.OrganizationId == UserSession.OrganizationId && c.PurchaseId == int.Parse(IvatDataProtector.Unprotect(id))).SelectAsync();
		var vm = new VmPurchaseDetail { PurchaseDetails = purchaseDetails, Purchase = purchase, DebitNotes = getNotes };
		return View(vm);
	}

	[VmsAuthorize(FeatureList.PURCHASE)]

	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_DUE_LIST_CAN_VIEW)]

	public async Task<IActionResult> PurchaseDue()
	{
		var purchaseDue = await _purchaseOrderService.GetPurchaseDueByOrgAndBranch(UserSession.OrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		purchaseDue = purchaseDue.OrderByDescending(x => x.PurchaseId);
		return View(purchaseDue);
	}

	[VmsAuthorize(FeatureList.PURCHASE)]

	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_DUE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_DUE_LIST_CAN_ADD_PAYMENT)]
	public async Task<IActionResult> PurchasePayment(string id)
	{
		var purchaseDetails = await _purchaseOrderService.GetPurchaseDetails(id);
		var payments = await _paymentMethodService.Query().SelectAsync();
		IEnumerable<CustomSelectListItem> paymentMethods = payments.Select(s => new CustomSelectListItem
		{
			Id = s.PaymentMethodId,
			Name = s.Name
		});
		int purchaseId = int.Parse(IvatDataProtector.Unprotect(id));
		VmDuePayment duePayment = new VmDuePayment
		{
			PurchaseId = purchaseId,
			PaymentMethodList = await _paymentMethodService.GetPaymentMethods(),
			PayableAmount = purchaseDetails.PayableAmount,
			PrevPaidAmount = purchaseDetails.PaidAmount,
			DueAmount = purchaseDetails.DueAmount,
			PaymentDate = DateTime.Now,
			PaymentDocumentOrTransDate = DateTime.Now,
			BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId)
		};

		return View(duePayment);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.PURCHASE)]

	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_DUE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_DUE_LIST_CAN_ADD_PAYMENT)]
	public async Task<IActionResult> PurchasePayment(VmDuePayment purchasePayment, string id)
	{
		int purchaseId = int.Parse(IvatDataProtector.Unprotect(id));
		var purchaseDetails = await _purchaseOrderService.Query().SingleOrDefaultAsync(p => p.PurchaseId == purchaseId, CancellationToken.None);

		var value = Convert.ToDecimal(purchasePayment.DueAmount);
		if (purchasePayment.PaidAmount <= Convert.ToDecimal(purchaseDetails.PayableAmount))
		{
			if (purchasePayment.PaidAmount <= Convert.ToDecimal(value))
			{
				//var totalPaidAmount = purchaseDetails.PaidAmount + purchasePayment.PaidAmount;
				vmPurchasePayment vmPurchasePayment = new vmPurchasePayment
				{
					PurchaseId = purchaseId,
					PaymentMethodId = purchasePayment.PaymentMethodId,
					PaidAmount = Convert.ToDecimal(purchasePayment.PaidAmount),
					CreatedBy = UserSession.UserId,
					BankId = purchasePayment.BankId,
					MobilePaymentWalletNo = purchasePayment.MobilePaymentWalletNo,
					PaymentDate = purchasePayment.PaymentDate,
					DocumentNoOrTransactionId = purchasePayment.DocumentNoOrTransactionId,
					PaymentDocumentOrTransDate = purchasePayment.PaymentDocumentOrTransDate,
					CreatedTime = DateTime.Now,
				};
				await _purchaseOrderService.ManagePurchaseDue(vmPurchasePayment);


				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;

				return RedirectToAction(ViewStaticData.PURCHASE_DUE, ControllerStaticData.PURCHASE);
			}
			else
			{
				ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.PaymentGreaterThanDue;
			}
		}
		else
		{
			ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.PURCHASE_DUE_PAID_MESSAGE;
		}

		var payments = await _paymentMethodService.Query().SelectAsync();
		IEnumerable<CustomSelectListItem> paymentMethods = payments.Select(s => new CustomSelectListItem
		{
			Id = s.PaymentMethodId,
			Name = s.Name
		});
		purchasePayment.PaymentMethodList = await _paymentMethodService.GetPaymentMethods(); ;
		return View(purchasePayment);
	}

	// move to puchase local

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
		vmPurchaseDebitNote.VehicleList = await _vType.GetVehicleTypeSelectList(UserSession.OrganizationId);
        ViewData["VType"] = new SelectList(await _vType.Query().SelectAsync(), "VehicleTypeId", "VehicleTypeName");

		return View(vmPurchaseDebitNote);
	}

    // move to puchase local

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

	// move to purchase local
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

	// move to purchase local
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

	[VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
	public async Task<IActionResult> Purchase()
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		var vendorData = await _vendorService.Query().SelectAsync(CancellationToken.None);
		ViewData[ControllerStaticData.PURCHASE_REASON_ID] = new SelectList(await _purchaseReasonService.Query().SelectAsync(), ControllerStaticData.PURCHASE_REASON_ID, ControllerStaticData.REASON);
		ViewData[ControllerStaticData.VENDOR_ID] = new SelectList(vendorData.Where(c => c.IsForeignVendor == false && c.OrganizationId == UserSession.OrganizationId), ControllerStaticData.VENDOR_ID, ViewStaticData.NAME);
		ViewData["VendorIdImport"] = new SelectList(vendorData.Where(c => c.IsForeignVendor == true && c.OrganizationId == UserSession.OrganizationId), ControllerStaticData.VENDOR_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.PURCHASE_TYPE_ID] = new SelectList(await _purchaseTypeService.Query().SelectAsync(), ControllerStaticData.PURCHASE_TYPE_ID, ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(await _measurementUnitService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId).SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.DOCUMENT_TYPE_ID] = new SelectList(await _documentType.Query().SelectAsync(), ControllerStaticData.DOCUMENT_TYPE_ID, "Name");
		ViewData[ControllerStaticData.PAYMENT_METHOD_ID] = new SelectList(await _paymentMethodService.Query().SelectAsync(), ControllerStaticData.PAYMENT_METHOD_ID, "Name");
		ViewData["ATPBankBranchId"] = new SelectList(await _bankService.Query().SelectAsync(), "BankId", "Name");
		ViewData["NbrEconomicCodeId"] = new SelectList(await _nbrEconomicCodeService.Query().SelectAsync(), "NbrEconomicCodeId", "EconomicTitle");
		var vatTypes = new List<SpGetVatType>
		{
			new SpGetVatType
			{
				ProductVATTypeId = 0,
				VatTypeName = ""
			}
		};
		var objVatTypes = await _autocompleteService.GetProductVatType(false, false, true, false, false);
		if (objVatTypes.Any())
		{
			vatTypes.AddRange(objVatTypes);
		}
		ViewData["VatType"] = new SelectList(vatTypes, "ProductVATTypeId", "VatTypeName");


		var objCustomVattypes = await _customsAndVatcommissionarateService.Query().SelectAsync(CancellationToken.None);

		ViewData["CustomsAndVatcommissionarateId"] = new SelectList(objCustomVattypes, "CustomsAndVatcommissionarateId", "Name");
		return View();
	}
	public async Task<IActionResult> Download(string filename)
	{
		if (filename == null)
			return Content("filename not present");

		var path = Path.Combine(
			Directory.GetCurrentDirectory(),
			"wwwroot", filename);

		var memory = new MemoryStream();
		using (var stream = new FileStream(path, FileMode.Open))
		{
			await stream.CopyToAsync(memory);
		}
		memory.Position = 0;
		return File(memory, GetContentType(path), Path.GetFileName(path));
	}
	private string GetContentType(string path)
	{
		var types = GetMimeTypes();
		var ext = Path.GetExtension(path).ToLowerInvariant();
		return types[ext];
	}
	private Dictionary<string, string> GetMimeTypes()
	{
		return new Dictionary<string, string>
		{
			{".txt", "text/plain"},
			{".pdf", "application/pdf"},
			{".doc", "application/vnd.ms-word"},
			{".docx", "application/vnd.ms-word"},
			{".xls", "application/vnd.ms-excel"},
			{".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
			{".png", "image/png"},
			{".jpg", "image/jpeg"},
			{".jpeg", "image/jpeg"},
			{".gif", "image/gif"},
			{".csv", "text/csv"}
		};
	}
	public async Task<IActionResult> TransferReceive()
	{
		ViewData[ControllerStaticData.DOCUMENT_TYPE_ID] = new SelectList(await _documentType.Query().SelectAsync(), ControllerStaticData.DOCUMENT_TYPE_ID, "Name");
		ViewData["InvoiceNo"] = new SelectList(await _saleService.Query().Where(c => c.OtherBranchOrganizationId == UserSession.OrganizationId && c.SalesTypeId == 3).SelectAsync(), "SalesId", "InvoiceNo");

		return View();
	}

	public async Task<IActionResult> TransferReceivePost(vmTransferReceive vm)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		bool status = false;

		if (vm.ContentInfoJson != null)
		{
			Content content;
			foreach (var contentInfo in vm.ContentInfoJson)
			{
				try
				{
					content = new Content();
					vm.ContentInfoJsonTest = new List<Content>();
					var File = contentInfo.UploadFile;
					var FileSaveFeedbackDto = await FileSaveAsync(File);
					content.FileUrl = FileSaveFeedbackDto.FileUrl;
					content.MimeType = FileSaveFeedbackDto.MimeType;
					content.DocumentTypeId = contentInfo.DocumentTypeId;
					vm.ContentInfoJsonTest.Add(content);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.InnerException);
				}
			}
		}

		if (vm.TransferSalesId > 0)
		{
			entity.viewModels.vmTransferReceive vmTransfer = new entity.viewModels.vmTransferReceive
			{
				ContentJson = vm.ContentInfoJsonTest,
				TransferSalesId = vm.TransferSalesId,
				OrganizationId = UserSession.OrganizationId,
				InvoiceNo = vm.InvoiceNo,
				PurchaseDate = DateTime.Now,
				PurchaseReasonId = 2,
				DeliveryDate = DateTime.Now,
				IsComplete = true,
				CreatedBy = UserSession.UserId,
				CreatedTime = DateTime.Now
			};

			status = await _purchaseOrderService.InsertTransferReceive(vmTransfer);
		}

		if (status == true)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		}
		else
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
		}

		return Json(status);
	}

	public async Task<JsonResult> PurchaseReceiveGetData(int saleId)
	{
		var organizationId = UserSession.OrganizationId;
		var data = await _salesDetailService.Query().Include(c => c.Product).Include(c => c.MeasurementUnit)
			.Where(c => c.SalesId == saleId).SelectAsync(CancellationToken.None);
		return new JsonResult(data.Select(c => new
		{
			Id = c.SalesDetailId,
			ProdName = c.Product.Name,
			ProductId = c.ProductId,
			ProductVATTypeId = c.ProductVattypeId,
			ProductTransactionBookId = c.ProductTransactionBookId,
			Quantity = c.Quantity,
			UnitPrice = c.UnitPrice,
			DiscountPerItem = c.DiscountPerItem,
			VATPercent = c.Vatpercent,
			SupplementaryDutyPercent = c.SupplementaryDutyPercent,
			MeasurementUnitId = c.MeasurementUnitId,
			UnitName = c.MeasurementUnit.Name
		}).ToList());
	}

	[VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_VDS_LIST_CAN_VIEW)]

	public async Task<IActionResult> VdsIndex()
	{
		var purchaseVdsList = await _purchaseService.GetVdsPurchaseListByOrganizationAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		return View(purchaseVdsList);
	}

	//[VmsAuthorize(FeatureList.PURCHASE)]
	//[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_VDS_LIST_CAN_VIEW)]

	//public async Task<IActionResult> VdsIndex(int? page, string Vendor = null, string search = null, bool? IsShowAll = null, DateTime? FromDate = null, DateTime? ToDate = null)
	//{
	//    var getPurchase = await _purchaseOrderService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId && c.IsVatDeductedInSource == true).Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.Vendor).OrderByDescending(c => c.PurchaseId).SelectAsync(CancellationToken.None);

	//    ViewBag.PageCount = getPurchase.Count();
	//    if (IsShowAll == false || IsShowAll == null)
	//    {
	//        getPurchase = getPurchase.Where(c => c.IsVdscertificatePrinted == false || c.IsVdscertificatePrinted == null);
	//    }

	//    if (IsShowAll == true)
	//    {
	//        getPurchase = getPurchase.Where(c => c.IsVatDeductedInSource == true);
	//    }

	//    if (Vendor != null)
	//    {
	//        getPurchase = getPurchase.Where(c => (c.Vendor != null && (c.Vendor.Name.ToLower().Contains(Vendor.Trim().ToLower()))));
	//        ViewData["Vendor"] = Vendor;
	//    }
	//    else
	//    {
	//        ViewData["Vendor"] = string.Empty;
	//    }
	//    if (search != null)
	//    {
	//        search = search.ToLower().Trim();
	//        getPurchase = getPurchase.Where(c => (c.VendorInvoiceNo != null && (c.VendorInvoiceNo.ToLower().Contains(search)))
	//                                             || c.InvoiceNo.ToLower().Contains(search)

	//        );
	//        ViewData[ViewStaticData.SEARCH_TEXT] = search;
	//    }
	//    else
	//    {
	//        ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
	//    }

	//    if (FromDate != null && ToDate != null)
	//    {
	//        getPurchase = getPurchase.Where(c => c.CreatedTime >= FromDate && c.CreatedTime <= ToDate.Value.AddDays(1));
	//        ViewData["FromDate"] = FromDate;
	//        ViewData["ToDate"] = ToDate;
	//    }
	//    else
	//    {
	//        ViewData["FromDate"] = string.Empty;
	//        ViewData["ToDate"] = string.Empty;
	//    }
	//    var pageNumber = page ?? 1;
	//    var listOfPurchase = getPurchase; //.ToPagedList(pageNumber, 10);
	//    listOfPurchase.OrderByDescending(x => x.InvoiceNo).ToList().ForEach(delegate (Purchase pur)
	//    {
	//        pur.EncryptedId = IvatDataProtector.Protect(pur.PurchaseId.ToString());
	//    });
	//    ViewData["IsShowAll"] = IsShowAll;
	//    return View(listOfPurchase);
	//}


	[VmsAuthorize(FeatureList.PURCHASE)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_VDS_LIST_CAN_VIEW)]

	public async Task<IActionResult> UploadSimplifiedPurchaseList()
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
	public async Task<IActionResult> UploadSimplifiedPurchase(VmSingleFileUpload model)
	{

		if (ModelState.IsValid)
		{
			var msg = ExcelFileValidationNew(model);
			if (!string.IsNullOrEmpty(msg))
			{
				TempData["ErrorMessage"] = msg;
			}
			else
			{
				try
				{
					var simplifiedRawPurchaseList = _excelService.ReadExcel<VmExcelSimplifiedPurchase>(model.UploadedFile);
					msg = ExcelFileValidationDupChk(simplifiedRawPurchaseList);
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

						await _excelSimplifiedPurchaseService.SaveSimplifiedPurchaseList(simplifiedRawPurchaseList,
							excelDataUpload.ExcelDataUploadId);
						await UnitOfWork.SaveChangesAsync();
						var process = await _purchaseService.ProcessUploadedSimplifiedPurchase(excelDataUpload.ExcelDataUploadId,
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

		return RedirectToAction(nameof(UploadSimplifiedPurchaseList));
	}


	// move to local purchase

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

    // move to local purchase

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


	/// <summary>
	/// created by:: Mohammed Moinul Hasan
	///Following method validate below validstions:
	///File type
	///File Size
	///Header Column Count
	///Header Seq
	///Blank File Validation
	/// </summary>
	/// 
	private string ExcelFileValidation(VmSingleFileUpload model)
	{
		//File type validation
		long FILE_SIZE = 20971520;
		//5242880-5mb/20971520-20mb
		string[] FILE_EXTENSION = new string[] { ".xls", ".xlsx" };
		int CELL_NUMBER = 41;

		string fileExtension = Path.GetExtension(model.UploadedFile.FileName).ToLower();

		if (!fileExtension.Equals(FILE_EXTENSION[0]) && !fileExtension.Equals(FILE_EXTENSION[1]))
		{
			return "Not a valid file. File extension must be *.xls or *.xlsx";
		}
		//File Size in byte

		else if (model.UploadedFile.Length > FILE_SIZE)
		{
			return "File size exceed";
		}
		//Header Column Count
		else
		{
			using var stream = model.UploadedFile.OpenReadStream();
			stream.Position = 0;
			var xssWorkbook = new XSSFWorkbook(stream);
			var sheet = xssWorkbook.GetSheetAt(0);
			var firstRow = sheet.GetRow(0);

			if (firstRow.Cells.Count != CELL_NUMBER)
			{
				return "Excel column missmatch";
			}

			//Header sequence
			VmExcelSimplifiedPurchase Obj = new VmExcelSimplifiedPurchase();
			int pocoRow = 0;
			foreach (var columnHeader in firstRow.Cells)
			{
				if (!columnHeader.StringCellValue.ToLower().Equals(Obj.GetType().GetProperties()[pocoRow++].Name.ToString().ToLower()))
					return "Column " + pocoRow + " or row sequence not matched";
			}
			//Row count
			if (sheet.LastRowNum == 0)//only header exist
				return "Blank excel file";
		}
		return "";
	}


	private string ExcelFileValidationNew(VmSingleFileUpload model)
	{
		//File type validation
		long FILE_SIZE = 20971520;
		//5242880-5mb/20971520-20mb
		string[] FILE_EXTENSION = { ".xls", ".xlsx" };

		int CELL_NUMBER = 41;

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

		var obj = new VmExcelSimplifiedPurchase();
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

	/// <summary>
	/// created by:: Mohammed Moinul Hasan
	/// 
	private string ExcelFileValidationDupChk(List<VmExcelSimplifiedPurchase> simplifiedPurchase)
	{
		ICollection<ValidationResult> results = null;

		var dupes = simplifiedPurchase.GroupBy(x => new { x.PurchaseId }).Where(x => x.Skip(1).Any()).ToArray();
		if (dupes.Any())
			return "Duplicate records found on excel";

		//Data Type and Length validation
		int i = 1;
		foreach (VmExcelSimplifiedPurchase model in simplifiedPurchase)
		{
			if (!Validate(model, out results))
				return "Excel row" + i++ + "--" + results.Select(o => o.ErrorMessage);
		}
		return "";
	}


	// move to local purchase
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

	/// <summary>
	/// created by:: Mohammed Moinul Hasan
	///
	static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
	{
		results = new List<ValidationResult>();

		return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
	}
}