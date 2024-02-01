using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.TransactionService;
using vms.utility.StaticData;
using vms.Utility;
using vmProductionReceive = vms.entity.viewModels.vmProductionReceive;
namespace vms.Controllers;

public class ProductionContractualController : ControllerBase
{
    private readonly IMeasurementUnitService _measurementUnitService;
    private readonly IProductionService _productionService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IOrganizationService _organizationService;
    private readonly IVendorService _vendorService;
    private readonly IContractVendorService _contractVendorService;
    private readonly IContractVendorProductDetailsService _contractVendorProductDetails;
    private readonly IContractVendorTransferRawMaterialService _rawMaterialService;
    private readonly IContractVendorTransferRawMaterialDetailsService _rawMaterialDetailsService;
    private readonly ICustomerService _customerService;
    private readonly IContractTypeService _contractTypeService;
    public ProductionContractualController(IContractTypeService contractTypeService, ICustomerService customerService, IContractVendorTransferRawMaterialDetailsService rawMaterialDetailsService, IContractVendorTransferRawMaterialService rawMaterialService, IContractVendorProductDetailsService contractVendorProductDetails, IContractVendorService contractVendorService, IVendorService vendorService, IWebHostEnvironment hostingEnvironment, IOrganizationService organizationService, ControllerBaseParamModel controllerBaseParamModel, IMeasurementUnitService measurementUnitService, IProductionService productionService) : base(controllerBaseParamModel)
    {
        _contractTypeService = contractTypeService;
        _customerService = customerService;
        _rawMaterialService = rawMaterialService;
        _rawMaterialDetailsService = rawMaterialDetailsService;
        _contractVendorProductDetails = contractVendorProductDetails;
        _contractVendorService = contractVendorService;
        _vendorService = vendorService;
        _measurementUnitService = measurementUnitService;
        _productionService = productionService;
        _hostingEnvironment = hostingEnvironment;
        _organizationService = organizationService;
    }
    [SessionExpireFilter]

    [VmsAuthorize(FeatureList.PRODUCTION)]
    [VmsAuthorize(FeatureList.PRODUCTION_PRODUCTION_LIST_CAN_VIEW)]
    public async Task<IActionResult> Index(int? page, string search = null)
    {
        var productionList = await _contractVendorService.GetContractualProductions(UserSession.OrganizationId);
        productionList = productionList.OrderByDescending(x => x.ContractualProductionId);
        TempData[ControllerStaticData.MESSAGE] = "";
        return View(productionList);
    }

    [VmsAuthorize(FeatureList.PRODUCTION)]
    [VmsAuthorize(FeatureList.PRODUCTION_PRODUCTION_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCTION_PRODUCTION_LIST_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(await _measurementUnitService.Query().SelectAsync(), "MeasurementUnitId", "Name");
        ViewData[ControllerStaticData.PRODUCTION_ID] = new SelectList(await _productionService.Query().SelectAsync(), ControllerStaticData.PRODUCTION_ID, ControllerStaticData.WORK_ORDER_ID);
        ViewData["VendorId"] = new SelectList(await _vendorService.Query().SelectAsync(), "VendorId", "Name");
        ViewData["ContractTypeId"] = new SelectList(await _contractTypeService.Query().SelectAsync(), "ContractTypeId", "Name");

        ViewData[ControllerStaticData.CUSTOMER] = new SelectList(await _customerService.Query().Where(c => c.IsActive == true && c.OrganizationId == UserSession.OrganizationId).SelectAsync(), "CustomerId", "Name");
        return View();
    }

    public async Task<FileSaveFeedbackDto> FileSaveAsync(IFormFile File)
    {
        var organizationId = UserSession.OrganizationId;
        FileSaveFeedbackDto fdto = new FileSaveFeedbackDto();
        var FileExtenstion = Path.GetExtension(File.FileName);

        string FileName = Guid.NewGuid().ToString();

        FileName += FileExtenstion;
        Organization organization = await _organizationService.Query().FirstOrDefaultAsync(c => c.OrganizationId == organizationId, CancellationToken.None);
        var FolderName = ControllerStaticData.APPLICATION_DOCUMENT + organization.OrganizationId;
        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, FolderName);

        fdto.MimeType = FileExtenstion;
        bool exists = Directory.Exists(uploads);
        if (!exists)
        {
            Directory.CreateDirectory(uploads);
        }
        if (File.Length > 0)
        {
            var filePath = Path.Combine(uploads, File.FileName);
            fdto.FileUrl = filePath;
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(fileStream);
            }
        }
        return fdto;
    }

    [HttpPost]
    public async Task<JsonResult> CreateAsync(Models.vmProductionReceive vmProduction)
    {
        var createdBy = UserSession.UserId;
        var organizationId = UserSession.OrganizationId;
        string status = "";
        if (vmProduction.ContentInfoJson != null)
        {
            Content content;
            foreach (var contentInfo in vmProduction.ContentInfoJson)
            {
                content = new Content();
                vmProduction.ContentInfoJsonTest = new List<Content>();
                var File = contentInfo.UploadFile;
                var FileSaveFeedbackDto = await FileSaveAsync(File);
                content.FileUrl = FileSaveFeedbackDto.FileUrl;
                content.MimeType = FileSaveFeedbackDto.MimeType;
                content.DocumentTypeId = contentInfo.DocumentTypeId;
                vmProduction.ContentInfoJsonTest.Add(content);
            }
        }
        if (vmProduction.ProductionReceiveDetailList.Count > 0)
        {
            vmProduction.CreatedBy = createdBy;
            vmProduction.OrganizationId = organizationId;
            vmProductionReceive productionReceive = new vmProductionReceive();
            productionReceive.BatchNo = vmProduction.BatchNo;
            productionReceive.OrganizationId = vmProduction.OrganizationId;
            productionReceive.ProductionId = 1;
            productionReceive.ProductId = vmProduction.ProductId;
            productionReceive.ReceiveQuantity = vmProduction.ReceiveQuantity;
            productionReceive.MeasurementUnitId = vmProduction.MeasurementUnitId;
            productionReceive.ReceiveTime = vmProduction.ReceiveTime;
            productionReceive.CreatedBy = createdBy;
            productionReceive.CreatedTime = DateTime.Now;

            productionReceive.ProductionReceiveDetailList = vmProduction.ProductionReceiveDetailList;
            productionReceive.ContentInfoJson = vmProduction.ContentInfoJsonTest;
            status = await _productionService.InsertData(productionReceive);
        }

        if (status.Equals("Successful"))
        {
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        }

        else
        {
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        }

        return Json(status);
    }

    public async Task<JsonResult> CreateContractVendor(vmContractVendor contract)
    {
        var createdBy = UserSession.UserId;
        var organizationId = UserSession.OrganizationId;
        bool status = false;
        ContractualProduction contractVendor = new ContractualProduction();
        contractVendor.OrganizationId = organizationId;
        contractVendor.CreatedBy = createdBy;
        contractVendor.ContractNo = contract.ContractNo;
        contractVendor.VendorId = contract.VendorId;
        contractVendor.ContractDate = contract.ContractDate;
        contractVendor.ClosingDate = contract.ClosingDate;
        contractVendor.CreatedTime = DateTime.Now;
        contractVendor.ChallanNo = contract.ChallanNo;
        contractVendor.IssueDate = contract.IssueDate;
        contractVendor.CustomerId = contract.CustomerId;
        contractVendor.ContractTypeId = contract.ContractTypeId;
        _contractVendorService.Insert(contractVendor);
        await UnitOfWork.SaveChangesAsync();
        if (contractVendor.ContractualProductionId > 0)
        {
            foreach (var item in contract.ContractVendorProductDetailses)
            {
                ContractualProductionProductDetail detail = new ContractualProductionProductDetail();
                detail.ContractualProductionId = contractVendor.ContractualProductionId;
                detail.ProductId = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.MeasurementUnitId = item.MeasurementUnitId;
                _contractVendorProductDetails.Insert(detail);
            }
            await UnitOfWork.SaveChangesAsync();
            status = true;

            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        }
        else
        {
            status = false;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        }

        return Json(status);
    }
    [VmsAuthorize(FeatureList.PRODUCTION)]
    [VmsAuthorize(FeatureList.PRODUCTION_CONTRACTUAL_PRODUCTION_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCTION_CONTRACTUAL_PRODUCTION_CAN_ADD_TRANSFER)]
    public async Task<IActionResult> TransferContract(string id)
    {
        var data = await _contractVendorService.GetTransferContract(id);

        ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] = new SelectList(await _measurementUnitService.Query().SelectAsync(), "MeasurementUnitId", "Name");

        data.EncryptedId = id;
        return View(data);
    }

    [VmsAuthorize(FeatureList.PRODUCTION)]
    [VmsAuthorize(FeatureList.PRODUCTION_CONTRACTUAL_PRODUCTION_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCTION_CONTRACTUAL_PRODUCTION_CAN_ADD_RAW)]
    public async Task<JsonResult> TransferRawMaterial(vmTransferRawMaterial vm)
    {
        var createdBy = UserSession.UserId;
        var organizationId = UserSession.OrganizationId;
        bool status = false;
        ContractualProductionTransferRawMaterial model = new ContractualProductionTransferRawMaterial();
        model.ContractualProductionId = vm.ContractualProductionId;
        model.TransfereDate = vm.TransfereDate;
        model.Location = vm.Location;
        model.ChallanNo = vm.ChallanNo;
        model.ChallanIssueDate = vm.ChallanIssueDate;
        model.CreatedBy = UserSession.UserId;
        model.CreatedDate = DateTime.Now;
        _rawMaterialService.Insert(model);
        await UnitOfWork.SaveChangesAsync();
        if (model.ContractualProductionTransferRawMaterialId > 0)
        {
            foreach (var item in vm.Details)
            {
                ContractualProductionTransferRawMaterialDetail detail = new ContractualProductionTransferRawMaterialDetail();
                detail.ContractualProductionTransferRawMaterialId = model.ContractualProductionTransferRawMaterialId;
                detail.RawMaterialId = item.RawMaterialId;
                detail.Quantity = item.Quantity;
                detail.MeasurementUnitId = item.MeasurementUnitId;
                _rawMaterialDetailsService.Insert(detail);
            }
            await UnitOfWork.SaveChangesAsync();
            status = true;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        }
        else
        {
            status = false;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        }

        return Json(status);
    }
    [VmsAuthorize(FeatureList.PRODUCTION)]
    [VmsAuthorize(FeatureList.PRODUCTION_CONTRACTUAL_PRODUCTION_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCTION_CONTRACTUAL_PRODUCTION_CAN_ADD_RAW)]
    public async Task<IActionResult> TransferedRawMaterials(string id)
    {

        var data = await _rawMaterialService.GetTransferedRawMaterials(id);
        return View(data);
    }
    [VmsAuthorize(FeatureList.PRODUCTION)]
    [VmsAuthorize(FeatureList.PRODUCTION_TRANSFER_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCTION_TRANSFER_LIST_CAN_VIEW_DETAILS)]
    public async Task<IActionResult> TransferRawMaterialList()
    {
        var productionList = await _rawMaterialService.Query().SelectAsync(CancellationToken.None);
        productionList.ToList().ForEach(delegate (ContractualProductionTransferRawMaterial pur)
        {
            pur.EncryptedId = IvatDataProtector.Protect(pur.ContractualProductionTransferRawMaterialId.ToString());
        });
        return View(productionList);
    }
    public async Task<IActionResult> RawMaterialsDetails(string id, string fromdate, string todate)
    {
        int value = int.Parse(IvatDataProtector.Unprotect(id));
        var productionList = await _rawMaterialDetailsService.Query().Include(c => c.RawMaterial).Include(c => c.MeasurementUnit)
            .Where(c => c.ContractualProductionTransferRawMaterialId == value).SelectAsync(CancellationToken.None);
        productionList.ToList().ForEach(delegate (ContractualProductionTransferRawMaterialDetail pur)
        {
            pur.FromDateEn = fromdate;
            pur.TodateEn = todate;
            pur.EncryptedId = IvatDataProtector.Protect(id);
        });
        return View(productionList);
    }
}