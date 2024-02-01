using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.Utility;
using vms.entity.viewModels.SalesPriceAdjustment;
using vms.service.Services.TransactionService;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class SalesPriceReduceController : ControllerBase
{
    private readonly ISalesDetailService _salesDetailService;
    private readonly IVehicleTypeService _vType;
    private readonly ISaleService _saleService;
    private readonly ISalesPriceAdjustmentService _salesPriceAdjustmentService;
	private readonly IOrgBranchService _branchService;




	public SalesPriceReduceController(ControllerBaseParamModel controllerBaseParamModel, ISalesDetailService salesDetailService, IVehicleTypeService vType, ISaleService saleService, ISalesPriceAdjustmentService salesPriceAdjustmentService, IOrgBranchService branchService) : base(controllerBaseParamModel)
    {
        _salesDetailService = salesDetailService;
        _vType = vType;
        _saleService = saleService;
        _salesPriceAdjustmentService = salesPriceAdjustmentService;
		_branchService = branchService;
	}

    [SessionExpireFilter]
    [VmsAuthorize(FeatureList.SALE)]
    [VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        return View(await _salesPriceAdjustmentService.GetSalesPriceDecrementedAdjustmentsByOrganizationAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch));
    }

    [VmsAuthorize(FeatureList.SALE)]
    [VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_ADD_CREDIT_NOTE)]
    public async Task<IActionResult> PriceChangeCreditNote(string id)
	{

		var protectedOrganizationId = UserSession.ProtectedOrganizationId;
		var sales = await _saleService.GetSaleData(id);
		var model = new SalesPriceAdjustmentCreditNoteViewModel();
	    model.SalesDetails = await _salesDetailService.GetAllSalesDetails(id);
	    model.SalesId = sales.SalesId;
        model.OrgBranchList = await _branchService.GetOrgBranchSelectListByUser(protectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		ViewData["VType"] = new SelectList(await _vType.Query().SelectAsync(), "VehicleTypeId", "VehicleTypeName");
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.SALE)]
    [VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.SALE_SALE_LIST_CAN_ADD_CREDIT_NOTE)]
    public async Task<JsonResult> PriceChangeCreditNoteSave(SalesPriceAdjustmentCreditNotePostViwModel model)
    {
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		model.OrganizationId = organizationId;
		model.CreatedBy = createdBy;
        model.CreatedTime = DateTime.Now;
		
		var id = IvatDataProtector.Protect((await _salesPriceAdjustmentService.InsertCreditNoteAkaDecreasePrice(model)).ToString());
		return Json(new { id = id });
	}


}