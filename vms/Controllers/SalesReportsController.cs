using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vms.Utility;
using vms.entity.viewModels.NewReports;
using System;
using vms.entity.viewModels;
using System.Collections.Generic;
using System.Linq;
using vms.utility.StaticData;
using vms.entity.models;
using AutoMapper;
using vms.entity.viewModels.ReportsViewModel;
using vms.entity.viewModels.SalesReport;
using vms.utility;
using vms.service.Services.TransactionService;
using vms.service.Services.ReportService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class SalesReportsController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly ISalesReportService _salesReportService;
    private readonly ISalesSummeryReportService _salesSummeryReportService;
    private readonly IOrgBranchService _branchService;
    private readonly ICustomerService _customerService;
    private readonly IReportOptionService _reportOptionService;
    private readonly IMapper _mapper;

    public SalesReportsController(ControllerBaseParamModel controllerBaseParamModel,
        ISaleService saleService,
        ISalesReportService salesReportService,
        IOrgBranchService branchService,
        ICustomerService customerService,
        IReportOptionService reportOptionService,
        IMapper mapper, ISalesSummeryReportService salesSummeryReportService) : base(controllerBaseParamModel)
    {
        _saleService = saleService;
        _salesReportService = salesReportService;
        _branchService = branchService;
        _customerService = customerService;
        _reportOptionService = reportOptionService;
        _mapper = mapper;
        _salesSummeryReportService = salesSummeryReportService;
    }

    #region All Sales Report

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public IActionResult Index()
    {
        var currentDate = DateTime.Today;
        var model = new SalesReportParameterViewModel
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> Index(SalesReportParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }

        var sales = await _salesReportService.GetSalesReportAll(UserSession.OrganizationId, model.FromDate,
            model.ToDate, UserSession.UserId);
        return ProcessReport(sales.ToList(),
            RdlcReportFileOption.SalesAllReportUrl,
            RdlcReportFileOption.SalesAllReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.SalesAllReportFileName),
            GetParameterForSales(model.FromDate, model.ToDate, "All Sales Report"), model.ReportProcessOptionId);
    }

    #endregion

    #region All Sales Group by Customer Report

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public IActionResult SalesReportCustomerGrouped()
    {
        var currentDate = DateTime.Today;
        var model = new SalesReportParameterViewModel
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesReportCustomerGrouped(SalesReportParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }

        var sales = await _salesReportService.GetSalesReportAll(UserSession.OrganizationId, model.FromDate,
            model.ToDate, UserSession.UserId);
        return ProcessReport(sales.ToList(),
            RdlcReportFileOption.SalesAllCustomerGroupedReportUrl,
            RdlcReportFileOption.SalesAllCustomerGroupedReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.SalesAllCustomerGroupedReportFileName),
            GetParameterForSales(model.FromDate, model.ToDate, "All Sales Report Grouped by Customer"),
            model.ReportProcessOptionId);
    }

    #endregion

    #region Sales Report By Branch

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesReportByBranch()
    {
        var currentDate = DateTime.Today;
        var model = new SalesReportByBranchParameterViewModel()
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
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesReportByBranch(SalesReportByBranchParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BranchSelectListItems =
                await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }

        var sales = await _salesReportService.GetSalesReportByBranch(UserSession.OrganizationId, model.BranchId,
            model.FromDate, model.ToDate, UserSession.UserId);
        return ProcessReport(sales.ToList(),
            RdlcReportFileOption.SalesReportByBranchUrl,
            RdlcReportFileOption.SalesReportByBranchDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.SalesReportByBranchFileName),
            GetParameterForSales(model.FromDate, model.ToDate, "Sales by Branch Report"), model.ReportProcessOptionId);
    }

    #endregion

    #region Sales Report By Customer

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesReportByCustomer()
    {
        var currentDate = DateTime.Today;
        var model = new SalesReportByCustomerParameterViewModel()
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            CustomerSelectListItems =
                await _customerService.GetCustomerSelectListItems(UserSession.ProtectedOrganizationId),
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesReportByCustomer(SalesReportByCustomerParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.CustomerSelectListItems =
                await _customerService.GetCustomerSelectListItems(UserSession.ProtectedOrganizationId);
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }

        var sales = await _salesReportService.GetSalesReportByCustomer(UserSession.OrganizationId, model.CustomerId,
            model.FromDate, model.ToDate, UserSession.UserId);
        return ProcessReport(sales.ToList(),
            RdlcReportFileOption.SalesReportByCustomerUrl,
            RdlcReportFileOption.SalesReportByCustomerDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.SalesReportByCustomerFileName),
            GetParameterForSales(model.FromDate, model.ToDate, "Sales by Customer Report"),
            model.ReportProcessOptionId);
    }

    #endregion

    #region Sales Report By Branch and Customer

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesReportByBranchAndCustomer()
    {
        var currentDate = DateTime.Today;
        var model = new SalesReportByBranchAndCustomerParameterViewModel()
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            BranchSelectListItems = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
            CustomerSelectListItems =
                await _customerService.GetCustomerSelectListItems(UserSession.ProtectedOrganizationId),
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesReportByBranchAndCustomer(
        SalesReportByBranchAndCustomerParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.BranchSelectListItems =
                await _branchService.GetOrgBranchSelectList(UserSession.ProtectedOrganizationId);
            model.CustomerSelectListItems =
                await _customerService.GetCustomerSelectListItems(UserSession.ProtectedOrganizationId);
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }

        ;
        var sales = await _salesReportService.GetSalesReportByBranchAndCustomer(UserSession.OrganizationId,
            model.BranchId, model.CustomerId,
            model.FromDate, model.ToDate, UserSession.UserId);
        return ProcessReport(sales.ToList(),
            RdlcReportFileOption.SalesReportByBranchAndCustomerUrl,
            RdlcReportFileOption.SalesReportByBranchAndCustomerDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.SalesReportByBranchAndCustomerFileName),
            GetParameterForSales(model.FromDate, model.ToDate, "Sales by Branch and Customer Report"),
            model.ReportProcessOptionId);
    }

    #endregion

    #region All Sales Summery Report

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public IActionResult Summery()
    {
        var currentDate = DateTime.Today;
        var model = new SalesReportParameterViewModel
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(currentDate.Year, currentDate.Month),
            ToDate = currentDate,
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> Summery(SalesReportParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }

        var sales = await _salesSummeryReportService.GetAll(UserSession.OrganizationId, model.FromDate,
            model.ToDate, UserSession.UserId);
        return ProcessReport(sales.ToList(),
            RdlcReportFileOption.SalesSummeryAllReportUrl,
            RdlcReportFileOption.SalesSummeryAllReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.SalesSummeryAllReportFileName),
            GetParameterForSales(model.FromDate, model.ToDate, "All Sales Summery Report"),
            model.ReportProcessOptionId);
    }

    #endregion

    #region Sales Due Aging Report

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public IActionResult SalesDueAging()
    {
        var model = new ReportOptionViewModel
        {
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesDueAging(ReportOptionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }

        var sales = await _saleService.GetSalesAgingReport(UserSession.ProtectedOrganizationId);
        return ProcessReport(sales.ToList(),
            RdlcReportFileOption.SalesDueAgingReportUrl,
            RdlcReportFileOption.SalesDueAgingReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.SalesDueAgingReportFileName),
            GetParameterForSales(DateTime.Now, DateTime.Now, "Sales Due Aging Report"), model.ReportProcessOptionId);
    }

    #endregion


    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public IActionResult SalesReport()
    {
        vmSalesReport model = new vmSalesReport();
        model.FromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        model.ToDate = DateTime.Today;
        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Sales Report";
        head.CompanyName = UserSession.OrganizationName;
        model.head = head;
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesReport(vmSalesReport model)
    {
        if (ModelState.IsValid)
        {
            //vmSalesReport report = new vmSalesReport();

            //report.salesList = await _saleService
            //    .Query()
            //    .Where(x => x.OrganizationId == _session.OrganizationId)
            //    .Where(x => x.SalesDate >= model.FromDate && x.SalesDate <= model.ToDate)
            //    .SelectAsync();
            model.salesList = await _saleService
                .Query()
                .Include(x => x.Customer)
                .Where(x => x.OrganizationId == UserSession.OrganizationId)
                .Where(x => x.SalesDate >= model.FromDate && x.SalesDate < model.ToDate.AddDays(1))
                .SelectAsync();
        }

        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Sales Report";
        head.CompanyName = UserSession.OrganizationName;
        model.head = head;
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_REPORT)]
    public async Task<IActionResult> SalesReportExcel(vmSalesReport model)
    {
        if (ModelState.IsValid)
        {
            model.salesList = await _saleService
                .Query()
                .Include(x => x.Customer)
                .Where(x => x.OrganizationId == UserSession.OrganizationId)
                .Where(x => x.SalesDate >= model.FromDate && x.SalesDate <= model.ToDate)
                .SelectAsync();

            var sales = _mapper.Map<List<Sale>, List<vmSalesReportRdlc>>(model.salesList.ToList());
            foreach (var item in sales)
            {
                item.OrgName = UserSession.OrganizationName;
                item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
                item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
            }

            return GetReportExcel(sales, RdlcReportFileOption.SalesrReportUrl,
                RdlcReportFileOption.SalesReportDsName, RdlcReportFileOption.SalesReportFileName);
        }

        return RedirectToAction(nameof(SalesReport));
    }


    public async Task<IActionResult> SalesDueReport()
    {
        int org = UserSession.OrganizationId;

        vmSalesDueReport report = new vmSalesDueReport();

        report.salesDueList = await _saleService.GetSalesDue(org);

        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Sales Due Report";

        report.head = head;

        return View(report);
    }

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_VDS_LIST_REPORT)]
    public async Task<IActionResult> SalesVDSListReport()
    {
        var fromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        var toDate = DateTime.Today;

        vmSalesVDSListReport report = new vmSalesVDSListReport();
        report.saleVDSList = await _saleService.Query()
            .Where(c => c.OrganizationId == UserSession.OrganizationId && c.IsVatDeductedInSource == true)
            .Include(p => p.Customer)
            .Include(c => c.CreditNotes)
            .Include(p => p.Organization)
            .Where(x => x.SalesDate >= fromDate && x.SalesDate < toDate.AddDays(1))
            .OrderByDescending(c => c.SalesId).SelectAsync();


        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Sales VDS List Report";
        report.FromDate = fromDate;
        report.ToDate = toDate;
        head.CompanyName = UserSession.OrganizationName;

        report.head = head;
        report.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();

        return View(report);
    }


    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_VDS_LIST_REPORT)]
    public async Task<IActionResult> SalesVDSListReport(vmSalesVDSListReport model)
    {
        //vmSalesVDSListReport report = new vmSalesVDSListReport();

        //      report.saleVDSList = await _saleService.Query()
        //              .Where(c => c.OrganizationId == UserSession.OrganizationId && c.IsVatDeductedInSource == true)
        //              .Include(p => p.Customer)
        //              .Include(c => c.CreditNotes)
        //              .Include(p => p.Organization)
        //              .Where(x => x.SalesDate >= model.FromDate && x.SalesDate < model.ToDate.AddDays(1))
        //              .OrderByDescending(c => c.SalesId).SelectAsync();

        //      var sales = _mapper.Map<List<Sale>, List<vmSalesReportRdlc>>(report.saleVDSList.ToList());
        //      foreach (var item in sales)
        //      {
        //          item.OrgName = UserSession.OrganizationName;
        //          item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
        //          item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
        //      }

        var salesData = await _saleService.GetSalesViewByOrgAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
            salesData = salesData.Where(x => x.SalesDate >= model.FromDate 
                                    && x.SalesDate < model.ToDate.AddDays(1))
                            .OrderByDescending(c => c.SalesId);



        return ProcessReport(salesData.ToList(),
           RdlcReportFileOption.SalesVdsListReportUrl,
           RdlcReportFileOption.SalesVdsListReportDsName,
           StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.SalesVdsListReportFileName),
           GetParameterForSales(model.FromDate, model.ToDate, "Sales VDS List Report"), model.ReportProcessOptionId
           );
    }

    //[HttpPost]
    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_SALES_VDS_LIST_REPORT)]
    //public async Task<IActionResult> SalesVDSListReportExcel(vmSalesVDSListReport model)
    //{
    //	vmSalesVDSListReport report = new vmSalesVDSListReport();

    //	if (ModelState.IsValid)
    //	{
    //		report.saleVDSList = await _saleService.Query()
    //			.Where(c => c.OrganizationId == UserSession.OrganizationId && c.IsVatDeductedInSource == true)
    //			.Include(p => p.Customer)
    //			.Include(c => c.CreditNotes)
    //			.Include(p => p.Organization)
    //			.Where(x => x.SalesDate >= model.FromDate && x.SalesDate < model.ToDate.AddDays(1))
    //			.OrderByDescending(c => c.SalesId).SelectAsync();

    //		var sales = _mapper.Map<List<Sale>, List<vmSalesReportRdlc>>(report.saleVDSList.ToList());
    //		foreach (var item in sales)
    //		{
    //			item.OrgName = UserSession.OrganizationName;
    //			item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
    //			item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
    //		}

    //		return GetReportExcel(sales, RdlcReportFileOption.SalesVdsListReportUrl,
    //			RdlcReportFileOption.SalesVdsListReportDsName, RdlcReportFileOption.SalesVdsListReportFileName);
    //	}

    //	return RedirectToAction(nameof(SalesVDSListReport));
    //}


    #region Sales Details List

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_DETAILS_LIST_REPORT)]
    public IActionResult SalesDetailsListReport()
    {
        var currentDate = DateTime.Today;
        var model = new SalesDetailsListReportParamViewModel
        {
            FromDate = DateTime.Now,
            ToDate = DateTime.Now,
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_DETAILS_LIST_REPORT)]
    public async Task<IActionResult> SalesDetailsListReport(SalesDetailsListReportParamViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }
        var salesDetailsData = await _salesReportService.GetSalesDetailsListDataByOrgAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
        salesDetailsData = salesDetailsData
        .Where(c =>
                    c.CreatedTime >= model.FromDate
                    && c.CreatedTime <= model.ToDate.AddDays(1));

        return ProcessReport(salesDetailsData.ToList(),
            RdlcReportFileOption.SalesDetailsListReportUrl,
            RdlcReportFileOption.SalesDetailsListReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.SalesDetailsListReportFileName),
            GetParameterForSales(model.FromDate, model.ToDate, "Sales Details List Report"), model.ReportProcessOptionId);
    }

    #endregion

    #region Credit note Details List

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_CREDIT_NOTE_DETAILS_LIST_REPORT)]
    public IActionResult CreditNoteDetailsReport()
    {
        var currentDate = DateTime.Today;
        var model = new CreditNoteDetailsListReportParamViewModel
        {
            FromDate = DateTime.Now,
            ToDate = DateTime.Now,
            ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList()
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_CREDIT_NOTE_DETAILS_LIST_REPORT)]
    public async Task<IActionResult> CreditNoteDetailsReport(CreditNoteDetailsListReportParamViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            model.ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList();
            return View(model);
        }
        var salesDetailsData = await _salesReportService.CreditNoteDetailsReportByOrgAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
        salesDetailsData = salesDetailsData
        .Where(c =>
                    c.CreatedTime >= model.FromDate
                    && c.CreatedTime <= model.ToDate.AddDays(1));

        return ProcessReport(salesDetailsData.ToList(),
            RdlcReportFileOption.CreditNoteDetailsListReportUrl,
            RdlcReportFileOption.CreditNoteDetailsListReportDsName,
            StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.CreditNoteDetailsListReportFileName),
            GetParameterForSales(model.FromDate, model.ToDate, "Credit Note Details List Report"), model.ReportProcessOptionId);
    }

    #endregion

    private Dictionary<string, string> GetParameterForSales(DateTime? fromDate, DateTime? toDate, string reportHeaderName)
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