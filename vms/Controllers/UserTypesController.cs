using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.service.Services.SecurityService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class UserTypesController : ControllerBase
{
    private readonly IUserTypeService _service;

    public UserTypesController(ControllerBaseParamModel controllerBaseParamModel, IUserTypeService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _service.Query().SelectAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userType = await _service.Query()
            .SingleOrDefaultAsync(p => p.UserTypeId == id, CancellationToken.None);
        if (userType == null)
        {
            return NotFound();
        }

        return View(userType);
    }

    public IActionResult Create()
    {
        return View();
    }

        
    [HttpPost]
         
    public async Task<IActionResult> Create( UserType userType)
    {
        if (ModelState.IsValid)
        {
            _service.Insert(userType);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(userType, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

               
            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.UserType,
                PrimaryKey = userType.UserTypeId,
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
            return RedirectToAction(nameof(Index));
        }
        return View(userType);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userType = await _service.Query().SingleOrDefaultAsync(p => p.UserTypeId == id, CancellationToken.None);
        if (userType == null)
        {
            return NotFound();
        }
        return View(userType);
    }

        
    [HttpPost]
         
    public async Task<IActionResult> Edit(int id, UserType userType)
    {
        if (id != userType.UserTypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _service.Update(userType);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(userType, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.UserType,
                    PrimaryKey = userType.UserTypeId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = jObj.ToString(),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };


                var au_status = await AuditLogCreate(au);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserTypeExists(userType.UserTypeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(userType);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userType = await _service
            .Query()
                
            .SingleOrDefaultAsync(p => p.UserTypeId == id, CancellationToken.None);
        if (userType == null)
        {
            return NotFound();
        }

        return View(userType);
    }

    [HttpPost, ActionName(ControllerStaticData.ERROR_CLASSNAME)]
         
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userType = await _service.Query().SingleOrDefaultAsync(p => p.UserTypeId == id, CancellationToken.None);
        _service.Delete(userType);
        await UnitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> UserTypeExists(int id)
    {
        return await _service.Query().AnyAsync(e => e.UserTypeId == id, CancellationToken.None);
    }
}