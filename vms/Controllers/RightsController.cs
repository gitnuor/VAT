using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.models;
using vms.Utility;
using vms.utility.StaticData;
using vms.service.Services.SecurityService;

namespace vms.Controllers;

public class RightsController : ControllerBase
{
    private readonly IRightService _service;

    public RightsController(ControllerBaseParamModel controllerBaseParamModel, IRightService service) : base(controllerBaseParamModel)
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

        var right = await _service.Query()
            .SingleOrDefaultAsync(p => p.RightId == id, CancellationToken.None);
        if (right == null)
        {
            return NotFound();
        }

        return View(right);
    }

    public IActionResult Create()
    {
        return View();
    }

        
    [HttpPost]
         
    public async Task<IActionResult> Create( Right right)
    {
        if (ModelState.IsValid)
        {
            _service.Insert(right);
            await UnitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(right);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var right = await _service.Query().SingleOrDefaultAsync(p => p.RightId == id, CancellationToken.None);
        if (right == null)
        {
            return NotFound();
        }
        return View(right);
    }

        
    [HttpPost]
         
      
    public async Task<IActionResult> Edit(int id, Right right)
    {
        if (id != right.RightId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _service.Update(right);
                await UnitOfWork.SaveChangesAsync();
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RightExists(right.RightId))
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
        return View(right);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var right = await _service
            .Query()
            .Include(p => p.RightName)
            .SingleOrDefaultAsync(p => p.RightId == id, CancellationToken.None);
        if (right == null)
        {
            return NotFound();
        }

        return View(right);
    }

    [HttpPost, ActionName(ControllerStaticData.DELETE_CLASSNAME)]
         
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var right = await _service.Query().SingleOrDefaultAsync(p => p.RightId == id, CancellationToken.None);
        _service.Delete(right);
        await UnitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> RightExists(int id)
    {
        return await _service.Query().AnyAsync(e => e.RightId == id, CancellationToken.None);
    }
}