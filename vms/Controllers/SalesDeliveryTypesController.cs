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

public class SalesDeliveryTypesController : ControllerBase
{
    private readonly ISalesDeliveryTypeService _service;

    public SalesDeliveryTypesController(ControllerBaseParamModel controllerBaseParamModel, ISalesDeliveryTypeService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_SALES_DELIVERY_TYPE_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var salesDeliveryTypes = await _service.GetSalesDeliveryTypes();
        salesDeliveryTypes = salesDeliveryTypes.OrderByDescending(x => x.SalesDeliveryTypeId);
        return View(salesDeliveryTypes);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var salesDeliveryType = await _service.Query()
            .SingleOrDefaultAsync(m => m.SalesDeliveryTypeId == id, CancellationToken.None);
        if (salesDeliveryType == null)
        {
            return NotFound();
        }

        return View(salesDeliveryType);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_SALES_DELIVERY_TYPE_CAN_VIEW)]

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_SALES_DELIVERY_TYPE_CAN_VIEW)]

    public async Task<IActionResult> Create( SalesDeliveryType salesDeliveryType)
    {
        if (ModelState.IsValid)
        {
            _service.Insert(salesDeliveryType);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(salesDeliveryType, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.SalesDeliveryType,
                PrimaryKey = salesDeliveryType.SalesDeliveryTypeId,
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
        return View(salesDeliveryType);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_SALES_DELIVERY_TYPE_CAN_VIEW)]

    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var salesDeliveryType = await _service.GetSalesDeliveryType(id);
        if (salesDeliveryType == null)
        {
            return NotFound();
        }

        salesDeliveryType.EncryptedId = id;
        return View(salesDeliveryType);
    }


    [HttpPost]

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_SALES_DELIVERY_TYPE_CAN_VIEW)]

    public async Task<IActionResult> Edit(string id, SalesDeliveryType salesDeliveryType)
    {
        int salesDeliveryTypeId = int.Parse(IvatDataProtector.Unprotect(id));
        if (salesDeliveryTypeId != salesDeliveryType.SalesDeliveryTypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var deliveryDT = await _service.Query()
                    .SingleOrDefaultAsync(m => m.SalesDeliveryTypeId == salesDeliveryTypeId, CancellationToken.None);
                var prevJObj = JsonConvert.SerializeObject(deliveryDT, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                deliveryDT.Name = salesDeliveryType.Name;
                deliveryDT.Description = salesDeliveryType.Description;
                _service.Update(deliveryDT);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(salesDeliveryType, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.SalesDeliveryType,
                    PrimaryKey = salesDeliveryType.SalesDeliveryTypeId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions =GetChangeInformation(prevJObj.ToString(), jObj.ToString()),
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
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME
            ;
        return View(salesDeliveryType);
    }

}