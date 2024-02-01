using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.entity.viewModels.BranchTransferSend;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.TransactionService;
using vms.service.Services.UploadService;
using vms.Utility;

namespace vms.Controllers;

public class BranchTransferReceiveController : ControllerBase
{
    private readonly IOrganizationService _organizationService;
    private readonly IMeasurementUnitService _measurementUnitService;
    private readonly IDocumentTypeService _documentType;
    private readonly IProductStoredProcedureService _productStoredProcedureService;
    private readonly IVehicleTypeService _vehicleTypeService;
    private readonly ISaleService _saleService;
    private readonly IOrgBranchService _branchService;
    private readonly IBranchTransferSendService _branchTransferSendService;
    private readonly IBranchTransferReceiveService _branchTransferReceiveService;




    public BranchTransferReceiveController(IDocumentTypeService documentType, IOrganizationService organizationService, ControllerBaseParamModel controllerBaseParamModel, 
       IMeasurementUnitService measurementUnitService, 
        IProductStoredProcedureService productStoredProcedureService,
        IVehicleTypeService vehicleTypeService, ISaleService saleService, IOrgBranchService branchService, IFileOperationService fileOperationService, IBranchTransferSendService branchTransferSendService, IBranchTransferReceiveService branchTransferReceiveService) : base(controllerBaseParamModel)
    {
        _documentType = documentType;
        _measurementUnitService = measurementUnitService;
        _organizationService = organizationService;
        _productStoredProcedureService = productStoredProcedureService;
        _vehicleTypeService = vehicleTypeService;
        _saleService = saleService;
        _branchService = branchService;
        _branchTransferSendService = branchTransferSendService;
        _branchTransferReceiveService = branchTransferReceiveService;
    }

    [SessionExpireFilter]
    [VmsAuthorize(FeatureList.SALE)]
    [VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        return View(await _branchTransferReceiveService.GetBranchTransferReceivesByOrganization(UserSession.ProtectedOrganizationId));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var protectedOrganizationId = UserSession.ProtectedOrganizationId;
        var organizationId = UserSession.OrganizationId;
        var org = await _organizationService.GetOrganization(protectedOrganizationId);
        var model = new BranchTransferSendCreateViewModel
		{
            OrganizationName = org.Name,
            OrganizationBin = org.Bin,
            OrganizationAddress = org.Address,
            VatResponsiblePersonName = org.VatResponsiblePersonName,
            VatResponsiblePersonDesignation = org.VatResponsiblePersonDesignation,
            DeliveryDate = DateTime.Today,
            ProductForTransferList = _productStoredProcedureService.GetProductForSale(organizationId, 0), //todo: Have to fix product
			MeasurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(protectedOrganizationId),
            DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(protectedOrganizationId),
            VehicleTypesList = await _vehicleTypeService.GetVehicleTypes(organizationId),
            OrgBranchList = await _branchService.GetOrgBranchByOrganization(protectedOrganizationId)
        };
        return View(model);
    }

    [HttpPost]
    public async Task<JsonResult> Create(VmSaleLocalPost model)
    {
        var createdBy = UserSession.UserId;
        var organizationId = UserSession.OrganizationId;
        model.OrganizationId = organizationId;
        model.CreatedBy = createdBy;
        model.SalesTypeId = 1;
        model.SalesDate = DateTime.Now;
        model.IsTaxInvoicePrined = false;
        string id = IvatDataProtector.Protect((await _saleService.InsertLocalSale(model)).ToString());
        await UnitOfWork.SaveChangesAsync();
        return Json(new { id = id });
    }

}