using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.ProductService;
using vms.utility.StaticData;
using vms.Utility;


namespace vms.Controllers;

public class OverHeadCostController : ControllerBase
{
    private readonly IOverHeadCostService _overHeadCost;

    public OverHeadCostController(IOverHeadCostService overHeadCost,
        ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
    {
        _overHeadCost = overHeadCost;
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_OVER_HEAD_COST_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var overHeadCost = await _overHeadCost.Query().Where(w => w.OrganizationId == UserSession.OrganizationId)
            .SelectAsync();
        overHeadCost.ToList().ForEach(delegate(OverHeadCost cost)
        {
            cost.EncryptedId = IvatDataProtector.Protect(cost.OverHeadCostId.ToString());
        });
        return View(overHeadCost);
    }


    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_OVER_HEAD_COST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_OVER_HEAD_COST_CAN_ADD)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_OVER_HEAD_COST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_OVER_HEAD_COST_CAN_ADD)]
    public async Task<IActionResult> Create(OverHeadCost overHead)
    {
        overHead.CreatedTime = DateTime.Now;
        overHead.CreatedBy = UserSession.UserId;
        overHead.OrganizationId = UserSession.OrganizationId;
        overHead.IsApplicableAsRawMaterial = true;
        overHead.IsActive = true;

        _overHeadCost.Insert(overHead);
        await UnitOfWork.SaveChangesAsync();
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        var jObj = JsonConvert.SerializeObject(overHead, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


        AuditLog au = new AuditLog();
        au.ObjectTypeId = (int)EnumObjectType.OverHeadCost;
        au.PrimaryKey = overHead.OverHeadCostId;
        au.AuditOperationId = (int)EnumOperations.Add;
        au.UserId = UserSession.UserId;
        au.Datetime = DateTime.Now;
        au.Descriptions = jObj.ToString();
        au.IsActive = true;
        au.CreatedBy = UserSession.UserId;
        au.CreatedTime = DateTime.Now;
        au.OrganizationId = UserSession.OrganizationId;
        await AuditLogCreate(au);

        return RedirectToAction(nameof(Index));
    }


    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_OVER_HEAD_COST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_OVER_HEAD_COST_CAN_ADD)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        int overHeadCostId = int.Parse(IvatDataProtector.Unprotect(id));
        var cost = await _overHeadCost.Query()
            .SingleOrDefaultAsync(p => p.OverHeadCostId == overHeadCostId, CancellationToken.None);
        if (cost == null)
        {
            return NotFound();
        }

        return View(cost);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_OVER_HEAD_COST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_OVER_HEAD_COST_CAN_ADD)]
    public async Task<IActionResult> Edit(OverHeadCost overHead)
    {
        if (ModelState.IsValid)
        {
            try
            {
                overHead.ModifiedTime = DateTime.Now;
                overHead.ModifiedBy = UserSession.UserId;
                overHead.OrganizationId = UserSession.OrganizationId;
                _overHeadCost.Update(overHead);
                await UnitOfWork.SaveChangesAsync();
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
        else
        {
            return View(overHead);
        }

        return RedirectToAction(nameof(Index));
    }
}