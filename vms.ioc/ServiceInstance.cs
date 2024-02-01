using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using URF.Core.Abstractions;
using URF.Core.EF;
using vms.entity.models;
using vms.entity.viewModels;
using vms.mapping;
using vms.repository.Implementation.sp;
using vms.repository.Implementation.tbl;
using vms.repository.Implementation.tbl.acc;
using vms.repository.Repository.sp;
using vms.repository.Repository.tbl;
using vms.repository.Repository.tbl.acc;
using vms.service.ServiceImplementations.BillingService;
using vms.service.ServiceImplementations.ProductService;
using vms.service.ServiceImplementations.ReportService;
using vms.service.ServiceImplementations.SecurityService;
using vms.service.ServiceImplementations.SettingService;
using vms.service.ServiceImplementations.ThirdPartyService;
using vms.service.ServiceImplementations.TransactionService;
using vms.service.ServiceImplementations.UploadService;
using vms.service.Services.TransactionService;
using vms.service.Services.ReportService;
using vms.service.Services.ProductService;
using vms.service.Services.UploadService;
using vms.service.Services.SecurityService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.SettingService;
using vms.service.Services.PaymentService;
using vms.service.ServiceImplementations.PaymentService;
using vms.service.Services.MushakService;
using vms.service.ServiceImplementations.MushakService;
using vms.service.Services.BillingService;

namespace vms.ioc;

public static class ServiceInstance
{
	public static void RegisterVmsServiceInstance(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("dev");

		services.AddAutoMapper(m => m.AddProfile<VmsMappingProfile>());
		services.AddDbContext<VmsContext>(options => options.UseSqlServer(connectionString));
		services.AddScoped<DbContext, VmsContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		//services.AddScoped<IDataProtector, Microsoft.AspNetCore.DataProtection>();
		services.AddSingleton<PurposeStringConstants>();
		services.AddScoped<IReportOptionService, ReportOptionService>();

		services.AddScoped<IProductStoredProcedureRepository, ProductStoredProcedureRepository>();
		services.AddScoped<IProductStoredProcedureService, ProductStoredProcedureService>();

		services.AddScoped<IAutocompleteRepository, AutocompleteRepository>();
		services.AddScoped<IAutocompleteService, AutocompleteService>();

		services.AddScoped<IMushakReturnPaymentRepository, MushakReturnPaymentRepository>();
		services.AddScoped<IMushakReturnPaymentService, MushakReturnPaymentService>();

		services.AddScoped<IMushakSubmissionsRepository, MushakSubmissionsRepository>();
		services.AddScoped<IMushakSubmissionsService, MushakSubmissionsService>();

		services.AddScoped<IApiSpInsertRepository, ApiSpInsertRepository>();
		services.AddScoped<IApiSpInsertService, ApiSpInsertService>();

		services.AddScoped<IMushakSubmissionTypeRepository, MushakSubmissionTypeRepository>();
		services.AddScoped<IMushakSubmissionTypeService, MushakSubmissionTypeService>();

		services.AddScoped<IMushakReturnRefundService, MushakReturnRefundService>();
		services.AddScoped<IMushakReturnRefundRepository, MushakReturnRefundRepository>();

		services.AddScoped<IBusinessNatureService, BusinessNatureService>();
		services.AddScoped<IBusinessNatureRepository, BusinessNatureRepository>();

		services.AddScoped<IBusinessCategoryService, BusinessCategoryService>();
		services.AddScoped<IBusinessCategoryRepository, BusinessCategoryRepository>();

		services.AddScoped<IOldAccountCurrentBalanceService, OldAccountCurrentBalanceService>();
		services.AddScoped<IOldAccountCurrentBalanceRepository, OldAccountCurrentBalanceRepository>();

		services.AddScoped<IMushakReturnPaymentTypeRepository, MushakReturnPaymentTypeRepository>();
		services.AddScoped<IMushakReturnPaymentTypeService, MushakReturnPaymentTypeService>();

		services.AddScoped<IBankRepository, BankRepository>();
		services.AddScoped<IBankService, BankService>();

		services.AddScoped<IMushok6P3ViewRepositoy, Mushok6P3ViewRepositoy>();
		services.AddScoped<IMushok6P3ViewService, Mushok6P3ViewService>();

		services.AddScoped<ISpGetSalePagedRepository, SpGetSalePagedRepository>();
		services.AddScoped<ISpGetSalePagedService, SpGetSalePagedService>();

		services.AddScoped<IDamageInvoiceListRepository, DamageInvoiceListRepository>();
		services.AddScoped<IDamageInvoiceListService, DamageInvoiceListService>();

		services.AddTransient<IOrganizationRepository, OrganizationRepository>();
		services.AddTransient<IOrganizationService, OrganizationService>();
		services.AddTransient<ICRMService, CRMService>();

		services.AddTransient<INbrEconomicCodeRepository, NbrEconomicCodeRepository>();
		services.AddTransient<INbrEconomicCodeService, NbrEconomicCodeService>();

		services.AddTransient<IBankBranchRepository, BankBranchRepository>();
		services.AddTransient<IBankBranchService, BankBranchService>();

		services.AddScoped<ITdsPaymentRepository, TdsPaymentRepository>();
		services.AddScoped<ITdsPaymentService, TdsPaymentService>();

		services.AddTransient<IMeasurementUnitRepository, MeasurementUnitRepository>();
		services.AddTransient<IMeasurementUnitService, MeasurementUnitService>();

		services.AddTransient<IOrderRepository, OrderRepository>();
		services.AddTransient<IOrderService, OrderService>();

		services.AddTransient<IVehicleTypeRepository, VehicleTypeRepository>();
		services.AddTransient<IVehicleTypeService, VehicleTypeService>();

		services.AddTransient<IProductTransactionBookRepository, ProductTransactionBookRepository>();
		services.AddTransient<IProductTransactionBookService, ProductTransactionBookService>();

		services.AddTransient<IProductRepository, ProductRepository>();
		services.AddTransient<IProductService, ProductService>();

		services.AddTransient<IProductOpeningBalanceRepository, ProductOpeningBalanceRepository>();
		services.AddTransient<IProductOpeningBalanceService, ProductOpeningBalanceService>();

		services.AddTransient<IBrandRepository, BrandRepository>();
		services.AddTransient<IBrandService, BrandService>();

		services.AddTransient<IProductGroupRepository, ProductGroupRepository>();
		services.AddTransient<IProductGroupService, ProductGroupService>();


		services.AddTransient<IMushakHighValueRepository, MushakHighValueRepository>();
		services.AddTransient<IMushakHighValue, MushakHighValue>();

		services.AddTransient<IDebitNoteRepository, DebitNoteRepository>();
		services.AddTransient<IDebitNoteService, DebitNoteService>();

		services.AddTransient<IDamageDetailRepository, DamageDetailRepository>();
		services.AddTransient<IDamageDetailService, DamageDetailService>();


		services.AddTransient<IDamageRepository, DamageRepository>();
		services.AddTransient<IDamageService, DamageService>();

		services.AddTransient<ICreditNoteRepository, CreditNoteRepository>();
		services.AddTransient<ICreditNoteService, CreditNoteService>();

		services.AddTransient<IPurchaseOrderRepository, PurchaseOrderRepository>();
		services.AddTransient<IPurchaseOrderService, PurchaseOrderService>();

		services.AddTransient<IPurchaseRepository, PurchaseRepository>();
		services.AddTransient<IPurchaseService, PurchaseService>();

		services.AddTransient<ISaleOrderRepository, SaleOrderRepository>();
		services.AddTransient<ISaleOrdersService, SaleOrdersService>();

		services.AddTransient<ISaleOrderDetailsRepository, SaleOrderDetailRepository>();
		services.AddTransient<ISaleOrderDetailService, SaleOrderDetailService>();

		services.AddTransient<IPurchaseOrderDetailsRepository, PurchaseOrderDetailRepository>();
		services.AddTransient<IPurchaseOrderDetailService, PurchaseOrderDetailService>();

		services.AddTransient<IPurchaseTypeRepository, PurchaseTypeRepository>();
		services.AddTransient<IPurchaseTypeService, PurchaseTypeService>();

		services.AddTransient<IRightRepository, RightRepository>();
		services.AddTransient<IRightService, RightService>();

		services.AddTransient<IRoleRepository, RoleRepository>();
		services.AddTransient<IRoleService, RoleService>();

		services.AddTransient<IRoleRightRepository, RoleRightRepository>();
		services.AddTransient<IRoleRightService, RoleRightService>();

		services.AddTransient<IUserRepository, UserRepository>();
		services.AddTransient<IUserService, UserService>();

		services.AddTransient<IUserLoginHistoryRepository, UserLoginHistoryRepository>();
		services.AddTransient<IUserLoginHistoryService, UserLoginHistoryService>();

		services.AddTransient<IUserTypeRepository, UserTypeRepository>();
		services.AddTransient<IUserTypeService, UserTypeService>();


		services.AddTransient<IContentRepository, ContentRepository>();
		services.AddTransient<IContentService, ContentService>();


		services.AddTransient<ICoagroupRepository, CoagroupRepository>();
		services.AddTransient<ICoagroupService, CoagroupService>();

		services.AddTransient<ICustomerRepository, CustomerRepository>();
		services.AddTransient<ICustomerService, CustomerService>();

		
		services.AddTransient<ICustomerCategoryRepository, CustomerCategoryRepository>();
		services.AddTransient<ICustomerCategoryService, CustomerCategoryService>();

		services.AddTransient<ICustomerSubscriptionRepository, CustomerSubscriptionRepository>();
		services.AddTransient<ICustomerSubscriptionService, CustomerSubscriptionService>();

		services.AddTransient<ICustomerSubscriptionCategoryRepository, CustomerSubscriptionCategoryRepository>();
		services.AddTransient<ICustomerSubscriptionCategoryService, CustomerSubscriptionCategoryService>();

		services.AddTransient<ICustomerSubscriptionDetailRepository, CustomerSubscriptionDetailRepository>();
        services.AddTransient<ICustomerSubscriptionDetailService, CustomerSubscriptionDetailService>();

		services.AddTransient<ISubscriptionBillRepository, SubscriptionBillRepository>();
		services.AddTransient<ISubscriptionBillService, SubscriptionBillService>();

        services.AddTransient<ISubscriptionBillDetailRepository, SubscriptionBillDetailRepository>();
        services.AddTransient<ISubscriptionBillDetailService, SubscriptionBillDetailService>();

		services.AddTransient<ICustomerBranchRepository, CustomerBranchRepository>();
		services.AddTransient<ICustomerBranchService, CustomerBranchService>();

		services.AddTransient<IUserBranchRepository, UserBranchRepository>();
		services.AddTransient<IUserBranchService, UserBranchService>();

		services.AddTransient<ICustomerDeliveryAddressRepository, CustomerDeliveryAddressRepository>();
		services.AddTransient<ICustomerDeliveryAddressService, CustomerDeliveryAddressService>();

		services.AddTransient<IPriceSetupProductCostRepository, PriceSetupProductCostRepository>();
		services.AddTransient<IPriceSetupProductCostService, PriceSetupProductCostService>();

		services.AddTransient<IExportTypeRepository, ExportTypeRepository>();
		services.AddTransient<IExportTypeService, ExportTypeService>();

		services.AddTransient<IPurchaseDetailRepository, PurchaseDetailRepository>();
		services.AddTransient<IPurchaseDetailService, PurchaseDetailService>();

		services.AddTransient<IVendorRepository, VendorRepository>();
		services.AddTransient<IVendorService, VendorService>();

		services.AddTransient<IVendorCategoryRepository, VendorCategoryRepository>();
		services.AddTransient<IVendorCategoryService, VendorCategoryService>();

		services.AddTransient<IProductVatRepository, ProductVatRepository>();
		services.AddTransient<IProductVatService, ProductVatService>();

		services.AddTransient<ISaleRepository, SaleRepository>();
		services.AddTransient<ISaleService, SaleService>();

		services.AddTransient<ISalesDeliveryTypeRepository, SalesDeliveryTypeRepository>();
		services.AddTransient<ISalesDeliveryTypeService, SalesDeliveryTypeService>();

		services.AddTransient<ISalesDetailRepository, SalesDetailRepository>();
		services.AddTransient<ISalesDetailService, SalesDetailService>();

		services.AddTransient<ISalesPriceAdjustmentRepository, SalesPriceAdjustmentRepository>();
		services.AddTransient<ISalesPriceAdjustmentService, SalesPriceAdjustmentService>();

		services.AddTransient<ISalesPriceAdjustmentDetailRepository, SalesPriceAdjustmentDetailRepository>();
		services.AddTransient<ISalesPriceAdjustmentDetailService, SalesPriceAdjustmentDetailService>();

		services.AddTransient<IBranchTransferSendRepository, BranchTransferSendRepository>();
		services.AddTransient<IBranchTransferSendService, BranchTransferSendService>();

		services.AddTransient<IBranchTransferSendDetailRepository, BranchTransferSendDetailRepository>();
		services.AddTransient<IBranchTransferSendDetailService, BranchTransferSendDetailService>();

		services.AddTransient<IBranchTransferReceiveRepository, BranchTransferReceiveRepository>();
		services.AddTransient<IBranchTransferReceiveService, BranchTransferReceiveService>();

		services.AddTransient<IBranchTransferReceiveDetailRepository, BranchTransferReceiveDetailRepository>();
		services.AddTransient<IBranchTransferReceiveDetailService, BranchTransferReceiveDetailService>();

		services.AddTransient<ISalesTypeRepository, SalesTypeRepository>();
		services.AddTransient<ISalesTypeService, SalesTypeService>();

		services.AddTransient<ITransectionTypeRepository, TransectionTypeRepository>();
		services.AddTransient<ITransectionTypeService, TransectionTypeService>();

		services.AddTransient<IProductVatTypeRepository, ProductVatTypeRepository>();
		services.AddTransient<IProductVatTypeService, ProductVatTypeService>();

		services.AddTransient<IAuditLogRepository, AuditLogRepository>();
		services.AddTransient<IAuditLogService, AuditLogService>();

		services.AddTransient<IOverHeadCostRepository, OverHeadCostRepository>();
		services.AddTransient<IOverHeadCostService, OverHeadCostService>();

		services.AddTransient<IAuditOperationRepository, AuditOperationRepository>();
		services.AddTransient<IAuditOperationService, AuditOperationService>();

		services.AddTransient<IObjectTypeRepository, ObjectTypeRepository>();
		services.AddTransient<IObjectTypeService, ObjectTypeService>();

		services.AddTransient<IPurchaseReasonRepository, PurchaseReasonRepository>();
		services.AddTransient<IPurchaseReasonService, PurchaseReasonService>();

		services.AddTransient<IPriceSetupRepository, PriceSetupRepository>();
		services.AddTransient<IPriceSetupService, PriceSetupService>();

		services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
		services.AddTransient<IProductTypeService, ProductTypeService>();

		services.AddTransient<IProductProductTypeMappingRepository, ProductProductTypeMappingRepository>();
		services.AddTransient<IProductProductTypeMappingService, ProductProductTypeMappingService>();

		services.AddTransient<IProductionRepository, ProductionRepository>();
		services.AddTransient<IProductionService, ProductionService>();

		services.AddTransient<IProductionDetailRepository, ProductionDetailRepository>();
		services.AddTransient<IProductionDetailService, ProductionDetailService>();

		services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
		services.AddTransient<IProductCategoryService, ProductCategoryService>();

		services.AddTransient<IMushakGenerationRepository, MushakGenerationRepository>();
		services.AddTransient<IMushakGenerationService, MushakGenerationService>();

		services.AddTransient<ISupplimentaryDutyRepository, SupplimentaryDutyRepository>();
		services.AddTransient<ISupplimentaryDutyService, SupplimentaryDutyService>();

		services.AddTransient<IDamageTypeRepository, DamageTypeRepository>();
		services.AddTransient<IDamageTypeService, DamageTypeService>();

		services.AddTransient<IDeliveryMethodRepository, DeliveryMethodRepository>();
		services.AddTransient<IDeliveryMethodService, DeliveryMethodService>();

		services.AddTransient<IDeliveryMethodRepository, DeliveryMethodRepository>();
		services.AddTransient<IDeliveryMethodService, DeliveryMethodService>();

		services.AddTransient<IDocumentTypeRepository, DocumentTypeRepository>();
		services.AddTransient<IDocumentTypeService, DocumentTypeService>();

		services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
		services.AddTransient<IPaymentMethodService, PaymentMethodService>();

		services.AddTransient<IPurchasePaymentRepository, PurchasePaymentRepository>();
		services.AddTransient<IPurchasePaymentService, PurchasePaymentService>();

		services.AddTransient<IPurchaseImportTaxPaymentTypeRepository, PurchaseImportTaxPaymentTypeRepository>();
		services.AddTransient<IPurchaseImportTaxPaymentTypeService, PurchaseImportTaxPaymentTypeService>();

		services.AddTransient<IPurchaseImportTaxPaymentRepository, PurchaseImportTaxPaymentRepository>();
		services.AddTransient<IPurchaseImportTaxPaymentService, PurchaseImportTaxPaymentService>();

		services.AddTransient<ISalesPaymentReceiveRepository, SalesPaymentReceiveRepository>();
		services.AddTransient<ISalesPaymentReceiveService, SalesPaymentReceiveService>();

		services.AddTransient<IContractVendorRepository, ContractVendorRepository>();
		services.AddTransient<IContractVendorService, ContractVendorService>();

		services.AddTransient<IContractVendorProductDetailsRepository, ContractVendorProductDetailsRepository>();
		services.AddTransient<IContractVendorProductDetailsService, ContractVendorProductDetailsService>();

		services.AddTransient<IContractVendorTransferRawMaterialRepository, ContractVendorTransferRawMaterialRepository>();
		services.AddTransient<IContractVendorTransferRawMaterialService, ContractVendorTransferRawMaterialService>();

		services.AddTransient<IContractVendorTransferRawMaterialDetailsRepository, ContractVendorTransferRawMaterialDetailsRepository>();
		services.AddTransient<IContractVendorTransferRawMaterialDetailsService, ContractVendorTransferRawMaterialDetailsService>();

		services.AddTransient<IContractTypeRepository, ContractTypeRepository>();
		services.AddTransient<IContractTypeService, ContractTypeService>();

		services.AddTransient<IMushakReturnRepository, MushakReturnRepository>();
		services.AddTransient<IMushakReturnService, MushakReturnService>();

		services.AddTransient<IAdjustmentRepository, AdjustmentRepository>();
		services.AddTransient<IAdjustmentService, AdjustmentService>();

		services.AddTransient<IAdjustmentTypeRepository, AdjustmentTypeRepository>();
		services.AddTransient<IAdjustmentTypeService, AdjustmentTypeService>();

		services.AddTransient<ICustomsAndVatcommissionarateRepository, CustomsAndVatcommissionarateRepository>();
		services.AddTransient<ICustomsAndVatcommissionarateService, CustomsAndVatcommissionarateService>();

		services.AddTransient<IDistrictRepository, DistrictRepository>();
		services.AddTransient<IDistrictService, DistrictService>();

		services.AddTransient<ICountryRepository, CountryRepository>();
		services.AddTransient<ICountryService, CountryService>();

		services.AddTransient<ICurrencyRepository, CurrencyRepository>();
		services.AddTransient<ICurrencyService, CurrencyService>();

		services.AddTransient<INbrEconomicCodeTypeRepository, NbrEconomicCodeTypeRepository>();
		services.AddTransient<INbrEconomicCodeTypeService, NbrEconomicCodeTypeService>();

		services.AddTransient<IOrgStaticDataRepository, OrgStaticDataRepository>();
		services.AddTransient<IOrgStaticDataService, OrgStaticDataService>();

		services.AddTransient<IOrgStaticDataTypeRepository, OrgStaticDataTypeRepository>();
		services.AddTransient<IOrgStaticDataTypeService, OrgStaticDataTypeService>();


		services.AddTransient<ISpPurchaseCalcBookService, SpPurchaseCalcBookService>();
		services.AddTransient<ISpPurchaseCalcBookRepository, SpPurchaseCalcBookRepository>();


		services.AddTransient<IDashboardRepository, DashboardRepository>();
		services.AddTransient<IDashboardService, DashboardService>();

		services.AddTransient<ISalesReportRepository, SalesReportRepository>();
		services.AddTransient<ISalesReportService, SalesReportService>();

		services.AddTransient<IPurchaseReportRepository, PurchaseReportRepository>();
		services.AddTransient<IPurchaseReportService, PurchaseReportService>();

		services.AddTransient<ISalesSummeryReportService, SalesSummeryReportService>();
		services.AddTransient<IPurchaseSummeryReportService, PurchaseSummeryReportService>();


		services.AddTransient<IDataImportService, DataImportService>();

		services.AddScoped<IOrgBranchTypeService, OrgBranchTypeService>();
		services.AddScoped<IOrgBranchTypeRepository, OrgBranchTypeRepository>();

		services.AddScoped<IOrgBranchService, OrgBranchService>();
		services.AddScoped<IOrgBranchRepository, OrgBranchRepository>();


		services.AddScoped<IFileOperationService, FileOperationService>();
		services.AddScoped<IFileOperationRepository, FileOperationRepository>();


		services.AddScoped<IDivisionOrStateRepository, DivisionOrStateRepository>();
		services.AddScoped<IDivisionOrStateService, DivisionOrStateService>();

		services.AddScoped<IDistrictOrCityRepository, DistrictOrCityRepository>();
		services.AddScoped<IDistrictOrCityService, DistrictOrCityService>();

		services.AddScoped<IExcelUploadedDataTypeRepository, ExcelUploadedDataTypeRepository>();
		services.AddScoped<IExcelUploadedDataTypeService, ExcelUploadedDataTypeService>();

		services.AddScoped<IExcelDataUploadRepository, ExcelDataUploadRepository>();
		services.AddScoped<IExcelDataUploadService, ExcelDataUploadService>();

		services.AddScoped<IExcelSimplifiedPurchaseRepository, ExcelSimplifiedPurchaseRepository>();
		services.AddScoped<IExcelSimplifiedPurchaseService, ExcelSimplifiedPurchaseService>();

		services.AddScoped<IExcelSimplifiedLocalPurchaseRepository, ExcelSimplifiedLocalPurchaseRepository>();
		services.AddScoped<IExcelSimplifiedLocalPurchaseService, ExcelSimplifiedLocalPurchaseService>();

		services.AddScoped<IExcelSimplifiedSalseRepository, ExcelSimplifiedSalseRepository>();
		services.AddScoped<IExcelSimplifiedSalseService, ExcelSimplifiedSalseService>();

		services.AddScoped<IExcelSimplifiedLocalSaleCalculateByVatRepository, ExcelSimplifiedLocalSaleCalculateByVatRepository>();
		services.AddScoped<IExcelSimplifiedLocalSaleCalculateByVatService, ExcelSimplifiedLocalSaleCalculateByVatService>();


		services.AddScoped<IVmsExcelService, VmsExcelService>();

		services.AddScoped<IProductReportService, ProductReportService>();
		services.AddScoped<IProductReportRepository, ProductReportRepository>();

		services.AddTransient<IByProductReceiveRepository, ByProductReceiveRepository>();
		services.AddTransient<IByProductReceiveService, ByProductReceiveService>();

		services.AddTransient<IProductMeasurementUnitRepository, ProductMeasurementUnitRepository>();
		services.AddTransient<IProductMeasurementUnitService, ProductMeasurementUnitService>();

		services.AddTransient<IProductUsedInServicesRepository, ProductUsedInServicesRepository>();
		services.AddTransient<IProductUsedInServicesService, ProductUsedInServicesService>();

		services.AddScoped<IIntegratedApplicationRepository, IntegratedApplicationRepository>();
		services.AddScoped<IIntegratedApplicationService, IntegratedApplicationService>();

		services.AddScoped<IIntegratedApplicationRefDataRepository, IntegratedApplicationRefDataRepository>();
		services.AddScoped<IIntegratedApplicationRefDataService, IntegratedApplicationRefDataService>();

		services.AddTransient<IOrganizationConfigurationBooleanRepository, OrganizationConfigurationBooleanRepository>();
		services.AddTransient<IOrganizationConfigurationBooleanService, OrganizationConfigurationBooleanService>();
	}
}