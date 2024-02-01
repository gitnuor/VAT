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
using vms.service.Services.PaymentService;

namespace vms.Controllers;

public class PaymentMethodsController : ControllerBase
{

    private readonly IPaymentMethodService _service;
    public PaymentMethodsController(ControllerBaseParamModel controllerBaseParamModel, IPaymentMethodService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PAYMENT_METHODS_CAN_VIEW)]
    public async Task<IActionResult> Index(int? page, string search = null)
    {
        var paymentMethods = await _service.GetPaymentMethods();
        paymentMethods = paymentMethods.OrderByDescending(x => x.PaymentMethodId);
        return View(paymentMethods);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paymentMethod = await _service.Query()
            .SingleOrDefaultAsync(m => m.PaymentMethodId == id, CancellationToken.None);
        if (paymentMethod == null)
        {
            return NotFound();
        }

        return View(paymentMethod);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PAYMENT_METHODS_CAN_VIEW)]
       
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PAYMENT_METHODS_CAN_VIEW)]
      
    public async Task<IActionResult> Create( PaymentMethod paymentMethod)
    {
        if (ModelState.IsValid)
        {
            _service.Insert(paymentMethod);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(paymentMethod, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.PaymentMethod,
                PrimaryKey = paymentMethod.PaymentMethodId,
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
        return View(paymentMethod);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PAYMENT_METHODS_CAN_VIEW)]
   
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paymentMethod = await _service.GetPaymentMethod(id);
        if (paymentMethod == null)
        {
            return NotFound();
        }

        paymentMethod.EncryptedId = id;
        return View(paymentMethod);
    }

        
    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PAYMENT_METHODS_CAN_VIEW)]
      
    public async Task<IActionResult> Edit(string id, PaymentMethod paymentMethod)
    {
        int paymentMethodsId = int.Parse(IvatDataProtector.Unprotect(id));
        if (paymentMethodsId != paymentMethod.PaymentMethodId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var payment = await _service.Query()
                    .SingleOrDefaultAsync(m => m.PaymentMethodId == paymentMethodsId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(payment, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                payment.Name = paymentMethod.Name;
                _service.Update(payment);
                var jObj = JsonConvert.SerializeObject(payment, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.PaymentMethod,
                    PrimaryKey = paymentMethod.PaymentMethodId,
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
                await UnitOfWork.SaveChangesAsync();
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
            }

			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(paymentMethod);
    }

}