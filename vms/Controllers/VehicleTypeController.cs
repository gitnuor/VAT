using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using vms.Utility;
using vms.entity.models;
using Newtonsoft.Json;
using vms.entity.Enums;
using vms.utility.StaticData;
using Newtonsoft.Json.Linq;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class VehicleTypeController : ControllerBase
{
    private readonly IVehicleTypeService _vehicleTypeService;
    public VehicleTypeController(ControllerBaseParamModel controllerBaseParamModel,IVehicleTypeService vehicleTypeService):base(controllerBaseParamModel)
    {
        _vehicleTypeService = vehicleTypeService;
    }
                

    public async Task<IActionResult> Index()
    {
        var types = await _vehicleTypeService.GetVehicleTypes(UserSession.OrganizationId);
          
        return View(types);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(VehicleType vehicleType)
    {
        if (ModelState.IsValid)
        {
            try
            {
                vehicleType.CreatedTime = DateTime.Now;
                vehicleType.CreatedBy = UserSession.UserId;
                vehicleType.OrganizationId = UserSession.OrganizationId;
                _vehicleTypeService.Insert(vehicleType);
                await UnitOfWork.SaveChangesAsync();

                var jObj = JObject.Parse(JsonConvert.SerializeObject(vehicleType));

                AuditLog au = new AuditLog();
                au.ObjectTypeId = (int)EnumObjectType.Vendor;
                au.PrimaryKey = vehicleType.VehicleTypeId;
                au.AuditOperationId = 1;
                au.UserId = UserSession.UserId;
                au.Datetime = DateTime.Now;
                au.Descriptions = jObj.ToString();
                au.IsActive = true;
                au.CreatedBy = UserSession.UserId;
                au.CreatedTime = DateTime.Now;
                au.OrganizationId = UserSession.OrganizationId;

                var status = await AuditLogCreate(au);

                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
                return View(vehicleType);
            }
        }
        else
        {
            return View(vehicleType);
        }    

    }

    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var type = await _vehicleTypeService.GetVehicleType(id);
            if (type is null)
            {
                return NotFound();
            }
            return View(type);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(VehicleType model)
    {
        if(ModelState.IsValid)
        {
            var type = await _vehicleTypeService.GetVehicleType(model.EncryptedId);
            try
            {
                //var vendorData = await _vehicleTypeService.Query().SingleOrDefaultAsync(p => p.VehicleTypeId == vendorId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(type, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                type.VehicleTypeName = model.VehicleTypeName;
                type.Description = model.Description;
                type.IsRequireRegistration = model.IsRequireRegistration;
                type.IsActive = model.IsActive;
                                        
                _vehicleTypeService.Update(type);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(type, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.Vendor,
                    PrimaryKey = type.VehicleTypeId,
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
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ChangeVehicleTypeStatus(string id)
    {
        try
        {

            var type = await _vehicleTypeService.GetVehicleType(id);

            if (type is not null)
            {
                if (type.IsActive == true)
                {
                    type.IsActive = false;
                    TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;

                }
                else
                {
                    type.IsActive = true;
                    TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;
                }

                _vehicleTypeService.Update(type);
                await UnitOfWork.SaveChangesAsync();
                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.User,
                    PrimaryKey = type.VehicleTypeId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = "IsActive:0",
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await AuditLogCreate(au);
            }
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return NotFound();
        }
                        
    }

    public async Task<IActionResult> Details(string id)
    {
        try
        {
            var type = await _vehicleTypeService.GetVehicleType(id);
            return View(type);
        }
        catch
        {
            return NotFound();
        }
            
    }
}