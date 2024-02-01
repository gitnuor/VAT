using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vms.Utility;
using System;
using vms.entity.viewModels;
using System.Collections.Generic;
using System.Linq;
using vms.utility.StaticData;
using vms.entity.viewModels.PurchaseReport;
using vms.utility;
using vms.service.Services.ReportService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class PurchaseReportsController : ControllerBase
{
    private readonly IPurchaseReportService _purchaseReportService;
    private readonly IPurchaseSummeryReportService _purchaseSummeryReportService;
    private readonly IOrgBranchService _branchService;
    private readonly IVendorService _vendorService;
    private readonly IReportOptionService _reportOptionService;

    public PurchaseReportsController(ControllerBaseParamModel controllerBaseParamModel,
        IPurchaseReportService purchaseReportService,
        IOrgBranchService branchService,
        IVendorService vendorService,
        IReportOptionService reportOptionService, IPurchaseSummeryReportService purchaseSummeryReportService) : base(controllerBaseParamModel)
    {
        _purchaseReportService = purchaseReportService;
        _branchService = branchService;
        _vendorService = vendorService;
        _reportOptionService = reportOptionService;
        _purchaseSummeryReportService = purchaseSummeryReportService;
    }

    #region All Purchase Report

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public IActionResult Index()
    {
        var currentDate = DateTime.Today;
        var model = new PurchaseReportParameterViewModel
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> Index(PurchaseReportParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }

        var purchase = await _purchaseReportService.GetPurchaseReportAll(UserSession.OrganizationId, model.FromDate,
            model.ToDate, UserSession.UserId);

        return ProcessReport(purchase.ToList(),
            RdlcReportFileOption.PurchaseAllReportUrl,
            RdlcReportFileOption.PurchaseAllReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.PurchaseAllReportFileName),
            GetParameterForPurchase(model.FromDate, model.ToDate, "All Purchase Report"), model.ReportProcessOptionId);
    }

    #endregion

    #region All Purchase Group by Vendor Report

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public IActionResult PurchaseReportVendorGrouped()
    {
        var currentDate = DateTime.Today;
        var model = new PurchaseReportParameterViewModel
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReportVendorGrouped(PurchaseReportParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }
        var purchase = await _purchaseReportService.GetPurchaseReportAll(UserSession.OrganizationId, model.FromDate,
            model.ToDate, UserSession.UserId);
        return ProcessReport(purchase.ToList(),
            RdlcReportFileOption.PurchaseAllVendorGroupedReportUrl,
            RdlcReportFileOption.PurchaseAllVendorGroupedReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.PurchaseAllVendorGroupedReportFileName),
            GetParameterForPurchase(model.FromDate, model.ToDate, "All Purchase Report Grouped by Vendor"), model.ReportProcessOptionId);
    }

    #endregion

    #region Purchase Report By Branch

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReportByBranch()
    {
        var currentDate = DateTime.Today;
        var model = new PurchaseReportByBranchParameterViewModel()
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            BranchSelectListItems = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReportByBranch(PurchaseReportByBranchParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BranchSelectListItems =
                await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }
        var purchase = await _purchaseReportService.GetPurchaseReportByBranch(UserSession.OrganizationId, model.BranchId,
            model.FromDate, model.ToDate, UserSession.UserId);
        return ProcessReport(purchase.ToList(),
            RdlcReportFileOption.PurchaseReportByBranchUrl,
            RdlcReportFileOption.PurchaseReportByBranchDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.PurchaseReportByBranchFileName),
            GetParameterForPurchase(model.FromDate, model.ToDate, "Purchase by Branch Report"), model.ReportProcessOptionId);
    }

    #endregion

    #region Purchase Report By Vendor

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReportByVendor()
    {
        var currentDate = DateTime.Today;
        var model = new PurchaseReportByVendorParameterViewModel()
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            VendorSelectListItems = await _vendorService.GetVendorSelectListItems(UserSession.ProtectedOrganizationId),
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReportByVendor(PurchaseReportByVendorParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.VendorSelectListItems =
                await _vendorService.GetVendorSelectListItems(UserSession.ProtectedOrganizationId);
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }
        var purchase = await _purchaseReportService.GetPurchaseReportByVendor(UserSession.OrganizationId, model.VendorId,
            model.FromDate, model.ToDate, UserSession.UserId);
        return ProcessReport(purchase.ToList(),
            RdlcReportFileOption.PurchaseReportByVendorUrl,
            RdlcReportFileOption.PurchaseReportByVendorDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.PurchaseReportByVendorFileName),
            GetParameterForPurchase(model.FromDate, model.ToDate, "Purchase by Supplier/Vendor Report"), model.ReportProcessOptionId);
    }

    #endregion

    #region Purchase Report By Branch and Vendor

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReportByBranchAndVendor()
    {
        var currentDate = DateTime.Today;
        var model = new PurchaseReportByBranchAndVendorParameterViewModel()
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            BranchSelectListItems = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
            VendorSelectListItems = await _vendorService.GetVendorSelectListItems(UserSession.ProtectedOrganizationId),
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReportByBranchAndVendor(PurchaseReportByBranchAndVendorParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BranchSelectListItems =
                await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);
            model.VendorSelectListItems =
                await _vendorService.GetVendorSelectListItems(UserSession.ProtectedOrganizationId);
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }

        var purchase = await _purchaseReportService.GetPurchaseReportByBranchAndVendor(UserSession.OrganizationId, model.BranchId, model.VendorId,
            model.FromDate, model.ToDate, UserSession.UserId);
        return ProcessReport(purchase.ToList(),
            RdlcReportFileOption.PurchaseReportByBranchAndVendorUrl,
            RdlcReportFileOption.PurchaseReportByBranchAndVendorDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.PurchaseReportByBranchAndVendorFileName),
            GetParameterForPurchase(model.FromDate, model.ToDate, "Purchase by Branch and Vendor Report"), model.ReportProcessOptionId);
    }

    #endregion

    #region All Purchase Summery Report

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public IActionResult Summery()
    {
        var currentDate = DateTime.Today;
        var model = new PurchaseReportParameterViewModel
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> Summery(PurchaseReportParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }
        var purchase = await _purchaseSummeryReportService.GetAll(UserSession.OrganizationId, model.FromDate,
            model.ToDate, UserSession.UserId);
        return ProcessReport(purchase.ToList(),
            RdlcReportFileOption.PurchaseSummeryAllReportUrl,
            RdlcReportFileOption.PurchaseSummeryAllReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.PurchaseSummeryAllReportFileName),
            GetParameterForPurchase(model.FromDate, model.ToDate, "All Purchase Summery Report"), model.ReportProcessOptionId);
    }

    #endregion

    #region Purchase details list report

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_DETAILS_LIST_REPORT)]
    public IActionResult PuchaseDetailsListReport()
    {
        var currentDate = DateTime.Today;
        var model = new PuchaseDetailsListReportParamViewModel
        {
            FromDate = DateTime.Now,
            ToDate = DateTime.Now,
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_DETAILS_LIST_REPORT)]
    public async Task<IActionResult> PuchaseDetailsListReport(PuchaseDetailsListReportParamViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }
        var purchaseDetailsData = await _purchaseReportService.GetPuchaseDetailsListDataByOrgAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
        purchaseDetailsData = purchaseDetailsData
        .Where(c =>
                    c.CreatedTime >= model.FromDate
                    && c.CreatedTime <= model.ToDate.AddDays(1));

        return ProcessReport(purchaseDetailsData.ToList(),
            RdlcReportFileOption.PurchaseDetailsListReportUrl,
            RdlcReportFileOption.PurchaseDetailsListReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.PurchaseDetailsListReportFileName),
            GetParameterForPurchase(model.FromDate, model.ToDate, "Purchase Details List Report"), model.ReportProcessOptionId);
    }

	#endregion

	#region Debit note report

	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_DEBIT_NOTE_DETAILS_LIST_REPORT)]
	public IActionResult DebitNoteDetailsReport()
	{
		var currentDate = DateTime.Today;
		var model = new DebitNoteDetailsListReportParamViewModel
		{
			FromDate = DateTime.Now,
			ToDate = DateTime.Now,
			ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
		};
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.REPORTS)]
	[VmsAuthorize(FeatureList.REPORTS_DEBIT_NOTE_DETAILS_LIST_REPORT)]
	public async Task<IActionResult> DebitNoteDetailsReport(DebitNoteDetailsListReportParamViewModel model)
	{
		if (!ModelState.IsValid)
		{
			model.FromDate = DateTime.Now;
			model.ToDate = DateTime.Now;
			model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
			return View(model);
		}
		var purchaseDetailsData = await _purchaseReportService.DebitNoteDetailsReportByOrgAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		purchaseDetailsData = purchaseDetailsData
		.Where(c =>
					c.CreatedTime >= model.FromDate
					&& c.CreatedTime <= model.ToDate.AddDays(1));

		return ProcessReport(purchaseDetailsData.ToList(),
			RdlcReportFileOption.DebitNoteDetailsListReportUrl,
			RdlcReportFileOption.DebitNoteDetailsListReportDsName,
			StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.DebitNoteDetailsListReportFileName),
			GetParameterForPurchase(model.FromDate, model.ToDate, "Debit Note Details Report"), model.ReportProcessOptionId);
	}

	#endregion

	private Dictionary<string, string> GetParameterForPurchase(DateTime fromDate, DateTime toDate, string reportHeaderName)
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
}