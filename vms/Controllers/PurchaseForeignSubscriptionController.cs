using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using vms.entity.viewModels;
using vms.Utility;
using Microsoft.AspNetCore.Hosting;
using vms.service.Services.MushakService;
using vms.service.Services.PaymentService;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.TransactionService;
using vms.service.Services.UploadService;
using Microsoft.AspNetCore.DataProtection;

namespace vms.Controllers
{
	public class PurchaseForeignSubscriptionController : ControllerBase
	{
		#region readonly field

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

		#endregion

		public PurchaseForeignSubscriptionController(ISalesDetailService salesDetailService,
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

		public IActionResult Index()
		{
			return View();
		}

		[VmsAuthorize(FeatureList.PURCHASE)]
		[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
		[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
		public async Task<IActionResult> PurchaseImportSubscription()
		{
			var organizationId = UserSession.ProtectedOrganizationId;
			var model = new VmPurchaseImportSubscription
			{
				PurchaseReasonSelectList = await _purchaseReasonService.GetSelectList(),
				VendorSelectList = await _vendorService.GetLocalForeignSelectList(organizationId),
				ProductList = _productStoredProcedureService.GetProductForPurchase(organizationId),
				MeasurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(organizationId),
				ProductVatTypes = await _vatTypeService.GetForeignPurchaseProductVatTypes(),
				//VatCommissionarateList =
				//	await _customsAndVatcommissionarateService.GetCustomsAndVatcommissionarateSelectList(),
				// AtpBankList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId),
				//DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(organizationId),
				//PaymentMethodList = await _paymentMethodService.GetPaymentMethods(),
				//LcDate = DateTime.Now,
				//BillOfEntryDate = DateTime.Now.AddDays(30),
				//DueDate = DateTime.Now.AddDays(90),
				PurchaseDate = DateTime.Now,
				//PaymentDate = DateTime.Now,
				//PaymentDocumentOrTransDate = DateTime.Now,
				//PurchaseImportTaxPaymentDocOrChallanDate = DateTime.Now,
				//PurchaseImportTaxPaymentDate = DateTime.Now,
				//BankSelectList = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId),
				//PurchaseImportTaxPaymentTypeList =
				//	await _importTaxPaymentTypeService.GetPurchaseImportTaxPaymentTypeSelectList(),
				//DistrictList = await _districtService.DistrictSelectList(),
				OrgBranchList = await _branchService.GetOrgBranchSelectListByUser(organizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
				//IsRequireGoodsId = UserSession.IsRequireGoodsId,
				//IsRequireSkuId = UserSession.IsRequireSkuId,
				//IsRequireSkuNo = UserSession.IsRequireSkuNo,
				//IsRequirePartNo = UserSession.IsRequirePartNo
			};
			return View(model);
		}


		[VmsAuthorize(FeatureList.PURCHASE)]
		[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_VIEW)]
		[VmsAuthorize(FeatureList.PURCHASE_PURCHASE_LIST_CAN_ADD)]
		[HttpPost]
		public async Task<JsonResult> PurchaseImportSubscription(VmPurchaseImportSubscriptionPost model)
		{
			model.OrganizationId = UserSession.OrganizationId;
			model.CreatedBy = UserSession.UserId;
			var id = IvatDataProtector.Protect((await _purchaseService.InsertImportPurchaseSubsription(model)).ToString());
			await UnitOfWork.SaveChangesAsync();
			return Json(new { id = id });
		}

		public async Task<IActionResult> Details(string id)
		{
			var purchase = await _purchaseOrderService.GetPurchaseDetails(id);
			var purchaseDetails = await _purchaseOdService.GetPurchaseDetails(id);

			var getNotes = await _debitNoteService.Query().Include(c => c.Purchase).Where(c => c.Purchase.OrganizationId == UserSession.OrganizationId && c.PurchaseId == int.Parse(IvatDataProtector.Unprotect(id))).SelectAsync();
			var vm = new VmPurchaseDetail { PurchaseDetails = purchaseDetails, Purchase = purchase, DebitNotes = getNotes };
			return View(vm);
		}
	}
}
