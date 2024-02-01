using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vms.entity.viewModels;
using vms.entity.viewModels.ReportsViewModel;
using vms.utility.StaticData;
using vms.Utility;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.entity.StoredProcedureModel;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.Util;
using Microsoft.AspNetCore.DataProtection;
using vms.entity.Enums;
using System.Threading;
using AutoMapper;
using vms.entity.models;
using vms.entity.viewModels.NewReports;
using vms.utility;
using VmPurchaseReport = vms.entity.viewModels.ReportsViewModel.VmPurchaseReport;
using System.Globalization;
using vms.service.Services.TransactionService;
using vms.service.Services.ReportService;
using vms.service.Services.ProductService;
using vms.service.Services.SecurityService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;
using vms.service.Services.MushakService;

namespace vms.Controllers;

public class ReportsController : ControllerBase
{
	private readonly ISpPurchaseCalcBookService _6p1service;
	private readonly IVendorService _vendorService;
	private readonly IMushakGenerationService _mushakGenerationService;
	private readonly IOrganizationService _organizationService;
	private readonly IPurchaseService _purchaseService;
	private readonly IPurchaseDetailService _purchaseDetailService;
	private readonly ISaleService _saleService;
	private readonly ISalesDetailService _salesDetailService;
	private readonly IDamageService _damageService;
	private readonly IDamageDetailService _damageDetailService;
	private readonly IMushakReturnService _mushakReturnService;
	private readonly IMushakHighValue _mushakHighService;
	private readonly IContractVendorTransferRawMaterialService _rawMaterialService;
	private readonly IContractVendorService _contractVendorService;
	private readonly IWebHostEnvironment _hostingEnvironment;
	private readonly IProductService _productService;
	private readonly IProductTransactionBookService _productTransactionBookService;
	private readonly IProductVatService _productVatService;
	private readonly IUserService _userService;
	private readonly IUserLoginHistoryService _loginHistoryService;
	private readonly IPriceSetupService _priceSetupService;
	private readonly IMapper _mapper;
	private readonly IOrgBranchService _branchService;
	private readonly IReportOptionService _reportOptionService;
	private readonly ICustomerService _customerService;
	private readonly IDashboardService _dashboardService;

	public ReportsController(ControllerBaseParamModel controllerBaseParamModel, IMapper mapper,
		ISpPurchaseCalcBookService M6p1service, IMushakReturnService mushakReturnService, IContractVendorTransferRawMaterialService rawMaterialService,
		IContractVendorService contractVendorService, IMushakHighValue mushakHighService,
		IWebHostEnvironment hostingEnvironment
		, IProductService productService, IVendorService vendorService,
		IMushakGenerationService mushakGenerationService, IOrganizationService organizationService,
		IPurchaseService purchaseService, IPurchaseDetailService purchaseDetailService, ISaleService saleService,
		ISalesDetailService salesDetailService, IDamageService damageService,
		IDamageDetailService damageDetailService, IProductTransactionBookService productTransactionBookService,
		IProductVatService productVatService, IUserService userService, IUserLoginHistoryService loginHistoryService,
		IPriceSetupService priceSetupService, IOrgBranchService branchService, IReportOptionService reportOptionService, ICustomerService customerService, IDashboardService dashboardService) : base(controllerBaseParamModel)
	{
		_customerService = customerService;
		_mushakReturnService = mushakReturnService;
		_mushakHighService = mushakHighService;
		_6p1service = M6p1service;
		_rawMaterialService = rawMaterialService;
		_contractVendorService = contractVendorService;
		_hostingEnvironment = hostingEnvironment;
		_productService = productService;
		_vendorService = vendorService;
		_mushakGenerationService = mushakGenerationService;
		_organizationService = organizationService;
		_purchaseService = purchaseService;
		_purchaseDetailService = purchaseDetailService;
		_saleService = saleService;
		_salesDetailService = salesDetailService;
		_damageService = damageService;
		_damageDetailService = damageDetailService;
		_productTransactionBookService = productTransactionBookService;
		_productVatService = productVatService;
		_mapper = mapper;
		_userService = userService;
		_loginHistoryService = loginHistoryService;
		_priceSetupService = priceSetupService;
		_branchService = branchService;
		_reportOptionService = reportOptionService;
		_dashboardService = dashboardService;
	}

	[VmsAuthorize(FeatureList.REPORTS)]
	public IActionResult Index()
	{
		return View();
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	public IActionResult Index(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok1()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok1(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok1Ka()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok1Ka(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok1Kha()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok1Kha(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok1Ga()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok1Ga(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok1Gha()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok1Gha(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok2Kha()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok2Kha(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_2_1)]
	public IActionResult Mushok2P1()
	{
		return View();
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_2_1)]
	public IActionResult Mushok2P1(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_2_3)]
	public IActionResult Mushok2P3()
	{
		return View();
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_2_3)]
	public IActionResult Mushok2P3(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok4()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok4(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok5()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok5(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok5Ka()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok5Ka(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok6()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok6(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}


	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_10)]
	public IActionResult Mushok6P10()
	{
		return View();
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_10)]
	public IActionResult Mushok6P10(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok7()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok7(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok8()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok8(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok9()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok9(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_9_1)]
	public IActionResult Mushok9P1(int year = 0, int month = 0, int language = 0)
	{
		var model = new VmMushakReturnDisplay();
		model.Month = month == 0 ? DateTime.Now.Month : month;
		model.Year = year == 0 ? DateTime.Now.Year : year;
		model.Language = language;
		model.VmMushakReturn = _mushakReturnService.GetMushakReturn(UserSession.OrganizationId,
			model.Year, model.Month);
		ViewBag.Year = model.Year;
		ViewBag.Month = model.Month;
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_9_1)]
	public IActionResult Mushok9P1SubKa(int year = 0, int month = 0, int language = 0)
	{
		var model = new VmMushakReturnDisplay();
		model.Month = month == 0 ? DateTime.Now.Month : month;
		model.Year = year == 0 ? DateTime.Now.Year : year;
		model.Language = language;
		model.VmMushakReturn = _mushakReturnService.GetMushakReturn(UserSession.OrganizationId,
			model.Year, model.Month);
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_9_1)]
	public IActionResult Mushok9P1SubKha(int year = 0, int month = 0, int language = 0)
	{
		var model = new VmMushakReturnDisplay();
		model.Month = month == 0 ? DateTime.Now.Month : month;
		model.Year = year == 0 ? DateTime.Now.Year : year;
		model.Language = language;
		model.VmMushakReturn = _mushakReturnService.GetMushakReturn(UserSession.OrganizationId,
			model.Year, model.Month);
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_9_1)]
	public IActionResult Mushok9P1SubGa(int year = 0, int month = 0, int language = 0)
	{
		var model = new VmMushakReturnDisplay();
		model.Month = month == 0 ? DateTime.Now.Month : month;
		model.Year = year == 0 ? DateTime.Now.Year : year;
		model.Language = language;
		model.VmMushakReturn = _mushakReturnService.GetMushakReturn(UserSession.OrganizationId,
			model.Year, model.Month);
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_9_1)]
	public IActionResult Mushok9P1SubGha(int year = 0, int month = 0, int language = 0)
	{
		var model = new VmMushakReturnDisplay();
		model.Month = month == 0 ? DateTime.Now.Month : month;
		model.Year = year == 0 ? DateTime.Now.Year : year;
		model.Language = language;
		model.VmMushakReturn = _mushakReturnService.GetMushakReturn(UserSession.OrganizationId,
			model.Year, model.Month);
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_9_1)]
	public IActionResult Mushok9P1SubUma(int year = 0, int month = 0, int language = 0)
	{
		var model = new VmMushakReturnDisplay();
		model.Month = month == 0 ? DateTime.Now.Month : month;
		model.Year = year == 0 ? DateTime.Now.Year : year;
		model.Language = language;
		model.VmMushakReturn = _mushakReturnService.GetMushakReturn(UserSession.OrganizationId,
			model.Year, model.Month);
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_9_1)]
	public IActionResult Mushok9P1SubCha(int year = 0, int month = 0, int language = 0)
	{
		var model = new VmMushakReturnDisplay();
		model.Month = month == 0 ? DateTime.Now.Month : month;
		model.Year = year == 0 ? DateTime.Now.Year : year;
		model.Language = language;
		model.VmMushakReturn = _mushakReturnService.GetMushakReturn(UserSession.OrganizationId,
			model.Year, model.Month);
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_9_1)]
	public IActionResult Mushok9P1SubChha(int year = 0, int month = 0, int language = 0)
	{
		var model = new VmMushakReturnDisplay();
		model.Month = month == 0 ? DateTime.Now.Month : month;
		model.Year = year == 0 ? DateTime.Now.Year : year;
		model.Language = language;
		model.VmMushakReturn = _mushakReturnService.GetMushakReturn(UserSession.OrganizationId,
			model.Year, model.Month);
		return View(model);
	}

	// [HttpPost]
	// [VmsAuthorize(FeatureList.MUSHAK)]
	// [VmsAuthorize(FeatureList.MUSHAK_MUSHAK_9_1)]
	// public IActionResult Mushok9P1(VmMushakReturnPost vmMushakReturnPost)
	// {
	//     if (ModelState.IsValid)
	//     {
	//         var model = new VmMushakReturnDisplay();
	//         model.Year = vmMushakReturnPost.Year;
	//         model.Month = vmMushakReturnPost.Month;
	//         model.VmMushakReturn = _mushakReturnService.GetMushakReturn(UserSession.OrganizationId,
	//             vmMushakReturnPost.Year, vmMushakReturnPost.Month);
	//         model.Language = vmMushakReturnPost.Language;
	//         return View(model);
	//     }
	//
	//     return View(vmMushakReturnPost);
	// }


	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_10)]
	public IActionResult Mushak6P10()
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


		model.YearList = _reportOptionService.GetYearSelectList();
		model.MonthList = _reportOptionService.GetMonthSelectList();
		model.Month = Convert.ToInt32(currentDate.Month);
		model.Year = Convert.ToInt32(currentDate.Year);
		model.OrgId = 0;

		model.ReportUrl = "";
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_10)]
	public async Task<IActionResult> Mushak6P10(VmMushakReturnPost vmMushakReturnPost)
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
		model.YearList = _reportOptionService.GetYearSelectList();
		model.MonthList = _reportOptionService.GetMonthSelectList();
		model.Year = vmMushakReturnPost.Year;
		model.Month = vmMushakReturnPost.Month;
		model.Language = vmMushakReturnPost.Language;

		model.VmHigh = _mushakHighService.GetMushakReturn(UserSession.OrganizationId, vmMushakReturnPost.Year,
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
	public async Task<ActionResult> Mushak6P10ExportToExcel(VmMushakReturnPost model)
	{
		try
		{
			if (ModelState.IsValid)
			{
				var mushak6p10 = await _mushakGenerationService.Mushak6P10(UserSession.OrganizationId, model.Year,
					model.Month, null);

				var org = await _organizationService.GetOrganization(
					IvatDataProtector.Protect(UserSession.OrganizationId.ToString()));

				string sWebRootFolder = _hostingEnvironment.WebRootPath;
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

					for (int i = 0; i <= 6; i++)
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
				return RedirectToAction(nameof(Mushak6P10));
			}
		}
		catch
		{
			return RedirectToAction(nameof(Mushak6P10));
		}
	}


	[VmsAuthorize(FeatureList.MUSHAK)]
	public IActionResult Mushok9P2()
	{
		return View();
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	public IActionResult Mushok9P2(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok10()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok10(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok11()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok11(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok11Ka()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok11Ka(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok11Kha()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok11Kha(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok11Ga()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok11Ga(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok11Gha()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok11Gha(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok11Purchase()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok11Purchase(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok11Sale()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok11Sale(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok12()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok12(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok12Ka()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok12Ka(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok12Kha()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok12Kha(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok15()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok15(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok16()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok16(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok17()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok17(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok17Ka()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok17Ka(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok18()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok18(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok19()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok19(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok20()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok20(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok21()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok21(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok22()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok22(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok23()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok23(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok24()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok24(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok25()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok25(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok26()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok26(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok27()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok27(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok28()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok28(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult Mushok29()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Mushok29(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public IActionResult MushokTr6()
	{
		return View();
	}

	[HttpPost]
	public IActionResult MushokTr6(ReoportOption reoportOption)
	{
		return View(reoportOption);
	}

	public async Task<IActionResult> PurchaseReport()
	{
		var model = new vmPurchaseReportByVendor
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now
		};

		var vendors = await _vendorService.Query().Where(x => x.OrganizationId == UserSession.OrganizationId)
			.SelectAsync();
		model.VendorList = vendors.Select(s => new SelectListItem { Text = s.Name, Value = s.VendorId.ToString() });

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT_BY_VENDOR)]
	public async Task<IActionResult> PurchaseReport(vmPurchaseReportByVendor model)
	{
		if (ModelState.IsValid)
		{
			model.OrgName = UserSession.OrganizationName;
			var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
			model.OrgAddress = org.Address;

			model.PurchaseList = await _purchaseService.MonthlyPurchaseReport(0, UserSession.OrganizationId,
				model.VendorId, "", model.FromDate, model.ToDate, UserSession.UserId);
		}

		var vendors = await _vendorService.Query().Where(x => x.OrganizationId == UserSession.OrganizationId)
			.SelectAsync();
		model.VendorList = vendors.Select(s => new SelectListItem { Text = s.Name, Value = s.VendorId.ToString() });

		return View(model);
	}

	public async Task<IActionResult> UserLoginReport()
	{
		var model = new vmPurchaseReportByVendor
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now
		};

		var vendors = await _vendorService.Query().Where(x => x.OrganizationId == UserSession.OrganizationId)
			.SelectAsync();
		model.VendorList = vendors.Select(s => new SelectListItem { Text = s.Name, Value = s.VendorId.ToString() });

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT_BY_VENDOR)]
	public async Task<IActionResult> UserLoginReport(vmPurchaseReportByVendor model)
	{
		if (ModelState.IsValid)
		{
			model.OrgName = UserSession.OrganizationName;
			var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
			model.OrgAddress = org.Address;

			model.PurchaseList = await _purchaseService.MonthlyPurchaseReport(0, UserSession.OrganizationId,
				model.VendorId, "", model.FromDate, model.ToDate, UserSession.UserId);
		}

		var vendors = await _vendorService.Query().Where(x => x.OrganizationId == UserSession.OrganizationId)
			.SelectAsync();
		model.VendorList = vendors.Select(s => new SelectListItem { Text = s.Name, Value = s.VendorId.ToString() });

		return View(model);
	}

	public async Task<IActionResult> PurchaseReportByProduct()
	{
		var model = new VmPurchaseReportByProduct
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now
		};
		model.ProductList =
			await _productService.GetProductsSelectList(
				(IvatDataProtector.Protect(UserSession.OrganizationId
					.ToString())));
		model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT_BY_VENDOR)]
	public async Task<IActionResult> PurchaseReportByProduct(VmPurchaseReportByProduct model)
	{

		var purchaseList =
			await _purchaseService.GetPurchaseReportByProduct(0, UserSession.OrganizationId,
				model.ProductId, "", model.FromDate, model.ToDate, UserSession.UserId);
		return ProcessReport(purchaseList,
			RdlcReportFileOption.GetPurchaseReportByProductUrl,
			RdlcReportFileOption.GetPurchaseReportByProductDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.GetPurchaseReportByProductFileName),
			GetParameterForPurchaseReport(model.FromDate, model.ToDate, "Purchase Report"), model.ReportProcessOptionId
			);
	}

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_VDS_LIST_REPORT)]
	public IActionResult PurchaseVDSListReport()
	{
		var model = new vmPurchaseVDSList
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now,
			ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList(),
		};
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_VDS_LIST_REPORT)]
	public async Task<IActionResult> PurchaseVDSListReport(vmPurchaseVDSList model)
	{
		var purchaseVDSList =
		   await _purchaseService.GetPurchaseVdsListByOrgAndBranch(UserSession.OrganizationId, model.FromDate, model.ToDate, UserSession.BranchIds, UserSession.IsRequireBranch);
		return ProcessReport(purchaseVDSList,
			RdlcReportFileOption.PurchaseVdsListReportUrl,
			RdlcReportFileOption.PurchaseVdsListReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.PurchaseVdsListReportFileName),
			GetParameterForPurchaseReport(model.FromDate, model.ToDate, "Purchase VDS List Report"), model.ReportProcessOptionId
			);
	}


	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_VENDOR_REPORT)]
	public IActionResult VendorReport()
	{
		var model = new vmVendorReport
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now,
			ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList(),
		};
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_VENDOR_REPORT)]
	public async Task<IActionResult> VendorReport(vmVendorReport model)
	{
		model.vendorList = await _vendorService.Query()
				.Where(x => x.OrganizationId == UserSession.OrganizationId)
				.Where(x => x.CreatedTime >= model.FromDate && x.CreatedTime < model.ToDate.AddDays(1))
				.SelectAsync();

		var vendors = _mapper.Map<List<Vendor>, List<vmVendorReportRdlc>>(model.vendorList.ToList());
		foreach (var item in vendors)
		{
			item.OrgName = UserSession.OrganizationName;
			item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
			item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
		}
		return ProcessReport(vendors,
			RdlcReportFileOption.VendorReportUrl,
			RdlcReportFileOption.VendorReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.VendorReportFileName),
			GetParameterForPurchaseReport(model.FromDate, model.ToDate, "Vendor Report"), model.ReportProcessOptionId
			);
	}

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
	public IActionResult YearlyComparisonReport()
	{
		var model = new VmComparisonInformationHtmlDisplayViewModel
		{
			FromYear = DateTime.Now.Year,
			ToYear = DateTime.Now.Year,
			ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList(),
		};
		return View(model);
	}


	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
	public async Task<IActionResult> YearlyComparisonReport(VmComparisonInformationHtmlDisplayViewModel report)
	{
		var yearlyComparisons =
			await _dashboardService.GetYearlyComparisonInfo(UserSession.OrganizationId, report.FromYear, report.ToYear, UserSession.UserId);
		return ProcessReport(yearlyComparisons.ToList(),
			RdlcReportFileOption.YearlyComparisonReportUrl,
			RdlcReportFileOption.YearlyComparisonReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.YearlyComparisonReportFileName),
			GetParameterForYearlyComparisonReport(report.FromYear, report.ToYear, "Yearly Comparison Report"), report.ReportProcessOptionId
			);
	}

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
	public IActionResult MonthlyComparisonReport()
	{
		var model = new VmMonthlyComparisonHtmlDisplayViewModel
		{
			FromYear = DateTime.Now.Year,
			FromMonth = DateTime.Now.Month,
			ToYear = DateTime.Now.Year,
			ToMonth = DateTime.Now.Month,
			MonthList = _reportOptionService.GetMonthSelectList(),

            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList(),
		};
		return View(model);
	}


	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
	public async Task<IActionResult> MonthlyComparisonReport(VmMonthlyComparisonHtmlDisplayViewModel report)
	{
		var yearlyComparisons =
			await _dashboardService.GetMonthlyComparisonInfo(UserSession.OrganizationId, report.FromYear, report.FromMonth, report.ToYear, report.ToMonth, UserSession.UserId);
		string fromMonthName = new DateTimeFormatInfo().GetMonthName(report.FromMonth);
		string toMonthName = new DateTimeFormatInfo().GetMonthName(report.ToMonth);
		return ProcessReport(yearlyComparisons.ToList(),
			RdlcReportFileOption.MonthlyComparisonReportUrl,
			RdlcReportFileOption.MonthlyComparisonReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.MonthlyComparisonReportFileName),
			GetParameterForMonthlyComparisonReport(report.FromYear, fromMonthName, report.ToYear, toMonthName, "Monthly Comparison Report"), report.ReportProcessOptionId
			);
	}

	private Dictionary<string, string> GetParameterForMonthlyComparisonReport(int? fromYear, string fromMonthName, int? toYear, string toMonthName, string reportHeaderName)
	{
		return new Dictionary<string, string>
		{
			{ "ReportNameHeader", reportHeaderName },
			{
				"DateHeader",
				$"From: {fromMonthName}, {fromYear} to: {toMonthName}, {toYear}"
			}
		};
	}

	private Dictionary<string, string> GetParameterForYearlyComparisonReport(int? fromYear, int? toYear, string reportHeaderName)
	{
		return new Dictionary<string, string>
		{
			{ "ReportNameHeader", reportHeaderName },
			{
				"DateHeader",
				$"From: {fromYear} to: {toYear}"
			}
		};
	}

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
	public IActionResult CustomerReport()
	{
		var model = new vmCustomerReport
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now,
			ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList(),
		};
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
	public async Task<IActionResult> CustomerReport(vmCustomerReport report)
	{
		var customerList = await _customerService.GetCustomerListByOrg(UserSession.ProtectedOrganizationId);
		customerList = customerList.Where(x => x.CreatedTime >= report.FromDate && x.CreatedTime < report.ToDate.AddDays(1));
		return ProcessReport(customerList.ToList(),
			RdlcReportFileOption.CustomerReportUrl,
			RdlcReportFileOption.CustomerReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.CustomerReportFileName),
			GetParameterForPurchaseReport(report.FromDate, report.ToDate, "Customer Report"), report.ReportProcessOptionId
			);
	}

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_ALL_CUSTOMER_REPORT)]
	public IActionResult AllCustomerReport()
	{
		var model = new vmCustomerReport
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now,
			ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList(),
		};
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_ALL_CUSTOMER_REPORT)]
	public async Task<IActionResult> AllCustomerReport(vmCustomerReport report)
	{
		var customerList = await _customerService.GetCustomerListByOrg(UserSession.ProtectedOrganizationId);
		return ProcessReport(customerList.ToList(),
			RdlcReportFileOption.AllCustomerReportUrl,
			RdlcReportFileOption.AllCustomerReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.AllCustomerReportFileName),
			GetParameterForPurchaseReport(DateTime.Now, DateTime.Now, "Customer Report"), report.ReportProcessOptionId
			);
	}

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_CONTRACTUAL_PRODUCTION_REPORT)]
	public IActionResult ContractualProductionReport()
	{
		var model = new vmContractualProductionReport
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now,
			ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList(),
		};
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_CONTRACTUAL_PRODUCTION_REPORT)]
	public async Task<IActionResult> ContractualProductionReport(vmContractualProductionReport model)
	{

		var listData = await _contractVendorService.ViewContractualProductions(UserSession.ProtectedOrganizationId);
		listData = listData.Where(x => x.ContractDate >= model.FromDate && x.ContractDate < model.ToDate.AddDays(1));
		return ProcessReport(listData.ToList(),
			RdlcReportFileOption.ContractualProductionReportUrl,
			RdlcReportFileOption.ContractualProductionReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.ContractualProductionReportFileName),
			GetParameterForPurchaseReport(model.FromDate, model.ToDate, "Contractual Production Report"), model.ReportProcessOptionId
			);

		//vmContractualProductionReport report = new vmContractualProductionReport();

		//if (ModelState.IsValid)
		//{
		//    report.contractualProductionList = await _contractVendorService.Query()
		//        .Include(x => x.Vendor).Include(x => x.ContractType)
		//        .Where(x => x.OrganizationId == UserSession.OrganizationId)
		//        .Where(x => x.ContractDate >= model.FromDate && x.ContractDate < model.ToDate.AddDays(1))
		//        .SelectAsync();
		//}


		//HeaderModel head = new HeaderModel();
		//head.LogoName = "";
		//head.ReportName = "Contractual Production Report";
		//head.CompanyName = UserSession.OrganizationName;

		//report.head = head;
		//report.FromDate = model.FromDate;
		//report.ToDate = model.ToDate;
		//return View(report);
	}

	public async Task<IActionResult> ProductPurchaseList()
	{
		var model = new VmProductPurchaseListReport
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now,
			BranchList = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
			ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
		};
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_PRODUCT_LIST_REPORT)]
	public async Task<IActionResult> ProductPurchaseList(VmProductPurchaseListReport model)
	{
		model.OgrId = UserSession.OrganizationId;
		var purchaseCalcBook =
			await _purchaseService.ProductPurchaseListReport(model.OgrId, model.OgrBranchId ?? 0, model.FromDate, model.ToDate);

		return ProcessReport(purchaseCalcBook,
			RdlcReportFileOption.ProductPurchaseListReportUrl,
			RdlcReportFileOption.ProductPurchaseListReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.ProductPurchaseListReportFileName),
			GetParameterForPurchaseReport(model.FromDate.Value, model.ToDate.Value, "Product Purchase List"), model.ReportProcessOptionId
			);

	}

	public async Task<IActionResult> ProductSalesList()
	{
		var model = new VmProductSalesListReport
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now,
			BranchList = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
			ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
		};
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_SALES_PRODUCT_LIST_REPORT)]
	public async Task<IActionResult> ProductSalesList(VmProductPurchaseListReport model)
	{
		model.OgrId = UserSession.OrganizationId;
		var salesCalcBook =
			await _saleService.ProductSalesListReport(model.OgrId, model.OgrBranchId ?? 0, model.FromDate, model.ToDate, UserSession.UserId);

		return ProcessReport(salesCalcBook,
			RdlcReportFileOption.ProductSalesListReportUrl,
			RdlcReportFileOption.ProductSalesListReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.ProductSalesListReportFileName),
			GetParameterForPurchaseReport(model.FromDate.Value, model.ToDate.Value, "Product Sales List"), model.ReportProcessOptionId
			);

	}

	private Dictionary<string, string> GetParameterForPurchaseReport(DateTime? fromDate, DateTime? toDate, string reportHeaderName)
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

	public async Task<IActionResult> InputOutputCoEfficientReport()
	{
		var model = new VmInputOutputCoEfficientReport();
		model.ProductList =
			await _productService.GetProductsSelectList(
				(IvatDataProtector.Protect(UserSession.OrganizationId
					.ToString())));
		model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT_BY_VENDOR)]
	public async Task<IActionResult> InputOutputCoEfficientReport(VmInputOutputCoEfficientReport model)
	{

		var data = await _priceSetupService.GetInputOutputCoEfficientReportData(UserSession.ProtectedOrganizationId);
		data = data.Where(x => x.ProductId == model.ProductId);
		return ProcessReport(data.ToList(),
			RdlcReportFileOption.InputOutputCoEfficientReportUrl,
			RdlcReportFileOption.InputOutputCoEfficientReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.InputOutputCoEfficientReportFileName),
			GetParameterForPurchaseReport(DateTime.Now, DateTime.Now, "Input Output CoEfficient Report"), model.ReportProcessOptionId
			);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT_BY_VENDOR)]
	public async Task<IActionResult> PurchaseReportExcel(vmPurchaseReportByVendor model)
	{
		if (ModelState.IsValid)
		{
			model.OrgName = UserSession.OrganizationName;
			var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
			model.OrgAddress = org.Address;

			model.PurchaseList = await _purchaseService.MonthlyPurchaseReport(0, UserSession.OrganizationId,
				model.VendorId, "", model.FromDate, model.ToDate, UserSession.UserId);
			var purchaseRdlc =
				_mapper.Map<List<SpMonthlyPurchaseReport>, List<vmPurchaseReportMonthlyRdlc>>(model.PurchaseList);

			foreach (var item in purchaseRdlc)
			{
				item.OrgName = model.OrgName;
				item.OrgAddress = model.OrgAddress;
				item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
				item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
			}

			return GetReportPdf(purchaseRdlc, RdlcReportFileOption.PurchaseOldReportByVendorUrl,
				RdlcReportFileOption.PurchaseOldReportByVendorDsName,
				RdlcReportFileOption.PurchaseOldReportByVendorFileName);
		}

		return RedirectToAction(nameof(PurchaseReport));
	}

	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT_BY_GR_NO)]
	public IActionResult PurchaseReportGr()
	{
		var model = new VmPurchaseReport();

		model.FromDate = DateTime.Now;
		model.ToDate = DateTime.Now;

		model.vendorId = 0;
		model.reason = 0;
		model.organizationId = 0;

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT_BY_GR_NO)]
	public IActionResult PurchaseReportGr(VmPurchaseReport model)
	{
		var form = (model.FromDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT);
		form = HttpUtility.UrlEncode(form);
		var to = (model.ToDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT);
		;
		to = HttpUtility.UrlEncode(to);

		return View(model);
	}

	#region Purchase Report By Month

	[VmsAuthorize(FeatureList.REPORTS_MONTHLY_PURCHASE_REPORT)]
	public IActionResult PurchaseReportMonth()
	{
		var currentDate = DateTime.Today;
		var model = new vmMonthlyPurchaseReport();
		model.Reason = 0;
		model.From = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month);
		model.To = currentDate;
		model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_MONTHLY_PURCHASE_REPORT)]
	public async Task<IActionResult> PurchaseReportMonth(vmMonthlyPurchaseReport model)
	{
		if (!ModelState.IsValid)
		{
			model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
			return View(model);
		}

		var purchase = await _purchaseService.MonthlyPurchaseReport(0, UserSession.OrganizationId, 0, "",
			model.From, model.To, UserSession.UserId);
		return ProcessReport(purchase.ToList(),
			RdlcReportFileOption.MonthlyPurchaseReportUrl,
			RdlcReportFileOption.MonthlyPurchaseReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.MonthlyPurchaseReportFileName),
			GetParameterForPurchaseReport(model.From.Value, model.To.Value, "Monthly Purchase Report"), model.ReportProcessOptionId);

	}

	#endregion

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_MONTHLY_PURCHASE_REPORT)]
	public async Task<IActionResult> PurchaseReportMonthExcel(vmMonthlyPurchaseReport model)
	{
		if (ModelState.IsValid)
		{
			var firstDayOfMonth = (DateCalculator.GetFirstDateOfMonth(model.Year, model.Month));
			var lastDayOfMonth = (DateCalculator.GetLastDateOfMonth(model.Year, model.Month));

			model.Reason = 1;
			model.From = firstDayOfMonth;
			model.To = lastDayOfMonth;
			model.OrgName = UserSession.OrganizationName;
			var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
			model.OrgAddress = org.Address;

			model.PurchaseList = await _purchaseService.MonthlyPurchaseReport(0, UserSession.OrganizationId, 0, "",
				firstDayOfMonth, lastDayOfMonth, UserSession.UserId);

			var purchaseRdlc =
				_mapper.Map<List<SpMonthlyPurchaseReport>, List<vmPurchaseReportMonthlyRdlc>>(model.PurchaseList);

			foreach (var item in purchaseRdlc)
			{
				item.OrgName = model.OrgName;
				item.OrgAddress = model.OrgAddress;
				item.FromDate = firstDayOfMonth.ToString("dd/MM/yyyy");
				item.ToDate = lastDayOfMonth.ToString("dd/MM/yyyy");
			}

			return GetReportExcel(purchaseRdlc, RdlcReportFileOption.MonthlyPurchaseReportUrl,
				RdlcReportFileOption.MonthlyPurchaseReportDsName,
				RdlcReportFileOption.MonthlyPurchaseReportFileName);
		}

		return RedirectToAction(nameof(PurchaseReportMonth));
	}

	[VmsAuthorize(FeatureList.REPORTS_FOR_EXPENSE)]
	public IActionResult ExpenceReport()
	{
		var model = new vmExpenseReport();

		model.FromDate = DateTime.Now;
		model.ToDate = DateTime.Now;
		model.Reason = 0;

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_FOR_EXPENSE)]
	public async Task<IActionResult> ExpenceReport(vmExpenseReport model)
	{
		if (ModelState.IsValid)
		{
			//var form = (model.FromDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT);
			//form = HttpUtility.UrlEncode(form);
			//var to = (model.ToDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT); ;
			//to = HttpUtility.UrlEncode(to);
			model.Reason = 2;
			model.OrgName = UserSession.OrganizationName;
			var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
			model.OrgAddress = org.Address;

			model.PurchaseList = await _purchaseService.MonthlyPurchaseReport(0, UserSession.OrganizationId, 0, "",
				model.FromDate, model.ToDate, UserSession.UserId);
			//model.ReportUrl = _reportServer.BaseUrl + $"Reports/FrmPurchaseByVendor?OrganizationId={_session.OrganizationId}&&PurchaseReasonId={model.reason}&&FromDate={form}&&ToDate={to}";
		}

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS_FOR_EXPENSE)]
	public async Task<IActionResult> ExpenceReportExcel(vmExpenseReport model)
	{
		if (ModelState.IsValid)
		{
			model.OrgName = UserSession.OrganizationName;
			var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
			model.OrgAddress = org.Address;

			model.PurchaseList = await _purchaseService.MonthlyPurchaseReport(0, UserSession.OrganizationId, 0, "",
				model.FromDate, model.ToDate, UserSession.UserId);

			var purchaseRdlc =
				_mapper.Map<List<SpMonthlyPurchaseReport>, List<vmPurchaseReportMonthlyRdlc>>(model.PurchaseList);

			foreach (var item in purchaseRdlc)
			{
				item.OrgName = model.OrgName;
				item.OrgAddress = model.OrgAddress;
				item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
				item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
			}

			return GetReportExcel(purchaseRdlc, RdlcReportFileOption.ExpenseReportUrl,
				RdlcReportFileOption.ExpenseReportDsName, RdlcReportFileOption.ExpenseReportFileName);
		}

		return RedirectToAction(nameof(ExpenceReport));
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6)]
	public async Task<IActionResult> Mushok6P6()
	{
		//var model = new VmPurchaseReport();
		var vdsList = await _purchaseService.Query()
			.Include(x => x.Vendor)
			.Where(m => m.IsVatDeductedInSource && m.PurchaseTypeId == (int)EnumPurchaseType.PurchaseTypeLocal &&
						m.MushakReturnPaymentForVds.Any()).SelectAsync(CancellationToken.None);

		//model.FromDate = DateTime.Now;
		//model.ToDate = DateTime.Now;

		//model.vendorId = 0;
		//model.reason = 0;
		//model.organizationId = 0;
		//model.PurchaseId = -1;
		return View(vdsList);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6)]
	public async Task<IActionResult> Mushok6P6ById(int id)
	{
		try
		{
			var vdsInvoiceList = new List<SpVdsPurchaseCertificate>();
			vdsInvoiceList = await _mushakGenerationService.Mushak6P6(id);
			if (vdsInvoiceList.Any())
			{
				return View(vdsInvoiceList);
			}

			return NotFound();
		}
		catch
		{
			return NotFound();
		}
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_5)]
	public async Task<IActionResult> Mushok6P5()
	{
		//var model = new VmPurchaseReport();

		var model = await _saleService.Query()
			.Include(x => x.Customer)
			.Where(m => m.SalesTypeId == (int)EnumSalesType.SalesTypeTransfer).SelectAsync(CancellationToken.None);

		//model.FromDate = DateTime.Now;
		//model.ToDate = DateTime.Now;

		//model.vendorId = 0;
		//model.reason = 0;
		//model.organizationId = 0;
		//model.PurchaseId = -1;
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_5)]
	public async Task<IActionResult> Mushok6P5ById(int id)
	{
		try
		{
			var model = new vmMushakBranchTransfer();
			model.TransferList = await _mushakGenerationService.Mushak6P5(id);
			model.SaleId = id;
			if (model.TransferList.Any())
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

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_5)]
	public IActionResult Mushok6P4()
	{
		var fromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
		var toDate = DateTime.Today;
		var model = new vmSearchContractualChalan();

		model.FromDate = fromDate;
		model.ToDate = toDate;
		model.TransferRawMaterial = null;
		model.ContractChallanList = null;
		return View(model);
	}

	[HttpPost]
	public IActionResult Mushok6P6(VmPurchaseReport model)
	{
		if (model.InvoiceNo == null)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.EMPTY_CLASSNAME;
		}

		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_5)]
	public IActionResult Mushok6P5(VmPurchaseReport model)
	{
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_4)]
	public async Task<IActionResult> Mushok6P4(vmSearchContractualChalan model)
	{
		if (ModelState.IsValid)
		{
			//var rawProductionList = await _rawMaterialService
			//                       .Query()
			//                       .Where(c =>
			//                       c.TransfereDate >= model.FromDate
			//                       && c.TransfereDate <= model.ToDate).SelectAsync();

			var productionList = await _contractVendorService
				.Query()
				.Include(x => x.ContractType)
				.Where(c =>
					c.IssueDate >= model.FromDate
					&& c.IssueDate <= model.ToDate)
				.SelectAsync();


			model.ContractChallanList = productionList;
		}

		return View(model);
	}


	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_1_CAN_VIEW)]
	public IActionResult Mushak6P1New()
	{
		var model = new VmPurchaseCalcBookNew();

		model.fromDate = DateTime.Now;
		model.toDate = DateTime.Now;

		model.productId = 0;
		model.ogrId = 0;
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_1)]
	public async Task<IActionResult> Mushak6P1New(VmPurchaseCalcBookNew model, string Vendor = null,
		string Product = null)
	{
		if (Vendor != null)
		{
			ViewData[ControllerStaticData.SEARCH_VENDOR] = Vendor;
		}
		else
		{
			ViewData[ControllerStaticData.SEARCH_VENDOR] = string.Empty;
			model.vendorId = 0;
		}

		if (Product != null)
		{
			ViewData[ControllerStaticData.SEARCH_PRODUCT] = Product;
		}
		else
		{
			ViewData[ControllerStaticData.SEARCH_PRODUCT] = string.Empty;

			model.productId = 0;
		}

		var form = (model.fromDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT);
		form = HttpUtility.UrlEncode(form);
		var to = (model.toDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT);
		;
		to = HttpUtility.UrlEncode(to);
		model.ogrId = UserSession.OrganizationId;
		model.PurchaseCallBook = await _6p1service.GetSpPurchaseCalcBook(model.ogrId, model.fromDate, model.toDate,
			model.vendorId, model.productId);


		return View(model);
	}


	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2_CAN_VIEW)]
	public IActionResult Mushak6P2New()
	{
		var model = new VmSalesCalcBookNew();

		model.fromDate = DateTime.Now;
		model.toDate = DateTime.Now;

		model.productId = 0;
		model.ogrId = 0;
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2_CAN_VIEW)]
	public async Task<IActionResult> Mushak6P2New(VmSalesCalcBookNew model, string Customer = null,
		string Product = null)
	{
		if (Customer != null)
		{
			ViewData[ControllerStaticData.SEARCH_VENDOR] = Customer;
		}
		else
		{
			ViewData[ControllerStaticData.SEARCH_VENDOR] = string.Empty;
			model.CustomerId = 0;
		}

		if (Product != null)
		{
			ViewData[ControllerStaticData.SEARCH_PRODUCT] = Product;
		}
		else
		{
			ViewData[ControllerStaticData.SEARCH_PRODUCT] = string.Empty;

			model.productId = 0;
		}

		var form = (model.fromDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT);
		form = HttpUtility.UrlEncode(form);
		var to = (model.toDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT);
		;
		to = HttpUtility.UrlEncode(to);
		model.ogrId = UserSession.OrganizationId;
		model.SaleCallBook = await _6p1service.GetSpSaleCalcBook(model.ogrId, model.fromDate, model.toDate,
			model.CustomerId, model.productId);


		return View(model);
	}


	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2_1_CAN_VIEW)]
	public IActionResult Mushak6P2P1New()
	{
		var model = new VmPurchaseSalesCalcBookNew();

		model.fromDate = DateTime.Now;
		model.toDate = DateTime.Now;

		model.productId = 0;
		model.ogrId = 0;
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_2_1_CAN_VIEW)]
	public async Task<IActionResult> Mushak6P2P1New(VmPurchaseSalesCalcBookNew model, string Customer = null,
		string Product = null)
	{
		if (Customer != null)
		{
			ViewData[ControllerStaticData.SEARCH_VENDOR] = Customer;
		}
		else
		{
			ViewData[ControllerStaticData.SEARCH_VENDOR] = string.Empty;
			model.CustomerId = 0;
		}

		if (Product != null)
		{
			ViewData[ControllerStaticData.SEARCH_PRODUCT] = Product;
		}
		else
		{
			ViewData[ControllerStaticData.SEARCH_PRODUCT] = string.Empty;

			model.productId = 0;
		}

		var form = (model.fromDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT);
		form = HttpUtility.UrlEncode(form);
		var to = (model.toDate).ToString(MessageStaticData.DATE_DISPLAY_FORMAT);
		;
		to = HttpUtility.UrlEncode(to);
		model.ogrId = UserSession.OrganizationId;
		model.PurSaleCallBook =
			await _6p1service.GetSpPurchaseSaleCalcBook(model.ogrId, model.fromDate, model.toDate, 0, 0,
				model.productId);


		return View(model);
	}


	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6_CAN_VIEW)]
	public IActionResult Mushok6P6New()
	{
		var model = new vmVDSReport();


		model.PurchaseId = -1;
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_6_CAN_VIEW)]
	public async Task<IActionResult> Mushok6P6New(vmVDSReport model)
	{
		model.Vds = await _6p1service.GetSpVds(model.PurchaseId);
		return View(model);
	}


	public async Task<IActionResult> Mushok6P7(int id)
	{
		var model = new List<SpCreditNoteMushak>();


		model = await _6p1service.GetSpCreditNote(id);
		return View(model);
	}

	public async Task<IActionResult> Mushok6p7ReportEnglish(int id, int language)
	{
		var model = new List<SpCreditNoteMushak>();


		model = await _6p1service.GetSpCreditNote(id);
		return View(model);
	}


	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_8)]
	public async Task<IActionResult> Mushok6P8(int id)
	{
		var model = new List<SpDebiNotetMushak>();


		model = await _6p1service.GetSpDebitNote(id);
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_8)]
	public async Task<IActionResult> Mushok6P8English(int id, int language)
	{
		var model = new List<SpDebiNotetMushak>();


		model = await _6p1service.GetSpDebitNote(id);
		return View(model);
	}


	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_3)]
	public async Task<IActionResult> Mushok6P3New(int id)
	{
		var model = new List<SpSalesTaxInvoice>();


		model = await _6p1service.GetChalan(id);
		return View(model);
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_4)]
	public async Task<IActionResult> Mushok6P4raw(int id)
	{
		try
		{
			var model = new vmContractualChalan();
			var rawProductionList = await _rawMaterialService
				.Query()
				.Where(c =>
					c.ContractualProductionId == id).SingleOrDefaultAsync();

			model.Sp6P4Result =
				await _6p1service.Get6P4Raw(rawProductionList.ContractualProductionTransferRawMaterialId);
			model.ContractualProductionTransferRawMaterialId = id;
			model.ContractualProductionId = rawProductionList.ContractualProductionId;
			return View(model);
		}
		catch
		{
			return NotFound();
		}
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_4)]
	public async Task<IActionResult> Mushok6P4RawExportToExcel(vmContractualChalan chalanData)
	{
		try
		{
			if (ModelState.IsValid)
			{
				var model = new List<Sp6p4>();

				model = await _6p1service.Get6P4Raw(chalanData.ContractualProductionTransferRawMaterialId.Value);
				if (model.Any())
				{
					var firstItem = model.First();

					string sWebRootFolder = _hostingEnvironment.WebRootPath;
					sWebRootFolder = Path.Combine(sWebRootFolder, "ExportExcel");
					if (!Directory.Exists(sWebRootFolder))
					{
						Directory.CreateDirectory(sWebRootFolder);
					}

					string sFileName = @"Mushak_6.4.xlsx";
					string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
					FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
					var memory = new MemoryStream();
					using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create,
							   FileAccess.Write))
					{
						IWorkbook workbook;
						workbook = new XSSFWorkbook();
						ISheet excelSheet = workbook.CreateSheet("Mushak6.4");
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
						excelSheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 4));
						excelSheet.DefaultRowHeightInPoints = 24;

						row = excelSheet.CreateRow(1);
						row.CreateCell(0).CellStyle = styleHeading;
						row.GetCell(0).SetCellValue("জাতীয় রাজস্ব বোর্ড");
						excelSheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 4));
						excelSheet.DefaultRowHeightInPoints = 24;

						row = excelSheet.CreateRow(2);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue("চুক্তিভিত্তিক উৎপাদনের চালানপত্র");
						excelSheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 4));

						row = excelSheet.CreateRow(3);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue("[বিধি ৪০ এর উপ-বিধি (১) এর দফা (ঘ) দ্রষ্টব্য]");
						excelSheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 4));


						row = excelSheet.CreateRow(4);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue("নিবন্ধিত ব্যক্তির নাম : " + firstItem.OrgName);
						excelSheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 4));

						row = excelSheet.CreateRow(5);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue("নিবন্ধিত ব্যক্তির বিআইএন : " + firstItem.OrgBin);
						excelSheet.AddMergedRegion(new CellRangeAddress(5, 5, 0, 4));

						row = excelSheet.CreateRow(6);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue("চালানপত্র ইস্যুর ঠিকানা : " + firstItem.OrgAddress);
						excelSheet.AddMergedRegion(new CellRangeAddress(6, 6, 0, 4));

						row = excelSheet.CreateRow(7);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue("");
						excelSheet.AddMergedRegion(new CellRangeAddress(7, 7, 0, 4));

						row = excelSheet.CreateRow(8);
						row.CreateCell(0).SetCellValue("পণ্য গ্রহীতার নাম : " + firstItem.VenName);
						excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 0, 2));
						row.CreateCell(3).SetCellValue("ফেরত গ্রহণকারী  ব্যাক্তির -");
						excelSheet.AddMergedRegion(new CellRangeAddress(8, 8, 3, 4));

						row = excelSheet.CreateRow(9);
						row.CreateCell(0).SetCellValue("গ্রহীতার বিআইএন : " + firstItem.VenBin);
						excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 0, 2));
						row.CreateCell(3).SetCellValue("চালানপত্র নাম্বর : " + firstItem.ChallanNo);
						excelSheet.AddMergedRegion(new CellRangeAddress(9, 9, 3, 4));

						row = excelSheet.CreateRow(10);
						row.CreateCell(0).SetCellValue("গন্তব্যস্থল : " + firstItem.VenAddress);
						excelSheet.AddMergedRegion(new CellRangeAddress(10, 10, 0, 2));
						row.CreateCell(3).SetCellValue("ইস্যুর তারিখ : " +
													   firstItem.ChallanIssueDate.Value.ToString("dd/MM/yyyy"));
						excelSheet.AddMergedRegion(new CellRangeAddress(10, 10, 3, 4));

						row = excelSheet.CreateRow(11);
						row.CreateCell(0).SetCellValue("");
						excelSheet.AddMergedRegion(new CellRangeAddress(11, 11, 0, 2));
						row.CreateCell(4)
							.SetCellValue("ইস্যুর সময় : " + firstItem.ChallanIssueDate.Value.ToString("hh:MM tt"));
						excelSheet.AddMergedRegion(new CellRangeAddress(11, 11, 3, 4));

						row = excelSheet.CreateRow(12);
						row.CreateCell(0).CellStyle = style;
						row.GetCell(0).SetCellValue("ক্রমিক সংখ্যা");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 0, 0));
						row.CreateCell(1).CellStyle = style;
						row.GetCell(1).SetCellValue("প্রকৃতি (উপকরণ বা উৎপাদিত পণ্য)");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 1, 1));
						row.CreateCell(2).CellStyle = style;
						row.GetCell(2).SetCellValue("পণ্যের বিবরণ");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 2, 2));
						row.CreateCell(3).CellStyle = style;
						row.GetCell(3).SetCellValue("পরিমাণ");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 3, 3));
						row.CreateCell(4).CellStyle = style;
						row.GetCell(4).SetCellValue("মন্তব্য");
						excelSheet.AddMergedRegion(new CellRangeAddress(12, 12, 4, 4));


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


						int rowCounter = 14, SlNo = 1;
						foreach (var data in model)
						{
							row = excelSheet.CreateRow(rowCounter);
							row.CreateCell(0).CellStyle = style;
							row.GetCell(0).SetCellValue(SlNo);
							row.CreateCell(1).CellStyle = style;
							row.GetCell(1).SetCellValue("উপকরণ :" + data.MesurementName);
							row.CreateCell(2).CellStyle = style;
							row.GetCell(2).SetCellValue(data.ProductName);
							row.CreateCell(3).CellStyle = style;
							row.GetCell(3).SetCellValue(data.Quantity.ToString());
							row.CreateCell(4).CellStyle = style;
							row.GetCell(4).SetCellValue("");

							rowCounter++;
							SlNo++;
						}

						row = excelSheet.CreateRow(rowCounter);
						row.CreateCell(0).SetCellValue("প্রতিষ্ঠান কর্তৃপক্ষদায়িত্ব প্রাপ্ত ব্যক্তির নাম: " +
													   firstItem.VatResponsiblePersonName);
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 4));

						row = excelSheet.CreateRow(++rowCounter);
						row.CreateCell(0).SetCellValue("পদবী : " + firstItem.VatResponsiblePersonDesignation);
						excelSheet.AddMergedRegion(new CellRangeAddress(rowCounter, rowCounter, 0, 4));

						for (int i = 0; i < 4; i++)
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
					return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						sFileName);
				}
			}
			else
			{
				return RedirectToAction("Mushok6P4raw", chalanData);
			}

			return RedirectToAction("Mushok6P4raw", chalanData);
		}
		catch
		{
			return RedirectToAction("Mushok6P4raw", chalanData);
		}
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_4)]
	public async Task<IActionResult> Mushok6P4finished(int id)
	{
		try
		{
			var model = new vmContractualChalan();
			model.ContractualProductionId = id;
			model.Sp6P4Result = await _6p1service.Get6P4Finish(id);
			return View(model);
		}
		catch
		{
			return NotFound();
		}
	}

	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_4)]
	public async Task<IActionResult> Mushok6P5NEW(int id)
	{
		var model = new List<Sp6p5>();


		model = await _6p1service.Get6P5(id);
		return View(model);
	}


	[VmsAuthorize(FeatureList.MUSHAK)]
	[VmsAuthorize(FeatureList.MUSHAK_MUSHAK_6_4)]
	public async Task<IActionResult> Mushok4P3NEW(int id)
	{
		var model = new List<Sp4p3>();


		model = await _6p1service.Get4P3(id);
		return View(model);
	}

	public async Task<IActionResult> Mushak4P4()
	{
		var model = await _damageService.Query().Where(x => x.DamageTypeId == (int)EnumDamageType.NormalDamage)
			.SelectAsync();

		return View(model);
	}

	public async Task<IActionResult> Mushak4P4ById(int id)
	{
		try
		{
			var model = new vmMushak4P4();
			var detailList = await _damageDetailService.Query()
				.Include(x => x.Damage)
				.Include(x => x.Product)
				.Where(x => x.DamageId == id).SelectAsync();

			var details = new List<vmMushak4P4Details>();
			foreach (var item in detailList)
			{
				var data = new vmMushak4P4Details();
				data.ProductName = item.Product.Name;
				data.DamageQty = item.DamageQty;
				data.SuggestedUnitPrice = item.SuggestedNewUnitPrice;
				data.ReasonOfDamage = item.DamageDescription;


				if (item.PurchaseDetailId != null)
				{
					var purchase = await _purchaseDetailService.Query()
						.Where(x => x.PurchaseDetailId == item.PurchaseDetailId).SingleOrDefaultAsync();

					data.RealUnitPrice = purchase.UnitPrice;
					data.VATPercent = purchase.Vatpercent;
					data.PurchaseChallanNo = purchase.PurchaseId.ToString();
				}
				else if (item.SalesDetailId != null)
				{
					var sale = await _salesDetailService.Query().Where(x => x.SalesDetailId == item.SalesDetailId)
						.SingleOrDefaultAsync();

					data.RealUnitPrice = sale.UnitPrice;
					data.VATPercent = sale.Vatpercent;
					data.PurchaseChallanNo = sale.SalesId.ToString();
				}
				else
				{
					var transBook = await _productTransactionBookService.Query()
						.Where(x => x.DamageDetailId == item.DamageDetailId).SingleOrDefaultAsync();

					data.RealUnitPrice = transBook.InitUnitPrice;
					data.PurchaseChallanNo = transBook.ProductTransactionBookId.ToString();

					var vatPercent = await _productVatService.Query().Include(x => x.ProductVattype)
						.Where(x => x.ProductId == item.ProductId).SingleOrDefaultAsync();

					data.VATPercent = vatPercent.ProductVattype.DefaultVatPercent;
				}

				model.Details.Add(data);
			}

			model.OrgName = UserSession.OrganizationName;
			var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
			model.OrgAddress = org.Address;
			model.OrgBin = org.Bin;
			model.VatResponsiblePersonName = org.VatResponsiblePersonName;
			model.VatResponsiblePersonDesignation = org.VatResponsiblePersonDesignation;

			return View(model);
		}
		catch
		{
			return NotFound();
		}
	}


	public async Task<IActionResult> Mushak4P5()
	{
		var model = await _damageService.Query().Where(x => x.DamageTypeId == (int)EnumDamageType.AccidentDamage)
			.SelectAsync();

		return View(model);
	}

	public async Task<IActionResult> Mushak4P5ById(int id)
	{
		try
		{
			var model = new vmMushak4P5();
			var detailList = await _damageDetailService.Query()
				.Include(x => x.Damage)
				.Include(x => x.Product)
				.Where(x => x.DamageId == id).SelectAsync();

			var details = new List<vmMushak4P5Details>();
			foreach (var item in detailList)
			{
				var data = new vmMushak4P5Details();
				data.ProductName = item.Product.Name;
				data.ProdcutQty = item.DamageQty;
				data.SuggestedUnitPrice = item.SuggestedNewUnitPrice;
				data.ReasonOfDamage = item.DamageDescription;
				if (item.PurchaseDetailId != null)
				{
					var unitPrice = await _purchaseDetailService.Query()
						.Where(x => x.PurchaseDetailId == item.PurchaseDetailId).SingleOrDefaultAsync();

					data.RealUnitPrice = unitPrice.UnitPrice;
				}
				else if (item.SalesDetailId != null)
				{
					var unitPrice = await _salesDetailService.Query()
						.Where(x => x.SalesDetailId == item.SalesDetailId).SingleOrDefaultAsync();

					data.RealUnitPrice = unitPrice.UnitPrice;
				}
				else
				{
					var unitPrice = await _productTransactionBookService.Query()
						.Where(x => x.DamageDetailId == item.DamageDetailId).SingleOrDefaultAsync();

					//data.RealUnitPrice = unitPrice.InitUnitPrice;
				}

				model.Details.Add(data);
			}

			model.OrgName = UserSession.OrganizationName;
			var org = await _organizationService.GetOrganization(UserSession.ProtectedOrganizationId);
			model.OrgAddress = org.Address;
			model.OrgBin = org.Bin;
			model.VatResponsiblePersonName = org.VatResponsiblePersonName;
			model.VatResponsiblePersonDesignation = org.VatResponsiblePersonDesignation;

			return View(model);
		}
		catch
		{
			return NotFound();
		}
	}

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_USERS_REPORT)]
	public IActionResult UsersReport(int userStatus = 0)
	{
		var userStat = (EnumUserStatus)userStatus;
		//var model = new VmUsersReport
		//{
		//    UserStatus = userStat,
		//    OrganizationName = UserSession.OrganizationName,
		//    OrganizationAddress = UserSession.OrganizationAddress,
		//    HeaderOtherString = $"{userStat.ToString()} users report",
		//    Users = await _userService.GetAllByOrganization(UserSession.ProtectedOrganizationId, userStat)
		//};
		//return View(model);
		var model = new VmUsersReport();
		model.UserStatus = userStat;
		model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_USERS_REPORT)]
	public async Task<IActionResult> UsersReport(VmUsersReport model)
	{
		var userStat = (EnumUserStatus)model.UserStatus;
		var userList = await _userService.GetAllByOrganization(UserSession.ProtectedOrganizationId, userStat);

		var users = _mapper.Map<List<User>, List<vmUserReportRdlc>>(userList.ToList());
		foreach (var item in users)
		{
			item.OrgName = UserSession.OrganizationName;

		}
		return null;
		//return ProcessReport(purchaseDetailsData.ToList(),
		//	RdlcReportFileOption.DebitNoteDetailsListReportUrl,
		//	RdlcReportFileOption.DebitNoteDetailsListReportDsName,
		//	StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.DebitNoteDetailsListReportFileName),
		//	GetParameterForPurchase(model.FromDate, model.ToDate, "Debit Note Details Report"), model.ReportProcessOptionId);
	}

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_USERS_REPORT)]
	public async Task<IActionResult> UsersReportExcel(int userStatus = 0)
	{
		try
		{
			var userStat = (EnumUserStatus)userStatus;
			var userList = await _userService.GetAllByOrganization(UserSession.ProtectedOrganizationId, userStat);

			var users = _mapper.Map<List<User>, List<vmUserReportRdlc>>(userList.ToList());
			foreach (var item in users)
			{
				item.OrgName = UserSession.OrganizationName;
			}

			var parameters = new Dictionary<string, string>
			{
				{ "OrganizationName", UserSession.OrganizationName },
				{ "OrganizationAddress", UserSession.OrganizationAddress },
				{ "HeaderOtherString", $"{userStat.ToString()} users report" }
			};
			return GetReportExcel(users, RdlcReportFileOption.UserReportUrl, RdlcReportFileOption.UserReportDsName, RdlcReportFileOption.UserReportFileName, parameters);
		}
		catch (Exception ex)
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			return RedirectToAction(nameof(UsersReport));
		}

	}

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_USERS_REPORT)]
	public async Task<IActionResult> UsersLoginReport(int userId = 0, DateTime? fromDate = null, DateTime? toDate = null)
	{
		var currentTime = DateTime.Now;
		var users = await _userService.GetAllByOrganization(UserSession.ProtectedOrganizationId, EnumUserStatus.All);
		var model = new VmUserLoginHistory
		{
			FromDate = fromDate ?? new DateTime(currentTime.Year, currentTime.Month, 1),
			ToDate = toDate ?? new DateTime(currentTime.Year, currentTime.Month, currentTime.Day),
			UserId = userId,
			UserList = await _userService.GetAllByOrganizationSelectList(UserSession.ProtectedOrganizationId),
			OrganizationName = UserSession.OrganizationName,
			OrganizationAddress = UserSession.OrganizationAddress
		};
		model.HeaderDateString = $"Login History from {StringGenerator.DateTimeToStringWithoutTime(model.FromDate)} to {StringGenerator.DateTimeToStringWithoutTime(model.ToDate)}";

		if (model.UserId != 0)
		{
			var user = await _userService.GetUserByOrgAndId(model.UserId, UserSession.OrganizationId);
			if (user != null)
			{
				model.HeaderOtherString = $"User: {user.FullName}";
			}
		}

		model.UserLoginHistories =
			(await _loginHistoryService.GetByOrganizationAndUser(UserSession.OrganizationId, model.UserId, model.FromDate, model.ToDate)).ToList();

		return View(model);
	}

	public async Task<IActionResult> MushakFourPointThreeReport()
	{
		var model = new VmPriceSetupReportParam();
		model.PriceDeclarId = 0;
		model.OrgId = 0;
		var priceSetupList = await _priceSetupService.Query().Include(x => x.Product).Where(x => x.IsActive && x.OrganizationId == UserSession.OrganizationId).SelectAsync();
		//var productList = await _productService.GetProductsSelectList(UserSession.ProtectedOrganizationId);
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem { Text = s.Product.Name, Value = s.PriceSetupId.ToString() });
		//model.PriceSetupList = productList;

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> MushakFourPointThreeReport(VmPriceSetupReportParam model)
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
		var priceSetupList = await _priceSetupService.Query().Include(x => x.Product).Where(x => x.IsActive && x.OrganizationId == UserSession.OrganizationId).SelectAsync();
		model.PriceSetupList = priceSetupList.Select(s => new SelectListItem { Text = s.Product.Name, Value = s.PriceSetupId.ToString() });

		var org = await _organizationService.GetOrganization(IvatDataProtector.Protect(UserSession.OrganizationId.ToString()));
		if (org != null)
		{
			model.OrgAddress = org.Address;
			model.OrgBin = org.Bin;
		}
		model.OrgName = UserSession.OrganizationName;

		return View(model);
	}



}