using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using vms.entity.models;
using vms.entity.viewModels;
using vms.Utility;
using vms.utility.StaticData;
using vms.service.Services.TransactionService;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.service.Services.UploadService;

namespace vms.Controllers;

public class ProductionController : ControllerBase
{
	private readonly IMeasurementUnitService _measurementUnitService;
	private readonly IProductionService _productionService;
	private readonly IProductionDetailService _productionDetailService;
	private readonly IHostEnvironment _hostingEnvironment;
	private readonly IOrganizationService _organizationService;
	private readonly IContractVendorService _contractVendor;
	private readonly IDocumentTypeService _documentType;
	private readonly IProductService _productService;
	private readonly IContractVendorService _contractVendorService;
	private readonly IOrgBranchService _branchService;

	public ProductionController(IDocumentTypeService documentType, IContractVendorService contractVendor,
		IHostEnvironment hostingEnvironment, IOrganizationService organizationService,
		ControllerBaseParamModel controllerBaseParamModel, IMeasurementUnitService measurementUnitService,
		IProductionService productionService, IProductionDetailService productionDetailService
		, IProductService productService,
		IContractVendorService contractVendorService, IOrgBranchService branchService) : base(controllerBaseParamModel)
	{
		_documentType = documentType;
		_contractVendor = contractVendor;
		_measurementUnitService = measurementUnitService;
		_productionService = productionService;
		_productionDetailService = productionDetailService;
		_hostingEnvironment = hostingEnvironment;
		_organizationService = organizationService;
		_productService = productService;
		_contractVendorService = contractVendorService;
		_branchService = branchService;
	}

	[VmsAuthorize(FeatureList.PRODUCTION)]
	public async Task<IActionResult> Index(int? page, string search = null)
	{
		//var productionList = await _productionService.Query()
		//	.Where(ps => ps.OrganizationId == UserSession.OrganizationId).Include(c => c.Organization)
		//	.Include(c => c.Product)
		//	.Include(c => c.MeasurementUnit)
		//	.Include(c => c.BillOfMaterials).SelectAsync(CancellationToken.None);

		//productionList = productionList.OrderByDescending(x => x.ProductionReceiveId);
		var productionList = await _productionService.ViewProductionReceiveByOrgAndBranch(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);

        return View(productionList);
	}

	[VmsAuthorize(FeatureList.PRODUCTION)]
	[VmsAuthorize(FeatureList.PRODUCTION_PRODUCTION_LIST_CAN_ADD)]
	public async Task<IActionResult> Create()
	{
		ViewData[ViewStaticData.MEASUREMENT_UNIT_ID] =
			new SelectList(await _measurementUnitService.Query().SelectAsync(), "MeasurementUnitId", "Name");
		ViewData[ControllerStaticData.PRODUCTION_ID] = new SelectList(await _productionService.Query().SelectAsync(),
			ControllerStaticData.PRODUCTION_ID, ControllerStaticData.WORK_ORDER_ID);
		ViewData[ControllerStaticData.DOCUMENT_TYPE_ID] = new SelectList(await _documentType.Query().SelectAsync(),
			ControllerStaticData.DOCUMENT_TYPE_ID, "Name");

		return View();
	}


	public async Task<IActionResult> SelfProduction()
	{
		var organizationId = UserSession.ProtectedOrganizationId;
		var model = new VmSelfProduction();
		model.ProductSelectList = _productService.SpGetProductForSelfProductionReceive(UserSession.OrganizationId);
		model.BranchSelectList = await _branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch);
		//model.MeasurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(organizationId);
		model.DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(organizationId);
		model.ReceiveTime = DateTime.Now;
		return View(model);
	}

	[HttpPost]
	public async Task<JsonResult> SelfProduction(VmSelfProduction vmProduction)
	{
		await _productionService.InserSelfProductiontData(vmProduction, UserSession);
		await UnitOfWork.SaveChangesAsync();
		return Json(1);
	}


	public async Task<IActionResult> ContractualProduction()
	{
		var organizationId = UserSession.ProtectedOrganizationId;
		var model = new VmContractualProduction();
		model.ProductSelectList =
			_productService.SpGetProductForContractualProductionReceive(UserSession.OrganizationId, 2);
		//model.MeasurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(organizationId);
		model.ContractualProductionContactSelectList =
			await _contractVendorService.GetContractualProductionsSelectList(UserSession.OrganizationId);
		model.DocumentTypeSelectList = await _documentType.GetDocumentTypeSelectList(organizationId);
		model.ReceiveTime = DateTime.Now;
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> ContractualProduction(VmContractualProduction model)
	{
		//await _productionService.InserSelfProductiontData(model, _session);
		await _productionService.InsertContractualProductionData(model, UserSession);
		return Json(1);
	}


	public async Task<FileSaveFeedbackDto> FileSaveAsync(IFormFile file)
	{
		var organizationId = UserSession.OrganizationId;
		FileSaveFeedbackDto fdto = new FileSaveFeedbackDto();
		var FileExtenstion = Path.GetExtension(file.FileName);

		string FileName = Guid.NewGuid().ToString();

		FileName += FileExtenstion;
		Organization organization = await _organizationService.Query()
			.FirstOrDefaultAsync(c => c.OrganizationId == organizationId, CancellationToken.None);
		var FolderName = ControllerStaticData.APPLICATION_DOCUMENT + organization.OrganizationId;
		var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, FolderName);

		fdto.MimeType = FileExtenstion;
		bool exists = Directory.Exists(uploads);
		if (!exists)
		{
			Directory.CreateDirectory(uploads);
		}

		if (file.Length > 0)
		{
			var filePath = Path.Combine(uploads, file.FileName);
			fdto.FileUrl = filePath;
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}
		}

		return fdto;
	}

	[HttpPost]
	public async Task<JsonResult> CreateAsync(Models.vmProductionReceive vmProduction)
	{
		var createdBy = UserSession.UserId;
		var organizationId = UserSession.OrganizationId;
		string status;
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
		productionReceive.IsContractual = vmProduction.IsContractual;
		productionReceive.ContractualProductionId = vmProduction.ContractualProductionId;
		productionReceive.ContractualProductionChallanNo = vmProduction.ContractualProductionChallanNo;
		productionReceive.ProductionReceiveDetailList = vmProduction.ProductionReceiveDetailList;
		productionReceive.ContentInfoJson = vmProduction.ContentInfoJsonTest;
		status = await _productionService.InsertData(productionReceive);


		if (status.Equals("Successful"))
		{
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
		}
		else
		{
		}

		return Json(status);
	}

	public async Task<JsonResult> ContractualContractNoAutoComplete(string filterText)
	{
		var organizationId = UserSession.OrganizationId;
		var productList = await _contractVendor.Query()
			.Where(c => c.IsClosed == false && c.OrganizationId == UserSession.OrganizationId &&
			            c.ContractNo.Contains(filterText)).SelectAsync(CancellationToken.None);
		return new JsonResult(productList.Select(x => new
		{
			Id = x.ContractualProductionId,
			ContractNo = x.ContractNo
		}).ToList());
	}

	public IActionResult PurchaseOrderList()
	{
		return View();
	}

	public IActionResult Vouchers()
	{
		return View();
	}

	public IActionResult PaymentJournalVoucherEntry()
	{
		return View();
	}

	public IActionResult MaterialConsume()
	{
		return View();
	}

	public IActionResult NewProductionOrder()
	{
		return View();
	}

	public IActionResult SectionDelivery()
	{
		return View();
	}

	public IActionResult BOM()
	{
		return View();
	}
}