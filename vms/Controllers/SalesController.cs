using Microsoft.AspNetCore.DataProtection;
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
using vms.entity.viewModels.ReportsViewModel;
using vms.Models;
using vms.utility.StaticData;
using vms.Utility;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using vms.utility;
using NPOI.SS.Util;
using vms.entity.Enums;
using vms.entity.StoredProcedureModel.HTMLMushak;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MathNet.Numerics;
using vms.entity.viewModels.SalesPriceAdjustment;
using vms.service.Services.TransactionService;
using vms.service.Services.UploadService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;
using vms.service.Services.PaymentService;
using vms.service.Services.MushakService;
using vms.service.Services.ProductService;

namespace vms.Controllers;

public class SalesController : ControllerBase
{
	private readonly ICreditNoteService _creditService;
	private readonly ISaleService _saleOrderService;
	private readonly ISalesDetailService _salesDetailService;
	private readonly ICustomerService _customerService;
	private readonly ISalesDeliveryTypeService _salesDeliveryTypeService;
	private readonly IExportTypeService _exportTypeService;
	private readonly ISaleOrderDetailService _saleODService;
	private readonly IOrganizationService _organizationService;
	private readonly IMeasurementUnitService _measurementUnitService;
	private readonly IPaymentMethodService _paymentMethodService;
	private readonly ISalesPaymentReceiveService _salesPaymentReceiveService;
	private readonly IDeliveryMethodService _deliveryMethodService;
	private readonly IDocumentTypeService _documentType;
	private readonly IProductVatTypeService _vatTypeService;

	private readonly IAutocompleteService _autocompleteService;

	//private int organizationId;
	private readonly ICountryService _countryService;
	private readonly IVehicleTypeService _vType;
	private readonly IBankService _bankService;
	private readonly IProductStoredProcedureService _productStoredProcedureService;
	private readonly IVehicleTypeService _vehicleTypeService;
	private readonly ISaleService _saleService;
	private readonly IMushakGenerationService _mushakGenerationService;
	private readonly IDamageTypeService _damageTypeService;
	private readonly IDamageService _damageService;
	private readonly IOrgBranchService _branchService;

	private readonly IVmsExcelService _excelService;
	private readonly IExcelDataUploadService _excelDataUploadService;

	private readonly IExcelSimplifiedSalseService _excelSimplifiedSimplifiedSalseService;
	private readonly IExcelSimplifiedLocalSaleCalculateByVatService _excelSimplifiedLocalSaleCalculateByVatService;

	private readonly IFileOperationService _fileOperationService;
	private readonly ISalesPriceAdjustmentService _salesPriceAdjustmentService;


	public SalesController(ICountryService countryService, IAutocompleteService autocompleteService,
		IProductVatTypeService vatTypeService, IDocumentTypeService documentType,
		IDeliveryMethodService deliveryMethodService, IOrganizationService organizationService,
		ControllerBaseParamModel controllerBaseParamModel, IExportTypeService exportTypeService,
		ISalesDeliveryTypeService salesDeliveryTypeService, ISaleService saleOrderService,
		ISaleOrderDetailService saleODService, IMeasurementUnitService measurementUnitService,
		ICustomerService customerService,
		ISalesDetailService salesDetailService, ICreditNoteService creditService,
		IPaymentMethodService paymentMethodService, ISalesPaymentReceiveService salesPaymentReceiveService
		, IVehicleTypeService vType,
		IBankService bankService,
		IProductStoredProcedureService productStoredProcedureService,
		IVehicleTypeService vehicleTypeService, ISaleService saleService,
		IMushakGenerationService mushakGenerationService
		, IDamageTypeService damageTypeService
		, IDamageService damageService, IOrgBranchService branchService, IVmsExcelService excelService,
		IExcelDataUploadService excelDataUploadService,
		IExcelSimplifiedSalseService excelSimplifiedSimplifiedSalseService, IFileOperationService fileOperationService,
		ISalesPriceAdjustmentService salesPriceAdjustmentService, 
		IExcelSimplifiedLocalSaleCalculateByVatService excelSimplifiedLocalSaleCalculateByVatService) : base(controllerBaseParamModel)
	{
		_vType = vType;
		_countryService = countryService;
		_autocompleteService = autocompleteService;
		_vatTypeService = vatTypeService;
		_documentType = documentType;
		_deliveryMethodService = deliveryMethodService;
		_creditService = creditService;
		_saleOrderService = saleOrderService;
		_saleODService = saleODService;
		_measurementUnitService = measurementUnitService;
		_customerService = customerService;
		_salesDetailService = salesDetailService;
		_salesDeliveryTypeService = salesDeliveryTypeService;
		_exportTypeService = exportTypeService;
		_organizationService = organizationService;
		_paymentMethodService = paymentMethodService;
		_salesPaymentReceiveService = salesPaymentReceiveService;
		_bankService = bankService;
		_productStoredProcedureService = productStoredProcedureService;
		_vehicleTypeService = vehicleTypeService;
		_saleService = saleService;
		_mushakGenerationService = mushakGenerationService;
		_damageTypeService = damageTypeService;
		_damageService = damageService;
		_branchService = branchService;
		_excelService = excelService;
		_excelDataUploadService = excelDataUploadService;
		_excelSimplifiedSimplifiedSalseService = excelSimplifiedSimplifiedSalseService;
		_fileOperationService = fileOperationService;
		_salesPriceAdjustmentService = salesPriceAdjustmentService;
		_excelSimplifiedLocalSaleCalculateByVatService = excelSimplifiedLocalSaleCalculateByVatService;
	}

	[SessionExpireFilter]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	public async Task<IActionResult> Index()
	{
		return View(await _saleService.GetSalesListByOrganizationByBranch(UserSession.ProtectedOrganizationId,
			UserSession.BranchIds, UserSession.IsRequireBranch));
	}

	[SessionExpireFilter]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_PRINT_MUSHAK_6_7)]
	public async Task<IActionResult> CreditNoteView(int? id, int? page, string search = null)
	{
		string txt = search;
		var getNotes = await _creditService.GetCreditNoteDataByOrganizationAndBranch(
			UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		var listOfSale = getNotes;
		return View(listOfSale);
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_PRINT_MUSHAK_6_7)]
	public async Task<IActionResult> CreditNoteViewById(int id)
	{
		try
		{
			var model = new vmCreditMushak();

			model.CreditMushakList = await _mushakGenerationService.Mushak6P7(id);
			model.CreditNoteId = id;
			if (model.CreditMushakList.Any())
			{
				return View(model);
			}

			return NotFound();
		}
		catch
		{
			return NotFound();
		}
	}

	public async Task<IActionResult> Mushok6P7ExportToExcel(vmCreditMushak data)
	{
		var model = new List<SpCreditMushak>();

		model = await _mushakGenerationService.Mushak6P7(data.CreditNoteId);
		if (model.Any())
		{
			var firstItem = model.First();

			string sWebRootFolder = Environment.WebRootPath;
			sWebRootFolder = Path.Combine(sWebRootFolder, "ExportExcel");
			if (!Directory.Exists(sWebRootFolder))
			{
				Directory.CreateDirectory(sWebRootFolder);
			}

			string sFileName = @"Mushak_6.7.xlsx";
			string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
			FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
			var memory = new MemoryStream();
			using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
			{
				IWorkbook workbook;
				workbook = new XSSFWorkbook();
				ISheet excelSheet = workbook.CreateSheet("Mushak6.7");
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
				row.GetCell(0).SetCellValue("ক্রেডিট নোট");
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
				row.CreateCell(0).SetCellValue("নাম : " + firstItem.CusName);
				excelSheet.AddMergedRegion(new CellRangeAddress(6, 6, 0, 2));
				row.CreateCell(3).SetCellValue("নাম : " + firstItem.OrgName);
				excelSheet.AddMergedRegion(new CellRangeAddress(6, 6, 3, 5));

				row = excelSheet.CreateRow(7);
				row.CreateCell(0).SetCellValue("বিআইএন : " + firstItem.CusBin);
				excelSheet.AddMergedRegion(new CellRangeAddress(7, 7, 0, 2));
				row.CreateCell(3).SetCellValue("বিআইএন : " + firstItem.OrgBin);
				excelSheet.AddMergedRegion(new CellRangeAddress(7, 7, 3, 5));

				row = excelSheet.CreateRow(8);
				row.CreateCell(0).SetCellValue("মূল চালান নম্বর : " + firstItem.SaleInvoice);
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 0, 2));
				row.CreateCell(3).SetCellValue("ক্রেডিট নোট নম্বর : " + firstItem.CreditNo);
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 3, 5));

				row = excelSheet.CreateRow(9);
				row.CreateCell(0).SetCellValue("মূল চালান ইস্যুর তারিখ : " +
				                               (firstItem.SaleDate == null
					                               ? ""
					                               : firstItem.SaleDate.Value.ToString("dd/MM/yyyy")));
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 0, 2));
				row.CreateCell(3).SetCellValue("ইস্যুর তারিখ : " +
				                               (firstItem.CrTime == null
					                               ? ""
					                               : firstItem.CrTime.Value.ToString("dd/MM/yyyy")));
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 3, 5));

				row = excelSheet.CreateRow(10);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(10, 10, 0, 2));
				row.CreateCell(3).SetCellValue("ইস্যুর সময় : " +
				                               (firstItem.CrTime == null
					                               ? ""
					                               : firstItem.CrTime.Value.ToString("hh:mm tt")));
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
					row.GetCell(3)
						.SetCellValue(item.ReturnQuantity == null ? "" : item.ReturnQuantity.Value.ToString());
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

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("ফেরতের কারণ : " + firstItem.ReasonOfReturn);
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("দায়িত্বপ্রাপ্ত ব্যাক্তির স্বাক্ষর : ");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("১) প্রতি একক পণ্য / সেবার মূসক ও সম্পূরক শুল্ক সহ মূল্য।");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("২) ফেরত প্রদানের জন্য কোন ধরনের কটন থাকিলে উহার পরিমাণ।");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 5));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("৩) মূসক ও সম্পূরক শুল্কের যোগফল।");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 5));

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

		return NotFound();
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	public async Task<IActionResult> Cancel(int? id)
	{
		try
		{
			var saleData = await _saleOrderService.Query()
				.SingleOrDefaultAsync(p => p.SalesId == id, CancellationToken.None);
			_saleOrderService.Update(saleData);
			await UnitOfWork.SaveChangesAsync();
		}

		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}

		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<JsonResult> GetSaleDetails(int saleId)
	{
		var saleDetailsList = await _saleODService.Query().Include(c => c.Product).Include(c => c.MeasurementUnit)
			.Include(c => c.Sales).Where(c => c.SalesId == saleId).SelectAsync(CancellationToken.None);

		return new JsonResult(saleDetailsList.Select(x => new
		{
			ProductId = x.ProductId,
			SalesId = x.SalesId,
			ProductVattypeId = x.ProductVattypeId,
			Iteams = x.Quantity,
			Amount = x.UnitPrice,
			Vat = x.Vatpercent,
			MeasurementUnitId = x.MeasurementUnitId,
			Unit = x.MeasurementUnit.Name,
			CreatedBy = x.CreatedBy,
			CreatedTime = x.CreatedTime,

			ProductName = x.Product.Name
		}).ToList());
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_ADD_CREDIT_NOTE)]
	public async Task<IActionResult> CreditNote(string id)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		var protectedOrganizationId = UserSession.ProtectedOrganizationId;
		var sale = await _saleOrderService.GetSaleData(id);
		var salesDetails = await _salesDetailService.GetAllSalesDetails(id);

		ViewData["VType"] = new SelectList(await _vType.Query().SelectAsync(), "VehicleTypeId", "VehicleTypeName");

		var vm = new VmSalesDetail { SalesDetails = salesDetails, Sale = sale };
		VmSalesCreditNote vmSalesCreditNote = new VmSalesCreditNote();
		vmSalesCreditNote.Sale = sale;
		vmSalesCreditNote.SalesDetails = salesDetails;
		//vmSalesCreditNote.OrgBranchList = await _branchService.GetOrgBranchByOrganization(protectedOrganizationId);
		vmSalesCreditNote.OrgBranchList = await _branchService.GetOrgBranchSelectListByUser(protectedOrganizationId,
			UserSession.BranchIds, UserSession.IsRequireBranch);
		return View(vmSalesCreditNote);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_ADD_CREDIT_NOTE)]
	public async Task<JsonResult> CreditNoteSave(VmSalesCreditNotePost vm)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		bool status = false;
		if (vm.vmSalesCreditNoteDetials.Count > 0)
		{
			vm.CreatedBy = createdBy;
			vm.CreatedTime = DateTime.Now;

			status = await _saleOrderService.InsertCreditNote(vm);
		}

		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		return Json(new { id = 1 });
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_ADD_CREDIT_NOTE)]
	public async Task<IActionResult> PriceChangeCreditNote(string id)
	{
		var sales = await _saleService.GetSaleData(id);
		var model = new SalesPriceAdjustmentCreditNoteViewModel();
		model.SalesDetails = await _salesDetailService.GetAllSalesDetails(id);
		model.SalesId = sales.SalesId;
		ViewData["VType"] = new SelectList(await _vType.Query().SelectAsync(), "VehicleTypeId", "VehicleTypeName");
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_ADD_CREDIT_NOTE)]
	public async Task<JsonResult> PriceChangeCreditNoteSave(SalesPriceAdjustmentCreditNotePostViwModel model)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		model.OrganizationId = organizationId;
		model.CreatedBy = createdBy;
		model.CreatedTime = DateTime.Now;
		var id = IvatDataProtector.Protect((await _salesPriceAdjustmentService.InsertCreditNoteAkaDecreasePrice(model))
			.ToString());
		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		return Json(new { id });
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_ADD_CREDIT_NOTE)]
	public async Task<IActionResult> PriceChangeDebitNote(string id)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		var sale = await _saleOrderService.GetSaleData(id);
		var salesDetails = await _salesDetailService.GetAllSalesDetails(id);
		ViewData["VType"] = new SelectList(await _vType.Query().SelectAsync(), "VehicleTypeId", "VehicleTypeName");

		var vm = new VmSalesDetail { SalesDetails = salesDetails, Sale = sale };
		VmSalesCreditNote vmSalesCreditNote = new VmSalesCreditNote();
		vmSalesCreditNote.Sale = sale;
		vmSalesCreditNote.SalesDetails = salesDetails;
		return View(vmSalesCreditNote);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_ADD_CREDIT_NOTE)]
	public async Task<JsonResult> PriceChangeDebitNoteSave(SalesPriceAdjustmentDebitNotePostViwModel model)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		model.OrganizationId = organizationId;
		model.CreatedBy = createdBy;
		model.CreatedTime = DateTime.Now;
		var id = IvatDataProtector.Protect((await _salesPriceAdjustmentService.InsertDebitNoteAkaIncreasePrice(model))
			.ToString());
		return Json(new { id = id });
	}

	public IActionResult SaleCancellations()
	{
		return View();
	}

	public IActionResult SaleCancellationsAccountsView()
	{
		return View();
	}

	public async Task<IActionResult> SaleOrder()
	{
		var saleList = await _saleOrderService.Query().Include(c => c.ExportType).SelectAsync();

		return View(saleList);
	}

	[VmsAuthorize(FeatureList.SALE)]
	public async Task<IActionResult> SaleForeign()
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;

		var customerList = new List<Customer>
		{
			new Customer
			{
				CustomerId = 0,
				Name = ViewStaticData.SELECT_OPTION
			}
		};
		var obj = await _customerService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId)
			.SelectAsync();
		if (obj.Any())
		{
			customerList.AddRange(obj);
		}

		ViewData[ViewStaticData.CUSTOMER_ID] =
			new SelectList(customerList, ViewStaticData.CUSTOMER_ID, ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(
			await _measurementUnitService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId)
				.SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);
		ViewData["DeliveryMethodId"] =
			new SelectList(await _deliveryMethodService.Query().SelectAsync(), "DeliveryMethodId", "Name");
		ViewData[ControllerStaticData.SALES_DELIVERY_TYPE_ID] = new SelectList(
			await _salesDeliveryTypeService.Query().SelectAsync(), ControllerStaticData.SALES_DELIVERY_TYPE_ID,
			ViewStaticData.NAME);
		ViewData[ControllerStaticData.EXPORT_TYPE_ID] = new SelectList(await _exportTypeService.Query().SelectAsync(),
			ControllerStaticData.EXPORT_TYPE_ID, ViewStaticData.DISPLAY_EXPORT_TYPE_NAME);
		ViewData[ControllerStaticData.DOCUMENT_TYPE_ID] = new SelectList(await _documentType.Query().SelectAsync(),
			ControllerStaticData.DOCUMENT_TYPE_ID, "Name");
		ViewData[ControllerStaticData.PAYMENT_METHOD_ID] = new SelectList(
			await _paymentMethodService.Query().SelectAsync(), ControllerStaticData.PAYMENT_METHOD_ID, "Name");
		return View();
	}

	[VmsAuthorize(FeatureList.SALE)]
	public async Task<IActionResult> Create()

	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;

		var customerList = new List<Customer>
		{
			new Customer
			{
				CustomerId = 0,
				Name = ViewStaticData.SELECT_OPTION
			}
		};
		var obj = await _customerService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId)
			.SelectAsync();
		if (obj.Any())
		{
			customerList.AddRange(obj);
		}

		ViewData[ViewStaticData.CUSTOMER_ID] =
			new SelectList(customerList, ViewStaticData.CUSTOMER_ID, ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(
			await _measurementUnitService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId)
				.SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.SALES_DELIVERY_TYPE_ID] = new SelectList(
			await _salesDeliveryTypeService.Query().SelectAsync(), ControllerStaticData.SALES_DELIVERY_TYPE_ID,
			ViewStaticData.NAME);
		ViewData[ControllerStaticData.EXPORT_TYPE_ID] = new SelectList(await _exportTypeService.Query().SelectAsync(),
			ControllerStaticData.EXPORT_TYPE_ID, ViewStaticData.DISPLAY_EXPORT_TYPE_NAME);
		ViewData["DeliveryMethodId"] =
			new SelectList(await _deliveryMethodService.Query().SelectAsync(), "DeliveryMethodId", "Name");
		ViewData[ControllerStaticData.DOCUMENT_TYPE_ID] = new SelectList(await _documentType.Query().SelectAsync(),
			ControllerStaticData.DOCUMENT_TYPE_ID, "Name");
		ViewData[ControllerStaticData.PAYMENT_METHOD_ID] = new SelectList(
			await _paymentMethodService.Query().SelectAsync(), ControllerStaticData.PAYMENT_METHOD_ID, "Name");
		return View();
	}

	public async Task<FileSaveFeedbackDto> FileSaveAsync(IFormFile File)
	{
		FileSaveFeedbackDto fdto = new FileSaveFeedbackDto();
		var FileExtenstion = Path.GetExtension(File.FileName);

		string FileName = Guid.NewGuid().ToString();

		FileName += FileExtenstion;
		Organization organization = await _organizationService.Query()
			.FirstOrDefaultAsync(c => c.OrganizationId == UserSession.OrganizationId, CancellationToken.None);
		var FolderName = ControllerStaticData.APPLICATION_DOCUMENT + organization.OrganizationId;
		var uploads = Path.Combine(Environment.WebRootPath, FolderName);

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

	public IActionResult FileUpload(vmSalesIndex requestData)
	{
		if (ModelState.IsValid)
		{
			try
			{
				/** This code should be moved to service as per needed. **/
				//IFormFile file = Request.Form.Files[0];
				IFormFile file = requestData.UploadedFile;
				string folderName = "UploadedExcel\\Sales";
				string rootPath = Environment.WebRootPath;
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

				// BulkSales Sheet Number One Start
				List<Sale> salesList = new List<Sale>();
				ISheet sheetSales = workbook.GetSheetAt(0);
				if (sheetSales != null)
				{
					int rowCount = sheetSales.LastRowNum;
					for (int i = 1; i <= rowCount; i++)
					{
						var data = new Sale();
						IRow curRow = sheetSales.GetRow(i);
						data.ReferenceKey = curRow.GetCell(0) == null ? "" : curRow.GetCell(0).StringCellValue.Trim();
						data.InvoiceNo = curRow.GetCell(1) == null ? "" : curRow.GetCell(1).ToString().Trim();

						data.VatChallanNo = curRow.GetCell(2) == null ? "" : curRow.GetCell(2).ToString().Trim();
						data.DiscountOnTotalPrice =
							Convert.ToDecimal(curRow.GetCell(3) == null ? "0" : curRow.GetCell(3).ToString().Trim());
						data.IsVatDeductedInSource =
							Convert.ToBoolean(curRow.GetCell(4) == null ? false : curRow.GetCell(4).ToString().Trim());
						data.Vdsamount = ExcelDataConverter.GetNullableDecimal(curRow.GetCell(5) == null
							? "0"
							: curRow.GetCell(5).ToString().Trim());
						data.IsVdscertificateReceived =
							Convert.ToBoolean(curRow.GetCell(6) == null ? false : curRow.GetCell(6).ToString().Trim());
						data.VdscertificateNo =
							curRow.GetCell(7) == null ? "" : curRow.GetCell(7).StringCellValue.Trim();
						data.VdscertificateIssueTime = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(8) == null
							? ""
							: curRow.GetCell(8).StringCellValue.Trim());
						data.VdspaymentBankId =
							ExcelDataConverter.GetNullableInt(curRow.GetCell(9) == null
								? ""
								: curRow.GetCell(9).ToString().Trim());
						data.VdspaymentDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(10) == null
							? ""
							: curRow.GetCell(10).StringCellValue.Trim());
						data.VdspaymentChallanNo =
							curRow.GetCell(11) == null ? "" : curRow.GetCell(11).StringCellValue.Trim();
						data.VdspaymentEconomicCode =
							curRow.GetCell(12) == null ? "" : curRow.GetCell(12).StringCellValue.Trim();
						data.VdspaymentBookTransferNo =
							curRow.GetCell(13) == null ? "" : curRow.GetCell(13).StringCellValue.Trim();
						data.Vdsnote = curRow.GetCell(14) == null ? "" : curRow.GetCell(14).StringCellValue.Trim();
						data.CustomerReferenceId =
							curRow.GetCell(15) == null ? "" : curRow.GetCell(15).StringCellValue.Trim();
						data.ReceiverName = curRow.GetCell(16) == null ? "" : curRow.GetCell(16).StringCellValue.Trim();
						data.ReceiverContactNo =
							curRow.GetCell(17) == null ? "" : curRow.GetCell(17).StringCellValue.Trim();
						data.ShippingAddress =
							curRow.GetCell(18) == null ? "" : curRow.GetCell(18).StringCellValue.Trim();
						data.ShippingCountryId = ExcelDataConverter.GetNullableInt(curRow.GetCell(19) == null
							? ""
							: curRow.GetCell(19).ToString
								().Trim());
						var salesId = curRow.GetCell(20) == null ? "0" : curRow.GetCell(20).ToString().Trim();
						data.SalesTypeId =
							Convert.ToInt32(curRow.GetCell(20) == null ? "0" : curRow.GetCell(20).ToString().Trim());
						data.SalesDeliveryTypeId =
							Convert.ToInt32(curRow.GetCell(21) == null ? "0" : curRow.GetCell(21).ToString().Trim());
						data.WorkOrderNo = curRow.GetCell(22) == null ? "" : curRow.GetCell(22).ToString();
						data.SalesDate = ExcelDataConverter.GetDatetime(curRow.GetCell(23) == null
							? ""
							: curRow.GetCell(23).ToString().Trim());
						data.ExpectedDeliveryDate =
							ExcelDataConverter.GetNullableDatetime(curRow.GetCell(24) == null
								? ""
								: curRow.GetCell(24).ToString().Trim());
						data.DeliveryDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(25) == null
							? ""
							: curRow.GetCell(25).ToString().Trim());
						data.DeliveryMethodId = ExcelDataConverter.GetNullableInt(curRow.GetCell(26) == null
							? ""
							: curRow.GetCell(26).ToString().Trim());
						data.ExportTypeId = ExcelDataConverter.GetNullableInt(curRow.GetCell(27) == null
							? ""
							: curRow.GetCell(27).ToString().Trim());
						data.LcNo = curRow.GetCell(28) == null ? "" : curRow.GetCell(28).StringCellValue.Trim();
						data.LcDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(29) == null
							? ""
							: curRow.GetCell(29).StringCellValue.Trim());
						data.BillOfEntry = curRow.GetCell(30) == null ? "" : curRow.GetCell(30).StringCellValue.Trim();
						data.BillOfEntryDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(31) == null
							? ""
							: curRow.GetCell(31).StringCellValue.Trim());
						data.DueDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(32) == null
							? ""
							: curRow.GetCell(32).StringCellValue.Trim());
						data.TermsOfLc = curRow.GetCell(33) == null ? "" : curRow.GetCell(33).StringCellValue.Trim();
						data.CustomerPoNumber =
							curRow.GetCell(34) == null ? "" : curRow.GetCell(34).StringCellValue.Trim();
						data.IsComplete = Convert.ToBoolean(curRow.GetCell(35) == null
							? false
							: curRow.GetCell(35).ToString().Trim());
						data.IsTaxInvoicePrined =
							Convert.ToBoolean(curRow.GetCell(36) == null
								? false
								: curRow.GetCell(36).ToString().Trim());
						data.TaxInvoicePrintedTime = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(37) == null
							? ""
							: curRow.GetCell(37).StringCellValue.Trim());
						data.CreatedBy = ExcelDataConverter.GetNullableInt(curRow.GetCell(38) == null
							? ""
							: curRow.GetCell(38).ToString().Trim());
						data.CreatedTime = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(39) == null
							? ""
							: curRow.GetCell(39).ToString().Trim());

						salesList.Add(data);
					}
				}
				// BulkSales Sheet Number One End

				// BulkSales Sheet Number Two start
				List<SalesDetail> salesDetailsList = new List<SalesDetail>();
				ISheet sheetSalesDetails = workbook.GetSheetAt(1);
				if (sheetSalesDetails != null)
				{
					int rowCount = sheetSalesDetails.LastRowNum;
					for (int i = 1; i <= rowCount; i++)
					{
						var data = new SalesDetail();
						IRow curRow = sheetSalesDetails.GetRow(i);
						data.ReferenceKey = curRow.GetCell(0) == null ? "" : curRow.GetCell(0).ToString().Trim();
						data.SalesIdReference =
							curRow.GetCell(1) == null ? "" : curRow.GetCell(1).StringCellValue.Trim();

						data.ProductId =
							Convert.ToInt32(curRow.GetCell(2) == null ? "0" : curRow.GetCell(2).ToString().Trim());
						data.ProductVattypeId =
							Convert.ToInt32(curRow.GetCell(3) == null ? "0" : curRow.GetCell(3).ToString().Trim());
						data.Quantity =
							Convert.ToDecimal(curRow.GetCell(4) == null ? "0" : curRow.GetCell(4).ToString().Trim());
						data.UnitPrice =
							Convert.ToDecimal(curRow.GetCell(5) == null ? "0" : curRow.GetCell(5).ToString().Trim());
						data.DiscountPerItem =
							Convert.ToDecimal(curRow.GetCell(6) == null ? "0" : curRow.GetCell(6).ToString().Trim());
						data.Vatpercent =
							Convert.ToDecimal(curRow.GetCell(7) == null ? "0" : curRow.GetCell(7).ToString().Trim());
						data.SupplementaryDutyPercent =
							Convert.ToDecimal(curRow.GetCell(8) == null ? "0" : curRow.GetCell(8).ToString().Trim());

						data.MeasurementUnitId =
							Convert.ToInt32(curRow.GetCell(9) == null ? "0" : curRow.GetCell(9).ToString().Trim());

						salesDetailsList.Add(data);
					}
				}
				// BulkSales Sheet Number Two End

				// BulkSales Sheet Number Three start
				List<SalesPaymentReceive> salesPaymentList = new List<SalesPaymentReceive>();
				ISheet sheetSalesPayment = workbook.GetSheetAt(2);
				if (sheetSalesPayment != null)
				{
					int rowCount = sheetSalesPayment.LastRowNum;
					for (int i = 1; i <= rowCount; i++)
					{
						var data = new SalesPaymentReceive();
						IRow curRow = sheetSalesPayment.GetRow(i);
						data.ReferenceKey = curRow.GetCell(0) == null ? "" : curRow.GetCell(0).ToString().Trim();
						data.SalesIdReference =
							curRow.GetCell(1) == null ? "0" : curRow.GetCell(1).StringCellValue.Trim();

						data.ReceivedPaymentMethodId =
							Convert.ToInt32(curRow.GetCell(2) == null ? "0" : curRow.GetCell(2).ToString().Trim());
						data.ReceiveAmount =
							Convert.ToDecimal(curRow.GetCell(3) == null ? "0" : curRow.GetCell(3).ToString());
						data.ReceiveDate =
							ExcelDataConverter.GetDatetime(curRow.GetCell(4) == null
								? ""
								: curRow.GetCell(4).ToString().Trim());

						salesPaymentList.Add(data);
					}
				}
				// BulkSales Sheet Number Three End

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
		string sWebRootFolder = Environment.WebRootPath;
		string sFileName = @"Sales.xlsx";
		string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
		FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
		var memory = new MemoryStream();
		using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
		{
			IWorkbook workbook;
			workbook = new XSSFWorkbook();
			ISheet excelSheet = workbook.CreateSheet("Sales");

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
			row.GetCell(0).SetCellValue("Invoice No.");
			excelSheet.DefaultRowHeightInPoints = 18;
			row.CreateCell(1).CellStyle = styleHeading;
			row.GetCell(1).SetCellValue("Sales Date");
			excelSheet.DefaultRowHeightInPoints = 18;
			row.CreateCell(2).CellStyle = styleHeading;
			row.GetCell(2).SetCellValue("Tax Invoice No.");
			excelSheet.DefaultRowHeightInPoints = 18;
			row.CreateCell(3).CellStyle = styleHeading;
			row.GetCell(3).SetCellValue("Tax Invoice Time");
			excelSheet.DefaultRowHeightInPoints = 18;
			row.CreateCell(4).CellStyle = styleHeading;
			row.GetCell(4).SetCellValue("Customer Name");
			excelSheet.DefaultRowHeightInPoints = 18;
			row.CreateCell(5).CellStyle = styleHeading;
			row.GetCell(5).SetCellValue("Price Without VAT");
			excelSheet.DefaultRowHeightInPoints = 18;
			row.CreateCell(6).CellStyle = styleHeading;
			row.GetCell(6).SetCellValue("VAT");
			excelSheet.DefaultRowHeightInPoints = 18;
			row.CreateCell(7).CellStyle = styleHeading;
			row.GetCell(7).SetCellValue("SD");
			excelSheet.DefaultRowHeightInPoints = 18;
			row.CreateCell(8).CellStyle = styleHeading;
			row.GetCell(8).SetCellValue("VDS?");
			excelSheet.DefaultRowHeightInPoints = 18;

			var Sales = await _saleOrderService.GetSalesDetails(UserSession.OrganizationId);
			int rowCounter = 1;
			foreach (var sale in Sales)
			{
				row = excelSheet.CreateRow(rowCounter);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue(sale.InvoiceNo);
				row.CreateCell(1).CellStyle = style;
				row.GetCell(1).SetCellValue(sale.SalesDate.ToString());
				row.CreateCell(2).CellStyle = style;
				row.GetCell(2).SetCellValue(sale.VatChallanNo);
				row.CreateCell(3).CellStyle = style;
				row.GetCell(3).SetCellValue(sale.TaxInvoicePrintedTime.ToString());
				row.CreateCell(4).CellStyle = style;
				row.GetCell(4).SetCellValue(sale.Customer.Name);
				row.CreateCell(5).CellStyle = style;
				row.GetCell(5).SetCellValue(sale.TotalPriceWithoutVat.ToString());
				row.CreateCell(6).CellStyle = style;
				row.GetCell(6).SetCellValue(sale.TotalVat.ToString());
				row.CreateCell(7).CellStyle = style;
				row.GetCell(7).SetCellValue(sale.TotalSupplimentaryDuty.ToString());
				row.CreateCell(8).CellStyle = style;
				row.GetCell(8).SetCellValue(sale.IsVatDeductedInSource == true ? "Yes" : "No");
				rowCounter++;
			}

			for (int i = 0; i <= 8; i++)
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

	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	public async Task<JsonResult> InvoiceKeyWordSearch(string filterText)
	{
		var product = await _saleOrderService.Query().Where(c => c.InvoiceNo.Contains(filterText))
			.SelectAsync(CancellationToken.None);
		return new JsonResult(product.Select(x => new
		{
			Id = x.SalesId,
			Name = x.InvoiceNo,
			UnitPrice = x.ReceivableAmount
		}).ToList());
	}

	public async Task<JsonResult> CustomerNameKewWordSearch(string filterText)
	{
		var product = await _saleOrderService.Query().Include(i => i.Customer)
			.Where(c => c.Customer.Name.Contains(filterText)).SelectAsync(CancellationToken.None);
		return new JsonResult(product.Select(x => new
		{
			Id = x.CustomerId,
			Name = x.Customer.Name,
			UnitPrice = x.ReceivableAmount
		}).ToList());
	}

	// move to local sale controller

	public async Task<IActionResult> Details(string id)
	{
		var sale = await _saleOrderService.GetSaleData(id);
		var salesDetails = await _salesDetailService.GetAllSalesDetails(id);
		var getNotes = await _creditService.Query().Include(c => c.Sales).Where(c =>
			c.Sales.OrganizationId == UserSession.OrganizationId &&
			c.SalesId == int.Parse(IvatDataProtector.Unprotect(id))).SelectAsync();
		var vm = new VmSalesDetail { SalesDetails = salesDetails, Sale = sale, CreditNotes = getNotes };
		return View(vm);
	}

	public async Task<IActionResult> CheckForApproval(string id)
	{
		var sale = await _saleOrderService.GetSaleData(id);
		var salesDetails = await _salesDetailService.GetAllSalesDetails(id);
		var approveOrRejectModel = new SaleApproveOrRejectViewModel
		{
			SalesId = sale.SalesId,
			EncSalesId = id
		};
		var vm = new SalesApproveOrRejectDisplayViewModel()
			{ SalesDetails = salesDetails, Sale = sale, ApproveOrRejectViewModel = approveOrRejectModel };
		return View(vm);
	}

	[HttpPost]
	public async Task<IActionResult> Approve(SaleApproveOrRejectViewModel model)
	{
		if (!ModelState.IsValid) return RedirectToAction(nameof(Index));
		model.UserId = UserSession.UserId;
		var sale = await _saleService.Approve(model);
		await UnitOfWork.SaveChangesAsync();
		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		return RedirectToAction(nameof(Index));
	}

	[HttpPost]
	public async Task<IActionResult> Reject(SaleApproveOrRejectViewModel model)
	{
		if (!ModelState.IsValid) return RedirectToAction(nameof(Index));
		model.UserId = UserSession.UserId;
		var sale = await _saleService.Reject(model);
		await UnitOfWork.SaveChangesAsync();
		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		return RedirectToAction(nameof(Index));
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW_DETAILS)]
	public async Task<IActionResult> GetSalesDetails(vmSalesDetails vm)
	{
		var data = await _saleOrderService.GetSalesDetailsAsync(vm);

		return Ok(data);
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_DUE_CAN_VIEW)]
	public async Task<IActionResult> SalesDue(int? page, string search = null)
	{
		var sales = await _saleService.GetSalesDueByOrganizationAndBranch(UserSession.ProtectedOrganizationId,
			UserSession.BranchIds, UserSession.IsRequireBranch);
		return View(sales);
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_DUE_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_DUE_CAN_ADD_SALES_RECEIVE)]
	public async Task<IActionResult> SalesPaymentReceive(string id)
	{
		var sale = await _saleOrderService.GetSaleData(id);

		int salesId = int.Parse(IvatDataProtector.Unprotect(id));
		VmDuePayment duePayment = new VmDuePayment
		{
			SalesId = salesId,
			PaymentMethodList = await _paymentMethodService.GetPaymentMethods(),
			PayableAmount = sale.ReceivableAmount,
			PrevPaidAmount = sale.PaymentReceiveAmount,
			DueAmount = sale.PaymentDueAmount,
			PaymentDate = DateTime.Now,
			PaymentDocumentOrTransDate = DateTime.Now,
			BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId)
		};

		return View(duePayment);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_DUE_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_DUE_CAN_ADD_SALES_RECEIVE)]
	public async Task<IActionResult> SalesPaymentReceive(VmDuePayment salesPayment, string id)
	{
		int salesId = int.Parse(IvatDataProtector.Unprotect(id));
		var sales = await _saleOrderService.Query()
			.SingleOrDefaultAsync(p => p.SalesId == salesId, CancellationToken.None);

		if (salesPayment.PaidAmount <= (sales.ReceivableAmount ?? 0))
		{
			if (salesPayment.PaidAmount <= Convert.ToDecimal(salesPayment.DueAmount ?? 0))
			{
				///var totalPaidAmount = sales.PaymentReceiveAmount + salesPayment.PaidAmount;
				VmSalesPaymentReceive vmSales = new VmSalesPaymentReceive
				{
					SalesId = salesId,
					PaymentMethodId = salesPayment.PaymentMethodId,
					PaidAmount = Convert.ToDecimal(salesPayment.PaidAmount),
					CreatedBy = UserSession.UserId,
					BankId = salesPayment.BankId,
					MobilePaymentWalletNo = salesPayment.MobilePaymentWalletNo,
					PaymentDate = salesPayment.PaymentDate,
					DocumentNoOrTransactionId = salesPayment.DocumentNoOrTransactionId,
					PaymentDocumentOrTransDate = salesPayment.PaymentDocumentOrTransDate,
					CreatedTime = DateTime.Now,
				};
				await _salesPaymentReceiveService.ManageSalesDueAsync(vmSales);
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
				return RedirectToAction(ViewStaticData.SALES_DUE, ControllerStaticData.SALES);
			}
			else
			{
				ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.PaymentGreaterThanDue;
			}
		}
		else
		{
			ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.SALES_DUE_PAID_MESSAGE;
		}

		var payments = await _paymentMethodService.Query().SelectAsync();
		IEnumerable<CustomSelectListItem> paymentMethods = payments.Select(s => new CustomSelectListItem
		{
			Id = s.PaymentMethodId,
			Name = s.Name
		});
		salesPayment.PaymentMethodList = await _paymentMethodService.GetPaymentMethods();

		return View(salesPayment);
	}

	[SessionExpireFilter]
	[VmsAuthorize(FeatureList.SALE)]
	public async Task<IActionResult> SaleTransfer()

	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;

		var customerList = new List<Customer>
		{
			new Customer
			{
				CustomerId = 0,
				Name = ViewStaticData.SELECT_OPTION
			}
		};
		var obj = await _customerService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId)
			.SelectAsync();
		if (obj.Any())
		{
			customerList.AddRange(obj);
		}

		var parentOrganization = await _organizationService.Query()
			.SingleOrDefaultAsync(c => c.OrganizationId == UserSession.OrganizationId, CancellationToken.None);
		ViewData["OtherBranchOrganizationId"] = new SelectList(
			await _organizationService.Query().Where(c =>
				c.ParentOrganizationId == UserSession.OrganizationId ||
				c.ParentOrganizationId == parentOrganization.ParentOrganizationId).SelectAsync(), "OrganizationId",
			ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(
			await _measurementUnitService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId)
				.SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.SALES_DELIVERY_TYPE_ID] = new SelectList(
			await _salesDeliveryTypeService.Query().SelectAsync(), ControllerStaticData.SALES_DELIVERY_TYPE_ID,
			ViewStaticData.NAME);
		ViewData[ControllerStaticData.EXPORT_TYPE_ID] = new SelectList(await _exportTypeService.Query().SelectAsync(),
			ControllerStaticData.EXPORT_TYPE_ID, ViewStaticData.DISPLAY_EXPORT_TYPE_NAME);
		ViewData["DeliveryMethodId"] =
			new SelectList(await _deliveryMethodService.Query().SelectAsync(), "DeliveryMethodId", "Name");
		ViewData[ControllerStaticData.DOCUMENT_TYPE_ID] = new SelectList(await _documentType.Query().SelectAsync(),
			ControllerStaticData.DOCUMENT_TYPE_ID, "Name");
		ViewData[ControllerStaticData.PAYMENT_METHOD_ID] = new SelectList(
			await _paymentMethodService.Query().SelectAsync(), ControllerStaticData.PAYMENT_METHOD_ID, "Name");
		return View();
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	public async Task<JsonResult> CreateAsync(vmSale vm)
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
					TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
				}
			}
		}

		if (vm.SalesDetailList.Count > 0)
		{
			vm.CreatedBy = createdBy;
			vm.OrganizationId = organizationId;
			vmSaleOrder sale = new vmSaleOrder();
			sale.InvoiceNo = vm.InvoiceNo;
			sale.VatChallanNo = vm.VatChallanNo;
			sale.OrganizationId = vm.OrganizationId;
			sale.DiscountOnTotalPrice = vm.DiscountOnTotalPrice;
			sale.IsVatDeductedInSource = vm.IsVatDeductedInSource;
			sale.CustomerId = vm.CustomerId;
			sale.ReceiverName = vm.ReceiverName;
			sale.ReceiverContactNo = vm.ReceiverContactNo;
			sale.ShippingAddress = vm.ShippingAddress;
			sale.ShippingCountryId = vm.ShippingCountryId;
			sale.SalesTypeId = vm.SalesTypeId;
			sale.SalesDeliveryTypeId = vm.SalesDeliveryTypeId;
			sale.WorkOrderNo = vm.WorkOrderNo;
			sale.SalesDate = DateTime.Now;
			sale.ExpectedDeliveryDate = vm.ExpectedDeliveryDate;
			sale.DeliveryDate = vm.DeliveryDate;
			sale.DeliveryMethodId = vm.DeliveryMethodId;
			sale.ExportTypeId = vm.ExportTypeId;
			sale.LcNo = vm.LcNo;
			sale.LcDate = vm.LcDate;
			sale.BillOfEntry = vm.BillOfEntry;
			sale.BillOfEntryDate = vm.BillOfEntryDate;
			sale.DueDate = vm.DueDate;
			sale.TermsOfLc = vm.TermsOfLc;
			sale.CustomerPoNumber = vm.CustomerPoNumber;
			sale.IsComplete = true;
			sale.IsTaxInvoicePrined = false;
			sale.CreatedBy = vm.CreatedBy;
			sale.VDSAmount = vm.VDSAmount;
			sale.CreatedTime = DateTime.Now;
			sale.SalesDetailList = vm.SalesDetailList;
			sale.SalesPaymentReceiveJson = vm.SalesPaymentReceiveJson;
			sale.ContentInfoJson = vm.ContentInfoJsonTest;
			sale.TaxInvoicePrintedTime = null;
			sale.ReferenceKey = vm.ReferenceKey;
			sale.VehicleTypeId = vm.VehicleTypeId;
			sale.VehicleName = vm.VehicleName;
			sale.VehicleRegNo = vm.VehicleRegNo;
			sale.VehicleDriverName = vm.VehicleDriverName;
			sale.VehicleDriverContactNo = vm.VehicleDriverContactNo;
			if (vm.SalesTypeId == 3)
			{
				sale.OtherBranchOrganizationId = vm.OtherBranchOrganizationId;
			}
			else
				sale.OtherBranchOrganizationId = null;

			status = await _saleOrderService.InsertData(sale);
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

	public async Task<JsonResult> SaleTransferPost(vmSale vm)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		bool status = false;
		if (vm.ContentInfoJson != null)
		{
			Content content;
			foreach (var contentInfo in vm.ContentInfoJson)
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
		}

		if (vm.SalesDetailList.Count > 0)
		{
			vm.CreatedBy = createdBy;
			vm.OrganizationId = organizationId;
			vmSaleOrder sale = new vmSaleOrder();
			sale.InvoiceNo = vm.InvoiceNo;
			sale.VatChallanNo = vm.VatChallanNo;
			sale.OrganizationId = vm.OrganizationId;
			sale.DiscountOnTotalPrice = vm.DiscountOnTotalPrice;
			sale.IsVatDeductedInSource = vm.IsVatDeductedInSource;
			sale.CustomerId = vm.CustomerId;
			sale.ReceiverName = vm.ReceiverName;
			sale.ReceiverContactNo = vm.ReceiverContactNo;
			sale.ShippingAddress = vm.ShippingAddress;
			sale.ShippingCountryId = vm.ShippingCountryId;
			sale.SalesTypeId = vm.SalesTypeId;
			sale.SalesDeliveryTypeId = vm.SalesDeliveryTypeId;
			sale.WorkOrderNo = vm.WorkOrderNo;
			sale.SalesDate = DateTime.Now;
			sale.ExpectedDeliveryDate = vm.ExpectedDeliveryDate;
			sale.DeliveryDate = vm.DeliveryDate;
			sale.DeliveryMethodId = vm.DeliveryMethodId;
			sale.ExportTypeId = vm.ExportTypeId;
			sale.LcNo = vm.LcNo;
			sale.LcDate = vm.LcDate;
			sale.BillOfEntry = vm.BillOfEntry;
			sale.BillOfEntryDate = vm.BillOfEntryDate;
			sale.DueDate = vm.DueDate;
			sale.TermsOfLc = vm.TermsOfLc;
			sale.CustomerPoNumber = vm.CustomerPoNumber;
			sale.IsComplete = true;
			sale.IsTaxInvoicePrined = false;
			sale.CreatedBy = vm.CreatedBy;
			sale.CreatedTime = DateTime.Now;
			sale.SalesDetailList = vm.SalesDetailList;
			sale.SalesPaymentReceiveJson = vm.SalesPaymentReceiveJson;
			sale.ContentInfoJson = vm.ContentInfoJsonTest;
			if (vm.SalesTypeId == 3)
			{
				sale.OtherBranchOrganizationId = vm.OtherBranchOrganizationId;
			}
			else
				sale.OtherBranchOrganizationId = null;

			status = await _saleOrderService.InsertData(sale);
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

	public async Task<JsonResult> InvoiceKeyWordSearchByBranch(string filterText)
	{
		var product = await _saleOrderService.Query().Where(c => c.InvoiceNo.Contains(filterText) && c.SalesTypeId == 3)
			.SelectAsync(CancellationToken.None);
		return new JsonResult(product.Select(x => new
		{
			Id = x.SalesId,
			InvoiceNo = x.InvoiceNo,
			UnitPrice = x.TotalPriceWithoutVat
		}).ToList());
	}

	public async Task<JsonResult> InvoiceKeyWordSearchByContract(string filterText)
	{
		var product = await _saleOrderService.Query()
			.Where(c => c.InvoiceNo.Contains(filterText) && c.SalesTypeId == 4 &&
			            c.OrganizationId == UserSession.OrganizationId).SelectAsync(CancellationToken.None);
		return new JsonResult(product.Select(x => new
		{
			Id = x.SalesId,
			InvoiceNo = x.InvoiceNo,
			UnitPrice = x.TotalPriceWithoutVat
		}).ToList());
	}

	public async Task<IActionResult> Sale()

	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;

		var customerList = new List<Customer>
		{
			new Customer
			{
				CustomerId = 0,
				Name = ""
			}
		};
		var customerListExport = new List<Customer>
		{
			new Customer
			{
				CustomerId = 0,
				Name = ""
			}
		};
		ViewData["VType"] = new SelectList(await _vType.Query().SelectAsync(), "VehicleTypeId", "VehicleTypeName");


		var vatTypes = new List<SpGetVatType>
		{
			new SpGetVatType
			{
				ProductVATTypeId = 0,
				VatTypeName = ""
			}
		};
		var objVatTypes = await _autocompleteService.GetProductVatType(true, false, false, false, false);
		if (objVatTypes.Any())
		{
			vatTypes.AddRange(objVatTypes);
		}

		var obj = await _customerService.Query().Where(c => c.OrganizationId == UserSession.OrganizationId)
			.SelectAsync();
		var objLocal = obj.Where(c => !c.IsForeignCustomer);
		var objExport = obj.Where(c => c.IsForeignCustomer);

		if (objLocal.Any())
		{
			customerList.AddRange(objLocal);
		}

		if (objExport.Any())
		{
			customerListExport.AddRange(objExport);
		}


		ViewData[ViewStaticData.CUSTOMER_ID] =
			new SelectList(customerList, ViewStaticData.CUSTOMER_ID, ViewStaticData.NAME);
		ViewData["CustomerIdExport"] =
			new SelectList(customerListExport, ViewStaticData.CUSTOMER_ID, ViewStaticData.NAME);
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(
			await _measurementUnitService.Query().Where(x => x.OrganizationId == UserSession.OrganizationId)
				.SelectAsync(), ViewStaticData.MEASUREMENT_UNIT_ID, ViewStaticData.NAME);
		ViewData[ControllerStaticData.SALES_DELIVERY_TYPE_ID] = new SelectList(
			await _salesDeliveryTypeService.Query().SelectAsync(), ControllerStaticData.SALES_DELIVERY_TYPE_ID,
			ViewStaticData.NAME);
		ViewData[ControllerStaticData.EXPORT_TYPE_ID] = new SelectList(await _exportTypeService.Query().SelectAsync(),
			ControllerStaticData.EXPORT_TYPE_ID, ViewStaticData.DISPLAY_EXPORT_TYPE_NAME);
		ViewData["DeliveryMethodId"] =
			new SelectList(await _deliveryMethodService.Query().SelectAsync(), "DeliveryMethodId", "Name");
		ViewData[ControllerStaticData.DOCUMENT_TYPE_ID] = new SelectList(await _documentType.Query().SelectAsync(),
			ControllerStaticData.DOCUMENT_TYPE_ID, "Name");
		ViewData[ControllerStaticData.PAYMENT_METHOD_ID] = new SelectList(
			await _paymentMethodService.Query().SelectAsync(), ControllerStaticData.PAYMENT_METHOD_ID, "Name");
		ViewData["VatType"] = new SelectList(vatTypes, "ProductVATTypeId", "VatTypeName");
		ViewData["ShippingCountryId"] =
			new SelectList(await _countryService.Query().SelectAsync(), "CountryId", "Name");
		return View();
	}

	// move to local sale controller
	[HttpGet]
	public async Task<IActionResult> SaleLocal()
	{
		var protectedOrganizationId = UserSession.ProtectedOrganizationId;
		var organizationId = UserSession.OrganizationId;
		var org = await _organizationService.GetOrganization(protectedOrganizationId);
		var model = new VmSaleLocal
		{
			OrganizationName = org.Name,
			OrganizationBin = org.Bin,
			OrganizationAddress = org.Address,
			VatResponsiblePersonName = org.VatResponsiblePersonName,
			VatResponsiblePersonDesignation = org.VatResponsiblePersonDesignation,
			DeliveryDate = DateTime.Today,
			CustomerList = await _customerService.GetCustomers(organizationId),
			DeliveryMethodSelectList = await _deliveryMethodService.GetDeliveryMethodSelectList(),
			ProductForSaleList =
				_productStoredProcedureService.GetProductForSale(organizationId, 0), //todo: Have to fix product
			MeasurementUnitSelectList =
				await _measurementUnitService.GetMeasurementUnitSelectList(protectedOrganizationId),
			ProductVatTypes = await _vatTypeService.GetLocalSaleProductVatTypes(),
			DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(protectedOrganizationId),
			PaymentMethodList = await _paymentMethodService.GetPaymentMethods(),
			PaymentDate = DateTime.Now,
			PaymentDocumentOrTransDate = DateTime.Now,
			BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId),
			VehicleTypesList = await _vehicleTypeService.GetVehicleTypes(organizationId),
			//OrgBranchList = await _branchService.GetOrgBranchByOrganization(protectedOrganizationId)
			OrgBranchList = await _branchService.GetOrgBranchSelectListByUser(protectedOrganizationId,
				UserSession.BranchIds, UserSession.IsRequireBranch)
		};
		return View(model);
	}

	// move to local sale controller
	[HttpGet]
	public async Task<IActionResult> SaleLocalSimplified()
	{
		var organizationId = UserSession.ProtectedOrganizationId;
		var model = new VmSaleLocalSimplified
		{
			DeliveryDate = DateTime.Today,
			CustomerList = await _customerService.GetCustomers(UserSession.OrganizationId),
			DeliveryMethodSelectList = await _deliveryMethodService.GetDeliveryMethodSelectList(),
			ProductForSaleList =
				_productStoredProcedureService.GetProductForSale(UserSession.OrganizationId,
					0), //todo: Have to fix product
			MeasurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(organizationId),
			ProductVatTypes = await _vatTypeService.GetLocalSaleProductVatTypes(),
			DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(organizationId),
			PaymentMethodList = await _paymentMethodService.GetPaymentMethods(),
			PaymentDate = DateTime.Now,
			PaymentDocumentOrTransDate = DateTime.Now,
			BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId),
			VehicleTypesList = await _vehicleTypeService.GetVehicleTypes(UserSession.OrganizationId),
			OrgBranchList = await _branchService.GetOrgBranchSelectList(organizationId),
			IsImposeServiceCharge = UserSession.IsImposeServiceCharge,
			DefaultServiceChargePercent = UserSession.ServiceChargePercent,
			ServiceChargePercent = UserSession.ServiceChargePercent,
		};
		return View(model);
	}


	public async Task<IActionResult> Damage(string id)
	{
		var salesDetails = await _salesDetailService.GetSalesDetails(id);
		var damageDetails = salesDetails.Select(s => new DamageDetail
		{
			DamageDetailId = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.DamageDetailId).FirstOrDefault()),
			DamageDescription = s.DamageDetails?.Select(s1 => s1.DamageDescription).FirstOrDefault(),
			DamageQty = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.DamageQty).FirstOrDefault()),
			SuggestedNewUnitPrice =
				Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.SuggestedNewUnitPrice).FirstOrDefault()),
			ApprovedNewUnitPrice =
				Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.ApprovedNewUnitPrice).FirstOrDefault()),
			ApprovedUsableQty = Convert.ToInt32(s.DamageDetails?.Select(s1 => s1.ApprovedUsableQty).FirstOrDefault()),
			MeasurementUnitId = Convert.ToInt32(s.Product.MeasurementUnitId),
			UsablePercent = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.UsablePercent).FirstOrDefault()),
			UsableQty = Convert.ToDecimal(s.DamageDetails?.Select(s1 => s1.DamageDetailId).FirstOrDefault()),
			Product = s.Product,
			ProductId = s.Product.ProductId,
			SalesDetailId = s.SalesDetailId,
			SalesDetail = salesDetails.FirstOrDefault()
		}).ToList();

		Damage damage = new Damage();
		VmSalesDamage vm = new VmSalesDamage();
		vm.DamageDetails = damageDetails;
		vm.Damage = damage;
		vm.SalesId = salesDetails.Select(s => s.SalesId).FirstOrDefault();
		ViewData["damageType"] = new SelectList(await _damageTypeService.Query().SelectAsync(), "DamageTypeId", "Name");
		return View("DamageNew", vm);
	}

	[HttpPost]
	public async Task<JsonResult> Damage(VmSalesDamagePost vm)
	{
		_damageService.InsertSalesDamage(vm, UserSession);
		await UnitOfWork.SaveChangesAsync();
		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		return Json(new { id = 1 });
	}

	// move to local sale controller
	[HttpPost]
	public async Task<JsonResult> SaleLocal(VmSaleLocalPost model)
	{
		//var saleBreakDowns = _excelService.ReadExcel<VmSaleLocalBreakDown>(model.FileSalesBreakDown);
		//if (saleBreakDowns.Sum(s => s.Amount) != model.Products.Sum(p => p.TotalPrice))
		//{
		//	throw new Exception("Data do not match with breakdown!");
		//}

		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		model.OrganizationId = organizationId;
		model.CreatedBy = createdBy;
		model.SalesTypeId = 1;
		model.SalesDate = DateTime.Now;
		model.IsTaxInvoicePrined = false;
		//model.SaleBreakDowns = saleBreakDowns;
		var id = IvatDataProtector.Protect((await _saleService.InsertLocalSale(model)).ToString());
		await UnitOfWork.SaveChangesAsync();

		// try
		// {
		//     MailMaster.SendEmailWithAttachment("osmanibits@gmail.com", "Test Mail", EmailTemplate.GetCreditLimitReminder(model.ReceiverName, 1000, "456"));
		// }
		// catch (Exception e)
		// {
		//  Console.WriteLine(e);
		//  // throw;
		// }
		return Json(new { id });
	}

	#region SaleLocalWithBreakdownForSpecialClient

	[HttpGet]
	public async Task<IActionResult> SaleLocalWithBreakdown()
	{
		var protectedOrganizationId = UserSession.ProtectedOrganizationId;
		var organizationId = UserSession.OrganizationId;
		var org = await _organizationService.GetOrganization(protectedOrganizationId);
		var model = new VmSaleLocalWithBreakdown
		{
			OrganizationName = org.Name,
			OrganizationBin = org.Bin,
			OrganizationAddress = org.Address,
			VatResponsiblePersonName = org.VatResponsiblePersonName,
			VatResponsiblePersonDesignation = org.VatResponsiblePersonDesignation,
			DeliveryDate = DateTime.Today,
			CustomerList = await _customerService.GetCustomers(organizationId),
			DeliveryMethodSelectList = await _deliveryMethodService.GetDeliveryMethodSelectList(),
			ProductForSaleList =
				_productStoredProcedureService.GetProductForSale(organizationId, 0), //todo: Have to fix product
			MeasurementUnitSelectList =
				await _measurementUnitService.GetMeasurementUnitSelectList(protectedOrganizationId),
			ProductVatTypes = await _vatTypeService.GetLocalSaleProductVatTypes(),
			DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(protectedOrganizationId),
			PaymentMethodList = await _paymentMethodService.GetPaymentMethods(),
			PaymentDate = DateTime.Now,
			PaymentDocumentOrTransDate = DateTime.Now,
			BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId),
			VehicleTypesList = await _vehicleTypeService.GetVehicleTypes(organizationId),
			OrgBranchList = await _branchService.GetOrgBranchByOrganization(protectedOrganizationId)
		};
		return View(model);
	}

	[HttpPost]
	public async Task<JsonResult> SaleLocalWithBreakdown(VmSaleLocalPostWithBreakdown model)
	{
		var saleBreakDowns = _excelService.ReadExcel<VmSaleLocalBreakDown>(model.FileSalesBreakDown);
		var breakdownSum = saleBreakDowns.Sum(s => s.Amount).Round(4);
		var totalPrice = model.Products.Sum(p => p.TotalPrice).Round(4);
		var priceDeviation = breakdownSum - totalPrice;
		if (priceDeviation > (decimal)0.0001 || priceDeviation < (decimal)-0.0001)
		{
			throw new Exception("Data do not match with breakdown!");
		}

		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		model.OrganizationId = organizationId;
		model.CreatedBy = createdBy;
		model.SalesTypeId = 1;
		model.SalesDate = DateTime.Now;
		model.IsTaxInvoicePrined = false;
		model.SaleBreakDowns = saleBreakDowns;
		var id = IvatDataProtector.Protect((await _saleService.InsertLocalSaleWithBreakdown(model)).ToString());
		await UnitOfWork.SaveChangesAsync();

		// try
		// {
		//     MailMaster.SendEmailWithAttachment("osmanibits@gmail.com", "Test Mail", EmailTemplate.GetCreditLimitReminder(model.ReceiverName, 1000, "456"));
		// }
		// catch (Exception e)
		// {
		//  Console.WriteLine(e);
		//  // throw;
		// }
		return Json(new { id });
	}

	#endregion

	[HttpPost]
	public async Task<JsonResult> SaleLocalDraft(VmSaleLocalPost model)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		model.OrganizationId = organizationId;
		model.CreatedBy = createdBy;
		model.SalesTypeId = 1;
		model.SalesDate = DateTime.Now;
		model.IsTaxInvoicePrined = false;
		string id = IvatDataProtector.Protect((await _saleService.InsertLocalSaleDraft(model)).ToString());
		await UnitOfWork.SaveChangesAsync();
		return Json(new { id = id });
	}

	// move to Sales Foreign
	[HttpGet]
	public async Task<IActionResult> SaleExport()
	{
		var organizationId = UserSession.ProtectedOrganizationId;
		var model = new VmSaleExport();
		model.DeliveryDate = DateTime.Today;
		//model.CustomerSelectList = await _customerService.GetForeignCustomersSelectList(organizationId);
		model.CustomerList =
			(await _customerService.GetCustomers(UserSession.OrganizationId)).Where(c => c.IsForeignCustomer);
		model.DeliveryMethodSelectList = await _deliveryMethodService.GetDeliveryMethodSelectList();
		model.CountrySelectList = await _countryService.CountrySelectList();
		model.ProductForSaleList =
			_productStoredProcedureService.GetProductForSale(UserSession.OrganizationId, 0); //todo: Have to fix product
		model.MeasurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(organizationId);
		model.DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(organizationId);
		model.ExportTypeSelectList = await _exportTypeService.GetExportTypesSelectList();
		model.PaymentMethodList = await _paymentMethodService.GetPaymentMethods();
		model.BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
		model.VehicleTypesList = await _vehicleTypeService.GetVehicleTypes(UserSession.OrganizationId);
		model.PaymentDate = DateTime.Now;
		model.PaymentDocumentOrTransDate = DateTime.Now;
		model.OrgBranchList = await _branchService.GetOrgBranchSelectList(organizationId);
		return View(model);
	}


	[HttpGet]
	public async Task<IActionResult> SaleToTransfer()
	{
		var organizationId = UserSession.ProtectedOrganizationId;
		var model = new VmSaleTransfer();
		model.DeliveryDate = DateTime.Today;
		//model.CustomerSelectList = await _customerService.GetForeignCustomersSelectList(organizationId);
		model.DeliveryMethodSelectList = await _deliveryMethodService.GetDeliveryMethodSelectList();
		//model.CountrySelectList = await _countryService.GetCountrySelectList();
		model.ProductForSaleList =
			_productStoredProcedureService.GetProductForSale(UserSession.OrganizationId, 0); //todo: Have to fix product
		model.MeasurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(organizationId);
		model.DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(organizationId);
		//model.ExportTypeSelectList = await _exportTypeService.GetExportTypesSelectList();
		//model.PaymentMethodList = await _paymentMethodService.GetPaymentMethods();
		// model.BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
		model.VehicleTypesList = await _vehicleTypeService.GetVehicleTypes(UserSession.OrganizationId);
		//model.PaymentDate = DateTime.Now;
		//model.PaymentDocumentOrTransDate = DateTime.Now;
		model.OrgBranchList = await _branchService.GetOrgBranchSelectList(organizationId);
		return View(model);
	}


	public async Task<JsonResult> GetOrgBranchWithOutSelf(int id)
	{
		var Branc = await _branchService.GetOrgBranchWithOutSelf(UserSession.ProtectedOrganizationId, id);
		return Json(Branc);
	}


	public async Task<JsonResult> GetCustomerByExportType(int id)
	{
		var customer = await _customerService.GetCustomerByExportType(UserSession.OrganizationId, id);
		return Json(customer);
	}


	[HttpPost]
	public ActionResult SaleToTransfer(VmSaleTransferPost model)
	{
		return Json(new { id = 1 });
	}

	// move to Sales Foreign
	[HttpPost]
	public async Task<IActionResult> SaleExport(VmSaleExportPost model)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		model.OrganizationId = organizationId;
		model.CreatedBy = createdBy;
		model.SalesTypeId = 2;
		model.SalesDate = DateTime.Now;
		model.IsTaxInvoicePrined = false;
		string id = IvatDataProtector.Protect((await _saleService.InsertLocalSaleExport(model)).ToString());
		await UnitOfWork.SaveChangesAsync();
		return Json(new { id = id });
	}

	[HttpPost]
	public async Task<IActionResult> SaleExportDraft(VmSaleExportPost model)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		model.OrganizationId = organizationId;
		model.CreatedBy = createdBy;
		model.SalesTypeId = 2;
		model.SalesDate = DateTime.Now;
		model.IsTaxInvoicePrined = false;
		string id = IvatDataProtector.Protect((await _saleService.InsertLocalSaleExportDraft(model)).ToString());
		await UnitOfWork.SaveChangesAsync();
		return Json(new { id = id });
	}


	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	public async Task<IActionResult> SaleVds()
	{
		var getSales = await _saleOrderService.Query()
			.Where(c => c.OrganizationId == UserSession.OrganizationId && c.IsVatDeductedInSource)
			.Include(p => p.Customer)
			.Include(c => c.CreditNotes)
			.Include(p => p.Organization)
			.OrderByDescending(c => c.SalesId).SelectAsync();
		if (UserSession.IsRequireBranch)
		{
			getSales = getSales.Where(s => UserSession.BranchIds.Contains(s.OrgBranchId.Value)).ToList();
		}

		getSales.ToList().ForEach(delegate(Sale sale)
		{
			sale.EncryptedId = IvatDataProtector.Protect(sale.SalesId.ToString());
		});
		return View(getSales);
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_ADD_VDS_RECEIVE)]
	public async Task<IActionResult> ConvertToVds(string id)
	{
		var saleToUpdate = await _saleService.GetSaleData(id);

		if (saleToUpdate == null)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.NotFount;
			return RedirectToAction("Index");
		}

		if (saleToUpdate.IsVatDeductedInSource)
		{
			TempData[ControllerStaticData.MESSAGE] = "Already VAT Deducted at Source";
			return RedirectToAction("Index");
		}

		if (saleToUpdate.SalesTypeId != (int)EnumSalesType.SalesTypeLocal)
		{
			TempData[ControllerStaticData.MESSAGE] = "Not compatible to VAT Deducted at Source";
			return RedirectToAction("Index");
		}

		var sale = new VmConvertToVds
		{
			SalesId = int.Parse(IvatDataProtector.Unprotect(id)),
			EncryptedId = id,
			CustomerName = saleToUpdate.Customer.Name,
			InvoiceNo = saleToUpdate.InvoiceNo,
			VatChallanNo = saleToUpdate.VatChallanNo,
			VatChallanTime = StringGenerator.DateTimeToStringWithoutTime(saleToUpdate.TaxInvoicePrintedTime),
			TotalPriceWithoutVat = saleToUpdate.TotalPriceWithoutVat,
			SalesDate = saleToUpdate.SalesDate,
			Vdsamount = saleToUpdate.TotalVat,
			TotalVat = saleToUpdate.TotalVat,
			VdsDate = DateTime.Now
		};
		return View(sale);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_ADD_VDS_RECEIVE)]
	public async Task<IActionResult> ConvertToVds(VmConvertToVds sale)
	{
		if (!ModelState.IsValid)
		{
			return View(sale);
		}


		var saleToUpdate = await _saleService.GetSaleData(sale.EncryptedId);

		if (saleToUpdate == null)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.NotFount;
			return RedirectToAction("Index");
		}


		saleToUpdate.IsVatDeductedInSource = true;
		saleToUpdate.Vdsamount = sale.Vdsamount;
		saleToUpdate.Vdsdate = sale.VdsDate;
		_saleOrderService.Update(saleToUpdate);
		try
		{
			await UnitOfWork.SaveChangesAsync(CancellationToken.None);
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
			return RedirectToAction("SaleVds");
		}
		catch (Exception ex)
		{
			TempData[ControllerStaticData.MESSAGE] = ex.Message;
			return RedirectToAction("Index");
		}
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_ADD_VDS_RECEIVE)]
	public async Task<IActionResult> SaleVdsReceive(string id)
	{
		var saleToUpdate = await _saleService.GetSaleData(id);

		if (saleToUpdate == null)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.NotFount;
			return RedirectToAction("SaleVds");
		}

		var sale = new vmSaleVdsReceive
		{
			SalesId = int.Parse(IvatDataProtector.Unprotect(id)),
			EncryptedId = id,
			SalesDate = saleToUpdate.SalesDate,
			BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId)
		};
		return View(sale);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_ADD_VDS_RECEIVE)]
	public async Task<IActionResult> SaleVdsReceive(vmSaleVdsReceive sale)
	{
		if (!ModelState.IsValid)
		{
			sale.BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
			return View(sale);
		}


		var saleToUpdate = await _saleService.GetSaleData(sale.EncryptedId);

		if (saleToUpdate == null)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.NotFount;
			return RedirectToAction("SaleVds");
		}

		var orgfolder = Environment.WebRootPath + "/ApplicationDocument/" + UserSession.OrganizationId;
		var fileLocation = Path.Combine(orgfolder + "/", sale.File.FileName);
		if (!System.IO.File.Exists(orgfolder))
		{
			Directory.CreateDirectory(orgfolder);
		}

		if (System.IO.File.Exists(fileLocation))
		{
			System.IO.File.Delete(fileLocation);
		}

		if (sale.File.Length > 0)
		{
			var filePath = Path.Combine(orgfolder, sale.File.FileName);
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await sale.File.CopyToAsync(fileStream);
			}
		}


		saleToUpdate.IsVdscertificateReceived = true;
		saleToUpdate.VdscertificateNo = sale.VdscertificateNo;
		saleToUpdate.VdspaymentChallanNo = sale.VdspaymentChallanNo;
		saleToUpdate.VdspaymentDate = DateTime.Now;
		saleToUpdate.VdspaymentEconomicCode = sale.VdspaymentEconomicCode;
		saleToUpdate.VdscertificateIssueTime = sale.VdscertificateIssueTime;
		saleToUpdate.VdspaymentBankId = sale.VdspaymentBankId;
		saleToUpdate.VdspaymentBankBranchName = sale.VdspaymentBankBranchName;
		saleToUpdate.VdspaymentBookTransferNo = sale.VdspaymentBookTransferNo;
		saleToUpdate.Vdsnote = sale.Vdsnote;
		_saleOrderService.Update(saleToUpdate);
		try
		{
			await UnitOfWork.SaveChangesAsync(CancellationToken.None);
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
			return RedirectToAction("SaleVds");
		}
		catch (Exception ex)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			return RedirectToAction("SaleVds");
		}
	}


	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	public async Task<IActionResult> UploadSimplifiedSalesList()
	{
		var model = new VmViewSimplifiedSales
		{
			Salses = await _excelDataUploadService.GetSimplifiedLocalSalesListAsync()
		};
		return View(model);
	}


	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	public async Task<IActionResult> UploadSimplifiedSalesDetails()
	{
		var model = new VmViewSimplifiedSales
		{
			Salses = await _excelDataUploadService.GetSimplifiedLocalSalesListAsync()
		};
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	public async Task<IActionResult> UploadSimplifiedSales(VmSingleFileUpload model)
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
					var simplifiedSalses = _excelService.ReadExcel<VmExcelSimplifiedSales>(model.UploadedFile);

					// msg = ExcelFileValidationDupChk(simplifiedSalses);
					msg = ExcelFileValidationLocalSalesDupChk(simplifiedSalses);
					if (string.IsNullOrEmpty(msg))
					{
						var fileSaveDto = new FileSaveDto
						{
							FileRootPath = ControllerStaticData.FileRootPath,
							FileModulePath = ControllerStaticData.FileUploadedSimplifiedLocalSalePath,
							OrganizationId = UserSession.OrganizationId
						};

						var fsf = await _fileOperationService.SaveFile(model.UploadedFile, fileSaveDto);

						var excelDataUpload = new ExcelDataUpload
						{
							ExcelUploadedDataTypeId = (int)EnumUploadedExcelDataType.SimplifiedSale,
							UploadedFileName = model.UploadedFile.FileName,
							StoredFilePath = fsf.FileUrl,
							CreatedBy = UserSession.UserId,
							OrganizationId = UserSession.OrganizationId,
							CreatedTime = DateTime.Now,
							UploadTime = DateTime.Now,
						};
						_excelDataUploadService.Insert(excelDataUpload);
						await UnitOfWork.SaveChangesAsync();
						await _excelSimplifiedSimplifiedSalseService.SaveSimplifiedSaleList(simplifiedSalses,
							excelDataUpload.ExcelDataUploadId);
						await UnitOfWork.SaveChangesAsync();
						var process = await _saleService.ProcessUploadedSimplifiedSale(
							excelDataUpload.ExcelDataUploadId,
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

		return RedirectToAction(nameof(UploadSimplifiedSalesList));
	}


	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	public async Task<IActionResult> UploadSimplifiedLocalSaleWithoutPaymentList()
	{
		var model = new VmViewSimplifiedSales
		{
			Salses = await _excelDataUploadService.GetSimplifiedLocalSalesListAsync()
		};
		return View(model);
	}


	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	public async Task<IActionResult> UploadSimplifiedLocalSaleWithoutPayment(VmSingleFileUpload model)
	{
		if (ModelState.IsValid)
		{
			var msg = ExcelFileSalesLocalWithoutPaymentValidationNew(model);

			if (!string.IsNullOrEmpty(msg))
			{
				TempData["ErrorMessage"] = msg;
			}
			else
			{
				try
				{
					var simplifiedSales =
						_excelService.ReadExcel<VmExcelSimplifiedLocalSalesWithoutPayment>(model.UploadedFile);
					msg = ExcelFileValidationLocalSalesWithoutPaymentDupChk(simplifiedSales);
					if (string.IsNullOrEmpty(msg))
					{
						var fileSaveDto = new FileSaveDto
						{
							FileRootPath = ControllerStaticData.FileRootPath,
							FileModulePath = ControllerStaticData.FileUploadedSimplifiedLocalSalePath,
							OrganizationId = UserSession.OrganizationId
						};

						var fileSaveFeedback = await _fileOperationService.SaveFile(model.UploadedFile, fileSaveDto);

						var excelDataUpload = new ExcelDataUpload
						{
							ExcelUploadedDataTypeId = (int)EnumUploadedExcelDataType.SimplifiedSale,
							UploadedFileName = model.UploadedFile.FileName,
							StoredFilePath = fileSaveFeedback.FileUrl,
							CreatedBy = UserSession.UserId,
							OrganizationId = UserSession.OrganizationId,
							CreatedTime = DateTime.Now,
							UploadTime = DateTime.Now,
						};
						_excelDataUploadService.Insert(excelDataUpload);
						await UnitOfWork.SaveChangesAsync();
						await _excelSimplifiedSimplifiedSalseService.SaveSimplifiedLocalSaleWithoutPaymentList(
							simplifiedSales,
							excelDataUpload.ExcelDataUploadId);
						await UnitOfWork.SaveChangesAsync();
						var process = await _saleService.ProcessUploadedSimplifiedSale(
							excelDataUpload.ExcelDataUploadId,
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

		return RedirectToAction(nameof(UploadSimplifiedLocalSaleWithoutPaymentList));
	}


	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	public async Task<IActionResult> UploadSimplifiedLocalSaleCalculateByVatList()
	{
		var model = new VmViewSimplifiedSales
		{
			Salses = await _excelDataUploadService.GetSimplifiedLocalSalesListAsync()
		};
		return View(model);
	}


	[HttpPost]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_VDS_LIST_CAN_VIEW)]
	public async Task<IActionResult> UploadSimplifiedLocalSaleCalculateByVat(VmSingleFileUpload model)
	{
		if (ModelState.IsValid)
		{
			var msg = ExcelFileSalesLocalWithoutPaymentValidationNew(model);

			if (!string.IsNullOrEmpty(msg))
			{
				TempData["ErrorMessage"] = msg;
			}
			else
			{
				try
				{
					var simplifiedSales =
						_excelService.ReadExcel<VmExcelSimplifiedLocalSalesCalculateByVat>(model.UploadedFile);
					msg = ExcelSimplifiedLocalSalesCalculateByVatDupChk(simplifiedSales);
					if (string.IsNullOrEmpty(msg))
					{
						var fileSaveDto = new FileSaveDto
						{
							FileRootPath = ControllerStaticData.FileRootPath,
							FileModulePath = ControllerStaticData.FileUploadedSimplifiedLocalSalePath,
							OrganizationId = UserSession.OrganizationId
						};

						var fileSaveFeedback = await _fileOperationService.SaveFile(model.UploadedFile, fileSaveDto);

						var excelDataUpload = new ExcelDataUpload
						{
							ExcelUploadedDataTypeId = (int)EnumUploadedExcelDataType.SimplifiedSale,
							UploadedFileName = model.UploadedFile.FileName,
							StoredFilePath = fileSaveFeedback.FileUrl,
							CreatedBy = UserSession.UserId,
							OrganizationId = UserSession.OrganizationId,
							CreatedTime = DateTime.Now,
							UploadTime = DateTime.Now,
						};
						_excelDataUploadService.Insert(excelDataUpload);
						await UnitOfWork.SaveChangesAsync();
						await _excelSimplifiedLocalSaleCalculateByVatService.SaveSimplifiedLocalSaleCalculatedByVatList(
							simplifiedSales,
							excelDataUpload.ExcelDataUploadId);
						await UnitOfWork.SaveChangesAsync();
						var process = await _saleService.ProcessUploadedSimplifiedSale(
							excelDataUpload.ExcelDataUploadId,
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

		return RedirectToAction(nameof(UploadSimplifiedLocalSaleWithoutPaymentList));
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
		int CELL_NUMBER = 40;

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
			VmExcelSimplifiedSales Obj = new VmExcelSimplifiedSales();
			int pocoRow = 0;
			foreach (var columnHeader in firstRow.Cells)
			{
				if (!columnHeader.StringCellValue.ToLower()
					    .Equals(Obj.GetType().GetProperties()[pocoRow++].Name.ToString().ToLower()))
					return "Column " + pocoRow + " or row sequence not matched";
			}

			//Row count
			if (sheet.LastRowNum == 0) //only header exist
				return "Blank excel file";
		}

		return "";
	}

	private string ExcelFileValidationNew(VmSingleFileUpload model)
	{
		//File type validation
		long FILE_SIZE = 20971520;
		//5242880-5mb/20971520-20mb
		string[] FILE_EXTENSION = new string[] { ".xls", ".xlsx" };
		int CELL_NUMBER = 52;

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
			VmExcelSimplifiedSales Obj = new VmExcelSimplifiedSales();
			int pocoRow = 0;
			foreach (var columnHeader in firstRow.Cells)
			{
				var colName = columnHeader.StringCellValue.ToLower().Replace("zone", "branch")
					.Replace("office", "branch");
				if (!colName.Equals(Obj.GetType().GetProperties()[pocoRow++].Name.ToString().ToLower()))
					return "Column " + pocoRow + " or row sequence not matched";
			}

			//Row count
			if (sheet.LastRowNum == 0) //only header exist
				return "Blank excel file";
		}

		return "";
	}

	private string ExcelFileSalesLocalWithoutPaymentValidationNew(VmSingleFileUpload model)
	{
		//File type validation
		long FILE_SIZE = 20971520;
		//5242880-5mb/20971520-20mb
		string[] FILE_EXTENSION = new string[] { ".xls", ".xlsx" };
		int CELL_NUMBER = 36;

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
			var obj = new VmExcelSimplifiedLocalSalesWithoutPayment();
			int pocoRow = 0;
			foreach (var columnHeader in firstRow.Cells)
			{
				var colName = columnHeader.StringCellValue.ToLower().Replace("zone", "branch")
					.Replace("office", "branch");
				if (!colName.Equals(obj.GetType().GetProperties()[pocoRow++].Name.ToString().ToLower()))
					return "Column " + pocoRow + " or row sequence not matched";
			}

			//Row count
			if (sheet.LastRowNum == 0) //only header exist
				return "Blank excel file";
		}

		return "";
	}

	/// <summary>
	/// created by:: Mohammed Moinul Hasan
	/// 
	private string ExcelFileValidationDupChk(List<VmExcelSimplifiedSales> simplifiedSalses)
	{
		ICollection<ValidationResult> results = null;

		var dupes = simplifiedSalses.GroupBy(x => new { x.SalesId }).Where(x => x.Skip(1).Any()).ToArray();
		if (dupes.Any())
			return "Duplicate records found on excel";

		//Data Type and Length validation
		int i = 1;
		foreach (VmExcelSimplifiedSales model in simplifiedSalses)
		{
			if (!Validate(model, out results))
				return "Excel row" + i++ + "--" + results.Select(o => o.ErrorMessage);
		}

		return "";
	}

	private string ExcelFileValidationLocalSalesDupChk(List<VmExcelSimplifiedSales> simplifiedSalesList)
	{
		var dupes = simplifiedSalesList.GroupBy(x => new { x.SalesId, x.SalesDetailId, x.PaymentReceiveId })
			.Where(x => x.Skip(1).Any()).ToArray();
		if (dupes.Any())
			return "Duplicate records found on excel";

		//Data Type and Length validation
		var i = 1;
		var message = string.Empty;
		var sb = new StringBuilder();
		foreach (var model in simplifiedSalesList)
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

	private string ExcelFileValidationLocalSalesWithoutPaymentDupChk(
		List<VmExcelSimplifiedLocalSalesWithoutPayment> simplifiedSalesList)
	{
		var dupes = simplifiedSalesList.GroupBy(x => new { x.SalesId, x.SalesDetailId })
			.Where(x => x.Skip(1).Any()).ToArray();
		if (dupes.Any())
			return "Duplicate records found on excel";

		//Data Type and Length validation
		var i = 1;
		var message = string.Empty;
		var sb = new StringBuilder();
		foreach (var model in simplifiedSalesList)
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

	private string ExcelSimplifiedLocalSalesCalculateByVatDupChk(
		List<VmExcelSimplifiedLocalSalesCalculateByVat> simplifiedSalesList)
	{
		var dupes = simplifiedSalesList.GroupBy(x => new { x.SalesId, x.SalesDetailId })
			.Where(x => x.Skip(1).Any()).ToArray();
		if (dupes.Any())
			return "Duplicate records found on excel";

		//Data Type and Length validation
		var i = 1;
		var message = string.Empty;
		var sb = new StringBuilder();
		foreach (var model in simplifiedSalesList)
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