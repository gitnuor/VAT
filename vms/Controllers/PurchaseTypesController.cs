using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.models;
using vms.service.Services.SettingService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class PurchaseTypesController : ControllerBase
{
    private readonly IPurchaseTypeService _service;

    public PurchaseTypesController(ControllerBaseParamModel controllerBaseParamModel, IPurchaseTypeService service) : base(controllerBaseParamModel)
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

        var purchaseType = await _service.Query()
            .Include(p => p.Name)
            .SingleOrDefaultAsync(p => p.PurchaseTypeId == id, CancellationToken.None);
        if (purchaseType == null)
        {
            return NotFound();
        }

        return View(purchaseType);
    }

    public IActionResult Create()
    {
        return View();
    }

        
    [HttpPost]
         
    public async Task<IActionResult> Create( PurchaseType purchaseType)
    {
        if (ModelState.IsValid)
        {
            _service.Insert(purchaseType);
            await UnitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(purchaseType);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var purchaseType = await _service.Query().SingleOrDefaultAsync(p => p.PurchaseTypeId == id, CancellationToken.None);
        if (purchaseType == null)
        {
            return NotFound();
        }
        return View(purchaseType);
    }

        
    [HttpPost]
         
    public async Task<IActionResult> Edit(int id, PurchaseType purchaseType)
    {
        if (id != purchaseType.PurchaseTypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _service.Update(purchaseType);
                await UnitOfWork.SaveChangesAsync();
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PurchaseTypeExistsAsync(purchaseType.PurchaseTypeId))
                {
                    return NotFound();
                }

                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(purchaseType);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var purchaseType = await _service
            .Query()
            .Include(p => p.Name)
            .SingleOrDefaultAsync(p => p.PurchaseTypeId == id, CancellationToken.None);
        if (purchaseType == null)
        {
            return NotFound();
        }

        return View(purchaseType);
    }

    [HttpPost, ActionName(ControllerStaticData.DELETE_CLASSNAME)]
         
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var purchaseType = await _service.Query().SingleOrDefaultAsync(p => p.PurchaseTypeId == id, CancellationToken.None);
        _service.Delete(purchaseType);
        await UnitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> PurchaseTypeExistsAsync(int id)
    {
        return await _service.Query().AnyAsync(e => e.PurchaseTypeId == id, CancellationToken.None);
    }
}