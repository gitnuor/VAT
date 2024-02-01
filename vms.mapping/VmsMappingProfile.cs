using AutoMapper;
using vms.entity.Dto.Customer;
using vms.entity.Dto.Product;
using vms.entity.Dto.PurchaseLocal;
using vms.entity.Dto.Sales;
using vms.entity.Dto.SalesCreate;
using vms.entity.Dto.SalesExport;
using vms.entity.Dto.SalesLocal;
using vms.entity.Dto.User;
using vms.entity.Dto.Vendor;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.ParamModel;
using vms.entity.StoredProcedureModel.Purchase;
using vms.entity.StoredProcedureModel.Sales;
using vms.entity.viewModels;
using vms.entity.viewModels.BranchTransferSend;
using vms.entity.viewModels.ByProductReceive;
using vms.entity.viewModels.CustomerViewModel;
using vms.entity.viewModels.NewReports;
using vms.entity.viewModels.Payment;
using vms.entity.viewModels.ProductMeasurementUnit;
using vms.entity.viewModels.ProductUsedInService;
using vms.entity.viewModels.PurchaseReport;
using vms.entity.viewModels.ReportsViewModel;
using vms.entity.viewModels.SalesPriceAdjustment;
using vms.entity.viewModels.SalesReport;
using vms.entity.viewModels.SubscriptionAndBilling;
using vms.entity.viewModels.User;
using vms.entity.viewModels.VendorViewModel;
using vms.entity.viewModels.VmPurchaseCombineParamsModels;
using vms.entity.viewModels.VmSalesCombineParamsModels;

namespace vms.mapping;

public class VmsMappingProfile : Profile
{
	public VmsMappingProfile()
	{
		// Start Purchase Local VM Model Mapping
		CreateMap<VmPurchaseLocalPost, SpInsertPurchaseCombinedParam>()
			.ForMember(o => o.PurchaseDetailList, b => b.MapFrom(z => z.Products))
			.ForMember(o => o.PurchasePayments, b => b.MapFrom(z => z.Payments))
			.ForMember(o => o.ContentInfoList, b => b.MapFrom(z => z.Documents));

		CreateMap<VmPurchaseLocalProduct, PurchaseDetailCombinedParamModel>()
			.ForMember(d =>
				d.VatPercent, s =>
				s.MapFrom(d => d.ProductVatPercent))
			.ForMember(d =>
				d.ProductVattypeId, s =>
				s.MapFrom(d => d.ProductVatTypeId));

		CreateMap<VmPurchaseLocalDocument, PurchaseContentInfoCombinedParamModel>()
			.ForMember(o =>
				o.Remarks, b =>
				b.MapFrom(z => z.DocumentRemarks));

		CreateMap<VmPurchaseLocalPayment, PurchasePaymentCombinedParamModel>();

		CreateMap<VmPurchaseLocalDocument, FIleDocumentInfo>()
			.ForMember(o => o.FormFIle, b => b.MapFrom(z => z.UploadedFile))
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentTypeId))
			.ForMember(o => o.DocumentRemarks, b => b.MapFrom(z => z.DocumentRemarks));

		CreateMap<FileSaveFeedbackDto, VmPurchaseLocalDocument>()
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentTypeId));

		//End Purchase Local Model VM Model Mapping

		// Start Purchase Impot VM Model Mapping
		CreateMap<VmPurchaseImportPost, SpInsertPurchaseCombinedParam>()
			.ForMember(o => o.PurchaseDetailList, b => b.MapFrom(z => z.Products))
			.ForMember(o => o.PurchasePayments, b => b.MapFrom(z => z.Payments))
			.ForMember(o => o.ContentInfoList, b => b.MapFrom(z => z.Documents))
			.ForMember(o => o.ImportTaxPayments, b => b.MapFrom(z => z.ImportTaxPayments));

		CreateMap<VmPurchaseImportDetail, PurchaseDetailCombinedParamModel>()
			.ForMember(d =>
				d.VatPercent, s =>
				s.MapFrom(d => d.ProductVatPercent))
			.ForMember(d =>
				d.ProductVattypeId, s =>
				s.MapFrom(d => d.ProductVatTypeId));

		CreateMap<VmPurchaseImportSubscriptionPost, SpInsertPurchaseCombinedParam>()
			.ForMember(o => o.PurchaseDetailList, b => b.MapFrom(z => z.Products));

		CreateMap<VmPurchaseImportSubscriptionDetail, PurchaseDetailCombinedParamModel>();


		CreateMap<VmPurchaseImportDocument, PurchaseContentInfoCombinedParamModel>()
			.ForMember(o => o.Remarks, b => b.MapFrom(z => z.DocumentRemarks));

		CreateMap<VmPurchaseImportPayment, PurchasePaymentCombinedParamModel>();

		CreateMap<VmPurchaseImportTaxPayment, PurchaseImportTaxPaymentCombinedParamModel>();

		CreateMap<VmPurchaseImportDocument, FIleDocumentInfo>()
			.ForMember(o => o.FormFIle, b => b.MapFrom(z => z.UploadedFile))
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentTypeId))
			.ForMember(o => o.DocumentRemarks, b => b.MapFrom(z => z.DocumentRemarks));

		CreateMap<FileSaveFeedbackDto, VmPurchaseImportDocument>()
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentTypeId));

		//End Purchase Import Model VM Model Mapping

		//Start Sale Local VM Model Mapping
		CreateMap<VmSaleLocalPost, VmSalesCombineParamsModel>()
			.ForMember(o => o.VehicleDriverName, b => b.MapFrom(z => z.DriverName))
			.ForMember(o => o.VehicleDriverContactNo, b => b.MapFrom(z => z.DriverMobile))
			.ForMember(o => o.VehicleRegNo, b => b.MapFrom(z => z.VehicleRegistrationNo))
			.ForMember(o => o.CustomerPoNumber, b => b.MapFrom(z => z.PONo))
			.ForMember(o => o.SalesDetailList, b => b.MapFrom(z => z.Products))
			.ForMember(o => o.SalesPaymentReceiveJson, b => b.MapFrom(z => z.Payments))
			.ForMember(o => o.ContentInfoJson, b => b.MapFrom(z => z.Documents));
		CreateMap<VmSaleLocalDetail, VmSaleDetailCombineParamsModel>()
			.ForMember(o => o.Vatpercent, b => b.MapFrom(z => z.ProductVatPercent))
			.ForMember(o => o.ProductVattypeId, b => b.MapFrom(z => z.ProductVatTypeId));

		CreateMap<VmSaleLocalDocument, VmSaleContentCombineParamsModel>()
			.ForMember(o => o.Remarks, b => b.MapFrom(z => z.DocumentRemarks))
			.ForMember(o => o.FileUrl, b => b.MapFrom(z => z.FileUrl));
		CreateMap<VmSaleLocalPayment, VmSalePaymentReceiveCombineParamsModel>()
			.ForMember(o => o.ReceivedPaymentMethodId, b => b.MapFrom(z => z.PaymentMethodId))
			.ForMember(o => o.ReceiveDate, b => b.MapFrom(z => z.PaymentDate))
			.ForMember(o => o.ReceiveAmount, b => b.MapFrom(z => z.PaidAmount));

		CreateMap<VmSaleLocalDocument, FIleDocumentInfo>()
			.ForMember(o => o.FormFIle, b => b.MapFrom(z => z.UploadedFile))
			.ForMember(o => o.DocumentRemarks, b => b.MapFrom(z => z.DocumentRemarks));

		CreateMap<FileSaveFeedbackDto, VmSaleLocalDocument>();

		// local sale with breakdown
		CreateMap<VmSaleLocalPostWithBreakdown, VmSalesCombineParamsModel>()
			.ForMember(o => o.VehicleDriverName, b => b.MapFrom(z => z.DriverName))
			.ForMember(o => o.VehicleDriverContactNo, b => b.MapFrom(z => z.DriverMobile))
			.ForMember(o => o.VehicleRegNo, b => b.MapFrom(z => z.VehicleRegistrationNo))
			.ForMember(o => o.CustomerPoNumber, b => b.MapFrom(z => z.PONo))
			.ForMember(o => o.SalesDetailList, b => b.MapFrom(z => z.Products))
			.ForMember(o => o.SalesPaymentReceiveJson, b => b.MapFrom(z => z.Payments))
			.ForMember(o => o.ContentInfoJson, b => b.MapFrom(z => z.Documents));
		//End Sale Local VM Model Mapping

		//Start Sale Local Export VM Model Mapping
		CreateMap<VmSaleExportPost, VmSalesCombineParamsModel>()
			.ForMember(o => o.VehicleDriverName, b => b.MapFrom(z => z.DriverName))
			.ForMember(o => o.VehicleDriverContactNo, b => b.MapFrom(z => z.DriverMobile))
			.ForMember(o => o.VehicleRegNo, b => b.MapFrom(z => z.VehicleRegistrationNo))
			.ForMember(o => o.CustomerPoNumber, b => b.MapFrom(z => z.PONo))
			.ForMember(o => o.SalesDetailList, b => b.MapFrom(z => z.VmSaleExportDetails))
			.ForMember(o => o.SalesPaymentReceiveJson, b => b.MapFrom(z => z.VmSaleLocalPayments))
			.ForMember(o => o.ContentInfoJson, b => b.MapFrom(z => z.VmSaleLocalDocuments));
		CreateMap<VmSaleExportDetail, VmSaleDetailCombineParamsModel>();

		CreateMap<VmSaleExportDocument, VmSaleContentCombineParamsModel>()
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentType))
			.ForMember(o => o.Remarks, b => b.MapFrom(z => z.DocumentRemarks))
			.ForMember(o => o.FileUrl, b => b.MapFrom(z => z.FileUrl));
		CreateMap<VmSaleExportPayment, VmSalePaymentReceiveCombineParamsModel>()
			.ForMember(o => o.ReceivedPaymentMethodId, b => b.MapFrom(z => z.PaymentMethodId))
			.ForMember(o => o.ReceiveDate, b => b.MapFrom(z => z.PaymentDate))
			.ForMember(o => o.ReceiveAmount, b => b.MapFrom(z => z.PaidAmount));

		CreateMap<VmSaleExportDocument, FIleDocumentInfo>()
			.ForMember(o => o.FormFIle, b => b.MapFrom(z => z.FileUpload))
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentType))
			.ForMember(o => o.DocumentRemarks, b => b.MapFrom(z => z.DocumentRemarks));

		CreateMap<FileSaveFeedbackDto, VmSaleExportDocument>()
			.ForMember(o => o.DocumentType, b => b.MapFrom(z => z.DocumentTypeId));
		CreateMap<VmSaleLocalBreakDown, VmSaleBreakDownParamModel>();
		//End Sale Local Export VM Model Mapping


		CreateMap<ViewSalesExport, SalesExportDto>()
			.ForMember(d => d.BranchId, s => s.MapFrom(d => d.OrgBranchId))
			.ForMember(d => d.TotalPrice, s => s.MapFrom(d => d.TotalPriceWithoutTax));

		CreateMap<ViewSalesExport, SalesExportWithDetailDto>()
			.ForMember(d => d.BranchId, s => s.MapFrom(d => d.OrgBranchId))
			.ForMember(d => d.TotalPrice, s => s.MapFrom(d => d.TotalPriceWithoutTax));

		CreateMap<ViewSalesDetail, SalesExportDetailDto>();


		CreateMap<ViewSalesLocal, SalesLocalDto>()
			.ForMember(d => d.BranchId, s => s.MapFrom(d => d.OrgBranchId));

		CreateMap<ViewSalesLocal, SalesLocalWithDetailDto>()
			.ForMember(d => d.BranchId, s => s.MapFrom(d => d.OrgBranchId));

		CreateMap<ViewSalesDetail, SalesLocalDetailDto>();


		CreateMap<ViewSale, SaleDto>()
			.ForMember(d => d.BranchId, s => s.MapFrom(d => d.OrgBranchId));

		CreateMap<ViewSale, SaleWithDetailDto>()
			.ForMember(d => d.BranchId, s => s.MapFrom(d => d.OrgBranchId));

		CreateMap<ViewSalesDetail, SalesDetailDto>();


		CreateMap<VmInputOutputCoEfficientPost, PriceSetup>()
			.ForMember(o => o.PriceSetupProductCosts, b => b.MapFrom(z => z.ProductCostList))
			.ForMember(o => o.PriceSetupProductCosts, b => b.MapFrom(z => z.OverheadCostList));

		CreateMap<InputOutputCoEfficientProductCost, PriceSetupProductCost>()
			.ForMember(o => o.RawMaterialId, b => b.MapFrom(z => z.ProductId))
			.ForMember(o => o.RequiredQty, b => b.MapFrom(z => z.RequiredQty))
			.ForMember(o => o.Cost, b => b.MapFrom(z => z.ProductCost));
		CreateMap<InputOutputCoEfficientOverheadCost, PriceSetupProductCost>()
			.ForMember(o => o.Cost, b => b.MapFrom(z => z.OverheadCostAmount));

		CreateMap<VmSelfProduction, vmProductionReceive>()
			.ForMember(o => o.ContentInfoJson, b => b.MapFrom(z => z.SelfProductionDocumentList));


		CreateMap<VmSelfProductionDocument, Content>()
			.ForMember(o => o.FileUrl, b => b.MapFrom(z => z.FileUrl))
			.ForMember(o => o.MimeType, b => b.MapFrom(z => z.MimeType))
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentTypeId))
			.ForMember(o => o.Remarks, b => b.MapFrom(z => z.DocumentRemarks));


		CreateMap<VmSelfProductionDocument, FIleDocumentInfo>()
			.ForMember(o => o.FormFIle, b => b.MapFrom(z => z.UploadedFile))
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentTypeId))
			.ForMember(o => o.DocumentRemarks, b => b.MapFrom(z => z.DocumentRemarks));

		CreateMap<FileSaveFeedbackDto, VmSelfProductionDocument>()
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentTypeId));


		CreateMap<VmContractualProduction, vmProductionReceive>()
			.ForMember(o => o.ContentInfoJson, b => b.MapFrom(z => z.ContractualProductionDocumentList));


		CreateMap<VmContractualProductionDocument, Content>()
			.ForMember(o => o.FileUrl, b => b.MapFrom(z => z.FileUrl))
			.ForMember(o => o.MimeType, b => b.MapFrom(z => z.MimeType))
			.ForMember(o => o.DocumentTypeId, b => b.MapFrom(z => z.DocumentTypeId))
			.ForMember(o => o.Remarks, b => b.MapFrom(z => z.DocumentRemarks));

		CreateMap<VmPurchaseDamagePost, Damage>()
			.ForMember(o => o.DamageDetails, b => b.MapFrom(z => z.DamageDetailList));
		CreateMap<VmPurchaseDamageDetail, DamageDetail>();


		CreateMap<VmSalesDamagePost, Damage>()
			.ForMember(o => o.DamageDetails, b => b.MapFrom(z => z.DamageDetailList));
		CreateMap<VmSalesDamageDetail, DamageDetail>();


		CreateMap<VmCombineDamagePost, Damage>()
			.ForMember(o => o.DamageDetails, b => b.MapFrom(z => z.DamageDetailList));
		CreateMap<VmCombineDamageDetail, DamageDetail>();

		CreateMap<VmSalesCreditNotePost, vmCreditNote>()
			.ForMember(o => o.CreditNoteDetails, b => b.MapFrom(z => z.vmSalesCreditNoteDetials));
		CreateMap<VmSalesCreditNoteDetial, CreditNoteDetail>()
			.ForMember(o => o.ReasonOfReturnInDetail, b => b.MapFrom(z => z.ReasonOfReturnInDetail));

		CreateMap<VmPurchaseDebitNotePost, vmDebitNote>()
			.ForMember(o => o.DebitNoteDetails, b => b.MapFrom(z => z.vmPurchaseDebitNoteDetials));
		CreateMap<VmPurchaseDebitNoteDetial, DebitNoteDetail>()
			.ForMember(o => o.ReasonOfReturn, b => b.MapFrom(z => z.ReasonOfReturnInDetail));
		CreateMap<Vendor, vmVendorReportRdlc>();
		CreateMap<SpMonthlyPurchaseReport, vmPurchaseReportMonthlyRdlc>();
		CreateMap<Customer, vmCustomerReportRdlc>();
		CreateMap<Sale, vmSalesReportRdlc>()
			.ForMember(o => o.CustName, b => b.MapFrom(x => x.Customer.Name));
		CreateMap<Purchase, vmPurchaseReportRdlc>()
			.ForMember(o => o.PurchaseTypeName, b => b.MapFrom(x => x.PurchaseType.Name))
			.ForMember(o => o.VendorName, b => b.MapFrom(x => x.Vendor.Name));
		CreateMap<User, vmUserReportRdlc>()
			.ForMember(o => o.RoleName, b => b.MapFrom(x => x.Role.RoleName));
		CreateMap<ContractualProduction, vmContractualProductionReportRdlc>()
			.ForMember(o => o.VendorName, b => b.MapFrom(x => x.Vendor.Name))
			.ForMember(o => o.ContractTypeName, b => b.MapFrom(x => x.ContractType.Name));

		CreateMap<vmCustomer, Customer>();
		CreateMap<Customer, vmCustomer>();
		CreateMap<CustomerLocalPostDto, Customer>()
			.ForMember(o => o.ReferenceKey, b =>
				b.MapFrom(z => z.Id));

		CreateMap<CustomerLocalCreatePostViewModel, Customer>();
		CreateMap<CustomerForeignCreateViewModel, Customer>();
		CreateMap<ViewCustomer, CustomerDto>();
		CreateMap<ViewCustomerForeign, CustomerForeignDto>();
		CreateMap<ViewCustomerLocal, CustomerLocalDto>();
		CreateMap<ViewCustomerLocal, CustomerLocalDetailViewModel>();

		CreateMap<ViewCustomerBranch, CustomerBranchViewModel>();


		CreateMap<vmVendor, Vendor>();
		CreateMap<Vendor, vmVendor>();
		CreateMap<ViewVendor, VendorDto>();
		CreateMap<ViewVendorForeign, VendorForeignDto>();
		CreateMap<ViewVendorLocal, VendorLocalDto>();
		CreateMap<VendorLocalPostDto, Vendor>()
			.ForMember(o => o.ReferenceKey, b =>
				b.MapFrom(z => z.Id));


		CreateMap<VendorForeignCreateViewModel, Vendor>();
		CreateMap<VendorLocalCreateViewModel, Vendor>();

		CreateMap<VmExcelSimplifiedPurchase, ExcelSimplifiedPurchase>();
		CreateMap<VmExcelSimplifiedLocalPurchase, ExcelSimplifiedLocalPurchase>();
		CreateMap<VmExcelSimplifiedSales, ExcelSimplifiedSalse>();
		CreateMap<VmExcelSimplifiedLocalSalesWithoutPayment, ExcelSimplifiedSalse>();
		CreateMap<VmExcelSimplifiedLocalSalesCalculateByVat, ExcelSimplifiedLocalSaleCalculateByVat>();
		CreateMap<UserUpdateViewModel, User>().ReverseMap();
        CreateMap<UserRoleUpdateViewModel, User>().ReverseMap();
        CreateMap<UserPasswordUpdateViewModel, User>().ReverseMap();
        CreateMap<UserImageUpdateViewModel, User>().ReverseMap();
        CreateMap<UserSignatureUpdateViewModel, User>().ReverseMap();
        CreateMap<UserBranchUpdateViewModel, User>().ReverseMap();

		CreateMap<CustomerCategoryCreateViewModel, CustomerCategory>().ReverseMap();
        CreateMap<CustomerCategoryUpdateViewModel, CustomerCategory>().ReverseMap();
        CreateMap<VndorCategoryCreateViewModel, VendorCategory>().ReverseMap();
        CreateMap<VndorCategoryUpdateViewModel, VendorCategory>().ReverseMap();
        CreateMap<CustomerSubscriptionCategoryCreateViewModel, CustomerSubscriptionCategory>().ReverseMap();
        CreateMap<CustomerSubscriptionCategoryUpdateViewModel, CustomerSubscriptionCategory>().ReverseMap();

        CreateMap<SpGetReportSalesModel, SalesReportViewModel>();
		CreateMap<SpGetReportPurchaseModel, PurchaseReportViewModel>();

		CreateMap<SpGetReportSummerySalesModel, SalesSummeryReportViewModel>();
		CreateMap<SpGetReportSummeryPurchaseModel, PurchaseSummeryReportViewModel>();

		CreateMap<SalesPriceAdjustmentCreditNotePostViwModel, SalesPriceAdjustmentCombineParamsViewModel>();
		CreateMap<SalesPriceAdjustmentCreditNoteDetailViewModel, SalesPriceAdjustmentCombineDetailParamsViewModel>()
			.ForMember(d =>
				d.ReasonOfChange, s =>
				s.MapFrom(d => d.ReasonOfChangeInDetail));

		CreateMap<SalesPriceAdjustmentDebitNotePostViwModel, SalesPriceAdjustmentCombineParamsViewModel>();
		CreateMap<SalesPriceAdjustmentDebitNoteDetailViewModel, SalesPriceAdjustmentCombineDetailParamsViewModel>()
			.ForMember(d =>
				d.ReasonOfChange, s =>
				s.MapFrom(d => d.ReasonOfChangeInDetail));

		CreateMap<BranchTransferSendCreatePostViewModel, BranchTransferSendParamViewModel>();
		CreateMap<BranchTransferSendProductPostViewModel, BranchTransferSendProductParamViewModel>();

		CreateMap<ViewProduct, ProductDto>();
		CreateMap<ProductVattype, ProductVatTypeDto>()
			.ForMember(d => d.ProductVatTypeId, s => s.MapFrom(d => d.ProductVattypeId));


		CreateMap<ViewUploadedContent, UploadedContentViewModel>();


		CreateMap<ViewMushakReturnSelfPayment, MushakReturnSelfPaymentListViewModel>();
		CreateMap<ViewMushakReturnVdsPayment, MushakReturnVdsPaymentListViewModel>();
		CreateMap<ViewTdsPayment, TdsPaymentListViewModel>();


		CreateMap<SalesLocalPostDto, SalesCombinedInsertParamDto>();
		CreateMap<SalesLocalDetailPostWithSalesDto, SalesDetailCreateParamDto>();

		CreateMap<SalesCombinedPostDto, SalesCombinedInsertParamDto>();
		CreateMap<SalesCombinedDetailPostWithSalesDto, SalesDetailCreateParamDto>();

		CreateMap<PurchaseLocalPostDto, SpInsertPurchaseFromApiCombinedParam>();
		CreateMap<PurchaseLocalDetailPostWithPurchaseDto, PurchaseDetailFromApiCombinedParamModel>();

		CreateMap<UserCreateViewModel, User>().ReverseMap();
		CreateMap<ByProductReceive, VmByProductReceive>()
			.ForMember(d => d.ProductName, d => d.MapFrom(z => z.Product.Name))
			.ForMember(d => d.BranchName, d => d.MapFrom(z => z.OrgBranch.Name))
			.ForMember(d => d.MeasurementUnitName, d => d.MapFrom(z => z.MeasurementUnit.Name))
			;

		#region Product Measurement Unit

		CreateMap<ProductMeasurementUnit, ProductMeasurementUnitIndexViewModel>()
			.ForMember(d => d.ProductName, d => d.MapFrom(z => z.Product.Name))
			.ForMember(d => d.MeasurementUnitName, d => d.MapFrom(z => z.MeasurementUnit.Name))
			;
		CreateMap<ProductMeasurementUnitCreateViewModel, ProductMeasurementUnit>().ReverseMap();

		#endregion

		#region product used in service

		CreateMap<ProductUsedInService, VmProductUsedInService>()
			.ForMember(d => d.ProductName, d => d.MapFrom(z => z.Product.Name))
			.ForMember(d => d.BranchName, d => d.MapFrom(z => z.OrgBranch.Name))
			.ForMember(d => d.MeasurementUnitName, d => d.MapFrom(z => z.MeasurementUnit.Name))
			.ForMember(d => d.CustomerName, d => d.MapFrom(z => z.Customer.Name))
			;

		#endregion

		CreateMap<ProductPostDto, Product>()
			.ForMember(o => o.ReferenceKey, b =>
				b.MapFrom(z => z.Id));

		CreateMap<ViewProduct, CustomerSubscriptionDetailCreateViewModel>()
			.ForMember(o => o.ProductVatTypeId, b => b.MapFrom(z => z.ProductVattypeId))
			.ForMember(o => o.ProductVatPercent, b => b.MapFrom(z => z.DefaultVatPercent));

		CreateMap<SpGetUnBilledSubscriptions, SubscriptionBillDetailCreateViewModel>();

		#region user api

		CreateMap<UserPostDto, User>()
					.ForMember(o => o.ReferenceKey, b =>
						b.MapFrom(z => z.Id));
		CreateMap<User, UserDto>();
		CreateMap<ViewUser, UserDto>();

		#endregion

	}
}