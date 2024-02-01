using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vms.entity.models;
using vms.Utility;
using vms.utility.StaticData;
using Newtonsoft.Json;
using vms.entity.Enums;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class OrgBranchTypeController : ControllerBase
{

    private readonly IOrgBranchTypeService _service;
    public OrgBranchTypeController(ControllerBaseParamModel controllerBaseParamModel, IOrgBranchTypeService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }



    public async Task<IActionResult> Index()
    {
        var OrgBranchType = await _service.GetOrgBranchTypeList();
        return View(OrgBranchType);
    }



    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var OrgBranchType = await _service.Query().SingleOrDefaultAsync(m => m.OrgBranchTypeId == id, CancellationToken.None);
        if (OrgBranchType == null)
        {
            return NotFound();
        }

        return View(OrgBranchType);
    }

        

    public IActionResult Create()
    {
           
        return View();
    }

    [HttpPost]
         

    public async Task<IActionResult> Create(OrgBranchType orgBranchType)
    {
        if (ModelState.IsValid)
        {

            _service.Insert(orgBranchType);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(orgBranchType, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.OrgBranchType,
                PrimaryKey = orgBranchType.OrgBranchTypeId,
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
        return View(orgBranchType);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orgBranchType = await _service.Query().SingleOrDefaultAsync(m => m.OrgBranchTypeId == id, CancellationToken.None);

        if (orgBranchType == null)
        {
            return NotFound();
        }
        return View(orgBranchType);
    }

    [HttpPost]
         
    public async Task<IActionResult> Edit(OrgBranchType orgBranchTypeModel)
    {

        var orgBranchType = await _service.Query()
            .SingleOrDefaultAsync(m => m.OrgBranchTypeId == orgBranchTypeModel.OrgBranchTypeId, CancellationToken.None);
        if (orgBranchType == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var prevObj = JsonConvert.SerializeObject(orgBranchType, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                orgBranchType.BranchTypeName = orgBranchTypeModel.BranchTypeName;
                orgBranchType.BranchTypeNameBangla = orgBranchTypeModel.BranchTypeNameBangla;
                _service.Update(orgBranchType);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(orgBranchType, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.OrgBranchType,
                    PrimaryKey = orgBranchType.OrgBranchTypeId,
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
	            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
            return RedirectToAction(nameof(Index));
        }
          
        return View(orgBranchTypeModel);
    }
       
}