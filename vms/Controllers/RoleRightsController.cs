using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.models;
using vms.service.Services.SecurityService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class RoleRightsController : ControllerBase
{
    private readonly IRoleRightService _service;
    private readonly IRoleService _roleService;

    public RoleRightsController(ControllerBaseParamModel controllerBaseParamModel, IRoleRightService service, IRoleService roleService) : base(controllerBaseParamModel)
    {
        _service = service;
        _roleService = roleService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _service.Query().Include(p => p.Role).SelectAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roleRight = await _service.Query()
            .SingleOrDefaultAsync(p => p.RoleRightId == id, CancellationToken.None);
        if (roleRight == null)
        {
            return NotFound();
        }

        return View(roleRight);
    }

    public async Task<IActionResult> Create()
    {
        ViewData[ControllerStaticData.ROLE_ID] = new SelectList(await _roleService.Query().SelectAsync(), ControllerStaticData.ROLE_ID, ControllerStaticData.ROLE_NAME);

        return View();
    }

        
    [HttpPost]
         
    public async Task<IActionResult> Create( RoleRight roleRight)
    {
        if (ModelState.IsValid)
        {
            _service.Insert(roleRight);
            await UnitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData[ControllerStaticData.ROLE_ID] = new SelectList(await _roleService.Query().SelectAsync(), ControllerStaticData.ROLE_ID, ControllerStaticData.ROLE_NAME);
        return View(roleRight);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roleRight = await _service.Query().SingleOrDefaultAsync(p => p.RoleRightId == id, CancellationToken.None);
        if (roleRight == null)
        {
            return NotFound();
        }

        ViewData[ControllerStaticData.ROLE_ID] = new SelectList(await _roleService.Query().SelectAsync(), ControllerStaticData.ROLE_ID, ControllerStaticData.ROLE_NAME);
        return View(roleRight);
    }

        
    [HttpPost]
         
    public async Task<IActionResult> Edit(int id, RoleRight roleRight)
    {
        if (id != roleRight.RoleRightId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _service.Update(roleRight);
                await UnitOfWork.SaveChangesAsync();
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RoleRightExists(roleRight.RoleRightId))
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
        ViewData[ControllerStaticData.ROLE_ID] = new SelectList(await _roleService.Query().SelectAsync(), ControllerStaticData.ROLE_ID, ControllerStaticData.ROLE_NAME);
        return View(roleRight);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roleRight = await _service
            .Query()
            .Include(p => p.Role)
            .SingleOrDefaultAsync(p => p.RoleRightId == id, CancellationToken.None);
        if (roleRight == null)
        {
            return NotFound();
        }

        return View(roleRight);
    }

    [HttpPost, ActionName(ControllerStaticData.DELETE_CLASSNAME)]
         
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var roleRight = await _service.Query().SingleOrDefaultAsync(p => p.RoleRightId == id, CancellationToken.None);
        _service.Delete(roleRight);
        await UnitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> RoleRightExists(int id)
    {
        return await _service.Query().AnyAsync(e => e.RoleRightId == id, CancellationToken.None);
    }
}