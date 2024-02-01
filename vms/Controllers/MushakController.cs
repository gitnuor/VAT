using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using vms.entity.Enums;
using vms.entity.viewModels;
using vms.entity.viewModels.MushakViewModel;
using vms.entity.viewModels.ReportsViewModel;
using vms.utility;
using vms.Utility;
using vms.utility.StaticData;
using vms.service.Services.TransactionService;
using vms.service.Services.ReportService;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.MushakService;

namespace vms.Controllers;

public class MushakController : ControllerBase
{
	private readonly IOrganizationService _organizationService;
	private readonly IOrgBranchService _branchService;
	private readonly IMushakGenerationService _mushakGenerationService;
	private readonly IPriceSetupService _priceSetupService;
	private readonly IProductService _productService;
	private readonly ISaleService _saleService;
	private readonly IPurchaseService _purchaseService;
	private readonly IDamageService _damageService;
	private readonly IMushakHighValue _mushakHighValueService;
	private readonly IBranchTransferSendService _branchTransferSendService;
	private readonly ISalesPriceAdjustmentService _salesPriceAdjustmentService;
	private readonly IReportOptionService _reportOptionService;

	public MushakController(ControllerBaseParamModel controllerBaseParamModel, IOrganizationService organizationService,
		IMushakGenerationService mushakGenerationService, IPriceSetupService priceSetupService,
		IProductService productService, ISaleService saleService, IPurchaseService purchaseService,
		IDamageService damageService, IMushakHighValue mushakHighValueService, IOrgBranchService branchService,
		IBranchTransferSendService branchTransferSendService, ISalesPriceAdjustmentService salesPriceAdjustmentService,
		IReportOptionService reportOptionService)
		: base(controllerBaseParamModel)
	{
		_organizationService = organizationService;
		_priceSetupService = priceSetupService;
		_productService = productService;
		_saleService = saleService;
		_purchaseService = purchaseService;
		_damageService = damageService;
		_mushakHighValueService = mushakHighValueService;
		_branchService = branchService;
		_branchTransferSendService = branchTransferSendService;
		_salesPriceAdjustmentService = salesPriceAdjustmentService;
		_mushakGenerationService = mushakGenerationService;
		_reportOptionService = reportOptionService;
	}

	public IActionResult Index()
	{
		return View();
	}

	#region Mushak- 4.3 (Input-Output Coefficient)

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_4_3)]
	public async Task<IActionResult> MushakFourPointThree()
	{
		var model = new VmPriceSetupReportParam();
		model.PriceDeclarId = 0;
		model.OrgId = 0;
		var priceSetupList = await _priceSetupService.Query().Include(x => x.Product)
			.Where(x => x.IsActive && x.OrganizationId == UserSession.OrganizationId).SelectAsync();
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem
		{
			Text = s.Product.Name 
			       + (string.IsNullOrEmpty(s.Product.ModelNo) ? "" : $" ({s.Product.ModelNo})")
			       + (string.IsNullOrEmpty(s.Product.Code) ? "" : $" ({s.Product.Code})")
			       + (string.IsNullOrEmpty(s.Product.Variant) ? "" : $" ({s.Product.Variant})")
			       + (string.IsNullOrEmpty(s.Product.Color) ? "" : $" ({s.Product.Color})")
			       + (string.IsNullOrEmpty(s.Product.Weight) ? "" : $" ({s.Product.Weight})")
			       + (string.IsNullOrEmpty(s.Product.Specification) ? "" : $" ({s.Product.Specification})"),
			Value = s.PriceSetupId.ToString()
		});

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_4_3)]
	public async Task<IActionResult> MushakFourPointThree(VmPriceSetupReportParam model)
	{
		var priceSetup = await _priceSetupService.ExistsAsync(model.PriceDeclarId);
		if (priceSetup)
		{
			model.PriceDeclarationList = await _mushakGenerationService.Mushak4P3(model.PriceDeclarId);
		}
		else
		{
			model.PriceDeclarId = 0;
			TempData["message"] = "NotFound";
		}

		model.OrgId = UserSession.OrganizationId;
		var priceSetupList = await _priceSetupService.Query().Include(x => x.Product)
			.Where(x => x.IsActive && x.OrganizationId == UserSession.OrganizationId).SelectAsync();
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem
		{
			Text = s.Product.Name
			       + (string.IsNullOrEmpty(s.Product.ModelNo) ? "" : $" ({s.Product.ModelNo})")
			       + (string.IsNullOrEmpty(s.Product.Code) ? "" : $" ({s.Product.Code})")
			       + (string.IsNullOrEmpty(s.Product.Variant) ? "" : $" ({s.Product.Variant})")
			       + (string.IsNullOrEmpty(s.Product.Color) ? "" : $" ({s.Product.Color})")
			       + (string.IsNullOrEmpty(s.Product.Weight) ? "" : $" ({s.Product.Weight})")
			       + (string.IsNullOrEmpty(s.Product.Specification) ? "" : $" ({s.Product.Specification})"),
			Value = s.PriceSetupId.ToString()
		});

		var org = await _organizationService.GetOrganization(
			IvatDataProtector.Protect(UserSession.OrganizationId.ToString()));
		if (org != null)
		{
			model.OrgAddress = org.Address;
			model.OrgBin = org.Bin;
		}

		model.OrgName = UserSession.OrganizationName;

		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_4_3)]
	public async Task<ActionResult> MushakFourPointThreeExportToExcel(VmPriceSetupReportParam model)
	{
		try
		{
			if (ModelState.IsValid)
			{
				var priceSetup = await _priceSetupService.ExistsAsync(model.PriceDeclarId);

				if (priceSetup)
				{
					var mushak4p3 = await _mushakGenerationService.Mushak4P3(model.PriceDeclarId);

					string sWebRootFolder = Environment.WebRootPath;
					sWebRootFolder = Path.Combine(sWebRootFolder, "ExportExcel");
					if (!Directory.Exists(sWebRootFolder))
					{
						Directory.CreateDirectory(sWebRootFolder);
					}

					string sFileName = @"Mushak_4.3.xlsx";
					string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
					FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
					var memory = new MemoryStream();
					using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create,
						       FileAccess.Write))
					{
						IWorkbook workbook;
						workbook = new XSSFWorkbook();
						ISheet excelSheet = workbook.CreateSheet("Mushak4.3");
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
						row.GetCell(0).SetCellValue(model.Language == 0
							? "গণপ্রজাতন্ত্রী বাংলাদেশ সরকার"
							: "GOVERNMENT OF THE PEOPLE'S REPUBLIC OF BANGLADESH");
						excelSheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));
						excelSheet.DefaultRowHeightInPoints = 24;

						row = excelSheet.CreateRow(1);
						row.CreateCell(0).CellStyle = styleHeading;
						row.GetCell(0)
							.SetCellValue(model.Language == 0 ? "জাতীয় রাজস্ব বোর্ড" : "NATIONAL BOARD OF REVENUE");
						excelSheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 11));
						excelSheet.DefaultRowHeightInPoints = 24;

						row = excelSheet.CreateRow(2);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue(model.Language == 0
							? "উপকরণ-উৎপাদ সহগ (Input-Output Coefficient)ঘোষণা"
							: "Materials-Product Coefficient (Input-Output Coefficient) Declaration");
						excelSheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 11));

						row = excelSheet.CreateRow(3);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue(model.Language == 0 ? "(বিধি ২১ দ্রষ্টব্য)" : "(Note Rule 21)");
						excelSheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 11));

						row = excelSheet.CreateRow(4);
						row.CreateCell(0).SetCellValue("");
						excelSheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 11));

						row = excelSheet.CreateRow(5);
						row.CreateCell(0)
							.SetCellValue((model.Language == 0 ? "প্রতিষ্ঠানের নাম : " : "Organization Name :") +
							              mushak4p3.Select(x => x.OrganizationName).FirstOrDefault());
						excelSheet.AddMergedRegion(new CellRangeAddress(5, 5, 0, 11));

						row = excelSheet.CreateRow(6);
						row.CreateCell(0).SetCellValue((model.Language == 0 ? "ঠিকানা : " : "Address : ") +
						                               mushak4p3.Select(x => x.OrganizationAddress).FirstOrDefault());
						excelSheet.AddMergedRegion(new CellRangeAddress(6, 6, 0, 11));

						row = excelSheet.CreateRow(7);
						row.CreateCell(0).SetCellValue((model.Language == 0 ? "বিআইএন : " : "BIN : ") +
						                               mushak4p3.Select(x => x.OrganizationBin).FirstOrDefault());
						excelSheet.AddMergedRegion(new CellRangeAddress(7, 7, 0, 11));


						DateTime? SubDate = model.PriceDeclarationList.Select(x => x.DateOfSubmission).FirstOrDefault();
						var subDatetText = model.Language == 0 ? "দাখিলের তারিখ : " : "Date of submission : ";
						if (SubDate != null)
						{
							subDatetText += SubDate.Value.ToString("dd/MM/yyyy");
						}

						row = excelSheet.CreateRow(8);
						row.CreateCell(0).SetCellValue(subDatetText);
						excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 0, 11));


						row = excelSheet.CreateRow(9);
						row.CreateCell(0).SetCellValue(
							(model.Language == 0
								? "ঘোষিত সহগ অনুযায়ী পণ্য/সেবার প্রথম সরবরাহের তারিখ : "
								: "Date of first delivery of goods/services as per declared coefficient : ") +
							model.PriceDeclarationList.Select(x => x.FirstSupplyDate).FirstOrDefault()
								.ToString("dd/MM/yyyy"));
						excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 0, 11));

						row = excelSheet.CreateRow(10);
						row.CreateCell(0).SetCellValue("");
						excelSheet.AddMergedRegion(new CellRangeAddress(10, 10, 0, 11));


						row = excelSheet.CreateRow(11);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue(model.Language == 0 ? "ক্রমিক সংখ্যা" : "Serial No");
						excelSheet.AddMergedRegion(new CellRangeAddress(11, 12, 0, 0));
						row.CreateCell(1).CellStyle = style;
						row.GetCell(1).SetCellValue(model.Language == 0
							? "পণ্যের এইচ এস কোড/সেবা কোড"
							: "Product HS Code/Service Code");
						excelSheet.AddMergedRegion(new CellRangeAddress(11, 12, 1, 1));
						row.CreateCell(2).CellStyle = style;
						row.GetCell(2).SetCellValue(model.Language == 0
							? "সরবরাহতব্য পণ্য/সেবার নাম ও বর্ণনা (প্রযোজ্য ক্ষেত্রে ব্রান্ড নামসহ)"
							: "Names and descriptions of products / services (including brand names where applicable)");
						excelSheet.AddMergedRegion(new CellRangeAddress(11, 12, 2, 2));
						row.CreateCell(3).CellStyle = style;
						row.GetCell(3).SetCellValue(model.Language == 0 ? "সরবরাহের একক" : "Unit of supply");
						excelSheet.AddMergedRegion(new CellRangeAddress(11, 12, 3, 3));
						row.CreateCell(4).CellStyle = style;
						row.GetCell(4).SetCellValue(model.Language == 0
							? "একক পণ্য/সেবাসরবরাহে ব্যবহায যাবতীয় উপকরণের/কাঁচামালের ও প্যাকিং সামগ্রীর বিবরণ, পরিমাণ ও ক্রয়মূল্য(উপকরনভিত্তিক অপচয়ের শতকরা হারসহ)"
							: "Details, quantity and purchase price of all materials / raw materials and packing materials used in a single product / service delivery (including percentage of waste based on equipment)");
						excelSheet.AddMergedRegion(new CellRangeAddress(11, 11, 4, 8));
						row.CreateCell(9).CellStyle = style;
						row.GetCell(9)
							.SetCellValue(model.Language == 0 ? "মূল্য সংযোজনের বিবরণ" : "Details of value addition");
						excelSheet.AddMergedRegion(new CellRangeAddress(11, 11, 9, 10));
						row.CreateCell(11).SetCellValue(model.Language == 0 ? "মন্তব্য" : "Remarks");
						excelSheet.AddMergedRegion(new CellRangeAddress(11, 12, 11, 11));


						row = excelSheet.CreateRow(12);
						row.CreateCell(4).CellStyle = style;
						row.GetCell(4).SetCellValue(model.Language == 0 ? "বিবরণ" : "Description");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 4, 4));
						row.CreateCell(5).CellStyle = style;
						row.GetCell(5).SetCellValue(model.Language == 0 ? "অপচয়সহ পরিমাণ" : "Quantity with waste");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 5, 5));
						row.CreateCell(6).CellStyle = style;
						row.GetCell(6).SetCellValue(model.Language == 0 ? "ক্রয় মূল্য" : "Purchase Price");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 6, 6));
						row.CreateCell(7).CellStyle = style;
						row.GetCell(7).SetCellValue(model.Language == 0 ? "অপচয়ের পরিমাণ" : "Waste Quantity");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 7, 7));
						row.CreateCell(8).CellStyle = style;
						row.GetCell(8).SetCellValue(model.Language == 0 ? "শতকরা হার" : "Percentage(%)");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 8, 8));
						row.CreateCell(9).CellStyle = style;
						row.GetCell(9)
							.SetCellValue(model.Language == 0 ? "মূল্য সংযোজনের খাত" : "The value addition sector");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 9, 9));
						row.CreateCell(10).CellStyle = style;
						row.GetCell(10).SetCellValue(model.Language == 0 ? "মূল্য" : "Price");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 10, 10));


						row = excelSheet.CreateRow(13);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue(model.Language == 0 ? "(১)" : "(1)");
						row.CreateCell(1).CellStyle = style;
						row.GetCell(1).SetCellValue(model.Language == 0 ? "(২)" : "(2)");
						row.CreateCell(2).CellStyle = style;
						row.GetCell(2).SetCellValue(model.Language == 0 ? "(৩)" : "(3)");
						row.CreateCell(3).CellStyle = style;
						row.GetCell(3).SetCellValue(model.Language == 0 ? "(৪)" : "(4)");
						row.CreateCell(4).CellStyle = style;
						row.GetCell(4).SetCellValue(model.Language == 0 ? "৫" : "(5)");
						row.CreateCell(5).CellStyle = style;
						row.GetCell(5).SetCellValue(model.Language == 0 ? "(৬)" : "(6)");
						row.CreateCell(6).CellStyle = style;
						row.GetCell(6).SetCellValue(model.Language == 0 ? "(৭)" : "(7)");
						row.CreateCell(7).CellStyle = style;
						row.GetCell(7).SetCellValue(model.Language == 0 ? "(৮)" : "(8)");
						row.CreateCell(8).CellStyle = style;
						row.GetCell(8).SetCellValue(model.Language == 0 ? "(৯)" : "(9)");
						row.CreateCell(9).CellStyle = style;
						row.GetCell(9).SetCellValue(model.Language == 0 ? "(১০)" : "(10)");
						row.CreateCell(10).CellStyle = style;
						row.GetCell(10).SetCellValue(model.Language == 0 ? "(১১)" : "(11)");
						row.CreateCell(11).CellStyle = style;
						row.GetCell(11).SetCellValue(model.Language == 0 ? "(১২)" : "(12)");


						int rowCounter = 14, SlNo = 1;
						foreach (var data in mushak4p3)
						{
							row = excelSheet.CreateRow(rowCounter);
							row.CreateCell(0).CellStyle = style;
							row.GetCell(0).SetCellValue(SlNo);
							row.CreateCell(1).CellStyle = style;
							row.GetCell(1).SetCellValue(data.FinishedProductHsCode);
							row.CreateCell(2).CellStyle = style;
							row.GetCell(2).SetCellValue(data.FinishedProductName);
							row.CreateCell(3).CellStyle = style;
							row.GetCell(3).SetCellValue(data.FinishedProductMeasurementUnit);
							row.CreateCell(4).CellStyle = style;
							row.GetCell(4).SetCellValue(data.RawmaterialName);
							row.CreateCell(5).CellStyle = style;
							row.GetCell(5).SetCellValue(data.RawmaterialRequiredQtyWithWastage.ToString());
							row.CreateCell(6).CellStyle = style;
							row.GetCell(6).SetCellValue(data.RawmaterialPurchasePrice.ToString());
							row.CreateCell(7).CellStyle = style;
							row.GetCell(7).SetCellValue(data.RawmaterialWastageQty.ToString());
							row.CreateCell(8).CellStyle = style;
							row.GetCell(8).SetCellValue(data.RawmaterialWastagePercentage.ToString());
							row.CreateCell(9).CellStyle = style;
							row.GetCell(9).SetCellValue(data.OverHeadCostName);
							row.CreateCell(10).CellStyle = style;
							row.GetCell(10).SetCellValue(data.OverHeadCostAmount.ToString());
							row.CreateCell(11).CellStyle = style;
							row.GetCell(11).SetCellValue("");

							rowCounter++;
							SlNo++;
						}

						row = excelSheet.CreateRow(rowCounter);
						row.CreateCell(0).SetCellValue("");
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 11));

						row = excelSheet.CreateRow(++rowCounter);
						row.CreateCell(0).SetCellValue("");
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 11));

						row = excelSheet.CreateRow(++rowCounter);
						row.CreateCell(0).SetCellValue(model.Language == 0
							? "প্রতিষ্ঠান কর্তৃপক্ষের দায়িত্বপ্রাপ্ত ব্যক্তির নাম : "
							: "Name of the person in charge of the institution authority : ");
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 1));

						row.CreateCell(2)
							.SetCellValue(mushak4p3.Select(x => x.VatResponsiblePersonName).FirstOrDefault());
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 2, 3));

						row = excelSheet.CreateRow(++rowCounter);
						row.CreateCell(0).SetCellValue(model.Language == 0 ? "পদবী : " : "Designation : ");
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 1));

						row.CreateCell(2)
							.SetCellValue(mushak4p3.Select(x => x.VatResponsiblePersonDesignation).FirstOrDefault());
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 2, 3));

						row = excelSheet.CreateRow(++rowCounter);
						row.CreateCell(0).SetCellValue(model.Language == 0 ? "স্বাক্ষর : " : "Signature : ");
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 1));

						row.CreateCell(2).SetCellValue("");
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 2, 3));

						row = excelSheet.CreateRow(++rowCounter);
						row.CreateCell(0).SetCellValue(model.Language == 0 ? "সীল : " : "Seal : ");
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter + 1, 0, 1));

						row.CreateCell(2).SetCellValue("");
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter + 1, 2, 3));

						for (int i = 0; i <= 11; i++)
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

				return RedirectToAction(nameof(MushakFourPointThree));
			}
			else
			{
				return RedirectToAction(nameof(MushakFourPointThree), model);
			}
		}
		catch
		{
			return RedirectToAction(nameof(MushakFourPointThree), model);
		}
	}

	#endregion

	#region Mushak- 4.4 (Normal Damage)

	public async Task<IActionResult> MushakFourPointFourList()
	{
		var model = await _damageService.Query().Where(x => x.DamageTypeId == (int)EnumDamageType.NormalDamage)
			.SelectAsync();
		return View(model);
	}

	//public async Task<IActionResult> MushakFourPointFour(int id)
	//{
	//    try
	//    {
	//        var model = new vmMushak4P4();
	//        var detailList = await _damageDetailService.Query()
	//        .Include(x => x.Damage)
	//        .Include(x => x.Product)
	//        .Where(x => x.DamageId == id).SelectAsync();

	//        var details = new List<vmMushak4P4Details>();
	//        foreach (var item in detailList)
	//        {
	//            var data = new vmMushak4P4Details();
	//            data.ProductName = item.Product.Name;
	//            data.DamageQty = item.DamageQty;
	//            data.SuggestedUnitPrice = item.SuggestedNewUnitPrice;
	//            data.ReasonOfDamage = item.DamageDescription;


	//            if (item.PurchaseDetailId != null)
	//            {
	//                var purchase = await _purchaseDetailService.Query().Where(x => x.PurchaseDetailId == item.PurchaseDetailId).SingleOrDefaultAsync();

	//                data.RealUnitPrice = purchase.UnitPrice;
	//                data.VATPercent = purchase.Vatpercent;
	//                data.PurchaseChallanNo = purchase.PurchaseId.ToString();
	//            }
	//            else if (item.SalesDetailId != null)
	//            {
	//                var sale = await _salesDetailService.Query().Where(x => x.SalesDetailId == item.SalesDetailId).SingleOrDefaultAsync();

	//                data.RealUnitPrice = sale.UnitPrice;
	//                data.VATPercent = sale.Vatpercent;
	//                data.PurchaseChallanNo = sale.SalesId.ToString();
	//            }
	//            else
	//            {
	//                var transBook = await _productTransactionBookService.Query().Where(x => x.DamageDetailId == item.DamageDetailId).SingleOrDefaultAsync();

	//                data.RealUnitPrice = transBook.InitUnitPrice;
	//                data.PurchaseChallanNo = transBook.ProductTransactionBookId.ToString();

	//                var vatPercent = await _productVatService.Query().Include(x => x.ProductVattype).Where(x => x.ProductId == item.ProductId).SingleOrDefaultAsync();

	//                data.VATPercent = vatPercent.ProductVattype.DefaultVatPercent;
	//            }

	//            model.Details.Add(data);
	//        }

	//        model.OrgName = UserSession.OrganizationName;
	//        var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
	//        model.OrgAddress = org.Address;
	//        model.OrgBin = org.Bin;
	//        model.VatResponsiblePersonName = org.VatResponsiblePersonName;
	//        model.VatResponsiblePersonDesignation = org.VatResponsiblePersonDesignation;

	//        return View(model);
	//    }
	//    catch
	//    {
	//        return NotFound();
	//    }

	//}

	#endregion

	#region Mushak- 4.5 (Accidental Damage)

	public async Task<IActionResult> MushakFourPointFiveList()
	{
		var model = await _damageService.GetDamage(UserSession.OrganizationId);
		model = model.Where(x => x.DamageTypeId == (int)EnumDamageType.AccidentDamage);

		return View(model);
	}

	//public async Task<IActionResult> MushakFourPointFive(int id)
	//{
	//    try
	//    {
	//        var model = new vmMushak4P5();
	//        var detailList = await _damageDetailService.Query()
	//        .Include(x => x.Damage)
	//        .Include(x => x.Product)
	//        .Where(x => x.DamageId == id).SelectAsync();

	//        var details = new List<vmMushak4P5Details>();
	//        foreach (var item in detailList)
	//        {
	//            var data = new vmMushak4P5Details();
	//            data.ProductName = item.Product.Name;
	//            data.ProdcutQty = item.DamageQty;
	//            data.SuggestedUnitPrice = item.SuggestedNewUnitPrice;
	//            data.ReasonOfDamage = item.DamageDescription;
	//            if (item.PurchaseDetailId != null)
	//            {
	//                var unitPrice = await _purchaseDetailService.Query().Where(x => x.PurchaseDetailId == item.PurchaseDetailId).SingleOrDefaultAsync();

	//                data.RealUnitPrice = unitPrice.UnitPrice;
	//            }
	//            else if (item.SalesDetailId != null)
	//            {
	//                var unitPrice = await _salesDetailService.Query().Where(x => x.SalesDetailId == item.SalesDetailId).SingleOrDefaultAsync();

	//                data.RealUnitPrice = unitPrice.UnitPrice;
	//            }
	//            else
	//            {
	//                var unitPrice = await _productTransactionBookService.Query().Where(x => x.DamageDetailId == item.DamageDetailId).SingleOrDefaultAsync();

	//                //data.RealUnitPrice = unitPrice.InitUnitPrice;

	//            }

	//            model.Details.Add(data);
	//        }

	//        model.OrgName = UserSession.OrganizationName;
	//        var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
	//        model.OrgAddress = org.Address;
	//        model.OrgBin = org.Bin;
	//        model.VatResponsiblePersonName = org.VatResponsiblePersonName;
	//        model.VatResponsiblePersonDesignation = org.VatResponsiblePersonDesignation;

	//        return View(model);
	//    }
	//    catch
	//    {
	//        return NotFound();
	//    }
	//}

	#endregion

	#region MUshak- 6.1 (Purchase Book)

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_1)]
	public async Task<IActionResult> MushakSixPointOne()
	{
		var model = new VmCalculationBookParam();
		var priceSetupList = await _productService.Query()
			.Where(x => x.OrganizationId == UserSession.OrganizationId && x.ProductTypeId != 3).SelectAsync();
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem
		{
			Text = s.Name + (string.IsNullOrEmpty(s.ModelNo) ? "" : $" ({s.ModelNo})") +
			       (string.IsNullOrEmpty(s.Size) ? "" : $" ({s.Size})") +
			       (string.IsNullOrEmpty(s.Weight) ? "" : $" ({s.Weight})"),
			Value = s.ProductId.ToString()
		});
		model.BranchList = await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);

		model.ProductId = 0;
		model.VendorId = 0;
		model.OgrId = 0;
		model.Year = DateTime.Now.Year;
		model.Month = DateTime.Now.Month;

		return View(model);
	}

  

    [HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_1)]
	public async Task<IActionResult> MushakSixPointOne(VmCalculationBookParam model)
	{
		var form = (DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");

		var to = (DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");
		model.OgrId = UserSession.OrganizationId;
		model.FromDateToDisplay =
			(DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString(
				MessageStaticData.DATE_DISPLAY_FORMAT);
		model.ToDateToDisplay =
			(DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString(MessageStaticData
				.DATE_DISPLAY_FORMAT);
		model.PurchaseCalcBook =
			await _mushakGenerationService.Mushak6P1(model.ProductId, model.OgrId, form, to, model.OgrBranchId??0);

		var org = await _organizationService.GetOrganization(
			IvatDataProtector.Protect(UserSession.OrganizationId.ToString()));
		if (org != null)
		{
			model.OrgAddress = org.Address;
			model.OrgBin = org.Bin;
		}

		model.OrgName = UserSession.OrganizationName;

		var priceSetupList = await _productService.Query()
			.Where(x => x.OrganizationId == UserSession.OrganizationId && x.ProductTypeId != 3).SelectAsync();
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem
		{
			Text = s.Name + (string.IsNullOrEmpty(s.ModelNo) ? "" : $" ({s.ModelNo})") +
			       (string.IsNullOrEmpty(s.Size) ? "" : $" ({s.Size})") +
			       (string.IsNullOrEmpty(s.Weight) ? "" : $" ({s.Weight})"),
			Value = s.ProductId.ToString()
		});
		model.BranchList = await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);

		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_1)]
	public async Task<IActionResult> MushakSixPointOneNew()
	{
		var model = new VmCalculationBookParam();
		var priceSetupList = await _productService.Query()
			.Where(x => x.OrganizationId == UserSession.OrganizationId && x.ProductTypeId != 3).SelectAsync();
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem
		{
			Text = s.Name + (string.IsNullOrEmpty(s.ModelNo) ? "" : $" ({s.ModelNo})") +
				   (string.IsNullOrEmpty(s.Size) ? "" : $" ({s.Size})") +
				   (string.IsNullOrEmpty(s.Weight) ? "" : $" ({s.Weight})"),
			Value = s.ProductId.ToString()
		});
		model.BranchList = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();


		model.ProductId = 0;
		model.VendorId = 0;
		model.OgrId = 0;
		model.Year = DateTime.Now.Year;
		model.Month = DateTime.Now.Month;

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_1)]
	public async Task<IActionResult> MushakSixPointOneNew(VmCalculationBookParam model)
	{

		var form = model.FromDate.Value.ToString("yyyy-MM-dd");
		var to = model.ToDate.Value.ToString("yyyy-MM-dd");
		model.OgrId = UserSession.OrganizationId;
		var purchaseCalcBook =
			await _mushakGenerationService.Mushak6P1(model.ProductId, model.OgrId, form, to, model.OgrBranchId ?? 0);

		return ProcessReport(purchaseCalcBook,
			RdlcReportFileOption.MushakSixPointOneReportUrl,
			RdlcReportFileOption.MushakSixPointOneReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.MushakSixPointOneReportFileName),
			GetParameterForMushakReport(model.FromDate.Value, model.ToDate.Value, "Mushak- 6.1"), model.ReportProcessOptionId
			);

	}

	private Dictionary<string, string> GetParameterForMushakReport(DateTime fromDate, DateTime toDate, string reportHeaderName)
	{
		return new Dictionary<string, string>
		{
			{ "ReportNameHeader", reportHeaderName },
			{
				"DateHeader",
				$"From: {StringGenerator.DateTimeToStringWithoutTime(fromDate)} to: {StringGenerator.DateTimeToStringWithoutTime(toDate)}"
			}
		};
	}

	public async Task<ActionResult> MushakSixPointOneExportToExcel(VmCalculationBookParam model)
	{
		// if (ModelState.IsValid)
		{
			var form = (DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");

			var to = (DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");

			model.FromDateToDisplay =
				(DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString(MessageStaticData
					.DATE_DISPLAY_FORMAT);
			model.ToDateToDisplay =
				(DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString(MessageStaticData
					.DATE_DISPLAY_FORMAT);

			model.OgrId = UserSession.OrganizationId;
			var mushak6p1 =
				await _mushakGenerationService.Mushak6P1(model.ProductId, model.OgrId, form, to, model.OgrBranchId??0);

			var orgInfo = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);

			string sWebRootFolder = Environment.WebRootPath;
			sWebRootFolder = Path.Combine(sWebRootFolder, "ExportExcel");
			if (!Directory.Exists(sWebRootFolder))
			{
				Directory.CreateDirectory(sWebRootFolder);
			}

			string sFileName = @"Mushak_6.1.xlsx";
			string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
			FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
			var memory = new MemoryStream();
			using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
			{
				IWorkbook workbook;
				workbook = new XSSFWorkbook();
				ISheet excelSheet = workbook.CreateSheet("Mushak6.1");
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
				row.GetCell(0).SetCellValue(orgInfo.Name);
				excelSheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 20));
				excelSheet.DefaultRowHeightInPoints = 24;

				row = excelSheet.CreateRow(1);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue(orgInfo.Address);
				excelSheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 20));

				row = excelSheet.CreateRow(2);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue(orgInfo.Bin);
				excelSheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 20));

				row = excelSheet.CreateRow(3);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue(model.Language == 0
					? "(পণ্য বা সেবা প্রক্রিয়াকরণে সম্পৃক্ত  এমন নিবন্ধিত বা তালিকাভুক্ত ব্যক্তির জন্য প্রযোজ্য)"
					: "(Applicable to registered or enlisted persons involved in the processing of goods or services)");
				excelSheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 20));

				row = excelSheet.CreateRow(4);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue(model.Language == 0
					? "[বিধি ৪০ (১) এর দফা (ক) ও বিধি ৪১ এর দফা (ক) দ্রষ্টব্য]"
					: "[Note clause (a) of rule 40 (1) and clause (a) of rule 41]");
				excelSheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 20));


				row = excelSheet.CreateRow(5);
				row.CreateCell(0).SetCellValue("From : " + model.FromDateToDisplay + " - To: " + model.ToDateToDisplay);
				excelSheet.AddMergedRegion(new CellRangeAddress(5, 5, 0, 20));

				row = excelSheet.CreateRow(6);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(6, 6, 0, 20));

				row = excelSheet.CreateRow(7);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0)
					.SetCellValue(model.Language == 0 ? "পণ্য/সেবার উপকরণ ক্রয়" : "Purchase of goods/services");
				excelSheet.AddMergedRegion(new CellRangeAddress(7, 7, 0, 20));

				row = excelSheet.CreateRow(8);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue(model.Language == 0 ? "ক্রমিক সংখ্যা" : "Serial No.");
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 10, 0, 0));
				row.CreateCell(1).CellStyle = style;
				row.GetCell(1).SetCellValue(model.Language == 0 ? "তারিখ" : "Date");
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 10, 1, 1));
				row.CreateCell(2).CellStyle = style;
				row.GetCell(2).SetCellValue(model.Language == 0
					? "মজুদ উপকরণের প্রারম্ভিক জের"
					: "Initial resale of stock materials");
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 2, 3));
				row.CreateCell(4).CellStyle = style;
				row.GetCell(4).SetCellValue(model.Language == 0 ? "ক্রয়কৃত উপকরণ" : "Purchased Goods");
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 4, 17));
				row.CreateCell(18).CellStyle = style;
				row.GetCell(18)
					.SetCellValue(model.Language == 0 ? "উপকরণের প্রান্তিক জের" : "Marginal perforation of materials");
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 18, 19));
				row.CreateCell(20).CellStyle = style;
				row.GetCell(20).SetCellValue(model.Language == 0 ? "মন্তব্য" : "Remarks");
				excelSheet.AddMergedRegion(new CellRangeAddress(8, 9, 20, 20));


				row = excelSheet.CreateRow(9);
				row.CreateCell(2).CellStyle = style;
				row.GetCell(2).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : "Quantity (Unit)");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 2, 2));
				row.CreateCell(3).CellStyle = style;
				row.GetCell(3).SetCellValue(model.Language == 0
					? @"মূল্য (সকল প্রকার কর ব্যতীত)"
					: "Price (Excluding All Types of Taxes)");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 3, 3));
				row.CreateCell(4).CellStyle = style;
				row.GetCell(4).SetCellValue(model.Language == 0
					? @"চালান(বিল অব এন্ট্ররি নম্বর"
					: "Challan (Bill of entry no.)");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 4, 4));
				row.CreateCell(5).CellStyle = style;
				row.GetCell(5).SetCellValue(model.Language == 0 ? "তারিখ" : "Date");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 5, 5));
				row.CreateCell(6).CellStyle = style;
				row.GetCell(6).SetCellValue(model.Language == 0 ? "বিক্রেতা/সরবরাহকারী" : "Vendor/Supplier");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 6, 8));
				row.CreateCell(9).CellStyle = style;
				row.GetCell(9).SetCellValue(model.Language == 0 ? "বিবরণ" : "Description");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 9, 9));
				row.CreateCell(10).CellStyle = style;
				row.GetCell(10).SetCellValue(model.Language == 0 ? "পরিমাণ" : "Quantity");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 10, 10));
				row.CreateCell(11).CellStyle = style;
				row.GetCell(11).SetCellValue(model.Language == 0
					? @"মূল্য (সকল প্রকার কর ব্যতীত)"
					: "Price (Excluding All Types of Taxes)");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 11, 11));
				row.CreateCell(12).CellStyle = style;
				row.GetCell(12)
					.SetCellValue(model.Language == 0 ? @"সম্পূরক শুল্ক(যদি থাকে)" : "Supplementary Duty (if any)");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 12, 12));
				row.CreateCell(13).CellStyle = style;
				row.GetCell(13).SetCellValue(model.Language == 0 ? "মূসক" : "Mushak");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 13, 13));
				row.CreateCell(14).CellStyle = style;
				row.GetCell(14).SetCellValue(model.Language == 0 ? "মোট উপকরণের পরিমাণ" : "Total Material Quantity");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 14, 15));
				row.CreateCell(16).CellStyle = style;
				row.GetCell(16).SetCellValue(model.Language == 0
					? "পণ্য প্রস্তুত প্রক্রিয়াকরণে উপকরণের ব্যবহার"
					: "Use of materials in product preparation processing");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 16, 17));
				//row.CreateCell(16).CellStyle = style;
				//row.GetCell(16).SetCellValue("পণ্য প্রস্তুত প্রক্রিয়া করনে উপকরণের ব্যবহার");
				//excelSheet.AddMergedRegion(new CellRangeAddress(0, 0, 16, 17));
				row.CreateCell(18).CellStyle = style;
				row.GetCell(18).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : "Quantity");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 18, 18));
				row.CreateCell(19).CellStyle = style;
				row.GetCell(19).SetCellValue(model.Language == 0
					? @"মূল্য (সকল প্রকার কর ব্যতীত)"
					: "Price (Excluding All Types of Taxes)");
				excelSheet.AddMergedRegion(new CellRangeAddress(9, 10, 19, 19));


				row = excelSheet.CreateRow(10);
				row.CreateCell(6).CellStyle = style;
				row.GetCell(6).SetCellValue(model.Language == 0 ? "নাম" : "Name");
				row.CreateCell(7).CellStyle = style;
				row.GetCell(7).SetCellValue(model.Language == 0 ? "ঠিকানা" : "Address");
				row.CreateCell(8).CellStyle = style;
				row.GetCell(8).SetCellValue(model.Language == 0
					? "নিবন্ধন/তালিকাভুক্তি/জাতীয় পরিচয়পত্র"
					: "Registration/Enrollment/National Identity Card");
				row.CreateCell(14).CellStyle = style;
				row.GetCell(14).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক" : "Quantity");
				row.CreateCell(15).CellStyle = style;
				row.GetCell(15).SetCellValue(model.Language == 0
					? @"মূল্য (সকল প্রকার কর ব্যতীত)"
					: "Price (Excluding All Types of Taxes)");
				row.CreateCell(16).CellStyle = style;
				row.GetCell(16).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক" : "Quantity");
				row.CreateCell(17).CellStyle = style;
				row.GetCell(17).SetCellValue(model.Language == 0
					? @"মূল্য (সকল প্রকার কর ব্যতীত)"
					: "Price (Excluding All Types of Taxes)");

				row = excelSheet.CreateRow(11);
				row.CreateCell(0).CellStyle = style;
				row.GetCell(0).SetCellValue(model.Language == 0 ? "(১)" : "(1)");
				row.CreateCell(1).CellStyle = style;
				row.GetCell(1).SetCellValue(model.Language == 0 ? "(২)" : "(2)");
				row.CreateCell(2).CellStyle = style;
				row.GetCell(2).SetCellValue(model.Language == 0 ? "(৩)" : "(3)");
				row.CreateCell(3).CellStyle = style;
				row.GetCell(3).SetCellValue(model.Language == 0 ? "(৪)" : "(4)");
				row.CreateCell(4).CellStyle = style;
				row.GetCell(4).SetCellValue(model.Language == 0 ? "৫" : "(5)");
				row.CreateCell(5).CellStyle = style;
				row.GetCell(5).SetCellValue(model.Language == 0 ? "(৬)" : "(6)");
				row.CreateCell(6).CellStyle = style;
				row.GetCell(6).SetCellValue(model.Language == 0 ? "(৭)" : "(7)");
				row.CreateCell(7).CellStyle = style;
				row.GetCell(7).SetCellValue(model.Language == 0 ? "(৮)" : "(8)");
				row.CreateCell(8).CellStyle = style;
				row.GetCell(8).SetCellValue(model.Language == 0 ? "(৯)" : "(9)");
				row.CreateCell(9).CellStyle = style;
				row.GetCell(9).SetCellValue(model.Language == 0 ? "(১০)" : "(10)");
				row.CreateCell(10).CellStyle = style;
				row.GetCell(10).SetCellValue(model.Language == 0 ? "(১১)" : "(11)");
				row.CreateCell(11).CellStyle = style;
				row.GetCell(11).SetCellValue(model.Language == 0 ? "(১২)" : "(12)");
				row.CreateCell(12).CellStyle = style;
				row.GetCell(12).SetCellValue(model.Language == 0 ? "(১৩)" : "(13)");
				row.CreateCell(13).CellStyle = style;
				row.GetCell(13).SetCellValue(model.Language == 0 ? "(১৪)" : "(14)");
				row.CreateCell(14).CellStyle = style;
				row.GetCell(14).SetCellValue(model.Language == 0 ? "(১৫)" : "(15)");
				row.CreateCell(15).CellStyle = style;
				row.GetCell(15).SetCellValue(model.Language == 0 ? "(১৬)" : "(16)");
				row.CreateCell(16).CellStyle = style;
				row.GetCell(16).SetCellValue(model.Language == 0 ? "(১৭)" : "(17)");
				row.CreateCell(17).CellStyle = style;
				row.GetCell(17).SetCellValue(model.Language == 0 ? "(১৮)" : "(18)");
				row.CreateCell(18).CellStyle = style;
				row.GetCell(18).SetCellValue(model.Language == 0 ? "(১৯)" : "(19)");
				row.CreateCell(19).CellStyle = style;
				row.GetCell(19).SetCellValue(model.Language == 0 ? "(২০)" : "(20)");
				row.CreateCell(20).CellStyle = style;
				row.GetCell(20).SetCellValue(model.Language == 0 ? "(২১)" : "(21)");


				int rowCounter = 12;
				if (mushak6p1.Any())
				{
					foreach (var data in mushak6p1)
					{
						row = excelSheet.CreateRow(rowCounter);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue(data.SlNo);
						row.CreateCell(1).CellStyle = style;
						DateTime? purchaseDate = data.PurchaseDate;
						row.GetCell(1)
							.SetCellValue(purchaseDate == null ? "" : purchaseDate.Value.ToString("dd/MM/yyyy"));
						row.CreateCell(2).CellStyle = style;
						row.GetCell(2).SetCellValue(data.MeasurementUnitName);
						row.CreateCell(3).CellStyle = style;
						row.GetCell(3).SetCellValue(data.InitPriceWithoutVat.ToString());
						row.CreateCell(4).CellStyle = style;
						row.GetCell(4).SetCellValue(data.VatChallanOrBillOfEntry);
						row.CreateCell(5).CellStyle = style;
						DateTime? challanOrEntrydate = data.VatChallanOrBillOfEntryDate;
						row.GetCell(5).SetCellValue(challanOrEntrydate == null
							? ""
							: challanOrEntrydate.Value.ToString("dd/MM/yyyy"));
						row.CreateCell(6).CellStyle = style;
						row.GetCell(6).SetCellValue(data.VendorName);
						row.CreateCell(7).CellStyle = style;
						row.GetCell(7).SetCellValue(data.VendorAddress);
						row.CreateCell(8).CellStyle = style;
						row.GetCell(8).SetCellValue(data.VendorBinOrNid);
						row.CreateCell(9).CellStyle = style;
						row.GetCell(9).SetCellValue(data.ProductName);
						row.CreateCell(10).CellStyle = style;
						row.GetCell(10).SetCellValue(data.PurchaseQty.ToString());
						row.CreateCell(11).CellStyle = style;
						row.GetCell(11).SetCellValue(data.PriceWithoutVat.ToString());
						row.CreateCell(12).CellStyle = style;
						row.GetCell(12).SetCellValue(data.VATAmount.ToString());
						row.CreateCell(13).CellStyle = style;
						row.GetCell(13).SetCellValue(data.VATAmount.ToString());
						row.CreateCell(14).CellStyle = style;
						row.GetCell(14).SetCellValue(data.TotalProdQty.ToString());
						row.CreateCell(15).CellStyle = style;
						row.GetCell(15).SetCellValue(data.TotalProdPrice.ToString());
						row.CreateCell(16).CellStyle = style;
						row.GetCell(16).SetCellValue(data.UsedInProductionQty.ToString());
						row.CreateCell(17).CellStyle = style;
						row.GetCell(17).SetCellValue(data.PriceWithoutVatForUsedInProduction.ToString());
						row.CreateCell(18).CellStyle = style;
						row.GetCell(18).SetCellValue(data.ClosingProdQty.ToString());
						row.CreateCell(19).CellStyle = style;
						row.GetCell(19).SetCellValue(data.ClosingTotalPrice.ToString());
						row.CreateCell(20).CellStyle = style;
						row.GetCell(20).SetCellValue("");
						rowCounter++;
					}
				}
				else
				{
					row = excelSheet.CreateRow(rowCounter);
					row.CreateCell(0).CellStyle = styleHeading;
					row.GetCell(0).SetCellValue("No Data Found");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));
					rowCounter++;
				}


				row = excelSheet.CreateRow(rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue("বিশেষ দ্রষ্টব্য : ");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));


				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0)
					.SetCellValue(
						"১. অর্থনৈতিক কার্যক্রম সংশ্লিষ্ট সকল প্রকার ক্রয়ের তথ্য এই ফরমে অন্তর্ভুক্ত করিতে হইবে।");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0).SetCellValue(
					"২. যে ক্ষেত্রে অনিবন্ধিত ব্যক্তির নিকট হইতে পণ্য ক্রয় করা হইবে সেই ক্ষেত্রে উক্ত ব্যক্তির পূর্ণাঙ্গ নাম, ঠিকানা ও জাতীয় পরিচয়পত্র নম্বর যথাযথভাবে সংশ্লিষ্ট কলাম [(৭),(৮) ও (৯)] এ আবশ্যিকভাবে উল্লেখ করিতে হইবে।");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

				row = excelSheet.CreateRow(++rowCounter);
				row.CreateCell(0)
					.SetCellValue(
						"৩. পণ্য ক্রয়ের সাপেক্ষে প্রামানিক দলিল হিসাবে বিল অব এন্ট্রি বা চালানের কপি সংরক্ষণ করিতে হইবে।");
				excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));


				for (int i = 0; i <= 20; i++)
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
		// else
		// {
		// 	return RedirectToAction(nameof(MushakSixPointOne));
		// }
	}

	#endregion

	#region Mushak- 6.2 (Sales Book)

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2)]
	public async Task<IActionResult> MushakSixPointTwo()
	{
		var model = new VmCalculationBookParam();
		var priceSetupList = await _productService.Query()
			.Where(x => x.OrganizationId == UserSession.OrganizationId && x.ProductType.IsSellable == true)
			.SelectAsync();
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem
		{
			Text = s.Name + (string.IsNullOrEmpty(s.ModelNo) ? "" : $" ({s.ModelNo})") +
			       (string.IsNullOrEmpty(s.Size) ? "" : $" ({s.Size})") +
			       (string.IsNullOrEmpty(s.Weight) ? "" : $" ({s.Weight})"),
			Value = s.ProductId.ToString()
		});
		model.BranchList = await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);
		model.ProductId = 0;
		model.VendorId = 0;
		model.OgrId = 0;
		model.Year = DateTime.Now.Year;
		model.Month = DateTime.Now.Month;

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2)]
	public async Task<IActionResult> MushakSixPointTwo(VmCalculationBookParam model)
	{
		var form = (DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");
		var to = (DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");
		model.OgrId = UserSession.OrganizationId;
		model.FromDateToDisplay =
			(DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString(
				MessageStaticData.DATE_DISPLAY_FORMAT);
		model.ToDateToDisplay =
			(DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString(MessageStaticData
				.DATE_DISPLAY_FORMAT);

		model.SaleCalcBook = await _mushakGenerationService.Mushak6P2(model.ProductId, UserSession.OrganizationId, form,
			to, model.OgrBranchId??0);

		var priceSetupList = await _productService.Query()
			.Where(x => x.OrganizationId == UserSession.OrganizationId && x.ProductType.IsSellable == true)
			.SelectAsync();
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem
		{
			Text = s.Name + (string.IsNullOrEmpty(s.ModelNo) ? "" : $" ({s.ModelNo})") +
			       (string.IsNullOrEmpty(s.Size) ? "" : $" ({s.Size})") +
			       (string.IsNullOrEmpty(s.Weight) ? "" : $" ({s.Weight})"),
			Value = s.ProductId.ToString()
		});
		model.BranchList = await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);

		var org = await _organizationService.GetOrganization(
			IvatDataProtector.Protect(UserSession.OrganizationId.ToString()));
		if (org != null)
		{
			model.OrgAddress = org.Address;
			model.OrgBin = org.Bin;
		}

		model.OrgName = UserSession.OrganizationName;

		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2)]
	public async Task<IActionResult> MushakSixPointTwoNew()
	{
		var model = new VmCalculationBookParam();
		var priceSetupList = await _productService.Query()
			.Where(x => x.OrganizationId == UserSession.OrganizationId && x.ProductType.IsSellable == true)
			.SelectAsync();
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem
		{
			Text = s.Name + (string.IsNullOrEmpty(s.ModelNo) ? "" : $" ({s.ModelNo})") +
				   (string.IsNullOrEmpty(s.Size) ? "" : $" ({s.Size})") +
				   (string.IsNullOrEmpty(s.Weight) ? "" : $" ({s.Weight})"),
			Value = s.ProductId.ToString()
		});
		model.BranchList = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
		model.ProductId = 0;
		model.VendorId = 0;
		model.OgrId = 0;
		model.Year = DateTime.Now.Year;
		model.Month = DateTime.Now.Month;

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2)]
	public async Task<IActionResult> MushakSixPointTwoNew(VmCalculationBookParam model)
	{

		var form = model.FromDate.Value.ToString("yyyy-MM-dd");
		var to = model.ToDate.Value.ToString("yyyy-MM-dd");
		model.OgrId = UserSession.OrganizationId;
		var saleCalcBook = await _mushakGenerationService.Mushak6P2(model.ProductId, UserSession.OrganizationId, form,
			to, model.OgrBranchId ?? 0);

		return ProcessReport(saleCalcBook,
			RdlcReportFileOption.MushakSixPointTwoReportUrl,
			RdlcReportFileOption.MushakSixPointTwoReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.MushakSixPointTwoReportFileName),
			GetParameterForMushakReport(model.FromDate.Value, model.ToDate.Value, "Mushak- 6.2"), model.ReportProcessOptionId
			);

	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2)]
	public async Task<ActionResult> MushakSixPointTwoExportToExcel(VmCalculationBookParam model)
	{
		try
		{
			// if (ModelState.IsValid)
			{
				var form = (DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");

				var to = (DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");
				model.FromDateToDisplay =
					(DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString(MessageStaticData
						.DATE_DISPLAY_FORMAT);
				model.ToDateToDisplay =
					(DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString(MessageStaticData
						.DATE_DISPLAY_FORMAT);
				model.OgrId = UserSession.OrganizationId;
				var mushak6p2 = await _mushakGenerationService.Mushak6P2(model.ProductId, UserSession.OrganizationId, form,
					to, model.OgrBranchId ?? 0);

				var orgInfo = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);

				string sWebRootFolder = Environment.WebRootPath;
				sWebRootFolder = Path.Combine(sWebRootFolder, "ExportExcel");
				if (!Directory.Exists(sWebRootFolder))
				{
					Directory.CreateDirectory(sWebRootFolder);
				}

				string sFileName = @"Mushak_6.2.xlsx";
				string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
				FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
				var memory = new MemoryStream();
				using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create,
					       FileAccess.Write))
				{
					IWorkbook workbook;
					workbook = new XSSFWorkbook();
					ISheet excelSheet = workbook.CreateSheet("Mushak6.2");

					ICellStyle style = workbook.CreateCellStyle();
					ICellStyle styleHeading = workbook.CreateCellStyle();
					ICellStyle styleSubHeading = workbook.CreateCellStyle();
					IFont fontHeading = workbook.CreateFont();
					fontHeading.FontHeightInPoints = 18;
					styleHeading.Alignment = HorizontalAlignment.Center;
					styleHeading.VerticalAlignment = VerticalAlignment.Center;
					styleHeading.SetFont(fontHeading);
					styleHeading.WrapText = true;

					IFont fontSubHeading = workbook.CreateFont();
					fontSubHeading.FontHeightInPoints = 14;
					styleSubHeading.Alignment = HorizontalAlignment.Center;
					styleSubHeading.VerticalAlignment = VerticalAlignment.Center;
					styleSubHeading.SetFont(fontSubHeading);
					styleSubHeading.WrapText = true;

					style.Alignment = HorizontalAlignment.Center;
					style.VerticalAlignment = VerticalAlignment.Center;
					style.WrapText = true;

					int rowCounter = 0;

					IRow row = excelSheet.CreateRow(rowCounter);
					row.CreateCell(0).CellStyle = styleHeading;
					row.GetCell(0).SetCellValue(orgInfo.Name);
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));
					excelSheet.DefaultRowHeightInPoints = 24;

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = styleSubHeading;
					row.GetCell(0).SetCellValue(orgInfo.Address);
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));
					excelSheet.DefaultRowHeightInPoints = 20;

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = styleSubHeading;
					row.GetCell(0).SetCellValue(orgInfo.Bin);
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));
					excelSheet.DefaultRowHeightInPoints = 20;

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "বিক্রয় হিসাব পুস্তক" : "Sales Ledger");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0
						? "(পণ্য বা সেবা প্রক্রিয়াকরণে সম্পৃক্ত  এমন নিবন্ধিত বা তালিকাভুক্ত ব্যক্তির জন্য প্রযোজ্য)"
						: "(Applicable to registered or enlisted persons involved in the processing of goods or services)");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0
						? "[বিধি ৪০ (১) এর দফা (ক) ও বিধি ৪১ এর দফা (ক) দ্রষ্টব্য]"
						: "[Note clause (a) of rule 40 (1) and clause (a) of rule 41]");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));


					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("From : " + model.FromDateToDisplay + " - To: " + model.ToDateToDisplay);
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "ক্রমিক সংখ্যা" : "Serial No.");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter + 1, 0, 0));
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "তারিখ" : "Date");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter + 1, 1, 1));
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0
						? "উৎপাদিত পণ্য/সেবার প্রারম্ভিক জের"
						: "Initial Cost of the Product/Service Produced");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 2, 3));
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? "উৎপাদন" : "Production");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 5));
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0
						? "মোট উৎপাদিত পণ্য/সেবার"
						: "Total Products/Services Produced");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 6, 7));
					row.CreateCell(8).CellStyle = style;
					row.GetCell(8).SetCellValue(model.Language == 0 ? "ক্রেতা/সরবরাহকারী" : "Buyer/Supplier");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 8, 10));
					row.CreateCell(11).CellStyle = style;
					row.GetCell(11).SetCellValue(model.Language == 0 ? "চালানপত্রের বিবরণ" : "Invoice Details");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 11, 12));
					row.CreateCell(13).CellStyle = style;
					row.GetCell(13).SetCellValue(model.Language == 0
						? "বিক্রিত/সরবরাহকৃত পণ্যের বিবরণ"
						: "Description of Products Sold/Supplied");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 13, 17));
					row.CreateCell(18).CellStyle = style;
					row.GetCell(18).SetCellValue(model.Language == 0 ? "পণ্যের প্রান্তিক জের" : "Product Margin");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 18, 19));
					row.CreateCell(20).CellStyle = style;
					row.GetCell(20).SetCellValue(model.Language == 0 ? "মন্তব্য" : "Remarks");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter + 1, 20, 20));


					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : @"Quantity (Unit)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 2, 2));
					row.CreateCell(3).CellStyle = style;
					row.GetCell(3).SetCellValue(model.Language == 0
						? @"মূল্য (সকল প্রকার কর ব্যতীত)"
						: "Price (Excluding All Types of Taxes)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 3, 3));
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : @"Quantity (Unit)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 4));
					row.CreateCell(5).CellStyle = style;
					row.GetCell(5).SetCellValue(model.Language == 0
						? @"মূল্য (সকল প্রকার কর ব্যতীত)"
						: "Price (Excluding All Types of Taxes)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 5, 5));
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : @"Quantity (Unit)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 6, 6));
					row.CreateCell(7).CellStyle = style;
					row.GetCell(7).SetCellValue(model.Language == 0
						? @"মূল্য (সকল প্রকার কর ব্যতীত)"
						: "Price (Excluding All Types of Taxes)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 7, 7));
					row.CreateCell(8).CellStyle = style;
					row.GetCell(8).SetCellValue(model.Language == 0 ? "নাম" : "Name");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 8, 8));
					row.CreateCell(9).CellStyle = style;
					row.GetCell(9).SetCellValue(model.Language == 0 ? "ঠিকানা" : "Address");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 9, 9));
					row.CreateCell(10).CellStyle = style;
					row.GetCell(10).SetCellValue(model.Language == 0
						? "নিবন্ধন/তালিকাভুক্তি/জাতীয় পরিচয়পত্র"
						: "Registration/Enrollment/National Identity Card");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 10, 10));
					row.CreateCell(11).CellStyle = style;
					row.GetCell(11).SetCellValue(model.Language == 0 ? "নম্বর" : "Number");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 11, 11));
					row.CreateCell(12).CellStyle = style;
					row.GetCell(12).SetCellValue(model.Language == 0 ? "তারিখ" : "Date");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 12, 12));
					row.CreateCell(13).CellStyle = style;
					row.GetCell(13).SetCellValue(model.Language == 0 ? "বিবরণ" : "Description");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 13, 13));
					row.CreateCell(14).CellStyle = style;
					row.GetCell(14).SetCellValue(model.Language == 0 ? "পরিমাণ" : "Quantity");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 14, 14));
					row.CreateCell(15).CellStyle = style;
					row.GetCell(15).SetCellValue(model.Language == 0
						? @"করযোগ্য মূল্য (সকল প্রকার)"
						: @"Taxable Value (All Types)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 15, 15));
					row.CreateCell(16).CellStyle = style;
					row.GetCell(16).SetCellValue(model.Language == 0
						? @"সম্পূরক শুল্ক(যদি থাকে)"
						: @"Supplementary Duty (If Any)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 16, 16));
					row.CreateCell(17).CellStyle = style;
					row.GetCell(17).SetCellValue(model.Language == 0 ? "মূসক" : "Mushak");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 17, 17));
					row.CreateCell(18).CellStyle = style;
					row.GetCell(18).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : @"Quantity (Unit)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 18, 18));
					row.CreateCell(19).CellStyle = style;
					row.GetCell(19).SetCellValue(model.Language == 0
						? @"মূল্য (সকল প্রকার কর ব্যতীত)"
						: "Price (Excluding All Types of Taxes)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 19, 19));


					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "(১)" : "(1)");
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "(২)" : "(2)");
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0 ? "(৩)" : "(3)");
					row.CreateCell(3).CellStyle = style;
					row.GetCell(3).SetCellValue(model.Language == 0 ? "(৪)" : "(4)");
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? "৫" : "(5)");
					row.CreateCell(5).CellStyle = style;
					row.GetCell(5).SetCellValue(model.Language == 0 ? "(৬)" : "(6)");
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0 ? "(৭)" : "(7)");
					row.CreateCell(7).CellStyle = style;
					row.GetCell(7).SetCellValue(model.Language == 0 ? "(৮)" : "(8)");
					row.CreateCell(8).CellStyle = style;
					row.GetCell(8).SetCellValue(model.Language == 0 ? "(৯)" : "(9)");
					row.CreateCell(9).CellStyle = style;
					row.GetCell(9).SetCellValue(model.Language == 0 ? "(১০)" : "(10)");
					row.CreateCell(10).CellStyle = style;
					row.GetCell(10).SetCellValue(model.Language == 0 ? "(১১)" : "(11)");
					row.CreateCell(11).CellStyle = style;
					row.GetCell(11).SetCellValue(model.Language == 0 ? "(১২)" : "(12)");
					row.CreateCell(12).CellStyle = style;
					row.GetCell(12).SetCellValue(model.Language == 0 ? "(১৩)" : "(13)");
					row.CreateCell(13).CellStyle = style;
					row.GetCell(13).SetCellValue(model.Language == 0 ? "(১৪)" : "(14)");
					row.CreateCell(14).CellStyle = style;
					row.GetCell(14).SetCellValue(model.Language == 0 ? "(১৫)" : "(15)");
					row.CreateCell(15).CellStyle = style;
					row.GetCell(15).SetCellValue(model.Language == 0 ? "(১৬)" : "(16)");
					row.CreateCell(16).CellStyle = style;
					row.GetCell(16).SetCellValue(model.Language == 0 ? "(১৭)" : "(17)");
					row.CreateCell(17).CellStyle = style;
					row.GetCell(17).SetCellValue(model.Language == 0 ? "(১৮)" : "(18)");
					row.CreateCell(18).CellStyle = style;
					row.GetCell(18).SetCellValue(model.Language == 0 ? "(১৯)" : "(19)");
					row.CreateCell(19).CellStyle = style;
					row.GetCell(19).SetCellValue(model.Language == 0 ? "(২০)" : "(20)");
					row.CreateCell(20).CellStyle = style;
					row.GetCell(20).SetCellValue(model.Language == 0 ? "(২১)" : "(21)");


					foreach (var data in mushak6p2)
					{
						row = excelSheet.CreateRow(++rowCounter);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue(data.SlNo.ToString());
						row.CreateCell(1).CellStyle = style;
						row.GetCell(1).SetCellValue(data.OperationTime.ToString());
						row.CreateCell(2).CellStyle = style;
						row.GetCell(2).SetCellValue(data.InitialQty + " (" + data.MeasurementUnitName + ")");
						row.CreateCell(3).CellStyle = style;
						row.GetCell(3).SetCellValue(data.InitPriceWithoutVat.ToString());
						row.CreateCell(4).CellStyle = style;
						row.GetCell(4).SetCellValue(data.ProductionQty + " (" + data.MeasurementUnitName + ")");
						row.CreateCell(5).CellStyle = style;
						row.GetCell(5).SetCellValue(data.PriceOfProdFromProduction.ToString());
						row.CreateCell(6).CellStyle = style;
						row.GetCell(6).SetCellValue(data.TotalProductionQty + " (" + data.MeasurementUnitName + ")");
						row.CreateCell(7).CellStyle = style;
						row.GetCell(7).SetCellValue(data.TotalProductionPrice.ToString());
						row.CreateCell(8).CellStyle = style;
						row.GetCell(8).SetCellValue(data.CustomerName);
						row.CreateCell(9).CellStyle = style;
						row.GetCell(9).SetCellValue(data.CustomerAddress);
						row.CreateCell(10).CellStyle = style;
						row.GetCell(10).SetCellValue(data.CustomerBinOrNid);
						row.CreateCell(11).CellStyle = style;
						row.GetCell(11).SetCellValue(data.VatChallanNo);
						row.CreateCell(12).CellStyle = style;
						row.GetCell(12).SetCellValue(data.SalesDate.ToString());
						row.CreateCell(13).CellStyle = style;
						row.GetCell(13).SetCellValue(data.ProductName);
						row.CreateCell(14).CellStyle = style;
						row.GetCell(14).SetCellValue(data.SalesQty + " (" + data.MeasurementUnitName + ")");
						row.CreateCell(15).CellStyle = style;
						row.GetCell(15).SetCellValue(data.TaxablePrice.ToString());
						row.CreateCell(16).CellStyle = style;
						row.GetCell(16).SetCellValue(data.SupplementaryDuty.ToString());
						row.CreateCell(17).CellStyle = style;
						row.GetCell(17).SetCellValue(data.ProdVatAmount.ToString());
						row.CreateCell(18).CellStyle = style;
						row.GetCell(18).SetCellValue(data.ClosingProdQty.ToString());
						row.CreateCell(19).CellStyle = style;
						row.GetCell(19).SetCellValue(data.ClosingProdPrice.ToString());
						row.CreateCell(20).CellStyle = style;
						row.GetCell(20).SetCellValue("");
					}

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("বিশেষ দ্রষ্টব্য : ");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue(
						"* যে ক্ষেত্রে অনিবন্ধিত ব্যক্তির নিকট হইতে পণ্য বিক্রয় করা হইবে সেই ক্ষেত্রে উক্ত ব্যক্তির পূর্ণাঙ্গ নাম, ঠিকানা ও জাতীয় পরিচয়পত্র নম্বর যথাযথভাবে সংশ্লিষ্ট কলাম [(৯),(১০)ও(১১)] এ আবশ্যিকভাবে উল্লেখ করিতে হইবে।");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 20));

					for (int i = 0; i <= 20; i++)
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
			// else
			// {
			// 	return RedirectToAction(nameof(MushakSixPointTwo), model);
			// }
		}
		catch
		{
			return RedirectToAction(nameof(MushakSixPointTwo), model);
		}
	}

	#endregion

	#region Mushak- 6.2.1 (Purchase and Sales Book)

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2_1)]
	public async Task<IActionResult> MushakSixPointTwoPointOne()
	{
		var model = new VmCalculationBookParam();
		var priceSetupList = await _productService.Query()
			.Where(x => x.IsActive && x.OrganizationId == UserSession.OrganizationId).SelectAsync();
		model.PriceSetupList =
			priceSetupList.Select(s => new SelectListItem { Text = s.Name, Value = s.ProductId.ToString() });
		model.ProductId = 0;
		model.VendorId = 0;
		model.OgrId = 0;
		model.Year = DateTime.Now.Year;
		model.Month = DateTime.Now.Month;
		model.BranchList = await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);

		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2_1)]
	public async Task<IActionResult> MushakSixPointTwoPointOneNew()
	{
		var model = new VmCalculationBookParam();
		var priceSetupList = await _productService.Query()
			.Where(x => x.IsActive && x.OrganizationId == UserSession.OrganizationId).SelectAsync();
		model.PriceSetupList =
			priceSetupList.Select(s => new SelectListItem { Text = s.Name, Value = s.ProductId.ToString() });
		model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
		model.ProductId = 0;
		model.VendorId = 0;
		model.OgrId = 0;
		model.Year = DateTime.Now.Year;
		model.Month = DateTime.Now.Month;
		model.BranchList = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2_1)]
	public async Task<IActionResult> MushakSixPointTwoPointOneNew(VmCalculationBookParam model)
	{

		var form = model.FromDate.Value.ToString("yyyy-MM-dd");
		var to = model.ToDate.Value.ToString("yyyy-MM-dd");
		model.OgrId = UserSession.OrganizationId;
		var purchaseSaleCalcBook = await _mushakGenerationService.Mushak6P2P1(model.ProductId,
			UserSession.OrganizationId, model.OgrBranchId ?? 0, form, to, model.VendorId, model.CustomerId);

		return ProcessReport(purchaseSaleCalcBook,
			RdlcReportFileOption.MushakSixPointTowPointOneRdlcUrl,
			RdlcReportFileOption.MushakSixPointTowPointOneRdlcUrlDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.MushakSixPointTowPointOneRdlcUrlFileName),
			GetParameterForMushakReport(model.FromDate.Value, model.ToDate.Value, "Mushak- 6.2.1"), model.ReportProcessOptionId
			);

	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2_1)]
	public async Task<IActionResult> MushakSixPointTwoPointOne(VmCalculationBookParam model)
	{
		var form = (DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");
		var to = (DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");

		model.OgrId = UserSession.OrganizationId;
		model.FromDateToDisplay =
			(DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString(
				MessageStaticData.DATE_DISPLAY_FORMAT);
		model.ToDateToDisplay =
			(DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString(MessageStaticData
				.DATE_DISPLAY_FORMAT);
		var priceSetupList = await _productService.Query()
			.Where(x => x.IsActive && x.OrganizationId == UserSession.OrganizationId).SelectAsync();
		model.PriceSetupList =
			priceSetupList.Select(s => new SelectListItem { Text = s.Name, Value = s.ProductId.ToString() });
		model.BranchList = await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);
		model.PurchaseSaleCalcBook = await _mushakGenerationService.Mushak6P2P1(model.ProductId,
			UserSession.OrganizationId, model.OgrBranchId ?? 0, form, to, model.VendorId, model.CustomerId);

		var org = await _organizationService.GetOrganization(
			IvatDataProtector.Protect(UserSession.OrganizationId.ToString()));
		if (org != null)
		{
			model.OrgAddress = org.Address;
			model.OrgBin = org.Bin;
		}

		model.OrgName = UserSession.OrganizationName;

		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2_1)]
	public async Task<ActionResult> MushakSixPointTwoPointOneExportToExcel(VmCalculationBookParam model)
	{
		try
		{
			// if (ModelState.IsValid)
			{
				var form = (DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");

				var to = (DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString("yyyy-MM-dd");
				model.FromDateToDisplay =
					(DateCalculator.GetFirstDateOfMonth(model.Year, model.Month)).ToString(MessageStaticData
						.DATE_DISPLAY_FORMAT);
				model.ToDateToDisplay =
					(DateCalculator.GetLastDateOfMonth(model.Year, model.Month)).ToString(MessageStaticData
						.DATE_DISPLAY_FORMAT);
				model.OgrId = UserSession.OrganizationId;
				var mushak6p2p1 = await _mushakGenerationService.Mushak6P2P1(model.ProductId,
					UserSession.OrganizationId, model.OgrBranchId ?? 0, form, to, model.VendorId,
					model.CustomerId); //todo: fix branch

				var orgInfo = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);

				string sWebRootFolder = Environment.WebRootPath;
				sWebRootFolder = Path.Combine(sWebRootFolder, "ExportExcel");
				if (!Directory.Exists(sWebRootFolder))
				{
					Directory.CreateDirectory(sWebRootFolder);
				}

				string sFileName = @"Mushak_6.2.1.xlsx";
				string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
				FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
				var memory = new MemoryStream();
				using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create,
					       FileAccess.Write))
				{
					IWorkbook workbook;
					workbook = new XSSFWorkbook();
					ISheet excelSheet = workbook.CreateSheet("Mushak6.2.1");

					ICellStyle style = workbook.CreateCellStyle();
					ICellStyle styleHeading = workbook.CreateCellStyle();
					ICellStyle styleSubHeading = workbook.CreateCellStyle();
					IFont fontHeading = workbook.CreateFont();
					fontHeading.FontHeightInPoints = 18;
					styleHeading.Alignment = HorizontalAlignment.Center;
					styleHeading.VerticalAlignment = VerticalAlignment.Center;
					styleHeading.SetFont(fontHeading);
					styleHeading.WrapText = true;

					IFont fontSubHeading = workbook.CreateFont();
					fontSubHeading.FontHeightInPoints = 14;
					styleSubHeading.Alignment = HorizontalAlignment.Center;
					styleSubHeading.VerticalAlignment = VerticalAlignment.Center;
					styleSubHeading.SetFont(fontSubHeading);
					styleSubHeading.WrapText = true;

					style.Alignment = HorizontalAlignment.Center;
					style.VerticalAlignment = VerticalAlignment.Center;
					style.WrapText = true;

					int rowCounter = 0;

					IRow row = excelSheet.CreateRow(rowCounter);
					row.CreateCell(0).CellStyle = styleHeading;
					row.GetCell(0).SetCellValue(orgInfo.Name);
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));
					excelSheet.DefaultRowHeightInPoints = 24;

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = styleSubHeading;
					row.GetCell(0).SetCellValue(orgInfo.Address);
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));
					excelSheet.DefaultRowHeightInPoints = 20;

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = styleSubHeading;
					row.GetCell(0).SetCellValue(orgInfo.Bin);
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));
					excelSheet.DefaultRowHeightInPoints = 20;

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "ক্রয়-বিক্রয় হিসাব" : "Purchase-Sale Book");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0
						? "(পণ্য বা সেবা প্রক্রিয়াকরণে সম্পৃক্ত  এমন নিবন্ধিত বা তালিকাভুক্ত ব্যক্তির জন্য প্রযোজ্য)"
						: "Materials-Product Coefficient (Input-Output Coefficient) Declaration");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0
						? "[বিধি ৪০ এর উপ-বিধি (১) এর দফা (খখ) ও বিধি ৪১ (ক) দ্রষ্টব্য]"
						: "[Note clause (b) of sub-rule (1) of rule 40 and rule 41 (a)]");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));


					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("From : " + model.FromDateToDisplay + " - To: " + model.ToDateToDisplay);
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "ক্রমিক সংখ্যা" : "Serial No.");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter + 1, 0, 0));
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "তারিখ" : "Date");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter + 1, 1, 1));
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0
						? "বিক্রয়যোগ্য পণ্যের প্রারম্ভিক"
						: "Salable Products Initial");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 2, 3));
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? "ক্রয়" : "Purchase");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 5));
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0 ? "মোট পণ্য" : "Total Products");
					excelSheet.AddMergedRegion(new CellRangeAddress(10, 10, 6, 7));
					row.CreateCell(8).CellStyle = style;
					row.GetCell(8).SetCellValue(model.Language == 0 ? "বিক্রেতার তথ্য" : "Vendor Information");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 8, 10));
					row.CreateCell(11).CellStyle = style;
					row.GetCell(11)
						.SetCellValue(model.Language == 0 ? "ক্রয় চালানপত্রের/বিল" : "Purchase Invoice/Bill");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 11, 12));
					row.CreateCell(13).CellStyle = style;
					row.GetCell(13).SetCellValue(model.Language == 0
						? "বিক্রিত/সরবরাহকৃত পণ্যের বিবরণ"
						: "Description of Products Sold/Supplied");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 13, 17));
					row.CreateCell(18).CellStyle = style;
					row.GetCell(18).SetCellValue(model.Language == 0 ? "ক্রেতার তথ্য" : "Buyer Information");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 18, 20));
					row.CreateCell(21).CellStyle = style;
					row.GetCell(21)
						.SetCellValue(model.Language == 0 ? "বিক্রয় চালানপত্রের বিবরণ" : "Details of Sales Invoices");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 21, 22));
					row.CreateCell(23).CellStyle = style;
					row.GetCell(23).SetCellValue(model.Language == 0 ? "পণ্যের প্রান্তিক জের" : "Product Margin");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 23, 24));
					row.CreateCell(25).CellStyle = style;
					row.GetCell(25).SetCellValue(model.Language == 0 ? "মন্তব্য" : "Remarks");
					//excelSheet.AddMergedRegion(new CellRangeAddress(9, rowCounter, 25, 25));


					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : @"Quantity (Unit)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 2, 2));
					row.CreateCell(3).CellStyle = style;
					row.GetCell(3).SetCellValue(model.Language == 0
						? @"মূল্য (সকল প্রকার কর ব্যতীত)"
						: "Price (Excluding All Types of Taxes)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 3, 3));
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : @"Quantity (Unit)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 4));
					row.CreateCell(5).CellStyle = style;
					row.GetCell(5).SetCellValue(model.Language == 0
						? @"মূল্য (সকল প্রকার কর ব্যতীত)"
						: "Price (Excluding All Types of Taxes)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 5, 5));
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : @"Quantity (Unit)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 6, 6));
					row.CreateCell(7).CellStyle = style;
					row.GetCell(7).SetCellValue(model.Language == 0
						? @"মূল্য (সকল প্রকার কর ব্যতীত)"
						: "Price (Excluding All Types of Taxes)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 7, 7));
					row.CreateCell(8).CellStyle = style;
					row.GetCell(8).SetCellValue(model.Language == 0 ? "নাম" : "Name");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 8, 8));
					row.CreateCell(9).CellStyle = style;
					row.GetCell(9).SetCellValue(model.Language == 0 ? "ঠিকানা" : "Address");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 9, 9));
					row.CreateCell(10).CellStyle = style;
					row.GetCell(10).SetCellValue(model.Language == 0
						? "নিবন্ধন/তালিকাভুক্তি/জাতীয় পরিচয়পত্র"
						: "Registration/Enrollment/National Identity Card");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 10, 10));
					row.CreateCell(11).CellStyle = style;
					row.GetCell(11).SetCellValue(model.Language == 0 ? "নম্বর" : "Number");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 11, 11));
					row.CreateCell(12).CellStyle = style;
					row.GetCell(12).SetCellValue(model.Language == 0 ? "তারিখ" : "Date");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 12, 12));
					row.CreateCell(13).CellStyle = style;
					row.GetCell(13).SetCellValue(model.Language == 0 ? "বিবরণ" : "Description");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 13, 13));
					row.CreateCell(14).CellStyle = style;
					row.GetCell(14).SetCellValue(model.Language == 0 ? "পরিমাণ" : "Quantity");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 14, 14));
					row.CreateCell(15).CellStyle = style;
					row.GetCell(15).SetCellValue(model.Language == 0
						? @"করযোগ্য মূল্য (সকল প্রকার)"
						: @"Taxable Value (All Types)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 15, 15));
					row.CreateCell(16).CellStyle = style;
					row.GetCell(16).SetCellValue(model.Language == 0
						? @"সম্পূরক শুল্ক(যদি থাকে)"
						: @"Supplementary Duty (If Any)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 16, 16));
					row.CreateCell(17).CellStyle = style;
					row.GetCell(17).SetCellValue(model.Language == 0 ? "মূসক" : "Mushak");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 17, 17));
					row.CreateCell(18).CellStyle = style;
					row.GetCell(18).SetCellValue(model.Language == 0 ? "নাম" : "Name");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 18, 18));
					row.CreateCell(19).CellStyle = style;
					row.GetCell(19).SetCellValue(model.Language == 0 ? "ঠিকানা" : "Address");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 19, 19));
					row.CreateCell(20).CellStyle = style;
					row.GetCell(20).SetCellValue(model.Language == 0
						? "নিবন্ধন/তালিকাভুক্তি/জাতীয় পরিচয়পত্র"
						: "Registration/Enrollment/National Identity Card");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 20, 20));
					row.CreateCell(21).CellStyle = style;
					row.GetCell(21).SetCellValue(model.Language == 0 ? "নম্বর" : "Number");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 21, 21));
					row.CreateCell(22).CellStyle = style;
					row.GetCell(22).SetCellValue(model.Language == 0 ? "তারিখ" : "Date");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 22, 22));
					row.CreateCell(23).CellStyle = style;
					row.GetCell(23).SetCellValue(model.Language == 0 ? @"পরিমাণ (একক)" : @"Quantity (Unit)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 23, 23));
					row.CreateCell(24).CellStyle = style;
					row.GetCell(24).SetCellValue(model.Language == 0
						? @"মূল্য (সকল প্রকার কর ব্যতীত)"
						: "Price (Excluding All Types of Taxes)");
					// excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 24, 24));


					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "(১)" : "(1)");
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "(২)" : "(2)");
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0 ? "(৩)" : "(3)");
					row.CreateCell(3).CellStyle = style;
					row.GetCell(3).SetCellValue(model.Language == 0 ? "(৪)" : "(4)");
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? "৫" : "(5)");
					row.CreateCell(5).CellStyle = style;
					row.GetCell(5).SetCellValue(model.Language == 0 ? "(৬)" : "(6)");
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0 ? "(৭)" : "(7)");
					row.CreateCell(7).CellStyle = style;
					row.GetCell(7).SetCellValue(model.Language == 0 ? "(৮)" : "(8)");
					row.CreateCell(8).CellStyle = style;
					row.GetCell(8).SetCellValue(model.Language == 0 ? "(৯)" : "(9)");
					row.CreateCell(9).CellStyle = style;
					row.GetCell(9).SetCellValue(model.Language == 0 ? "(১০)" : "(10)");
					row.CreateCell(10).CellStyle = style;
					row.GetCell(10).SetCellValue(model.Language == 0 ? "(১১)" : "(11)");
					row.CreateCell(11).CellStyle = style;
					row.GetCell(11).SetCellValue(model.Language == 0 ? "(১২)" : "(12)");
					row.CreateCell(12).CellStyle = style;
					row.GetCell(12).SetCellValue(model.Language == 0 ? "(১৩)" : "(13)");
					row.CreateCell(13).CellStyle = style;
					row.GetCell(13).SetCellValue(model.Language == 0 ? "(১৪)" : "(14)");
					row.CreateCell(14).CellStyle = style;
					row.GetCell(14).SetCellValue(model.Language == 0 ? "(১৫)" : "(15)");
					row.CreateCell(15).CellStyle = style;
					row.GetCell(15).SetCellValue(model.Language == 0 ? "(১৬)" : "(16)");
					row.CreateCell(16).CellStyle = style;
					row.GetCell(16).SetCellValue(model.Language == 0 ? "(১৭)" : "(17)");
					row.CreateCell(17).CellStyle = style;
					row.GetCell(17).SetCellValue(model.Language == 0 ? "(১৮)" : "(18)");
					row.CreateCell(18).CellStyle = style;
					row.GetCell(18).SetCellValue(model.Language == 0 ? "(১৯)" : "(19)");
					row.CreateCell(19).CellStyle = style;
					row.GetCell(19).SetCellValue(model.Language == 0 ? "(২০)" : "(20)");
					row.CreateCell(20).CellStyle = style;
					row.GetCell(20).SetCellValue(model.Language == 0 ? "(২১)" : "(21)");
					row.CreateCell(21).CellStyle = style;
					row.GetCell(21).SetCellValue(model.Language == 0 ? "(২২)" : "(22)");
					row.CreateCell(22).CellStyle = style;
					row.GetCell(22).SetCellValue(model.Language == 0 ? "(২৩)" : "(23)");
					row.CreateCell(23).CellStyle = style;
					row.GetCell(23).SetCellValue(model.Language == 0 ? "(২৪)" : "(24)");
					row.CreateCell(24).CellStyle = style;
					row.GetCell(24).SetCellValue(model.Language == 0 ? "(২৫)" : "(25)");
					row.CreateCell(25).CellStyle = style;
					row.GetCell(25).SetCellValue(model.Language == 0 ? "(২৬)" : "(26)");


					foreach (var data in mushak6p2p1)
					{
						row = excelSheet.CreateRow(++rowCounter);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue(data.SlNo.ToString());
						row.CreateCell(1).CellStyle = style;
						row.GetCell(1).SetCellValue(data.TransactionTime.ToString());
						row.CreateCell(2).CellStyle = style;
						row.GetCell(2).SetCellValue(data.InitialQty + " (" + data.MeasurementUnitName + ")");
						row.CreateCell(3).CellStyle = style;
						row.GetCell(3).SetCellValue(data.InitPriceWithoutVat.ToString());
						row.CreateCell(4).CellStyle = style;
						row.GetCell(4).SetCellValue(data.PurchaseQty + " (" + data.MeasurementUnitName + ")");
						row.CreateCell(5).CellStyle = style;
						row.GetCell(5).SetCellValue(data.PurchasePriceWithoutVat.ToString());
						row.CreateCell(6).CellStyle = style;
						row.GetCell(6).SetCellValue(data.QtyAfterPurchase + " (" + data.MeasurementUnitName + ")");
						row.CreateCell(7).CellStyle = style;
						row.GetCell(7).SetCellValue(data.ProductPriceWithoutVatAfterPurchase.ToString());
						row.CreateCell(8).CellStyle = style;
						row.GetCell(8).SetCellValue(data.VendorName);
						row.CreateCell(9).CellStyle = style;
						row.GetCell(9).SetCellValue(data.VendorAddress);
						row.CreateCell(10).CellStyle = style;
						row.GetCell(10).SetCellValue(data.VendorBinOrNidNo);
						row.CreateCell(11).CellStyle = style;
						row.GetCell(11).SetCellValue(data.PurcVatChallanOrBillOfEntryNo);
						row.CreateCell(12).CellStyle = style;
						row.GetCell(12).SetCellValue(data.PurcVatChallanOrBillOfEntryDate.ToString());
						row.CreateCell(13).CellStyle = style;
						row.GetCell(13).SetCellValue(data.ProductName);
						row.CreateCell(14).CellStyle = style;
						row.GetCell(14).SetCellValue(data.SoldQty + " (" + data.MeasurementUnitName + ")");
						row.CreateCell(15).CellStyle = style;
						row.GetCell(15).SetCellValue(data.SalesPriceWithoutVat.ToString());
						row.CreateCell(16).CellStyle = style;
						row.GetCell(16).SetCellValue(data.SalesSupplimentaryDuty.ToString());
						row.CreateCell(17).CellStyle = style;
						row.GetCell(17).SetCellValue(data.SalesVat.ToString());
						row.CreateCell(18).CellStyle = style;
						row.GetCell(18).SetCellValue(data.CustomerName);
						row.CreateCell(19).CellStyle = style;
						row.GetCell(19).SetCellValue(data.CustomerAddress);
						row.CreateCell(20).CellStyle = style;
						row.GetCell(20).SetCellValue(data.CustomerBinOrNidNo);
						row.CreateCell(21).CellStyle = style;
						row.GetCell(21).SetCellValue(data.SalesVatChallanNo);
						row.CreateCell(22).CellStyle = style;
						row.GetCell(22).SetCellValue(data.SalesVatChallanDate.ToString());
						row.CreateCell(23).CellStyle = style;
						row.GetCell(23).SetCellValue(data.ClosingProdQty + " (" + data.MeasurementUnitName + ")");
						row.CreateCell(24).CellStyle = style;
						row.GetCell(24).SetCellValue(data.ClosingTotalPriceWithoutVat.ToString());
						row.CreateCell(25).CellStyle = style;
						row.GetCell(25).SetCellValue("");
					}

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue("বিশেষ দ্রষ্টব্য : ");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue(
						"* যে ক্ষেত্রে অনিবন্ধিত ব্যক্তির নিকট হইতে পণ্য ক্রয় বা অনিবন্ধিত ব্যক্তির নিকট হইতে পণ্য বিক্রয় করা হইবে সেই ক্ষেত্রে উক্ত ব্যক্তির পূর্ণাঙ্গ নাম, ঠিকানা ও জাতীয় পরিচয়পত্র নম্বর যথাযথভাবে সংশ্লিষ্ট কলাম [(৯), (১০), (১১),(১৯),(২০)ও(২১)] এ আবশ্যিকভাবে উল্লেখ করিতে হইবে।");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 25));


					for (int i = 0; i <= 25; i++)
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
			// else
			// {
			// 	return RedirectToAction(nameof(MushakSixPointTwoPointOne), model);
			// }
		}
		catch
		{
			return RedirectToAction(nameof(MushakSixPointTwoPointOne), model);
		}
	}

	#endregion

	#region Mushak-6.3 (VAT Chalan)

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_3)]
	public async Task<IActionResult> MushakSixPointThreeList()
	{
		var getSales = await _saleService.GetSalesByOrganizationAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		return View(getSales.Where(s => s.IsComplete));
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_3)]
	public async Task<IActionResult> MushakSixPointThree(string id, int language)
	{
        try
		{
			
            var model = new vmMushak6P3ById
			{
				Language = language,
				SalesTaxInvoices = await _mushakGenerationService.Mushak6P3(int.Parse(IvatDataProtector.Unprotect(id))),
				InvoiceNameBan = UserSession.InvoiceNameBan,
				InvoiceNameEng = UserSession.InvoiceNameEng,
				IsSaleSimplified = UserSession.IsSaleSimplified,				
            };
			return View(model);
		}
		catch
		{
			return NotFound();
		}
	}

    [VmsAuthorize(FeatureList.MUSHAK)]
    [VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_3)]
    public async Task<IActionResult> MushakSixPointThreeRdlc(string id, int language)
    {
        try
        {
            var salestaxInvoices = await _mushakGenerationService.Mushak6P3(int.Parse(IvatDataProtector.Unprotect(id)));
            var amountInWords = VmsNumberToWord.ConvertAmountUsingTakaPoishaInEng(salestaxInvoices.Sum(x => x.ProdPriceInclVATAndDuty).Value);
            Dictionary<string, string> paramReport =
                       new Dictionary<string, string>();
			paramReport.Add("AmountInWordsParam", amountInWords);
            var model = new vmMushak6P3ById()
            {
                Language = language,
                SalesTaxInvoices = salestaxInvoices
            };
            return ProcessReport(model.SalesTaxInvoices.ToList(),
                RdlcReportFileOption.MushakSixPointThreeRdlcUrl,
                RdlcReportFileOption.MushakSixPointThreeRdlcUrlDsName,
                StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.MushakSixPointThreeRdlcUrlFileName),
                paramReport
                );
        }
        catch
        {
            return NotFound();
        }
    }

    #endregion

    #region Mushak-6.5 (Branch Transfer)

    [VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_3)]
	public async Task<IActionResult> MushakSixPointFiveList()
	{
		return View(
			await _branchTransferSendService.GetBranchTransferSendsByOrganization(UserSession.ProtectedOrganizationId));
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_3)]
	public async Task<IActionResult> MushakSixPointFive(string id, int language)
	{
		try
		{
			var model = new TransferChallanViewModel
			{
				Language = language,
				ChallanModel = await _branchTransferSendService.GetBranchTransferChallan(id, UserSession.UserId),
				InvoiceNameBan = UserSession.InvoiceNameBan,
				InvoiceNameEng = UserSession.InvoiceNameEng,
				IsSaleSimplified = UserSession.IsSaleSimplified
			};
			return View(model);
		}
		catch
		{
			return NotFound();
		}
	}

	#endregion

	#region Mushak-6.6 (VDS Certificate)

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6)]
	public async Task<IActionResult> MushakSixPointSixList()
	{
		return View(await _purchaseService.GetVdsPurchasesWithVdsPayment(UserSession.ProtectedOrganizationId));
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6)]
	public async Task<IActionResult> ViewAndSendEmailMushakSixPointSix(string id)
	{
		try
		{
			var model = new VmMushakSixPointSix
			{
				Language = EnumLanguage.English,
				VdsPurchaseCertificates =
					await _mushakGenerationService.Mushak6P6(int.Parse(IvatDataProtector.Unprotect(id)))
			};
			if (!model.VdsPurchaseCertificates.Any()) return NotFound();
			var singleInfo = model.VdsPurchaseCertificates.First();
			if (!string.IsNullOrEmpty(singleInfo.VendorEmailAddress))
			{
				MailMaster.SendEmailWithAttachment(singleInfo.VendorEmailAddress, "VDS Certificate",
					$"Dear Concern, <br/>Greetings! Hope you are doing great. We have attached VDS Certificate for Challan No: {singleInfo.VatChallanNo}, Date: {singleInfo.VatChallanIssueDate.DateTimeToStringWithoutTime()}. <br/><br/><strong>Finance and Accounts Team</strong>",
					new Dictionary<string, Stream>()
					{
						{
							$"{RdlcReportFileOption.VdsCertificateFileName}.pdf", GetPdfWithOpenStream(
								model.VdsPurchaseCertificates.ToList(), RdlcReportFileOption.VdsCertificateUrl,
								RdlcReportFileOption.VdsCertificateDsName,
								StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.VdsCertificateFileName))
						}
					});
			}


			return ProcessReport(model.VdsPurchaseCertificates.ToList(),
				RdlcReportFileOption.VdsCertificateUrl,
				RdlcReportFileOption.VdsCertificateDsName,
				StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.VdsCertificateFileName));
		}
		catch
		{
			return NotFound();
		}
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6)]
	public async Task<IActionResult> MushakSixPointSix(string id, int language)
	{
		try
		{
			var model = new VmMushakSixPointSix
			{
				Language = (EnumLanguage)language,
				VdsPurchaseCertificates =
					await _mushakGenerationService.Mushak6P6(int.Parse(IvatDataProtector.Unprotect(id)))
			};
			return View(model);
		}
		catch
		{
			return NotFound();
		}
	}

	#endregion

	#region TDS Certificate

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6)]
	public async Task<IActionResult> TdsCertificateList()
	{
		return View(await _purchaseService.GetVdsPurchasesWithTdsPayment(UserSession.ProtectedOrganizationId));
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6)]
	public async Task<IActionResult> TdsCertificate(string id, int language)
	{
		try
		{
			var model = new TdsCertificateViewModel()
			{
				Language = EnumLanguage.English,
				TdsCertificates =
					await _mushakGenerationService.GetTdsCertificate(int.Parse(IvatDataProtector.Unprotect(id)))
			};
			return ProcessReport(model.TdsCertificates.ToList(),
				RdlcReportFileOption.TdsCertificateUrl,
				RdlcReportFileOption.TdsCertificateDsName,
				StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.TdsCertificateFileName));
		}
		catch
		{
			return NotFound();
		}
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6)]
	public async Task<IActionResult> ViewAndSendEmailTdsCertificate(string id)
	{
		try
		{
			var model = new TdsCertificateViewModel()
			{
				Language = EnumLanguage.English,
				TdsCertificates =
					await _mushakGenerationService.GetTdsCertificate(int.Parse(IvatDataProtector.Unprotect(id)))
			};
			if (!model.TdsCertificates.Any()) return NotFound();
			var singleInfo = model.TdsCertificates.First();
			if (!string.IsNullOrEmpty(singleInfo.VendorEmailAddress))
			{
				MailMaster.SendEmailWithAttachment(singleInfo.VendorEmailAddress, "TDS Certificate",
					$"Dear Concern, <br/>Greetings! Hope you are doing great. We have attached TDS Certificate for Challan No: {singleInfo.VatChallanNo}, Date: {singleInfo.VatChallanIssueDate.DateTimeToStringWithoutTime()}. <br/><br/><strong>Finance and Accounts Team</strong>",
					new Dictionary<string, Stream>()
					{
						{
							$"{RdlcReportFileOption.TdsCertificateFileName}.pdf", GetPdfWithOpenStream(
								model.TdsCertificates.ToList(), RdlcReportFileOption.TdsCertificateUrl,
								RdlcReportFileOption.TdsCertificateDsName,
								StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.VdsCertificateFileName))
						}
					});
			}


			return ProcessReport(model.TdsCertificates.ToList(),
				RdlcReportFileOption.TdsCertificateUrl,
				RdlcReportFileOption.TdsCertificateDsName,
				StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.TdsCertificateFileName));
		}
		catch
		{
			return NotFound();
		}
	}

	#endregion

	#region Mushak-6.7 (CreditNote for sales price reduce)

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_PRINT_MUSHAK_6_7)]
	public async Task<IActionResult> MushakSixPointSevenForSalesPriceReduceList()
	{
		return View(
			await _salesPriceAdjustmentService.GetSalesPriceAdjustmentsByOrganizationAndBranch(UserSession
				.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch));
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_PRINT_MUSHAK_6_7)]
	public async Task<IActionResult> MushakSixPointSevenForSalesPriceReduce(string id, int language)
	{
		return View(await _salesPriceAdjustmentService.GetCreditNoteMushakForSalesPriceReduce(id, UserSession.UserId));
	}

	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_PRINT_MUSHAK_6_7)]
	public async Task<IActionResult> MushakSixPointSevenForSalesPriceReduceEnglish(string id, int language)
	{
		return View(await _salesPriceAdjustmentService.GetCreditNoteMushakForSalesPriceReduce(id, UserSession.UserId));
	}

	#endregion

	#region Mushak- 6.10 (Sale/Purchase greater than 2 Lakhs)

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_10)]
	public IActionResult MushakSixPointTen()
	{
		var model = new VmMushakHighValDisplay();
		var currentDate = DateTime.Today;
		//int currentYear = Convert.ToInt32(currentDate.Year);
		//var previousYears = new List<SelectListItem>();
		//for (int i = 0; i < 10; i++)
		//{
		//	previousYears.Add(new SelectListItem
		//	{
		//		Value = currentYear.ToString(),
		//		Text = currentYear.ToString()
		//	});
		//	currentYear--;
		//}


		//model.YearList = previousYears;
		model.Month = Convert.ToInt32(currentDate.Month);
		model.Year = Convert.ToInt32(currentDate.Year);
		model.OrgId = 0;

		model.ReportUrl = "";
		model.YearList = _reportOptionService.GetYearSelectList();
		model.MonthList = _reportOptionService.GetMonthSelectList();
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_10)]
	public async Task<IActionResult> MushakSixPointTen(VmMushakReturnPost vmMushakReturnPost)
	{
		var model = new VmMushakHighValDisplay();
		var currentDate = DateTime.Today;
		//int currentYear = Convert.ToInt32(currentDate.Year);
		//var previousYears = new List<SelectListItem>();
		//for (int i = 0; i < 10; i++)
		//{
		//	previousYears.Add(new SelectListItem
		//	{
		//		Value = currentYear.ToString(),
		//		Text = currentYear.ToString()
		//	});
		//	currentYear--;
		//}

		model.OrgId = UserSession.OrganizationId;
		//model.YearList = previousYears;
		model.Year = vmMushakReturnPost.Year;
		model.Month = vmMushakReturnPost.Month;
		model.Language = vmMushakReturnPost.Language;
		model.YearList = _reportOptionService.GetYearSelectList();
		model.MonthList = _reportOptionService.GetMonthSelectList();

		model.VmHigh = _mushakHighValueService.GetMushakReturn(UserSession.OrganizationId, vmMushakReturnPost.Year,
			vmMushakReturnPost.Month, 200000);
		model.VmHigh.SpPurcSalesChallanForHighValuePurchase =
			model.VmHigh.SpPurcSalesChallanForHighValuePurchaseList.FirstOrDefault();
		var result =
			await _mushakGenerationService.Mushak6P10(UserSession.OrganizationId, model.Year, model.Month, null);
		model.VmHigh.SpPurcSalesChallanForHighValuePurchaseList = result.SpPurcSalesChallanForHighValuePurchaseList;
		model.VmHigh.SpPurcSalesChallanForHighValueSaleList = result.SpPurcSalesChallanForHighValueSaleList;

		var org = await _organizationService.GetOrganization(
			IvatDataProtector.Protect(UserSession.OrganizationId.ToString()));
		if (org != null)
		{
			model.OrgAddress = org.Name;
			model.OrgBin = org.Bin;
		}

		model.OrgName = UserSession.OrganizationName;
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_10)]
	public async Task<ActionResult> MushakSixPointTenExportToExcel(VmMushakReturnPost model)
	{
		try
		{
			if (ModelState.IsValid)
			{
				var mushak6p10 =
					await _mushakGenerationService.Mushak6P10(UserSession.OrganizationId, model.Year, model.Month,
						null);

				var org = await _organizationService.GetOrganization(
					IvatDataProtector.Protect(UserSession.OrganizationId.ToString()));

				string sWebRootFolder = Environment.WebRootPath;
				sWebRootFolder = Path.Combine(sWebRootFolder, "ExportExcel");
				if (!Directory.Exists(sWebRootFolder))
				{
					Directory.CreateDirectory(sWebRootFolder);
				}

				string sFileName = @"Mushak_6.10.xlsx";
				string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
				FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
				var memory = new MemoryStream();
				using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create,
					       FileAccess.Write))
				{
					IWorkbook workbook;
					workbook = new XSSFWorkbook();
					ISheet excelSheet = workbook.CreateSheet("Mushak6.10");
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


					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0
						? "গণপ্রজাতন্ত্রী বাংলাদেশ সরকার"
						: "GOVERNMENT OF THE PEOPLE'S REPUBLIC OF BANGLADESH");
					excelSheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));

					row = excelSheet.CreateRow(1);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0)
						.SetCellValue(model.Language == 0 ? "জাতীয় রাজস্ব বোর্ড" : "NATIONAL BOARD OF REVENUE");
					excelSheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6));

					row = excelSheet.CreateRow(2);
					row.CreateCell(0).CellStyle = styleHeading;
					row.GetCell(0).SetCellValue(model.Language == 0
						? "দুই লক্ষ টাকার অধিক মূল্যমানের ক্রয়-বিক্রয় চালানপত্রের তথ্য"
						: "Information of purchase and sale invoices worth more than 2 (two) lakh taka");
					excelSheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 6));
					excelSheet.DefaultRowHeightInPoints = 24;

					row = excelSheet.CreateRow(3);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0
						? "[বিধি ৪২ এর উপ-বিধি (১) দ্রষ্টব্য]"
						: "[Note sub-rule (1) of rule 42]");
					excelSheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 6));

					row = excelSheet.CreateRow(4);
					row.CreateCell(0).SetCellValue("");
					excelSheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 6));

					row = excelSheet.CreateRow(5);
					row.CreateCell(0)
						.SetCellValue(
							(model.Language == 0
								? "নিবন্ধিত/তালিকাভুক্ত ব্যক্তির নাম : "
								: "Name of the registered/enlisted person  :") + UserSession.OrganizationName);
					excelSheet.AddMergedRegion(new CellRangeAddress(5, 5, 0, 4));
					row.CreateCell(5).SetCellValue((model.Language == 0 ? "বিআইএন : " : "BIN : ") + org.Bin);
					excelSheet.AddMergedRegion(new CellRangeAddress(5, 5, 5, 6));

					row = excelSheet.CreateRow(6);
					row.CreateCell(0).SetCellValue("");
					excelSheet.AddMergedRegion(new CellRangeAddress(6, 6, 0, 6));

					row = excelSheet.CreateRow(7);
					row.CreateCell(0).SetCellValue(model.Language == 0
						? "অংশ-ক : ক্রয় হিসাব তথ্য"
						: "Part-A: Purchase Account Information");
					excelSheet.AddMergedRegion(new CellRangeAddress(7, 7, 0, 6));

					row = excelSheet.CreateRow(8);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "ক্রমিক সংখ্যা" : "Serial No.");
					excelSheet.AddMergedRegion(new CellRangeAddress(8, 9, 0, 0));
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "ক্রয়" : "Purchase");
					excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 1, 6));


					row = excelSheet.CreateRow(9);
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "চালানপত্র নং" : "Invoice No.");
					excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 1, 1));
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0 ? "ইস্যুর তারিখ" : "Issue Date");
					excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 2, 2));
					row.CreateCell(3).CellStyle = style;
					row.GetCell(3).SetCellValue(model.Language == 0 ? "মূল্য" : "Price");
					excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 3, 3));
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? "বিক্রেতার নাম" : "Vendor Name");
					excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 4, 4));
					row.CreateCell(5).CellStyle = style;
					row.GetCell(5).SetCellValue(model.Language == 0 ? "বিক্রেতার ঠিকানা" : " Vendor Address");
					excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 5, 5));
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0
						? "বিক্রেতার বিআইএন/জাতীয় পরিচয় পত্র নং *"
						: "Vendor BIN/National Identity Card No. * ");
					excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 6, 6));


					row = excelSheet.CreateRow(10);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "(১)" : "(1)");
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "(২)" : "(2)");
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0 ? "(৩)" : "(3)");
					row.CreateCell(3).CellStyle = style;
					row.GetCell(3).SetCellValue(model.Language == 0 ? "(৪)" : "(4)");
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? "৫" : "(5)");
					row.CreateCell(5).CellStyle = style;
					row.GetCell(5).SetCellValue(model.Language == 0 ? "(৬)" : "(6)");
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0 ? "(৭)" : "(7)");

					int rowCounter = 11;
					foreach (var data in mushak6p10.SpPurcSalesChallanForHighValuePurchaseList)
					{
						row = excelSheet.CreateRow(rowCounter);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue(data.Sl.Value);
						row.CreateCell(1).CellStyle = style;
						row.GetCell(1).SetCellValue(data.VatChallanNo);
						row.CreateCell(2).CellStyle = style;
						row.GetCell(2).SetCellValue(data.VatChallanIssueDate.ToString());
						row.CreateCell(3).CellStyle = style;
						row.GetCell(3).SetCellValue(data.ProdPriceInclVATAndDuty.ToString());
						row.CreateCell(4).CellStyle = style;
						row.GetCell(4).SetCellValue(data.VendorName);
						row.CreateCell(5).CellStyle = style;
						row.GetCell(5).SetCellValue(data.VendorAddress);
						row.CreateCell(6).CellStyle = style;
						row.GetCell(6).SetCellValue(data.VendorBinOrNid);

						rowCounter++;
					}


					row = excelSheet.CreateRow(rowCounter);
					row.CreateCell(0).SetCellValue("");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 6));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).SetCellValue(model.Language == 0
						? "অংশ-খ : বিক্রয় হিসাব তথ্য"
						: "Part-B: Sales Account Information");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 6));

					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "ক্রমিক সংখ্যা" : "Serial No");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter + 1, 0, 0));
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "বিক্রয়" : "Sale");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 1, 6));


					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "চালানপত্র নং" : "Invoice No.");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 1, 1));
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0 ? "ইস্যুর তারিখ" : "Issue Date");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 2, 2));
					row.CreateCell(3).CellStyle = style;
					row.GetCell(3).SetCellValue(model.Language == 0 ? "মূল্য" : "Price");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 3, 3));
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? "ক্রেতার নাম" : "Customer Name");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 4, 4));
					row.CreateCell(5).CellStyle = style;
					row.GetCell(5).SetCellValue(model.Language == 0 ? "ক্রেতার ঠিকানা" : "Customer Address");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 5, 5));
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0
						? "ক্রেতার বিআইএন/জাতীয় পরিচয় পত্র নং *"
						: "Customer BIN/National Identity Card No. * ");
					excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 6, 6));


					row = excelSheet.CreateRow(++rowCounter);
					row.CreateCell(0).CellStyle = style;
					row.GetCell(0).SetCellValue(model.Language == 0 ? "(১)" : "(1)");
					row.CreateCell(1).CellStyle = style;
					row.GetCell(1).SetCellValue(model.Language == 0 ? "(২)" : "(2)");
					row.CreateCell(2).CellStyle = style;
					row.GetCell(2).SetCellValue(model.Language == 0 ? "(৩)" : "(3)");
					row.CreateCell(3).CellStyle = style;
					row.GetCell(3).SetCellValue(model.Language == 0 ? "(৪)" : "(4)");
					row.CreateCell(4).CellStyle = style;
					row.GetCell(4).SetCellValue(model.Language == 0 ? "৫" : "(5)");
					row.CreateCell(5).CellStyle = style;
					row.GetCell(5).SetCellValue(model.Language == 0 ? "(৬)" : "(6)");
					row.CreateCell(6).CellStyle = style;
					row.GetCell(6).SetCellValue(model.Language == 0 ? "(৭)" : "(7)");

					foreach (var data in mushak6p10.SpPurcSalesChallanForHighValueSaleList)
					{
						row = excelSheet.CreateRow(++rowCounter);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue(data.Sl.Value);
						row.CreateCell(1).CellStyle = style;
						row.GetCell(1).SetCellValue(data.VatChallanNo);
						row.CreateCell(2).CellStyle = style;
						row.GetCell(2).SetCellValue(data.SalesDate);
						row.CreateCell(3).CellStyle = style;
						row.GetCell(3).SetCellValue(data.ProdPriceInclVATAndDuty.ToString());
						row.CreateCell(4).CellStyle = style;
						row.GetCell(4).SetCellValue(data.CustomerName);
						row.CreateCell(5).CellStyle = style;
						row.GetCell(5).SetCellValue(data.CustomerAddress);
						row.CreateCell(6).CellStyle = style;
						row.GetCell(6).SetCellValue(data.CustomerBIN);
					}

					for (var i = 0; i <= 6; i++)
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
			else
			{
				return RedirectToAction(nameof(MushakSixPointTen));
			}
		}
		catch
		{
			return RedirectToAction(nameof(MushakSixPointTen));
		}
	}

	#endregion
}