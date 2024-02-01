using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vms.entity.models;
using vms.Utility;
using vms.entity.viewModels;
using vms.utility.StaticData;
using Newtonsoft.Json;
using vms.entity.Enums;
using Microsoft.AspNetCore.DataProtection;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class PurchaseReasonsController : ControllerBase
{

    private readonly IPurchaseReasonService _service;
    public PurchaseReasonsController(ControllerBaseParamModel controllerBaseParamModel, IPurchaseReasonService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }


    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PURCHASE_REASONES_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var PurchaseReasons = await _service.GetPurchaseReasons();
        PurchaseReasons = PurchaseReasons.OrderByDescending(x => x.PurchaseReasonId);
        return View(PurchaseReasons);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var purchaseReason = await _service.Query()
            .SingleOrDefaultAsync(m => m.PurchaseReasonId == id, CancellationToken.None);
        if (purchaseReason == null)
        {
            return NotFound();
        }

        return View(purchaseReason);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PURCHASE_REASONES_CAN_VIEW)]
       
    public IActionResult Create()
    {
        return View();
    }

       
    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PURCHASE_REASONES_CAN_VIEW)]
       
    public async Task<IActionResult> Create( PurchaseReason purchaseReason)
    {
        if (ModelState.IsValid)
        {
            _service.Insert(purchaseReason);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(purchaseReason, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.PurchaseReason,
                PrimaryKey = purchaseReason.PurchaseReasonId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = jObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };


            var au_status = await AuditLogCreate(au);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            return RedirectToAction(nameof(Index));
              
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(purchaseReason);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PURCHASE_REASONES_CAN_VIEW)]
       
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var purchaseReason = await _service.GetPurchaseReason(id);
        if (purchaseReason == null)
        {
            return NotFound();
        }

        purchaseReason.EncryptedId = id;
        return View(purchaseReason);
    }

       
    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PURCHASE_REASONES_CAN_VIEW)]

    public async Task<IActionResult> Edit(string id, PurchaseReason purchaseReason)
    {
        int purchaseReasonsId = int.Parse(IvatDataProtector.Unprotect(id));
        if (purchaseReasonsId != purchaseReason.PurchaseReasonId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var deliveryDT = await _service.Query()
                    .SingleOrDefaultAsync(m => m.PurchaseReasonId == purchaseReasonsId, CancellationToken.None);
                var  prevObj= JsonConvert.SerializeObject(deliveryDT, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                deliveryDT.Reason = purchaseReason.Reason;
                _service.Update(deliveryDT);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(deliveryDT, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.PurchaseReason,
                    PrimaryKey = purchaseReason.PurchaseReasonId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions =GetChangeInformation(prevObj.ToString(),jObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };


                var au_status = await AuditLogCreate(au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
            }
            catch (Exception ex)
            {
	            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
            return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(purchaseReason);
    }

       
}