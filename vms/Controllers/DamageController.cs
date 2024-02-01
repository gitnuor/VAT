using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.TransactionService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class DamageController : ControllerBase
{
    private readonly IDamageTypeService _service;
    private readonly IDamageInvoiceListService _invoiceService;
    private readonly IDamageService _damageService;
    private readonly IProductService _productService;
    private readonly IDamageDetailService _damageDetailService;
    public DamageController(ControllerBaseParamModel controllerBaseParamModel, IDamageTypeService service, IDamageInvoiceListService invoiceService, IDamageService damageService, IProductService productService,
        IDamageDetailService damageDetailService) : base(controllerBaseParamModel)
    {
        _service = service;
        _invoiceService = invoiceService;
        _damageService = damageService;
        _productService = productService;
        _damageDetailService = damageDetailService;
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
            
        int org = UserSession.OrganizationId;
        var damage = await _damageService.GetDamage(org);
        return View(damage);
    }



    public async Task<ActionResult> Details(int id)
    {
        VmDamageDetailView vmDamageDetailView = new VmDamageDetailView();
        var damage= await _damageService.GetDamagewithDetails(UserSession.OrganizationId,id);
        if (damage != null)
        {
            vmDamageDetailView.damage = damage;
            vmDamageDetailView.damageDetails = await _damageDetailService.GetDamageDetails(damage.DamageId);
        }


        return View(vmDamageDetailView);
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        var DamageTypes = await _service.GetDamageTypeSelectList(UserSession.OrganizationId);

        var product = await _productService.GetProductforDamage(UserSession.OrganizationId);


        var model = new VmCombineDamage();

        model.vMDamageType = DamageTypes;
        model.productList = product;

        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_ADD)]
    public async Task<JsonResult> Create(VmCombineDamagePost model)
    {
        _damageService.InsertDamage(model, UserSession);
        await UnitOfWork.SaveChangesAsync();
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        return Json(new { id = 1 });
    }

    public async Task<JsonResult> InvoiceAndBatchList(int Pid)
    {
        var organizationId = UserSession.OrganizationId;
        var InvoiceList = await _invoiceService.GetDamageInvoiceList(organizationId, Pid);

        return new JsonResult(InvoiceList.Select(x => new
        {
            Id = x.StockInId,
            Invoice = x.InvoiceNo,
            Batch = x.BatchNo,
            CurentStock = x.CurrentStock,
        }).ToList());
    }
}