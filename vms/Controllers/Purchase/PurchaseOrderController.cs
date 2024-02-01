using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.ActionFilter;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.Models;
using vms.service.dbo;
using vms.Utility;
using X.PagedList;

namespace vms.Controllers.Purchase
{
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IMeasurementUnitService _measurementUnitService;
        private readonly IPurchaseTypeService _purchaseTypeService;
        private readonly IVendorService _vendorService;
        private readonly IPurchaseReasonService _purchaseReasonService;
        private readonly IPurchaseOrderDetailService _purchaseODService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOrganizationService _organizationService;
        private int organizationId;

        public PurchaseOrderController(IOrganizationService organizationService, IHostingEnvironment hostingEnvironment, ControllerBaseParamModel controllerBaseParamModel, IPurchaseReasonService purchaseReasonService, IPurchaseOrderService purchaseOrderService, IPurchaseOrderDetailService purchaseOrderDetailService, IMeasurementUnitService measurementUnitService, IPurchaseTypeService purchaseTypeService, IVendorService vendorService) : base(controllerBaseParamModel)
        {
            _purchaseOrderService = purchaseOrderService;
            _purchaseTypeService = purchaseTypeService;
            _purchaseODService = purchaseOrderDetailService;
            _measurementUnitService = measurementUnitService;
            _vendorService = vendorService;
            _purchaseReasonService = purchaseReasonService;
            _hostingEnvironment = hostingEnvironment;
            _organizationService = organizationService;
            organizationId = _session.OrganizationId;
        }

        public async System.Threading.Tasks.Task<IActionResult> Index(int? page, string search = null)
        {
            var getPurchase = await _purchaseOrderService.Query().Where(c => c.OrganizationId == organizationId).Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.Vendor).OrderByDescending(c => c.PurchaseId).SelectAsync(CancellationToken.None);
            if (search != null)
            {
                search = search.ToLower().Trim();
                getPurchase = getPurchase.Where(c => c.VendorInvoiceNo.ToLower().Contains(search)
                                               || c.InvoiceNo.ToLower().Contains(search)
                                               || c.NoOfIteams.ToString().Contains(search)
                                               || c.TotalVat.ToString().Contains(search)
                                               || c.TotalPriceWithoutVat.ToString().Contains(search)
                                               || c.ExpectedDeleveryDate.ToString().Contains(search)
                                               || c.DeliveryDate.ToString().Contains(search)
                                               || c.Organization.Name.ToLower().Contains(search)
                                               || c.PurchaseType.Name.ToLower().Contains(search)
                                               || c.Vendor.Name.ToLower().Contains(search)
                //|| ((c.IsCanceled == true) ? "Active" : "Canceled").ToString().Contains(search)
                );
                ViewData["searchText"] = search;
            }
            else
            {
                ViewData["searchText"] = "";
            }
            var pageNumber = page ?? 1;
            var listOfPurchase = getPurchase.ToPagedList(pageNumber, 10);

            return View(listOfPurchase);
        }

        public async System.Threading.Tasks.Task<IActionResult> PurchaseOrder()
        {
            var createdBy = _session.UserId;
            var organizationId = _session.OrganizationId;
            ViewData["PurchaseReasonId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _purchaseReasonService.Query().SelectAsync(), "PurchaseReasonId", "Reason");
            ViewData["VendorId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _vendorService.Query().Where(c => c.CreatedBy == createdBy).SelectAsync(), "VendorId", "Name");
            ViewData["PurchaseTypeId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _purchaseTypeService.Query().SelectAsync(), "PurchaseTypeId", "Name");
            ViewData["MeasurementUnitId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _measurementUnitService.Query().Where(c => c.OrganizationId == organizationId).SelectAsync(), "MeasurementUnitId", "Name");

            return View();
        }

        [TypeFilter(typeof(Logged), Arguments = new object[] { Operations.Add, ObjectTypeEnum.Purchase })]
        public async System.Threading.Tasks.Task<JsonResult> CreateAsync(vmPurchase vm)
        {
            var createdBy = _session.UserId;
            var organizationId = _session.OrganizationId;
            bool status = false;
            var File = vm.UploadFile;
            var FileExtenstion = Path.GetExtension(File.FileName);
            string FileName = Guid.NewGuid().ToString();
            FileName += FileExtenstion;
            vm.FileName = FileName;
            Organization organization = await _organizationService.Query().FirstOrDefaultAsync(c => c.OrganizationId == organizationId, CancellationToken.None);
            var FolderName = organization.Name;
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, FolderName);
            bool exists = Directory.Exists(uploads);
            if (!exists)
            {
                Directory.CreateDirectory(uploads);
            }

            if (File.Length > 0)
            {
                var filePath = Path.Combine(uploads, File.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(fileStream);
                }
            }
            if (vm.PurchaseOrderDetailList.Count > 0)
            {
                vm.CreatedBy = createdBy;
                vm.OrganizationId = organizationId;
                vmPurchaseOrder vmPurchase=new vmPurchaseOrder();
                vmPurchase.PurchaseOrderDetailList = vm.PurchaseOrderDetailList;
                vmPurchase.VendorId = vm.VendorId;
                vmPurchase.VendorInvoiceNo = vm.VendorInvoiceNo;
                vmPurchase.InvoiceNo = vm.InvoiceNo;
                vmPurchase.OrganizationId = vm.OrganizationId;
                vmPurchase.PurchaseTypeId = vm.PurchaseTypeId;
                vmPurchase.Discount = vm.Discount;
                vmPurchase.ExpectedDeleveryDate = vm.ExpectedDeleveryDate;
                vmPurchase.DeliveryDate = vm.DeliveryDate;
                vmPurchase.IsComplete = vm.IsComplete;
                vmPurchase.CreatedBy = vm.CreatedBy;
                vmPurchase.CreatedTime = vm.CreatedTime;
                vmPurchase.PurchaseReasonId = vm.PurchaseReasonId;
                status = await _purchaseOrderService.InsertData(vmPurchase);
            }

            return Json(status);
        }

        public async System.Threading.Tasks.Task<IActionResult> CreateForeignOrder()
        {
            var createdBy = _session.UserId;
            var organizationId = _session.OrganizationId;
            ViewData["PurchaseReasonId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _purchaseReasonService.Query().SelectAsync(), "PurchaseReasonId", "Reason");
            ViewData["VendorId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _vendorService.Query().Where(c => c.CreatedBy == createdBy).SelectAsync(), "VendorId", "Name");
            ViewData["PurchaseTypeId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _purchaseTypeService.Query().Where(c => c.PurchaseTypeId == 2).SelectAsync(), "PurchaseTypeId", "Name");
            ViewData["MeasurementUnitId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _measurementUnitService.Query().Where(c => c.OrganizationId == organizationId).SelectAsync(), "MeasurementUnitId", "Name");
            return View();
        }

        public async Task<IActionResult> Cancel(int? id)
        {
            try
            {
                var purchaseData = await _purchaseOrderService.Query().SingleOrDefaultAsync(p => p.PurchaseId == id, CancellationToken.None);
                //                purchaseData.IsCanceled = true;
                _purchaseOrderService.Update(purchaseData);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetPurchaseDetails(int purchaseId)
        {
            var purchaseDetails = await _purchaseODService.Query().Include(c => c.Product).Include(c => c.MeasurementUnit).Include(c => c.ProductVattype).Include(c => c.Purchase).Where(c => c.PurchaseId == purchaseId).SelectAsync(CancellationToken.None);
            return new JsonResult(purchaseDetails.Select(x => new
            {
                PurchaseDetailId = x.PurchaseDetailId,
                ProductId = x.ProductId,
                PurchaseId = x.PurchaseId,
                ProductVattypeId = x.ProductVattypeId,
                Iteams = x.Quantity,
                Amount = x.UnitPrice,
                Vat = x.Vatpercent,
                Unit = x.MeasurementUnit.Name,
                MeasurementUnitId = x.MeasurementUnitId,
                CreatedBy = x.CreatedBy,
                CreatedTime = x.CreatedTime,
                //                InitialAmount = x.InitialQty,
                ProductName = x.Product.Name
            }).ToList());
        }

        public async Task<JsonResult> InvoiceKeyWordSearch(string filterText)
        {
            var product = await _purchaseOrderService.Query().Where(c => c.InvoiceNo.Contains(filterText)).SelectAsync(CancellationToken.None);
            return new JsonResult(product.Select(x => new
            {
                Id = x.PurchaseId,
                InvoiceNo = x.InvoiceNo,
                UnitPrice = x.TotalPriceWithoutVat
            }).ToList());
        }

        public async Task<IActionResult> CancelAdd(int id)
        {
            var createdBy = _session.UserId;
            var organizationId = _session.OrganizationId;
            ViewData["PurchaseReasonId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _purchaseReasonService.Query().SelectAsync(), "PurchaseReasonId", "Reason");
            ViewData["VendorId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _vendorService.Query().Where(c => c.CreatedBy == createdBy).SelectAsync(), "VendorId", "Name");
            ViewData["PurchaseTypeId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _purchaseTypeService.Query().SelectAsync(), "PurchaseTypeId", "Name");
            ViewData["MeasurementUnitId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _measurementUnitService.Query().Where(c => c.OrganizationId == organizationId).SelectAsync(), "MeasurementUnitId", "Name");

            var purchaseList = await _purchaseOrderService.Query()
                .SingleOrDefaultAsync(c => c.PurchaseId == id, CancellationToken.None);
            return View(purchaseList);
        }

        public IActionResult AccountsView()
        {
            return View();
        }

        //public async System.Threading.Tasks.Task<IActionResult> Details(int id)
        //{
        //    entity.models.Purchase gePurchase = await _purchaseOrderService.Query().Include(c => c.Vendor).Include(c => c.Organization).Include(c => c.PurchaseType).Include(p => p.PurchaseReason).Include(c => c.PurchaseDetails).SingleOrDefaultAsync(c => c.PurchaseId == id, CancellationToken.None);
        //    return View(gePurchase);
        //}
        public async System.Threading.Tasks.Task<IActionResult> Details(int id)
        {
            var purchase = await _purchaseOrderService.Query().Include(c => c.Vendor).Include(c => c.Organization).Include(c => c.PurchaseType).Include(p => p.PurchaseReason).Include(c => c.PurchaseDetails).SingleOrDefaultAsync(c => c.PurchaseId == id, CancellationToken.None);
            var purchaseDetails = await _purchaseODService.Query().Include(sd => sd.Product).Include(sd => sd.MeasurementUnit).Where(sd => sd.PurchaseId == id).SelectAsync();
            var vm = new VmPurchaseDetail { PurchaseDetails = purchaseDetails, Purchase = purchase };
            return View("DetailsNew", vm);
        }
    }
}