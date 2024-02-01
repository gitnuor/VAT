using AutoMapper;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.ParamModel;
using vms.entity.StoredProcedureModel.Purchase;
using vms.entity.StoredProcedureModel.Sales;
using vms.entity.viewModels;
using vms.entity.viewModels.BranchTransferSend;
using vms.entity.viewModels.CustomerViewModel;
using vms.entity.viewModels.NewReports;
using vms.entity.viewModels.PurchaseReport;
using vms.entity.viewModels.ReportsViewModel;
using vms.entity.viewModels.SalesPriceAdjustment;
using vms.entity.viewModels.SalesReport;
using vms.entity.viewModels.User;
using vms.entity.viewModels.VendorViewModel;
using vms.entity.viewModels.VmPurchaseCombineParamsModels;
using vms.entity.viewModels.VmSalesCombineParamsModels;

namespace vms.mapping;

public class VmsDtoMappingProfile : Profile
{
    public VmsDtoMappingProfile()
    {

    }
}