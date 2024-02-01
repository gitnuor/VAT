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
using vms.service.Services.UploadService;

namespace vms.Controllers;

public class ExportTypesController : ControllerBase
{

    private readonly IExportTypeService _service;
    public ExportTypesController(ControllerBaseParamModel controllerBaseParamModel, IExportTypeService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }


    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_EXPORT_TYPE_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var exportType = await _service.GetExportTypes();
        exportType = exportType.OrderByDescending(x => x.ExportTypeId);
        return View(exportType);

    }

   
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exportType = await _service.Query()
            .SingleOrDefaultAsync(m => m.ExportTypeId == id, CancellationToken.None);
        if (exportType == null)
        {
            return NotFound();
        }

        return View(exportType);
    }


    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_EXPORT_TYPE_CAN_VIEW)]

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_EXPORT_TYPE_CAN_VIEW)]
       
    public async Task<IActionResult> Create(ExportType exportType)
    {
        if (ModelState.IsValid)
        {
            exportType.CreatedBy = UserSession.UserId;
            exportType.CreatedTime = DateTime.Now;
             
            _service.Insert(exportType);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(exportType, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.ExportType,
                PrimaryKey = exportType.ExportTypeId,
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
        return View(exportType);
    }


    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_EXPORT_TYPE_CAN_VIEW)]
  
    public async Task<IActionResult> Edit(string id)
    {

        if (id == null)
        {
            return NotFound();
        }

        var exportType = await _service.GetExportType(id);
                
        if (exportType == null)
        {
            return NotFound();
        }

        exportType.EncryptedId = id;
        return View(exportType);
    }

       
    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_EXPORT_TYPE_CAN_VIEW)]
        
    public async Task<IActionResult> Edit(string id, ExportType exportType)
    {
        int exportTypeId = int.Parse(IvatDataProtector.Unprotect(id));
        if (exportTypeId != exportType.ExportTypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var exportDt = await _service.Query()
                    .SingleOrDefaultAsync(m => m.ExportTypeId == exportTypeId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(exportDt, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                exportDt.ExportTypeName = exportType.ExportTypeName;
                _service.Update(exportDt);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(exportDt, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.ExportType,
                    PrimaryKey = exportType.ExportTypeId,
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
        return View(exportType);
    }

      

}