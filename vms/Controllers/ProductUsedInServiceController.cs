using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.entity.viewModels.ProductUsedInService;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.TransactionService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers
{
    public class ProductUsedInServiceController : ControllerBase
    {
        private readonly IOrgBranchService _branchService;
        private readonly IProductService _productService;
        private readonly IProductUsedInServicesService _iProductUsedInServicesService;
        private readonly IMeasurementUnitService _iMeasurementUnitService;
        private readonly ICustomerService _iCustomerService;
        public ProductUsedInServiceController(ControllerBaseParamModel controllerBaseParamModel, IOrgBranchService branchService, IProductService productService, IProductUsedInServicesService iProductUsedInServicesService, IMeasurementUnitService iMeasurementUnitService, ICustomerService iCustomerService) : base(
        controllerBaseParamModel)
        {
            _branchService = branchService;
            _productService = productService;
            _iProductUsedInServicesService = iProductUsedInServicesService;
            _iMeasurementUnitService = iMeasurementUnitService;
            _iCustomerService = iCustomerService;
        }
        [VmsAuthorize(FeatureList.PRODUCT)]
        [VmsAuthorize(FeatureList.PRODUCT_USED_IN_SERVICE_LIST_CAN_VIEW)]
        public async Task<IActionResult> Index()
        {
            var productReceiveList = await _iProductUsedInServicesService.GetProductUsedInServiceList(UserSession.ProtectedOrganizationId);
            return View(productReceiveList);
        }

        [VmsAuthorize(FeatureList.PRODUCT)]
        [VmsAuthorize(FeatureList.PRODUCT_USED_IN_SERVICE_LIST_CAN_VIEW)]
        [VmsAuthorize(FeatureList.PRODUCT_USED_IN_SERVICE_LIST_CAN_ADD)]
        public async Task<IActionResult> Create()
        {
            var model = new VmProductUsedInServicePostModel
            {
                OrgBranchList = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
            };
            return View(model);
        }
        [HttpPost]
        [VmsAuthorize(FeatureList.PRODUCT)]
        [VmsAuthorize(FeatureList.PRODUCT_USED_IN_SERVICE_LIST_CAN_VIEW)]
        [VmsAuthorize(FeatureList.PRODUCT_USED_IN_SERVICE_LIST_CAN_ADD)]
        public async Task<IActionResult> Create(VmProductUsedInServicePostModel vmByProductReceivePostModel)
        {
            var createdBy = UserSession.UserId;
            var organizationId = UserSession.OrganizationId;
            vmByProductReceivePostModel.OrganizationId = organizationId;
            vmByProductReceivePostModel.CreatedBy = createdBy;
            vmByProductReceivePostModel.CreatedTime = DateTime.Now;
            vmByProductReceivePostModel.ModifiedBy = createdBy;
            vmByProductReceivePostModel.ModifiedTime = DateTime.Now;
            vmByProductReceivePostModel.IsActive = true;
            vmByProductReceivePostModel.ApiTransactionId = null;
            vmByProductReceivePostModel.ExcelDataUploadId = null;


            var id = IvatDataProtector.Protect((await _iProductUsedInServicesService.InsertProductUsedInServiceData(vmByProductReceivePostModel)).ToString());
            await UnitOfWork.SaveChangesAsync();

            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            return RedirectToAction(nameof(Index));
        }
    }
}
