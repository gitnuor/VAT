using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.SecurityService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class RolesController : ControllerBase
{
    private readonly IRoleService _service;
    private readonly IRightService _rightService;
    private readonly IRoleRightService _roleRightService;

    public RolesController(ControllerBaseParamModel controllerBaseParamModel, IRoleService service, IRightService rightService, IRoleRightService roleRightService) : base(controllerBaseParamModel)
    {
        _service = service;
        _rightService = rightService;
        _roleRightService = roleRightService;
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_VIEW)]
    public async Task<IActionResult> Index(int? page, string search = null)
    {
        var role = await _service.GetRoles(UserSession.OrganizationId);

        role.ToList().ForEach(delegate (Role rol)
        {
            rol.EncryptedId = IvatDataProtector.Protect(rol.RoleId.ToString());
                
        });

        return View(role);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var role = await _service.Query()
            .SingleOrDefaultAsync(p => p.RoleId == id, CancellationToken.None);

        if (role == null)
        {
            return NotFound();
        }

        return View(role);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_ADD)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_ADD)]
    public async Task<IActionResult> Create(Role role)
    {
        if (ModelState.IsValid)
        {
            role.CreatedTime = DateTime.Now;
            role.CreatedBy = UserSession.UserId;
            role.IsActive = true;
            role.OrganizationId = UserSession.OrganizationId;
            _service.Insert(role);
            await UnitOfWork.SaveChangesAsync();

            var jObj = JsonConvert.SerializeObject(role, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.Role,
                PrimaryKey = role.RoleId,
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

        return View(role);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_ASSIGN_RIGHTS)]
    public async Task<IActionResult> AssignRights(string Id)
    {
        int p_id = int.Parse(IvatDataProtector.Unprotect(Id));
        var role = await _service.Query().SingleOrDefaultAsync(p => p.RoleId == p_id, CancellationToken.None);
        var listOfrights = await _rightService.Query().Include(p => p.RoleRights).Include(p => p.RightCategory).SelectAsync();

        var data = listOfrights.Select(s => new RoleRightModel
        {
            RightCategoryId = s.RightCategory.RightCategoryId,
            RightId = s.RightId,
            RightName = s.RightName,
            RightCategoryName = s.RightCategory.RightCategoryName,
            IsAssigned = s.RoleRights.Any(w => w.RoleId == p_id && w.RightId == s.RightId)
        }).OrderBy(d => d.RightCategoryName);
        vmRoleRights vm = new vmRoleRights
        {
            RoleId = p_id,
            RoleFeatures = data.ToList()
        };

        return View(vm);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_ASSIGN_RIGHTS)]
    [RequestFormSizeLimit(valueCountLimit: 5000)]
    public async Task<IActionResult> AssignRights(vmRoleRights vm, string Id)
    {
        int UserId = int.Parse(IvatDataProtector.Unprotect(Id));
        var items = await _roleRightService.Query().Where(w => w.RoleId == UserId).SelectAsync();

        if (items.Count() > 0)
        {
            _roleRightService.DeleteObjectList(items.ToList());
            await UnitOfWork.SaveChangesAsync();
        }
        List<RoleRight> rolesRights = new List<RoleRight>();

        if (vm.cBox != null)
        {
            if (vm.cBox.Count() > 0)
            {
                foreach (var i in vm.cBox)
                {
                    RoleRight cp = new RoleRight
                    {
                        RoleId = vm.RoleId,
                        RightId = i,
                        CreatedBy = UserSession.UserId,
                        CreatedTime = DateTime.Now
                    };
                    rolesRights.Add(cp);
                }

                _roleRightService.InsertObjectList(rolesRights);
                await UnitOfWork.SaveChangesAsync();
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
                var jObj = JsonConvert.SerializeObject(vm.cBox, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.RoleRight,
                    PrimaryKey = vm.RoleId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = jObj.ToString(),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };

                await AuditLogCreate(au);
            }
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SAVED_CLASSNAME;
        return RedirectToAction(nameof(Index));
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _service.GetRole(id);
        if (role == null)
        {
            return NotFound();
        }

        role.EncryptedId = id;
        return View(role);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_ROLES_CAN_EDIT)]
    public async Task<IActionResult> Edit(Role role)
    {
        int roleId = int.Parse(IvatDataProtector.Unprotect(role.EncryptedId));
        role.OrganizationId = UserSession.OrganizationId;


        if (roleId != role.RoleId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var roleData = await _service.Query().SingleOrDefaultAsync(p => p.RoleId == roleId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(roleData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                roleData.RoleName = role.RoleName;
                roleData.IsActive = role.IsActive;
                _service.Update(roleData);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(roleData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.Role,
                    PrimaryKey = role.RoleId,
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
            catch (DbUpdateConcurrencyException)
            {
                if (!await RoleExists(role.RoleId))
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

        return View(role);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _service
            .Query()
            .Include(p => p.RoleName)
            .Include(p => p.RoleRights)
            .SingleOrDefaultAsync(p => p.RoleId == id, CancellationToken.None);
        if (role == null)
        {
            return NotFound();
        }

        return View(role);
    }

    [HttpPost, ActionName(ControllerStaticData.DELETE_CLASSNAME)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var role = await _service.Query().SingleOrDefaultAsync(p => p.RoleId == id, CancellationToken.None);
        _service.Delete(role);
        await UnitOfWork.SaveChangesAsync();

        var cusObj = JObject.Parse(JsonConvert.SerializeObject(role));

        AuditLog cu = new AuditLog();

        cu.ObjectTypeId = (int)EnumObjectType.Role;
        cu.PrimaryKey = role.RoleId;
        cu.AuditOperationId = 3;
        cu.UserId = UserSession.UserId;
        cu.Datetime = DateTime.Now;
        cu.Descriptions = cusObj.ToString();
        cu.IsActive = true;
        cu.CreatedBy = UserSession.UserId;
        cu.CreatedTime = DateTime.Now;
        cu.OrganizationId = UserSession.OrganizationId;

        var cus_status = await AuditLogCreate(cu);

        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME
            ;
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> RoleExists(int id)
    {
        return await _service.Query().AnyAsync(e => e.RoleId == id, CancellationToken.None);
    }
}