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

public class DeliveryMethodsController : ControllerBase
{
    private readonly IDeliveryMethodService _service;
    public DeliveryMethodsController(ControllerBaseParamModel controllerBaseParamModel, IDeliveryMethodService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }



        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DELIVERY_METHOD_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var deliveryMethods = await _service.GetDeliveryMethods();
        deliveryMethods = deliveryMethods.OrderByDescending(x => x.DeliveryMethodId);
        return View(deliveryMethods);
          
    }
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DELIVERY_METHOD_CAN_VIEW)]
        
    public IActionResult Create()
    {
        return View();
    }

       
    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DELIVERY_METHOD_CAN_VIEW)]
        
    public async Task<IActionResult> Create(DeliveryMethod deliveryMethod)
    {
        if (ModelState.IsValid)
        {
            _service.Insert(deliveryMethod);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(deliveryMethod, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.DeliveryMethod,
                PrimaryKey = deliveryMethod.DeliveryMethodId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = jObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };
            await AuditLogCreate(au);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(deliveryMethod);
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DELIVERY_METHOD_CAN_VIEW)]
      
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var deliveryMethod = await _service.GetDeliveryMethod(id);
        if (deliveryMethod == null)
        {
            return NotFound();
        }
        deliveryMethod.EncryptedId = id;
        return View(deliveryMethod);
    }

    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DELIVERY_METHOD_CAN_VIEW)]
        
    public async Task<IActionResult> Edit(string id, DeliveryMethod deliveryMethod)
    {
        int DeliveryMethodId = int.Parse(IvatDataProtector.Unprotect(id));
        if (DeliveryMethodId != deliveryMethod.DeliveryMethodId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var deliveryDT = await _service.Query()
                    .SingleOrDefaultAsync(m => m.DeliveryMethodId == DeliveryMethodId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(deliveryDT, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                deliveryDT.Name = deliveryMethod.Name;
                _service.Update(deliveryDT);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(deliveryDT, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.DeliveryMethod,
                    PrimaryKey = deliveryMethod.DeliveryMethodId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = GetChangeInformation(prevObj.ToString(), jObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await AuditLogCreate(au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
			}
			catch (Exception ex)
			{
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
			return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(deliveryMethod);
    }
}