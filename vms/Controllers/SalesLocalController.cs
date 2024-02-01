using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
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

public class SalesLocalController : ControllerBase
{
	#region private variable declaration

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
	private readonly IOrganizationConfigurationBooleanService _iOrganizationConfigurationBooleanService;

	#endregion

	#region constructor

	public SalesLocalController(ControllerBaseParamModel controllerBaseParamModel, ISaleService saleService
		, ICountryService countryService, IAutocompleteService autocompleteService,
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
		ISalesPriceAdjustmentService salesPriceAdjustmentService,
		IOrganizationConfigurationBooleanService iOrganizationConfigurationBooleanService
		) : base(
		controllerBaseParamModel)
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
		_iOrganizationConfigurationBooleanService = iOrganizationConfigurationBooleanService;
	}


	#endregion



	[SessionExpireFilter]
	[VmsAuthorize(FeatureList.SALE)]
	[VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
	public async Task<IActionResult> Index()
	{
		return View(await _saleService.GetSalesLocalListByOrganization(UserSession.ProtectedOrganizationId));
	}

	#region sale local save

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
            OrgBranchList = await _branchService.GetOrgBranchSelectListByUser(protectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
			SalesDeliveryTypeId = (int)EnumSalesDeliveryType.NotRequired,
			ViewOrganizationConfigurationBooleanData = await _iOrganizationConfigurationBooleanService.GetOrganizationConfigurationBoolean(UserSession.ProtectedOrganizationId)

		};
		return View(model);
	}

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
			DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(UserSession.ProtectedOrganizationId),
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