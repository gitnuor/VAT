using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.Utility;
using vms.entity.viewModels.ByProductReceive;
using vms.utility.StaticData;
using System;
using Microsoft.AspNetCore.DataProtection;
using vms.service.Services.TransactionService;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;

namespace vms.Controllers
{
    public class ByProductReceiveController : ControllerBase
	{
		private readonly IOrgBranchService _branchService;
		private readonly IProductService _productService;
		private readonly IByProductReceiveService _iByProductReceiveService;
        private readonly IMeasurementUnitService _iMeasurementUnitService;
        public ByProductReceiveController(ControllerBaseParamModel controllerBaseParamModel, IOrgBranchService branchService, IProductService productService, IByProductReceiveService iByProductReceiveService, IMeasurementUnitService iMeasurementUnitService) : base(
		controllerBaseParamModel)
		{
			_branchService = branchService;
			_productService = productService;
			_iByProductReceiveService = iByProductReceiveService;
			_iMeasurementUnitService = iMeasurementUnitService;
		}
		[VmsAuthorize(FeatureList.PRODUCT)]
		[VmsAuthorize(FeatureList.PRODUCT_BYPRODUCT_RECEIVE_LIST_CAN_VIEW)]
		public async Task<IActionResult> Index()
		{
			var productReceiveList = await _iByProductReceiveService.GetByProductReceiveList(UserSession.ProtectedOrganizationId);
			return View(productReceiveList);
		}

		[VmsAuthorize(FeatureList.PRODUCT)]
		[VmsAuthorize(FeatureList.PRODUCT_BYPRODUCT_RECEIVE_LIST_CAN_VIEW)]
		[VmsAuthorize(FeatureList.PRODUCT_BYPRODUCT_RECEIVE_CAN_ADD)]
		public async Task<IActionResult> Create()
		{
			var model = new VmByProductReceivePostModel
			{
				ReceiveDate = DateTime.Now,
				OrgBranchList = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
				ByProductList = await _productService.GetProductsSelectList(UserSession.ProtectedOrganizationId),
				MeasurementUnitList = await _iMeasurementUnitService.GetMeasurementUnitSelectList(UserSession.ProtectedOrganizationId)

            };
			return View(model);
		}
		[HttpPost]
        [VmsAuthorize(FeatureList.PRODUCT)]
        [VmsAuthorize(FeatureList.PRODUCT_BYPRODUCT_RECEIVE_LIST_CAN_VIEW)]
        [VmsAuthorize(FeatureList.PRODUCT_BYPRODUCT_RECEIVE_CAN_ADD)]
        public async Task<IActionResult> Create(VmByProductReceivePostModel vmByProductReceivePostModel)
        {
            var createdBy = UserSession.UserId;
            var organizationId = UserSession.OrganizationId;
            vmByProductReceivePostModel.OrganizationId = organizationId;
            vmByProductReceivePostModel.CreatedBy = createdBy;
            vmByProductReceivePostModel.CreatedTime = DateTime.Now;


            var id = IvatDataProtector.Protect((await _iByProductReceiveService.InsertByProductReceiveData(vmByProductReceivePostModel)).ToString());
            await UnitOfWork.SaveChangesAsync();

            //return Json(new { id });

            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            return RedirectToAction(nameof(Index));
        }
    }
}
