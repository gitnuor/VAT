using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vms.Utility;
using vms.entity.viewModels.NewReports;
using Microsoft.AspNetCore.DataProtection;
using System;
using vms.entity.viewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using vms.utility.StaticData;
using vms.entity.models;
using AutoMapper;
using vms.utility;
using vms.service.Services.TransactionService;
using vms.service.Services.ReportService;
using vms.service.Services.SecurityService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.PaymentService;

namespace vms.Controllers;

public class NewReportsController : ControllerBase
{
    private readonly IDamageInvoiceListService _invoiceService;
    private readonly IVendorService _vendorService;
    private readonly ICustomerService _customerService;
    private readonly ISaleService _saleService;
    private readonly IPurchaseOrderService _purchaseOrderService;
    private readonly IAuditLogService _auditLogService;
    private readonly IProductionService _productionService;
    private readonly IUserService _userService;
    private readonly IContractVendorService _contractVendorService;
    private readonly IContractVendorTransferRawMaterialService _rawMaterialService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ISalesPaymentReceiveService _salesPaymentReceiveService;
    private readonly IDashboardService _dashboardService;
    private readonly IMapper _mapper;
    private readonly IReportOptionService _reportOptionService;

    public NewReportsController(
        ControllerBaseParamModel controllerBaseParamModel,
        IDamageInvoiceListService invoiceService, IVendorService vendorService, ICustomerService customerService,
        ISaleService saleService, IPurchaseOrderService purchaseOrderService, IAuditLogService auditLogService,
        IProductionService productionService, IUserService userService,
        IContractVendorService contractVendorService, IContractVendorTransferRawMaterialService rawMaterialService,
        IPaymentMethodService paymentMethodService, ISalesPaymentReceiveService salesPaymentReceiveService,
        IMapper mapper, IDashboardService dashboardService, IReportOptionService reportOptionService) : base(controllerBaseParamModel)
    {
        _invoiceService = invoiceService;
        _vendorService = vendorService;
        _customerService = customerService;
        _saleService = saleService;
        _purchaseOrderService = purchaseOrderService;
        _auditLogService = auditLogService;
        _productionService = productionService;
        _userService = userService;
        _contractVendorService = contractVendorService;
        _rawMaterialService = rawMaterialService;
        _paymentMethodService = paymentMethodService;
        _salesPaymentReceiveService = salesPaymentReceiveService;
        _mapper = mapper;
        _dashboardService = dashboardService;
        _reportOptionService = reportOptionService;
    }

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_DAMAGE_REPORT)]
    public async Task<IActionResult> DamageReport()
    {
        int org = UserSession.OrganizationId;


        VmDamageRepot report = new VmDamageRepot();


        report.SpDamageList = await _invoiceService.GetDamageList(org);

        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Damage Report";


        report.head = head;


        return View(report);
    }

    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_VENDOR_REPORT)]
    //public async Task<IActionResult> VendorReport()
    //{
    //    vmVendorReport report = new vmVendorReport();

    //    report.FromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
    //    report.ToDate = DateTime.Today;

    //    report.vendorList = await _vendorService.Query().Where(x => x.OrganizationId == UserSession.OrganizationId)
    //        .Where(x => x.CreatedTime >= report.FromDate && x.CreatedTime <= report.ToDate).SelectAsync();

    //    HeaderModel head = new HeaderModel();
    //    head.LogoName = "";
    //    head.ReportName = "Vendor Report";
    //    head.CompanyName = UserSession.OrganizationName;

    //    report.head = head;

    //    return View(report);
    //}

    //[HttpPost]
    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_VENDOR_REPORT)]
    //public async Task<IActionResult> VendorReport(vmVendorReport report)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        report.vendorList = await _vendorService.Query()
    //            .Where(x => x.OrganizationId == UserSession.OrganizationId)
    //            .Where(x => x.CreatedTime >= report.FromDate && x.CreatedTime < report.ToDate.AddDays(1))
    //            .SelectAsync();
    //    }

    //    HeaderModel head = new HeaderModel();
    //    head.LogoName = "";
    //    head.ReportName = "Vendor Report";
    //    head.CompanyName = UserSession.OrganizationName;
    //    report.head = head;

    //    return View(report);
    //}

    //[HttpPost]
    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_VENDOR_REPORT)]
    //public async Task<IActionResult> VendorReportExcel(vmVendorReport report)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        report.vendorList = await _vendorService.Query()
    //            .Where(x => x.OrganizationId == UserSession.OrganizationId)
    //            .Where(x => x.CreatedTime >= report.FromDate && x.CreatedTime < report.ToDate.AddDays(1))
    //            .SelectAsync();

    //        var vendors = _mapper.Map<List<Vendor>, List<vmVendorReportRdlc>>(report.vendorList.ToList());
    //        foreach (var item in vendors)
    //        {
    //            item.OrgName = UserSession.OrganizationName;
    //            item.FromDate = report.FromDate.ToString("dd/MM/yyyy");
    //            item.ToDate = report.ToDate.ToString("dd/MM/yyyy");
    //        }

    //        return GetReportExcel(vendors, RdlcReportFileOption.VendorReportUrl,
    //            RdlcReportFileOption.VendorReportDsName, RdlcReportFileOption.VendorReportFileName);
    //    }

    //    return RedirectToAction(nameof(VendorReport));
    //}

    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
    //public async Task<IActionResult> CustomerReport()
    //{
    //    var fromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
    //    var toDate = DateTime.Today;
    //    var report = new vmCustomerReport();

    //    report.customerList = await _customerService
    //        .Query()
    //        .Where(x => x.OrganizationId == UserSession.OrganizationId)
    //        .Where(x => x.CreatedTime >= fromDate && x.CreatedTime < toDate.AddDays(1))
    //        .SelectAsync();
    //    HeaderModel head = new HeaderModel();
    //    head.LogoName = "";
    //    head.ReportName = "Customer Report";
    //    head.CompanyName = UserSession.OrganizationName;
    //    report.FromDate = fromDate;
    //    report.ToDate = toDate;

    //    report.head = head;
    //    return View(report);
    //}

    //[HttpPost]
    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
    //public async Task<IActionResult> CustomerReport(vmCustomerReport report)
    //{
    //    // vmCustomerReport report = new vmCustomerReport();
    //    if (ModelState.IsValid)
    //    {
    //        report.customerList = await _customerService
    //            .Query()
    //            .Where(x => x.OrganizationId == UserSession.OrganizationId)
    //            .Where(x => x.CreatedTime >= report.FromDate && x.CreatedTime < report.ToDate.AddDays(1))
    //            .SelectAsync();
    //    }

    //    HeaderModel head = new HeaderModel();
    //    head.LogoName = "";
    //    head.ReportName = "Customer Report";
    //    head.CompanyName = UserSession.OrganizationName;

    //    report.head = head;

    //    return View(report);
    //}

    //[HttpPost]
    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
    //public async Task<IActionResult> CustomerReportExcel(vmCustomerReport report)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        report.customerList = await _customerService
    //            .Query()
    //            .Where(x => x.OrganizationId == UserSession.OrganizationId)
    //            .Where(x => x.CreatedTime >= report.FromDate && x.CreatedTime <= report.ToDate)
    //            .SelectAsync();

    //        var customers = _mapper.Map<List<Customer>, List<vmCustomerReportRdlc>>(report.customerList.ToList());
    //        foreach (var item in customers)
    //        {
    //            item.OrgName = UserSession.OrganizationName;
    //            item.FromDate = report.FromDate.ToString("dd/MM/yyyy");
    //            item.ToDate = report.ToDate.ToString("dd/MM/yyyy");
    //        }

    //        return GetReportExcel(customers, RdlcReportFileOption.CustomerReportUrl,
    //            RdlcReportFileOption.CustomerReportDsName, RdlcReportFileOption.CustomerReportFileName);
    //    }

    //    return RedirectToAction(nameof(CustomerReport));
    //}


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

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReport()
    {
        var fromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        var toDate = DateTime.Today;

        var model = new VmPurchaseReport
        {
            FromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month),
            ToDate = DateTime.Today,
            PurchaseList = await _purchaseOrderService
                .Query()
                .Include(x => x.PurchaseType)
                .Include(x => x.Vendor)
                .Where(x => x.OrganizationId == UserSession.OrganizationId)
                .Where(x => x.PurchaseDate >= fromDate)
                .Where(x => x.PurchaseDate < toDate.AddDays(1))
                .SelectAsync(),
            HeaderModel = new HeaderModel
            {
                LogoName = "",
                ReportName = "Purchase Report",
                CompanyName = UserSession.OrganizationName
            }
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReport(VmPurchaseReport model)
    {
        if (ModelState.IsValid)
        {
            model.PurchaseList = await _purchaseOrderService
                .Query()
                .Include(x => x.Vendor)
                .Include(x => x.PurchaseType)
                .Where(x => x.OrganizationId == UserSession.OrganizationId)
                .Where(x => x.PurchaseDate >= model.FromDate)
                .Where(x => x.PurchaseDate < model.ToDate.AddDays(1))
                .SelectAsync();
        }

        model.HeaderModel = new HeaderModel
        {
            LogoName = "",
            ReportName = "Purchase Report",
            CompanyName = UserSession.OrganizationName
        };
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PURCHASE_REPORT)]
    public async Task<IActionResult> PurchaseReportExcel(VmPurchaseReport model)
    {
        if (ModelState.IsValid)
        {
            model.PurchaseList = await _purchaseOrderService
                .Query()
                .Include(x => x.Vendor)
                .Include(x => x.PurchaseType)
                .Where(x => x.OrganizationId == UserSession.OrganizationId)
                .Where(x => x.PurchaseDate >= model.FromDate)
                .Where(x => x.PurchaseDate < model.ToDate.AddDays(1))
                .SelectAsync();

            var purchase = _mapper.Map<List<Purchase>, List<vmPurchaseReportRdlc>>(model.PurchaseList.ToList());
            foreach (var item in purchase)
            {
                item.OrgName = UserSession.OrganizationName;
                item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
                item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
            }

            return GetReportExcel(purchase, RdlcReportFileOption.PurchaseReportUrl,
                RdlcReportFileOption.PurchaseReportDsName, RdlcReportFileOption.PurchaseReportFileName);
        }

        return RedirectToAction(nameof(PurchaseReport));
    }

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_AUDIT_LOG_REPORT)]
    public async Task<IActionResult> AuditLogReport()
    {
        var fromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        var toDate = DateTime.Today;
        vmAuditLogReport report = new vmAuditLogReport();

        report.auditLogList = await _auditLogService.Query()
            .Where(x => x.OrganizationId == UserSession.OrganizationId)
            .Where(x => x.CreatedTime >= fromDate && x.CreatedTime < toDate.AddDays(1))
            .SelectAsync();


        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Audit Log Report";
        head.CompanyName = UserSession.OrganizationName;

        report.head = head;
        report.FromDate = fromDate;
        report.ToDate = toDate;
        return View(report);
    }


    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_AUDIT_LOG_REPORT)]
    public async Task<IActionResult> AuditLogReport(vmAuditLogReport model)
    {
        vmAuditLogReport report = new vmAuditLogReport();

        if (ModelState.IsValid)
        {
            report.auditLogList = await _auditLogService.Query()
                .Where(x => x.OrganizationId == UserSession.OrganizationId)
                .Where(x => x.CreatedTime >= model.FromDate && x.CreatedTime < model.ToDate.AddDays(1))
                .SelectAsync();
        }

        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Audit Log Report";
        head.CompanyName = UserSession.OrganizationName;

        report.head = head;
        report.FromDate = model.FromDate;
        report.ToDate = model.ToDate;
        return View(report);
    }

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PRODUCTION_REPORT)]
    public async Task<IActionResult> ProductionReport()
    {
        var fromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        var toDate = DateTime.Today;

        vmProductionReport report = new vmProductionReport();

        report.productionList = await _productionService.Query()
            .Include(x => x.Product)
            .Where(x => x.OrganizationId == UserSession.OrganizationId)
            .Where(x => x.ReceiveTime >= fromDate && x.ReceiveTime < toDate.AddDays(1))
            .SelectAsync();

        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Production Report";
        head.CompanyName = UserSession.OrganizationName;

        report.head = head;
        report.FromDate = fromDate;
        report.ToDate = toDate;

        return View(report);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_PRODUCTION_REPORT)]
    public async Task<IActionResult> ProductionReport(vmProductionReport model)
    {
        vmProductionReport report = new vmProductionReport();

        if (ModelState.IsValid)
        {
            report.productionList = await _productionService
                .Query().Include(x => x.Product).Where(x => x.OrganizationId == UserSession.OrganizationId)
                .Where(x => x.ReceiveTime >= model.FromDate && x.ReceiveTime < model.ToDate.AddDays(1))
                .SelectAsync();
        }

        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Production Report";
        head.CompanyName = UserSession.OrganizationName;

        report.head = head;
        report.FromDate = model.FromDate;
        report.ToDate = model.ToDate;

        return View(report);
    }

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_USERS_REPORT)]
    public async Task<IActionResult> UsersReport()
    {
        vmUsersOldReport report = new vmUsersOldReport();
        report.usersList = await _userService.Query()
            .Include(x => x.Role)
            .Where(x => x.OrganizationId == UserSession.OrganizationId)
            .SelectAsync();

        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "iVAT Users Report";
        head.CompanyName = UserSession.OrganizationName;

        report.head = head;

        return View(report);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_USERS_REPORT)]
    public async Task<IActionResult> UserReportExcel(vmUsersOldReport report)
    {
        try
        {
            report.usersList = await _userService.Query()
                .Include(x => x.Role)
                .Where(x => x.OrganizationId == UserSession.OrganizationId)
                .SelectAsync();

            var users = _mapper.Map<List<User>, List<vmUserReportRdlc>>(report.usersList.ToList());
            foreach (var item in users)
            {
                item.OrgName = UserSession.OrganizationName;
            }

            return GetReportExcel(users, RdlcReportFileOption.UserReportUrl, RdlcReportFileOption.UserReportDsName,
                RdlcReportFileOption.UserReportFileName);
        }
        catch
        {
            return RedirectToAction(nameof(UsersReport));
        }
    }

    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_CONTRACTUAL_PRODUCTION_REPORT)]
    //public async Task<IActionResult> ContractualProductionReport()
    //{
    //    var fromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
    //    var toDate = DateTime.Today;

    //    vmContractualProductionReport report = new vmContractualProductionReport();
    //    report.contractualProductionList = await _contractVendorService.Query()
    //        .Include(x => x.Vendor).Include(x => x.ContractType)
    //        .Where(x => x.OrganizationId == UserSession.OrganizationId)
    //        .Where(x => x.ContractDate >= fromDate && x.ContractDate < toDate.AddDays(1))
    //        .SelectAsync();


    //    HeaderModel head = new HeaderModel();
    //    head.LogoName = "";
    //    head.ReportName = "Contractual Production Report";
    //    head.CompanyName = UserSession.OrganizationName;

    //    report.head = head;
    //    report.FromDate = fromDate;
    //    report.ToDate = toDate;
    //    return View(report);
    //}

    //[HttpPost]
    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_CONTRACTUAL_PRODUCTION_REPORT)]
    //public async Task<IActionResult> ContractualProductionReport(vmContractualProductionReport model)
    //{
    //    vmContractualProductionReport report = new vmContractualProductionReport();

    //    if (ModelState.IsValid)
    //    {
    //        report.contractualProductionList = await _contractVendorService.Query()
    //            .Include(x => x.Vendor).Include(x => x.ContractType)
    //            .Where(x => x.OrganizationId == UserSession.OrganizationId)
    //            .Where(x => x.ContractDate >= model.FromDate && x.ContractDate < model.ToDate.AddDays(1))
    //            .SelectAsync();
    //    }


    //    HeaderModel head = new HeaderModel();
    //    head.LogoName = "";
    //    head.ReportName = "Contractual Production Report";
    //    head.CompanyName = UserSession.OrganizationName;

    //    report.head = head;
    //    report.FromDate = model.FromDate;
    //    report.ToDate = model.ToDate;
    //    return View(report);
    //}

    //[HttpPost]
    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_CONTRACTUAL_PRODUCTION_REPORT)]
    //public async Task<IActionResult> ContractualProductionReportExcel(vmContractualProductionReport model)
    //{
    //    vmContractualProductionReport report = new vmContractualProductionReport();

    //    if (ModelState.IsValid)
    //    {
    //        report.contractualProductionList = await _contractVendorService.Query()
    //            .Include(x => x.Vendor).Include(x => x.ContractType)
    //            .Where(x => x.OrganizationId == UserSession.OrganizationId)
    //            .Where(x => x.ContractDate >= model.FromDate && x.ContractDate < model.ToDate.AddDays(1))
    //            .SelectAsync();

    //        var productions =
    //            _mapper.Map<List<ContractualProduction>, List<vmContractualProductionReportRdlc>>(
    //                report.contractualProductionList.ToList());
    //        foreach (var item in productions)
    //        {
    //            item.OrgName = UserSession.OrganizationName;
    //            item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
    //            item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
    //        }

    //        return GetReportExcel(productions, RdlcReportFileOption.ContractualProductionReportUrl,
    //            RdlcReportFileOption.ContractualProductionReportDsName,
    //            RdlcReportFileOption.ContractualProductionReportFileName);
    //    }

    //    return RedirectToAction(nameof(ContractualProductionReport));
    //}

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


    public async Task<IActionResult> SalesPaymentReceive(string id)
    {
        var sale = await _saleService.GetSaleData(id);
        var payments = await _paymentMethodService.Query().SelectAsync();
        IEnumerable<CustomSelectListItem> paymentMethods = payments.Select(s => new CustomSelectListItem
        {
            Id = s.PaymentMethodId,
            Name = s.Name
        });
        int salesId = int.Parse(IvatDataProtector.Unprotect(id));
        VmDuePayment duePayment = new VmDuePayment
        {
            SalesId = salesId,
            PaymentMethods = paymentMethods,
            PayableAmount = sale.ReceivableAmount,
            PrevPaidAmount = sale.PaymentReceiveAmount,
            DueAmount = sale.PaymentDueAmount,
        };

        return View(duePayment);
    }

    [HttpPost]
    public async Task<IActionResult> SalesPaymentReceive(VmDuePayment salesPayment, string id)
    {
        int salesId = int.Parse(IvatDataProtector.Unprotect(id));
        var salesDetails = await _saleService.Query()
            .SingleOrDefaultAsync(p => p.SalesId == salesId, CancellationToken.None);

        var value = Convert.ToInt32(salesPayment.DueAmount);
        if (salesPayment.PaidAmount <= Convert.ToDecimal(salesDetails.ReceivableAmount))
        {
            if (salesPayment.PaidAmount <= Convert.ToDecimal(value))
            {
                var totalPaidAmount = salesDetails.PaymentReceiveAmount + salesPayment.PaidAmount;
                VmSalesPaymentReceive vmSales = new VmSalesPaymentReceive
                {
                    SalesId = salesId,
                    PaymentMethodId = salesPayment.PaymentMethodId,
                    PaidAmount = Convert.ToDecimal(totalPaidAmount),
                    CreatedBy = UserSession.UserId
                };
                await _salesPaymentReceiveService.ManageSalesDueAsync(vmSales);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
                return RedirectToAction(ViewStaticData.SALES_DUE, ControllerStaticData.SALES);
            }
            else
            {
                ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.SALES_DUE_MESSAGE;
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

        salesPayment.PaymentMethods = paymentMethods;

        return View(salesPayment);
    }


    public async Task<IActionResult> PurchaseDueReport()
    {
        int org = UserSession.OrganizationId;

        vmPurchaseDueReport report = new vmPurchaseDueReport();

        report.purchaseDueList = await _purchaseOrderService.GetPurchaseDue(org);

        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Purchase Due Report";

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

        return View(report);
    }


    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_VDS_LIST_REPORT)]
    public async Task<IActionResult> SalesVDSListReport(vmSalesVDSListReport model)
    {
        vmSalesVDSListReport report = new vmSalesVDSListReport();

        if (ModelState.IsValid)
        {
            report.saleVDSList = await _saleService.Query()
                .Where(c => c.OrganizationId == UserSession.OrganizationId && c.IsVatDeductedInSource == true)
                .Include(p => p.Customer)
                .Include(c => c.CreditNotes)
                .Include(p => p.Organization)
                .Where(x => x.SalesDate >= model.FromDate && x.SalesDate < model.ToDate.AddDays(1))
                .OrderByDescending(c => c.SalesId).SelectAsync();
        }


        HeaderModel head = new HeaderModel();
        head.LogoName = "";
        head.ReportName = "Sales VDS List Report";
        report.FromDate = model.FromDate;
        report.ToDate = model.ToDate;
        head.CompanyName = UserSession.OrganizationName;

        report.head = head;

        return View(report);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_VDS_LIST_REPORT)]
    public async Task<IActionResult> SalesVDSListReportExcel(vmSalesVDSListReport model)
    {
        vmSalesVDSListReport report = new vmSalesVDSListReport();

        if (ModelState.IsValid)
        {
            report.saleVDSList = await _saleService.Query()
                .Where(c => c.OrganizationId == UserSession.OrganizationId && c.IsVatDeductedInSource == true)
                .Include(p => p.Customer)
                .Include(c => c.CreditNotes)
                .Include(p => p.Organization)
                .Where(x => x.SalesDate >= model.FromDate && x.SalesDate < model.ToDate.AddDays(1))
                .OrderByDescending(c => c.SalesId).SelectAsync();

            var sales = _mapper.Map<List<Sale>, List<vmSalesReportRdlc>>(report.saleVDSList.ToList());
            foreach (var item in sales)
            {
                item.OrgName = UserSession.OrganizationName;
                item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
                item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
            }

            return GetReportExcel(sales, RdlcReportFileOption.SalesVdsListReportUrl,
                RdlcReportFileOption.SalesVdsListReportDsName, RdlcReportFileOption.SalesVdsListReportFileName);
        }

        return RedirectToAction(nameof(SalesVDSListReport));
    }

    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_PURCHASE_VDS_LIST_REPORT)]
    //public async Task<IActionResult> PurchaseVDSListReport()
    //{
    //    //var fromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
    //    //var toDate = DateTime.Today;
    //    //vmPurchaseVDSList report = new vmPurchaseVDSList();
    //    //report.purchaseVDSList = await _purchaseOrderService.Query().Where(c =>
    //    //        c.OrganizationId == UserSession.OrganizationId
    //    //        && c.IsVatDeductedInSource == true && c.PurchaseDate >= fromDate &&
    //    //        c.PurchaseDate < toDate.AddDays(1))
    //    //    .Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.Vendor)
    //    //    .OrderByDescending(c => c.PurchaseId).SelectAsync();

    //    //HeaderModel head = new HeaderModel();
    //    //head.LogoName = "";
    //    //head.ReportName = "Purchase VDS List Report";
    //    //head.CompanyName = UserSession.OrganizationName;

    //    //report.head = head;
    //    //report.FromDate = fromDate;
    //    //report.ToDate = toDate;
    //    //return View(report);
    //    var model = new vmPurchaseVDSList
    //    {
    //        FromDate = DateTime.Now,
    //        ToDate = DateTime.Now,
    //        ReportOptionSelectListItems = _reportOptionService.GetReportDisplayOrExportTypeSelectList(),
    //    };
    //    return View(model);
    //}

    //[HttpPost]
    //[VmsAuthorize(FeatureList.REPORTS)]
    //[VmsAuthorize(FeatureList.REPORTS_PURCHASE_VDS_LIST_REPORT)]
    //public async Task<IActionResult> PurchaseVDSListReport(vmPurchaseVDSList model)
    //{
    //    //vmPurchaseVDSList report = new vmPurchaseVDSList();
    //    //if (ModelState.IsValid)
    //    //{
    //    //    report.purchaseVDSList = await _purchaseOrderService.Query().Where(c =>
    //    //            c.OrganizationId == UserSession.OrganizationId
    //    //            && c.IsVatDeductedInSource == true && c.PurchaseDate >= model.FromDate &&
    //    //            c.PurchaseDate < model.ToDate.AddDays(1)).Include(c => c.Organization)
    //    //        .Include(c => c.PurchaseType)
    //    //        .Include(c => c.Vendor).OrderByDescending(c => c.PurchaseId).SelectAsync();
    //    //}

    //    //HeaderModel head = new HeaderModel();
    //    //head.LogoName = "";
    //    //head.ReportName = "Purchase VDS List Report";
    //    //head.CompanyName = UserSession.OrganizationName;

    //    //report.head = head;
    //    //report.FromDate = model.FromDate;
    //    //report.ToDate = model.ToDate;
    //    //return View(report);
    //    var purchaseVDSList =
    //       await _purchaseOrderService.Query().Where(c =>
    //                c.OrganizationId == UserSession.OrganizationId
    //                && c.IsVatDeductedInSource == true && c.PurchaseDate >= model.FromDate &&
    //                c.PurchaseDate < model.ToDate.AddDays(1)).Include(c => c.Organization)
    //            .Include(c => c.PurchaseType)
    //            .Include(c => c.Vendor).OrderByDescending(c => c.PurchaseId).SelectAsync();
    //    return ProcessReport(purchaseVDSList,
    //        RdlcReportFileOption.PurchaseVdsListReportUrl,
    //        RdlcReportFileOption.PurchaseVdsListReportDsName,
    //        StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.PurchaseVdsListReportFileName),
    //        GetParameterForPurchaseReport(model.FromDate, model.ToDate, "Purchase VDS List Report"), model.ReportProcessOptionId
    //        );
    //}

    private Dictionary<string, string> GetParameterForPurchaseReport(DateTime fromDate, DateTime toDate, string reportHeaderName)
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

    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_SALES_VDS_LIST_REPORT)]
    //public async Task<IActionResult> PurchaseVDSListReportExcel(vmPurchaseVDSList model)
    //{
    //    vmPurchaseVDSList report = new vmPurchaseVDSList();
    //    if (ModelState.IsValid)
    //    {
    //        report.purchaseVDSList = await _purchaseOrderService.Query().Where(c =>
    //                c.OrganizationId == UserSession.OrganizationId
    //                && c.IsVatDeductedInSource == true && c.PurchaseDate >= model.FromDate &&
    //                c.PurchaseDate < model.ToDate.AddDays(1)).Include(c => c.Organization)
    //            .Include(c => c.PurchaseType)
    //            .Include(c => c.Vendor).OrderByDescending(c => c.PurchaseId).SelectAsync();

    //        var purchase = _mapper.Map<List<Purchase>, List<vmPurchaseReportRdlc>>(report.purchaseVDSList.ToList());
    //        foreach (var item in purchase)
    //        {
    //            item.OrgName = UserSession.OrganizationName;
    //            item.FromDate = model.FromDate.ToString("dd/MM/yyyy");
    //            item.ToDate = model.ToDate.ToString("dd/MM/yyyy");
    //        }

    //        return GetReportExcel(purchase, RdlcReportFileOption.PurchaseVDSListReportUrl,
    //            RdlcReportFileOption.PurchaseVDSListReportDsName,
    //            RdlcReportFileOption.PurchaseVDSListReportFileName);
    //    }

    //    return RedirectToAction(nameof(PurchaseVDSListReport));
    //}

    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_TRANSFER_RAW_MATERIAL_REPORT)]
    public async Task<IActionResult> TransferRawMaterialReport()
    {
        var fromDate = DateCalculator.GetFirstDateOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        var toDate = DateTime.Today;
        vmTransferRawMaterialReport report = new vmTransferRawMaterialReport();

        report.TransferRawMaterialList = await _rawMaterialService.Query()
            .Where(c => c.TransfereDate >= fromDate && c.TransfereDate < toDate.AddDays(1)).SelectAsync();
        var head = new HeaderModel
        {
            ReportName = "Transfer Raw Material List Report",
            CompanyName = UserSession.OrganizationName
        };

        report.HeaderModel = head;
        report.FromDate = fromDate;
        report.ToDate = toDate;

        return View(report);
    }


    [HttpPost]
    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_TRANSFER_RAW_MATERIAL_REPORT)]
    public async Task<IActionResult> TransferRawMaterialReport(vmTransferRawMaterialReport model)
    {
        vmTransferRawMaterialReport report = new vmTransferRawMaterialReport();

        if (ModelState.IsValid)
        {
            report.TransferRawMaterialList = await _rawMaterialService.Query()
                .Where(c => c.TransfereDate >= model.FromDate && c.TransfereDate < model.ToDate.AddDays(1))
                .SelectAsync();
        }

        var head = new HeaderModel
        {
            ReportName = "Transfer Raw Material List Report",
            CompanyName = UserSession.OrganizationName
        };

        report.HeaderModel = head;
        report.FromDate = model.FromDate;
        report.ToDate = model.ToDate;

        return View(report);
    }




    [VmsAuthorize(FeatureList.REPORTS)]
    [VmsAuthorize(FeatureList.REPORTS_CUSTOMER_REPORT)]
    public async Task<IActionResult> YearlyComparisonReport(int? fromYear, int? toYear)
    {
        
        var head = new HeaderModel
        {
            LogoName = "",
            ReportName = "Yearly Comparison Report",
            CompanyName = UserSession.OrganizationName
        };
        var report = new VmComparisonInformationHtmlDisplayViewModel
        {
            HeaderModel = head,
            FromYear = fromYear ?? DateTime.Now.Year,
            ToYear = toYear ?? DateTime.Now.Year
        };
        report.YearlyComparisons =
	        await _dashboardService.GetYearlyComparisonInfo(UserSession.OrganizationId, report.FromYear, report.ToYear, UserSession.UserId);
        return View(report);
    }
}