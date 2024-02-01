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

public class DamageTypesController : ControllerBase
{

    private readonly IDamageTypeService  _service;
    public DamageTypesController(ControllerBaseParamModel controllerBaseParamModel, IDamageTypeService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }



        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var damageTypes =await _service.GetDamageTypes(UserSession.OrganizationId);
        damageTypes = damageTypes.OrderByDescending(x => x.DamageTypeId);
        return View(damageTypes);
    }

        

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var damageType = await _service.Query()
            .SingleOrDefaultAsync(m => m.DamageTypeId == id, CancellationToken.None);
        if (damageType == null)
        {
            return NotFound();
        }

        return View(damageType);
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_ADD)]
    public IActionResult Create()
    {

        return View();
    }


    [HttpPost]

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_ADD)]
    public async Task<IActionResult> Create(DamageType damageType)
    {
        if (ModelState.IsValid)
        {
                

            _service.Insert(damageType);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(damageType, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


                
            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.DamageType,
                PrimaryKey = damageType.DamageTypeId,
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
        return View(damageType);
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var damageType = await _service.GetDamageType(id);
        if (damageType == null)
        {
            return NotFound();
        }

        damageType.EncryptedId = id;
        return View(damageType);
    }
    [HttpPost]

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_EDIT)]
    public async Task<IActionResult> Edit( DamageType damageType, string id)
    {

        int damageTypeId = int.Parse(IvatDataProtector.Unprotect(id));
        if (damageTypeId != damageType.DamageTypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var damageData = await _service.Query()
                    .SingleOrDefaultAsync(m => m.DamageTypeId == damageTypeId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(damageData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                damageData.Name = damageType.Name;
                _service.Update(damageData);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(damageData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.DamageType,
                    PrimaryKey = damageType.DamageTypeId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = GetChangeInformation(prevObj.ToString(), jObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                var au_status = await AuditLogCreate(au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
                await UnitOfWork.SaveChangesAsync();
            }

			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(damageType);
    }

       
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_DAMAGE_TYPE_CAN_DELETE)]
    public async Task<IActionResult> Delete(int? id)
    {
        var damage = await _service.Query().SingleOrDefaultAsync(p => p.DamageTypeId == id, CancellationToken.None);

            
        _service.Update(damage);
        await UnitOfWork.SaveChangesAsync();
        AuditLog au = new AuditLog
        {
            ObjectTypeId = (int)EnumObjectType.DamageType,
            PrimaryKey = damage.DamageTypeId,
            AuditOperationId = (int)EnumOperations.Delete,
            UserId = UserSession.UserId,
            Datetime = DateTime.Now,
            Descriptions = "IsActive:0",
            IsActive = true,
            CreatedBy = UserSession.UserId,
            CreatedTime = DateTime.Now,
            OrganizationId = UserSession.OrganizationId
        };
        var au_status = await AuditLogCreate(au);
        return RedirectToAction(nameof(Index));
    }


}