using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.Utility;
using vms.entity.viewModels;
using vms.utility.StaticData;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;
using vms.service.Services.ProductService;

namespace vms.Controllers;

public class MeasurementUnitsController : ControllerBase
{
    private readonly IMeasurementUnitService _service;
       
    public MeasurementUnitsController(ControllerBaseParamModel controllerBaseParamModel, IMeasurementUnitService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var organizationId = UserSession.ProtectedOrganizationId;
        var MeasurementUnits = await _service.GetMeasurementUnits(organizationId);
        MeasurementUnits.OrderByDescending(x => x.MeasurementUnitId);
        return View(MeasurementUnits);
    }

        
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var measurementUnit = await _service.Query()
            .SingleOrDefaultAsync(p => p.MeasurementUnitId == id, CancellationToken.None);

        if (measurementUnit == null)
        {
            return NotFound();
        }

        return View(measurementUnit);
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_ADD)]
    public IActionResult Create()
    {
        return View();
    }

        
    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_ADD)]
    public async Task<IActionResult> Create( MeasurementUnit measurementUnit)
    {
        if (ModelState.IsValid)
        {
            measurementUnit.CreatedBy = UserSession.UserId;
            measurementUnit.CreatedTime = DateTime.Now;
            measurementUnit.IsActive = true;
            measurementUnit.OrganizationId = UserSession.OrganizationId;
            _service.Insert(measurementUnit);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(measurementUnit, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.MeasurementUnit,
                PrimaryKey = measurementUnit.MeasurementUnitId,
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
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME
                ;
            return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(measurementUnit);
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var measurementUnit = await _service.GetMeasurementUnit(id);

        if (measurementUnit == null)
        {
            return NotFound();
        }

        measurementUnit.EncryptedId = id;
        return View(measurementUnit);
    }

       
    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_EDIT)]
    public async Task<IActionResult> Edit( MeasurementUnit measurementUnit, string id)
    {
        int measurementUnitId = int.Parse(IvatDataProtector.Unprotect(id));
        if (measurementUnitId != measurementUnit.MeasurementUnitId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var measurementUnits = await _service.Query().SingleOrDefaultAsync(p => p.MeasurementUnitId == measurementUnitId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(measurementUnits, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                measurementUnits.Name = measurementUnit.Name;

                _service.Update(measurementUnits);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(measurementUnit, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.MeasurementUnit,
                    PrimaryKey = measurementUnit.MeasurementUnitId,
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
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME
                    ;
            }

			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return RedirectToAction(nameof(Index));
        }
           
        return View(measurementUnit);
    }
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_DELETE)]
    public async Task<IActionResult> ChangeMeasurementUnitStatus(string id)
            
    {
        var msu = await _service.Query().SingleOrDefaultAsync(p => p.MeasurementUnitId == int.Parse(IvatDataProtector.Unprotect(id)), CancellationToken.None);

        if (msu.IsActive == true)
        {
            msu.IsActive = false;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;

        }
        else
        {
            msu.IsActive = true;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;

        }
        _service.Update(msu);
        await UnitOfWork.SaveChangesAsync();
          
        AuditLog au = new AuditLog
        {
            ObjectTypeId = (int)EnumObjectType.MeasurementUnit,
            PrimaryKey = msu.MeasurementUnitId,
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