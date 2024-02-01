using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.viewModels;
using vms.service.Services.MushakService;
using vms.service.Services.PaymentService;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.TransactionService;
using vms.service.Services.UploadService;
using vms.Utility;

namespace vms.Controllers;

public class SalesForeignController : ControllerBase
{
	#region private fields

	private readonly ISaleService _saleService;
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
	private readonly ICountryService _countryService;
	private readonly IVehicleTypeService _vType;
	private readonly IBankService _bankService;
	private readonly IProductStoredProcedureService _productStoredProcedureService;
	private readonly IVehicleTypeService _vehicleTypeService;
	private readonly IMushakGenerationService _mushakGenerationService;
	private readonly IDamageTypeService _damageTypeService;
	private readonly IDamageService _damageService;
	private readonly IOrgBranchService _branchService;
	private readonly IVmsExcelService _excelService;
	private readonly IExcelDataUploadService _excelDataUploadService;
	private readonly IExcelSimplifiedSalseService _excelSimplifiedSimplifiedSalseService;
	private readonly IFileOperationService _fileOperationService;
	private readonly ISalesPriceAdjustmentService _salesPriceAdjustmentService;
	private readonly ICreditNoteService _creditService;

	#endregion

	#region constructor

	public SalesForeignController(ControllerBaseParamModel controllerBaseParamModel, ISaleService saleService, ICountryService countryService, IAutocompleteService autocompleteService,
		IProductVatTypeService vatTypeService, IDocumentTypeService documentType,
		IDeliveryMethodService deliveryMethodService, IOrganizationService organizationService, IExportTypeService exportTypeService,
		ISalesDeliveryTypeService salesDeliveryTypeService, ISaleService saleOrderService,
		ISaleOrderDetailService saleODService, IMeasurementUnitService measurementUnitService,
		ICustomerService customerService,
		ISalesDetailService salesDetailService, ICreditNoteService creditService,
		IPaymentMethodService paymentMethodService, ISalesPaymentReceiveService salesPaymentReceiveService
		, IVehicleTypeService vType,
		IBankService bankService,
		IProductStoredProcedureService productStoredProcedureService,
		IVehicleTypeService vehicleTypeService,
		IMushakGenerationService mushakGenerationService
		, IDamageTypeService damageTypeService
		, IDamageService damageService, IOrgBranchService branchService, IVmsExcelService excelService,
		IExcelDataUploadService excelDataUploadService,
		IExcelSimplifiedSalseService excelSimplifiedSimplifiedSalseService, IFileOperationService fileOperationService,
		ISalesPriceAdjustmentService salesPriceAdjustmentService) : base(controllerBaseParamModel)
	{
		_saleService = saleService;
		_vType = vType;
		_countryService = countryService;
		_autocompleteService = autocompleteService;
		_vatTypeService = vatTypeService;
		_documentType = documentType;
		_deliveryMethodService = deliveryMethodService;
		_creditService = creditService;
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
		_mushakGenerationService = mushakGenerationService;
		_damageTypeService = damageTypeService;
		_damageService = damageService;
		_branchService = branchService;
		_excelService = excelService;
		_excelDataUploadService = excelDataUploadService;
		_excelSimplifiedSimplifiedSalseService = excelSimplifiedSimplifiedSalseService;
		_fileOperationService = fileOperationService;
		_salesPriceAdjustmentService = salesPriceAdjustmentService;
	}

	#endregion





	[SessionExpireFilter]
    [VmsAuthorize(FeatureList.SALE)]
    [VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        return View(await _saleService.GetSalesExportListByOrganization(UserSession.ProtectedOrganizationId));
    }

	#region Insert foreign sale data


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
		model.SalesDeliveryTypeId = (int)EnumSalesDeliveryType.NotRequired;
		//model.ProductForSaleList =
		//	_productStoredProcedureService.GetProductForSale(UserSession.OrganizationId, 0); //todo: Have to fix product
		//model.MeasurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(organizationId);
		model.DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(organizationId);
		model.ExportTypeSelectList = await _exportTypeService.GetExportTypesSelectList();
		model.PaymentMethodList = await _paymentMethodService.GetPaymentMethods();
		model.BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
		model.VehicleTypesList = await _vehicleTypeService.GetVehicleTypes(UserSession.OrganizationId);
		model.PaymentDate = DateTime.Now;
		model.PaymentDocumentOrTransDate = DateTime.Now;
		//model.OrgBranchList = await _branchService.GetOrgBranchSelectList(organizationId);
		model.OrgBranchList = await _branchService.GetOrgBranchSelectListByUser(organizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		return View(model);
	}

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

	public async Task<IActionResult> Details(string id)
	{
		var sale = await _saleService.GetSaleData(id);
		var salesDetails = await _salesDetailService.GetAllSalesDetails(id);
		var getNotes = await _creditService.Query().Include(c => c.Sales).Where(c =>
			c.Sales.OrganizationId == UserSession.OrganizationId &&
			c.SalesId == int.Parse(IvatDataProtector.Unprotect(id))).SelectAsync();
		var vm = new VmSalesDetail { SalesDetails = salesDetails, Sale = sale, CreditNotes = getNotes };
		return View(vm);
	}

	#endregion

}