using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.service.Services.SettingService;
using vms.service.Services.TransactionService;
using vms.Utility;
// using X.PagedList;

namespace vms.Controllers;

public class DamageBkController : ControllerBase
{
    private readonly IDamageTypeService _service;
    private readonly IDamageInvoiceListService _invoiceService;

    public DamageBkController(ControllerBaseParamModel controllerBaseParamModel, IDamageTypeService service, IDamageInvoiceListService invoiceService) : base(controllerBaseParamModel)
    {
        _service = service;
        _invoiceService = invoiceService;
    }

    // [VmsAuthorize(FeatureList.PRODUCT)]
    // [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_VIEW)]
    // public async Task<IActionResult> Index(int? page, string search = null)
    //     
    //
    // {
    //         
    //     int org = UserSession.OrganizationId;
    //     List<SpDamage> listOfDamange = await _invoiceService.GetDamageList(org);
    //
    //     ViewBag.PageCount = listOfDamange.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         listOfDamange = listOfDamange.Where(c => c.DamageId.ToString().Contains(search)
    //                                                  || c.Org_Name.ToLower().Contains(search)
    //                                                  || c.Pr_Name.ToLower().Contains(search)
    //                                                  || c.Qty.ToString().Contains(search)
    //                                                  || c.D_Type.ToLower().Contains(search)
    //                                                  || c.Description.ToLower().Contains(search)
    //                                                  || c.U_Name.ToLower().Contains(search)
    //
    //         ).ToList();
    //     }
    //
    //     var pageNumber = page ?? 1;
    //     var listOfDam = listOfDamange.OrderByDescending(x => x.DamageId).ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfDam);
    // }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        var DamageTypes = await _service.Query().SelectAsync();

        IEnumerable<CustomSelectListItem> List = DamageTypes.Select(s => new CustomSelectListItem
        {
            Id = s.DamageTypeId,
            Name = s.Name
        });

        var model = new VmDamage();

        model.DamageTypes = List;

        return View(model);
    }

    // [HttpPost]
    // [VmsAuthorize(FeatureList.PRODUCT)]
    // [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.PRODUCT_DAMAGE_LIST_CAN_ADD)]
    // public async Task<IActionResult> Create(VmDamage model)
    // {
    //     if (model.DamageQty > 0)
    //     {
    //         var inmodel = new SpDamageInsert
    //         {
    //             OrganizationId = UserSession.OrganizationId,
    //             ProductId = model.ProductId,
    //                 
    //             DamageTypeId = model.DamageTypeId,
    //             DamageQty = model.DamageQty,
    //             CreatedBy = UserSession.UserId,
    //             CreatedTime = DateTime.Now,
    //             Description = model.Description ?? ControllerStaticData.DAMAGED_DESCRIPTION,
    //             VoucherNo = model.VoucherNo
    //         };
    //
    //         await _invoiceService.InsertDamage(inmodel);
    //             
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //     }
    //     else
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     }
    //
    //     return RedirectToAction(ControllerStaticData.DISPLAY_INDEX);
    // }

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