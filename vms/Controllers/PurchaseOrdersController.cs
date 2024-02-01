using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using vms.service.dbo;
using vms.Utility;

namespace vms.Controllers
{
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        private readonly IOrganizationService _organizationService;

        public PurchaseOrdersController(ControllerBaseParamModel controllerBaseParamModel, IPurchaseOrderService service, IOrganizationService organizationService) : base(controllerBaseParamModel)
        {
            _service = service;
            _organizationService = organizationService;
        }

        // GET: PurchaseOrders
        public async Task<IActionResult> Index()
        {
            return View(await _service.Query().SelectAsync());
        }

        // GET: PurchaseOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _service.Query().SelectAsync();
            //.Include(p => p.Organization)
            // .SingleOrDefaultAsync(p => p.i == id, CancellationToken.None);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Create
        public async Task<IActionResult> Create()
        {
            ViewData["OrganizationId"] = new SelectList(await _organizationService.Query().SelectAsync(), "OrganizationId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Purchase purchaseOrder)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Insert(purchaseOrder);
        //        await UnitOfWork.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //   // ViewData["OrganizationId"] = new SelectList(await _organizationService.Query().SelectAsync(), "OrganizationId", "Name", purchaseOrder.OrganizationId);
        //    return View(purchaseOrder);
        //}

        // GET: PurchaseOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _service.Query().SelectAsync(); //SingleOrDefaultAsync(p => p.PurchaseOrderId == id, CancellationToken.None);
            if (purchaseOrder == null)
            {
                return NotFound();
            }
            //    ViewData["OrganizationId"] = new SelectList(await _organizationService.Query().SelectAsync(), "OrganizationId", "Name", purchaseOrder.OrganizationId);
            return View(purchaseOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, PurchaseOrder purchaseOrder)
        //{
        //    //if (id != purchaseOrder.PurchaseOrderId)
        //    //{
        //    //    return NotFound();
        //    //}

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _service.Update(purchaseOrder);
        //            await UnitOfWork.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            //if (!await PurchaseOrderExists(purchaseOrder.PurchaseOrderId))
        //            //{
        //            //    return NotFound();
        //            //}

        //            throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //  //  ViewData["OrganizationId"] = new SelectList(await _organizationService.Query().SelectAsync(), "OrganizationId", "Name", purchaseOrder.OrganizationId);
        //    return View(purchaseOrder);
        //}

        // GET: PurchaseOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _service
                .Query().SelectAsync();
            //.Include(p => p.Organization)
            //.Include(p => p.PurchaseOrderDetails)
            // .SingleOrDefaultAsync(p => p.PurchaseOrderId == id, CancellationToken.None);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseOrder = await _service.Query().SelectAsync(); //SingleOrDefaultAsync(p => p.PurchaseOrderId == id, CancellationToken.None);
            _service.Delete(purchaseOrder.First());
            await UnitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private async Task<bool> PurchaseOrderExists(int id)
        //{
        //    return await _service.Query().SelectAsync(); //AnyAsync(e => e.PurchaseOrderId == id, CancellationToken.None);
        //}
    }
}