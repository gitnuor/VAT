using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnumsNET;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.ProductService;
using vms.service.Services.SettingService;
using vms.utility;
using vms.utility.StaticData;
using vms.Utility;


namespace vms.Controllers;

public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly IPriceSetupService _priceService;
    private readonly IProductGroupService _productGrpService;
    private readonly IProductVatTypeService _productVatTypeService;
    private readonly IProductVatService _productVatService;
    private readonly IProductTypeService _productTypeService;
    private readonly IMeasurementUnitService _measurementUnitService;
    private readonly IProductProductTypeMappingService _productProductTypeMappingService;
    private readonly IProductCategoryService _productCategoryService;
    private readonly ISupplimentaryDutyService _supplimentaryDutyService;
    private IAutocompleteService _autocompleteService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IOverHeadCostService _overHeadCost;
    private readonly IPriceSetupProductCostService _productCost;
    private readonly IBrandService _brandService;

    private vms.entity.models.Product prod;

    //private int organizationId;
    private readonly IProductStoredProcedureService _productStoredProcedureService;

    public ProductsController(IPriceSetupProductCostService productCost,
        IOverHeadCostService overHeadCost, ControllerBaseParamModel controllerBaseParamModel,
        IProductService service, IProductGroupService productGrpService,
        IMeasurementUnitService measurementUnitService, IProductVatTypeService productVatTypeService,
        IProductVatService productVatService, IPriceSetupService priceService,
        IProductTypeService productTypeService, IProductProductTypeMappingService productProductTypeMappingService
        , IProductCategoryService productCategoryService, ISupplimentaryDutyService supplimentaryDutyService,
        IProductStoredProcedureService productStoredProcedureService,
        IAutocompleteService autocompleteService, IWebHostEnvironment hostingEnvironment, IBrandService brandService) : base(
        controllerBaseParamModel)
    {
        _productCost = productCost;
        _overHeadCost = overHeadCost;
        _service = service;
        _priceService = priceService;
        _productTypeService = productTypeService;
        _productGrpService = productGrpService;
        _measurementUnitService = measurementUnitService;
        _productVatTypeService = productVatTypeService;
        _productVatService = productVatService;
        _productProductTypeMappingService = productProductTypeMappingService;
        _productCategoryService = productCategoryService;
        _supplimentaryDutyService = supplimentaryDutyService;
        _autocompleteService = autocompleteService;
        _hostingEnvironment = hostingEnvironment;
        _brandService = brandService;
        //organizationId = _session.OrganizationId;
        _productStoredProcedureService = productStoredProcedureService;
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        vmProductsIndex model = new vmProductsIndex();
        model.ProductList = await _service.GetProducts(UserSession.ProtectedOrganizationId);
        return View(model);
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW_DETAILS)]
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _service.GetProduct(id);
        if (product == null)
        {
            return NotFound();
        }

        vmProductPriceSetup details = new vmProductPriceSetup();
        details.Name = product.Name;
        details.ModelNo = product.ModelNo;
        details.Code = product.Code;
        details.Hscode = product.Hscode;
        details.IsActive = product.IsActive;
        details.OrganizationName = product.Organization.Name;
        details.MeasurementUnitName = product.MeasurementUnit.Name;
        details.ProductGroupName = product.ProductGroup.Name;
        details.ProductCategoryName = product.ProductCategory == null ? "" : product.ProductCategory.Name;
        var price = new List<PriceSetup>();
        var productCosts = new List<PriceSetupProductCost>();
        PriceSetup productCostDetails = new PriceSetup();
        PriceSetupProductCost priceSetupProductCost = new PriceSetupProductCost();
        foreach (var item in product.PriceSetups)
        {
            if (item.IsActive == true)
            {
                price.Add(item);
                productCostDetails = await _priceService.Query().Include(c => c.Product)
                    .Include(c => c.PriceSetupProductCosts)
                    .FirstOrDefaultAsync(c => c.PriceSetupId == item.PriceSetupId, CancellationToken.None);
            }

            foreach (var costDetails in productCostDetails.PriceSetupProductCosts)
            {
                priceSetupProductCost = await _productCost.Query().Include(c => c.OverHeadCost)
                    .Include(c => c.RawMaterial).Include(c => c.MeasurementUnit).FirstOrDefaultAsync(c =>
                        c.PriceSetupProductCostId == costDetails.PriceSetupProductCostId, CancellationToken.None);
                productCosts.Add(priceSetupProductCost);
            }
        }

        details.PriceSetupProductCosts = productCosts;
        details.PriceSetups = price;


        return View(details);
    }

    [HttpGet]
    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        var strOrgId = UserSession.ProtectedOrganizationId;
        var measurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(strOrgId);
        var productGroupSelectList = await _productGrpService.GetProductGroupSelectList(strOrgId);
        var productTypeSelectList = await _productTypeService.GetProductTypeSelectList();
        var productCat = await _productCategoryService.GetProductCategorySelectList(UserSession.ProtectedOrganizationId);
        var productVatType = await _productVatTypeService.Query().SelectAsync();

        vmProduct pro = new vmProduct
        {
            ProductTypeSelectListItems = productTypeSelectList,
            MeasurementUnits = measurementUnitSelectList,
            ProductCategories = productCat,
            ProductGroups = productGroupSelectList,
            ProductVatTypeList = productVatType,
            BrandsSelectListItems = await _brandService.GetBrandSelectList(strOrgId)
        };
        return View(pro);
    }

    [HttpGet]
    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_ADD)]
    public async Task<IActionResult> CreateProductByType(string typeName)
    {
        var strOrgId = UserSession.ProtectedOrganizationId;
        var measurementUnitSelectList = await _measurementUnitService.GetMeasurementUnitSelectList(strOrgId);
        var productGroupSelectList = await _productGrpService.GetProductGroupSelectList(strOrgId);
        var productCat = await _productCategoryService.GetProductCategorySelectList(UserSession.ProtectedOrganizationId);
        var productVatType = await _productVatTypeService.Query().SelectAsync();
        var productTypeId = GetTypeIdByTypeName(typeName);
        var productType = await _productTypeService.FindAsync(productTypeId);

        vmProduct pro = new vmProduct
        {
            MeasurementUnits = measurementUnitSelectList,
            ProductCategories = productCat,
            ProductGroups = productGroupSelectList,
            ProductVatTypeList = productVatType,
            BrandsSelectListItems = await _brandService.GetBrandSelectList(strOrgId),
            ProductTypeId = GetTypeIdByTypeName(typeName)
        };
        ViewData["Title"] = $"Add Product/Service (Type: {productType.Name})";
        return View(pro);
    }

    private int GetTypeIdByTypeName(string typeName)
    {
        var isRecognized = Enum.TryParse(typeName, true, out EnumProductType pursedProductType);
        if (!isRecognized)
            throw new Exception("Invalid Argument!");
        
        return (int) pursedProductType;
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_ADD)]
    public async Task<IActionResult> Create(vmProduct product)
    {
        if (ModelState.IsValid)
        {
            entity.models.Product products = new entity.models.Product
            {
                Name = product.Name,
                BrandId = product.BrandId,
                ProductNumber = product.ProductNumber,
                ModelNo = product.ModelNo,
                DeviceModel = product.DeviceModel,
                Code = product.Code,
                PartNo = product.PartNo,
                PartCode = product.PartCode,
                Variant = product.Variant,
                Color = product.Color,
                Size = product.Size,
                Weight = product.Weight,
                WeightInKg = product.WeightInKg,
                Specification = product.Specification,
                Hscode = product.Hscode,
                ProductTypeId = product.ProductTypeId,
                ProductCategoryId = product.ProductCategoryId,
                ProductGroupId = product.ProductGroupId,
                MeasurementUnitId = product.MeasurementUnitId,
                Description = product.Description,
                IsNonRebateable = product.IsNonRebateable,
                CreatedTime = DateTime.Now,
                EffectiveFrom = DateTime.Now,
                IsActive = true,
                CreatedBy = UserSession.UserId,
                TotalQuantity = 0,
                OrganizationId = UserSession.OrganizationId,
                EffectiveTo = null,
                ReferenceKey = product.ReferenceKey,
			};
            _service.Insert(products);
            await UnitOfWork.SaveChangesAsync();

            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            if (product.ProductVattypeId != 0)
            {
                ProductVat prodVat = new ProductVat
                {
                    ProductId = products.ProductId,
                    ProductVattypeId = product.ProductVattypeId,
                    EffectiveFrom = DateTime.Now,
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    ProductDefaultVatPercent = product.ProductDefaultVatPercent
                };
                _productVatService.Insert(prodVat);
                await UnitOfWork.SaveChangesAsync();

                var vatObj = JsonConvert.SerializeObject(prodVat, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                AuditLog vat_au = new AuditLog();
                vat_au.ObjectTypeId = (int)EnumObjectType.Product;
                vat_au.PrimaryKey = products.ProductId;
                vat_au.AuditOperationId = (int)EnumOperations.Add;
                vat_au.UserId = UserSession.UserId;
                vat_au.Datetime = DateTime.Now;
                vat_au.Descriptions = vatObj.ToString();
                vat_au.IsActive = true;
                vat_au.CreatedBy = UserSession.UserId;
                vat_au.CreatedTime = DateTime.Now;
                vat_au.OrganizationId = UserSession.OrganizationId;

                var vat_status = await AuditLogCreate(vat_au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            }

            var productObj = JsonConvert.SerializeObject(products, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.Product,
                PrimaryKey = products.ProductId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = productObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };

            var product_status = await AuditLogCreate(au);

            return RedirectToAction(nameof(Details),
                new { id = IvatDataProtector.Protect(products.ProductId.ToString()) });
        }


		var strOrgId = UserSession.ProtectedOrganizationId;
		var measurementUnitDT = await _measurementUnitService.GetMeasurementUnitSelectList(strOrgId);
		var productGroupDT = await _productGrpService.GetProductGroupSelectList(strOrgId);
		var productTypeDT = await _productTypeService.GetProductTypeSelectList();
		var productCat = await _productCategoryService.GetProductCategorySelectList(UserSession.ProtectedOrganizationId);
		var productVatType = await _productVatTypeService.Query().SelectAsync();

        product.MeasurementUnits = measurementUnitDT;
        product.ProductGroups = productGroupDT;
        product.ProductVatTypeList = productVatType;
        product.ProductCategories = productCat;
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(product);
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _service.GetProduct(id);
        prod = product;
        if (product == null)
        {
            return NotFound();
        }

        var strOrgId = UserSession.ProtectedOrganizationId;
        var measurementUnitDT = await _measurementUnitService.GetMeasurementUnitSelectList(strOrgId);
        var productGroupDT = await _productGrpService.GetProductGroupSelectList(strOrgId);
        var productTypeDT = await _productTypeService.GetProductTypeSelectList();
        var productCat = await _productCategoryService.GetProductCategorySelectList(UserSession.ProtectedOrganizationId);
        var productVatType = await _productVatTypeService.Query().SelectAsync();
        var productVats = await _productVatService.Query()
            .Where(w => w.ProductId == product.ProductId && w.IsActive == true)
            .SelectAsync();
        var brandsSelectListItems = await _brandService.GetBrandSelectList(strOrgId);

		var model = new ProductForUpdatePostViewModel
		{
            ProductId = product.ProductId,
            Name = product.Name,
            ModelNo = product.ModelNo,
            Code = product.Code,
            ReferenceKey = product.ReferenceKey,
            Hscode = product.Hscode,
            IsActive = product.IsActive,
            ProductCategoryId = product.ProductCategoryId,
            MeasurementUnitId = product.MeasurementUnitId,
            OrganizationId = product.OrganizationId,
            ProductGroupId = product.ProductGroupId,
            ProductVatTypeList = productVatType,
            MeasurementUnits = measurementUnitDT,
            ProductCategories = productCat,
            ProductGroups = productGroupDT,
            ProductTypeSelectListItems = productTypeDT,
            IsNonRebateable = product.IsNonRebateable,
            ProductTypeId = product.ProductTypeId,
            BrandId = product.BrandId,
            ProductNumber = product.ProductNumber,
            DeviceModel = product.DeviceModel,
            PartNo = product.PartNo,
            PartCode = product.PartCode,
            Variant = product.Variant,
            Color = product.Color,
            Size = product.Size,
            Weight = product.Weight,
            WeightInKg = product.WeightInKg,
            Specification = product.Specification,
            Description = product.Description,
            BrandsSelectListItems = brandsSelectListItems,
		};

        product.EncryptedId = id;
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_EDIT)]
    public async Task<IActionResult> Edit(ProductForUpdatePostViewModel product, string id)
    {
        int productId = int.Parse(IvatDataProtector.Unprotect(id));
        if (productId != product.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var productData = await _service.Query()
                    .SingleOrDefaultAsync(p => p.ProductId == productId, CancellationToken.None);
                var previousObj = JsonConvert.SerializeObject(productData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                productData.Name = product.Name;
                productData.BrandId = product.BrandId;
                productData.ProductNumber = product.ProductNumber;
                productData.ModelNo = product.ModelNo;
                productData.DeviceModel = product.DeviceModel;
                productData.Code = product.Code;
				productData.PartNo = product.PartNo;
                productData.PartCode = product.PartCode;
                productData.Variant = product.Variant;
                productData.Color = product.Color;
                productData.Size = product.Size;
                productData.Weight = product.Weight;
                productData.WeightInKg = product.WeightInKg;
                productData.Specification = product.Specification;
				productData.Description = product.Description;
				productData.ReferenceKey = product.ReferenceKey;
                productData.Hscode = product.Hscode;
                productData.MeasurementUnitId = product.MeasurementUnitId;
                productData.ProductGroupId = product.ProductGroupId;
                productData.ProductCategoryId = product.ProductCategoryId;
                productData.ProductTypeId = product.ProductTypeId;
                productData.IsNonRebateable = product.IsNonRebateable;
                productData.OrganizationId = UserSession.OrganizationId;

                _service.Update(productData);
                await UnitOfWork.SaveChangesAsync();

                var currentObj = JsonConvert.SerializeObject(productData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                var au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.Product,
                    PrimaryKey = productData.ProductId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = GetChangeInformation(previousObj.ToString(), currentObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };

                var product_status = await AuditLogCreate(au);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
            return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.LIST_PRODUCTS);
        }

		var strOrgId = UserSession.ProtectedOrganizationId;
		var measurementUnitDT = await _measurementUnitService.GetMeasurementUnitSelectList(strOrgId);
		var productGroupDT = await _productGrpService.GetProductGroupSelectList(strOrgId);
		var productTypeDT = await _productTypeService.GetProductTypeSelectList();
		var productCat = await _productCategoryService.GetProductCategorySelectList(UserSession.ProtectedOrganizationId);
		var productVatType = await _productVatTypeService.Query().SelectAsync();


        product.MeasurementUnits = measurementUnitDT;
        product.ProductGroups = productGroupDT;
        product.ProductCategories = productCat;
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(product);
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_EDIT_VAT)]
    public async Task<IActionResult> VAtEdit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _service.GetProduct(id);
        prod = product;
        if (product == null)
        {
            return NotFound();
        }

        var productVatTypeDT = await _productVatTypeService.Query().SelectAsync();

        //IEnumerable<SelectListItems> ProductVattypes = productVatTypeDT.Select(s => new SelectListItems
        //{
        //    Id = s.ProductVattypeId,
        //    Name = s.Name
        //}).ToList();

        var productVats = await _productVatService.Query()
            .Where(w => w.ProductId == product.ProductId && w.IsActive == true)
            .SelectAsync();
        decimal vatPercent = 0;
        if (productVats.Any())
        {
            vatPercent = productVats.FirstOrDefault().ProductDefaultVatPercent;
        }

        vmProduct vmProduct = new vmProduct
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Code = product.Code,
            Hscode = product.Hscode,

            IsActive = product.IsActive,
            ProductCategoryId = product.ProductCategoryId,
            MeasurementUnitId = product.MeasurementUnitId,
            OrganizationId = product.OrganizationId,
            ProductGroupId = product.ProductGroupId,
            ProductVattypeId = productVats.Count() == 0 ? 1 : productVats.First().ProductVattypeId,

            ProductVatTypeList = productVatTypeDT,
            ProductDefaultVatPercent = vatPercent
        };

        product.EncryptedId = id;
        return View(vmProduct);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_EDIT_VAT)]
    public async Task<IActionResult> VAtEdit(vmProduct product, string id)
    {
        if (product.ProductId == 0)
        {
            return NotFound();
        }

        try
        {
            if (product.ProductVattypeId != 0)
            {
                var existingProductvats = await _productVatService.Query()
                    .Where(w => w.ProductId == product.ProductId && w.IsActive == true).SelectAsync();
                foreach (var existingProductvat in existingProductvats)
                {
                    existingProductvat.IsActive = false;
                    existingProductvat.EffectiveTo = DateTime.Now;
                    _productVatService.Update(existingProductvats.First());
                }

                ProductVat prodVat = new ProductVat
                {
                    ProductId = product.ProductId,
                    ProductVattypeId = product.ProductVattypeId,
                    EffectiveFrom = DateTime.Now,
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    ProductDefaultVatPercent = product.ProductDefaultVatPercent
                };
                _productVatService.Insert(prodVat);
                await UnitOfWork.SaveChangesAsync();
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(product.ProductId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
        return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.LIST_PRODUCTS);
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_DELETE)]
    public async Task<IActionResult> ChangeProductStatus(string id)
    {
        var productData =
            await _service.Query().SingleOrDefaultAsync(p => p.ProductId == int.Parse(IvatDataProtector.Unprotect(id)),
                CancellationToken.None);
        try
        {
            if (productData.IsActive == true)
            {
                productData.IsActive = false;
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;
            }
            else
            {
                productData.IsActive = true;
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;
            }

            productData.EffectiveTo = DateTime.Now;
            _service.Update(productData);
            await UnitOfWork.SaveChangesAsync();


            var proObj = JsonConvert.SerializeObject(productData, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.Product,
                PrimaryKey = productData.ProductId,
                AuditOperationId = (int)EnumOperations.Delete,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = "IsActive:0",
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };

            var au_status = await AuditLogCreate(au);
        }
        catch (Exception ex)
        {
	        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
		}

        return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.LIST_PRODUCTS);
    }

    private async Task<bool> ProductExists(int id)
    {
        return await _service.Query().AnyAsync(e => e.ProductId == id, CancellationToken.None);
    }

    public async Task<JsonResult> ProductionReceiveAutoComplete(string filterText, int contructId = 0)
    {
        var organizationId = UserSession.OrganizationId;
        var productList =
            await _autocompleteService.ProductionReceiveAutoComplete(organizationId, filterText, contructId);
        return new JsonResult(productList.Select(x => new
        {
            Id = x.ProductId,
            Name = x.ProductName,
            ModelNo = x.ModelNo,
            Code = x.Code,
            MeasurementUnitId = x.MeasurementUnitId,
            MeasurementUnitName = x.MeasurementUnitName
        }).ToList());
    }

    public JsonResult GetNumberOfFinishedProductsWithNotifiableChange(int productId, decimal unitPrice)
    {
        return new JsonResult(
            new
            {
                TotalItem =
                    _productStoredProcedureService.SpGetNumberOfFinishedProductsWithNotifiableChange(productId,
                        unitPrice)
            });
    }

    public JsonResult GetNumberOfRawMaterialWithNotifiableChange(int productId)
    {
        return new JsonResult(
            new
            {
                TotalItem = _productStoredProcedureService.SpGetNumberOfRawMaterialWithNotifiableChange(productId)
            });
    }

    public async Task<JsonResult> ProductionReceiveGetData(int prodId, int branchId)
    {
        var organizationId = UserSession.OrganizationId;

        var productList = await _autocompleteService.GetRawMaterialForProduction(organizationId, branchId, prodId);
        return new JsonResult(productList.Select(c => new
        {
            Id = c.Id,
            ProdName = c.ProdName,
            ProductDescription = c.ProductDescription,
            CurrentStock = c.CurrentStock,
            UnitName = c.UnitName,
            RequiredQtyPerUnitProduction = c.RequiredQtyPerUnitProduction
        }).ToList());
    }

    public async Task<JsonResult> ProductionReceiveAutoCompleteBOM(string filterText)
    {
        var organizationId = UserSession.OrganizationId;
        var productList = await _autocompleteService.ProductionReceiveAutoCompleteBOM(organizationId, filterText);
        return new JsonResult(productList.Select(x => new
        {
            Id = x.ProductId,
            Name = x.ProductName,
            ModelNo = x.ModelNo,
            Code = x.Code,
            MaxUseQty = x.MaxUseQty,
            MeasurementUnitId = x.MeasurementUnitId,
            MeasurementUnitName = x.MeasurementUnitName
        }).ToList());
    }

    public async Task<JsonResult> ProductionReceiveAutoCompletePriceSetup(string filterText)
    {
        var organizationId = UserSession.OrganizationId;
        var productList =
            await _autocompleteService.ProductionReceiveAutoCompletePriceSetup(organizationId, filterText);
        return new JsonResult(productList.Select(x => new
        {
            Id = x.ProductId,
            Name = x.ProductName,
            ModelNo = x.ModelNo,
            Code = x.Code,
            MaxUseQty = x.MaxUseQty,
            UnitPrice = x.UnitPrice,
            MeasurementUnitId = x.MeasurementUnitId,
            MeasurementUnitName = x.MeasurementUnitName,
            IsApplicableAsRawMaterial = x.IsApplicableAsRawMaterial,
            ItemType = x.ItemType
        }).ToList());
    }

    public async Task<JsonResult> PurchaseProductAutoComplete(string filterText)
    {
        var organizationId = UserSession.OrganizationId;
        var productList = await _autocompleteService.GetProductAutocompleteForPurchases(organizationId, filterText);
        return new JsonResult(productList.Select(x => new
        {
            Id = x.ProductId,
            Name = x.ProductName,
            UnitPrice = 0,
            Vat = x.DefaultVatPercent,
            SupplimentaryDuty = x.DefaultSupplimentaryDutyPercent,
            Unit = x.MeasurementUnitId,
            DefaultImportDutyPercent = x.DefaultImportDutyPercent,
            DefaultRegulatoryDutyPercent = x.DefaultRegulatoryDutyPercent,
            DefaultAdvanceTaxPercent = x.DefaultAdvanceTaxPercent,
            DefaultAdvanceIncomeTaxPercent = x.DefaultAdvanceIncomeTaxPercent,
            ProductVATTypeId = x.ProductVATTypeId
        }).ToList());
    }

    public async Task<JsonResult> SaleProductAutoComplete(string filterText)
    {
        var organizationId = UserSession.OrganizationId;
        var productList = await _autocompleteService.GetProductAutocompleteForSales(organizationId, filterText);
        return new JsonResult(productList.Select(x => new
        {
            Id = x.ProductId,
            Name = x.ProductName,
            UnitPrice = x.SalesUnitPrice,
            Unit = x.MeasurementUnitId,
            Vat = x.DefaultVatPercent,
            SupplimentaryDuty = x.SupplimentaryDutyPercent,
            MaxSaleQty = x.MaxSaleQty,
            ProductVATTypeId = x.ProductVATTypeId
        }).ToList());
    }

    public async Task<JsonResult> ProductAutoComplete(string filterText)
    {
        var product = await _productVatService.Query()
            .Where(c => c.Product.OrganizationId == UserSession.OrganizationId && c.IsActive == true)
            .Include(c => c.Product).Include(c => c.Product.PriceSetups).Include(c => c.ProductVattype)
            .Where(c => c.Product.Name.Contains(filterText) &&
                        c.Product.OrganizationId == UserSession.OrganizationId)
            .SelectAsync(CancellationToken.None);
        return new JsonResult(product.Select(x => new
        {
            Id = x.ProductId,
            Name = x.Product.Name,
            UnitPrice = x.Product.PriceSetups.Select(c => c.Mrp).FirstOrDefault(),
            Vat = x.ProductVattype.DefaultVatPercent,
            PriceDeclarId = x.Product.PriceSetups.AsQueryable().Where(c => c.IsActive == true)
                .Select(c => c.PriceSetupId).LastOrDefault()
        }).ToList());
    }

    public async Task<JsonResult> ProductAutoCompleteForMushak6P1(string filterText)
    {
        var product = await _service.Query()
            .Where(p => p.OrganizationId == UserSession.OrganizationId
                        && p.IsActive
                        && p.ProductType.IsInventory
                        && p.ProductType.IsPurchaseable == true
                        && p.Name.ToLower().Contains(filterText.ToLower()))
            .SelectAsync(CancellationToken.None);
        return new JsonResult(product.Select(x => new
        {
            Id = x.ProductId,
            Name = x.Name
        }).ToList());
    }

    public async Task<JsonResult> ProductAutoCompleteForMushak6P2(string filterText)
    {
        var product = await _service.Query()
            .Where(p => p.OrganizationId == UserSession.OrganizationId
                        && p.IsActive
                        && p.ProductType.IsInventory
                        && p.ProductType.IsSellable == true
                        && p.Name.ToLower().Contains(filterText.ToLower()))
            .SelectAsync(CancellationToken.None);
        return new JsonResult(product.Select(x => new
        {
            Id = x.ProductId,
            Name = x.Name
        }).ToList());
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_ADD_NEW_PRICE)]
    public async Task<IActionResult> PriceSetup(string id)
    {
        var measurementUnitDT = await _measurementUnitService.Query()
            .Where(w => w.OrganizationId == UserSession.OrganizationId).SelectAsync();
        IEnumerable<CustomSelectListItem> MeasurementUnits = measurementUnitDT.Select(s => new CustomSelectListItem
        {
            Id = s.MeasurementUnitId,
            Name = s.Name
        }).ToList();

        var overHeadCost = await _overHeadCost.Query().SelectAsync();
        IEnumerable<CustomSelectListItem> overHeadCosts = overHeadCost
            .Where(x => x.OrganizationId == UserSession.OrganizationId).Select(s => new CustomSelectListItem
            {
                Id = s.OverHeadCostId,
                Name = s.Name
            });

        var product = await _service.GetProduct(id);

        PriceSetup priceSetup = new PriceSetup
        {
            MeasurementUnits = MeasurementUnits
        };
        priceSetup.OverHeadCost = overHeadCosts;
        priceSetup.MeasurementUnitName = product.MeasurementUnit.Name;
        priceSetup.ProductGroupName = product.ProductGroup.Name;
        priceSetup.ProductName = product.Name;
        priceSetup.ProductCategoryName = product.ProductCategory == null ? "" : product.ProductCategory.Name;
        priceSetup.HSCode = product.Hscode;
        priceSetup.ProductId = product.ProductId;

        product.EncryptedId = id;
        return View(priceSetup);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_ADD_NEW_PRICE)]
    public async Task<IActionResult> PriceSetup(PriceSetup priceSetup, string id)
    {
        try
        {
            int pro_Id = int.Parse(IvatDataProtector.Unprotect(id));
            var productData = await _priceService.Query()
                .SingleOrDefaultAsync(p => p.ProductId == pro_Id && p.IsActive == true, CancellationToken.None);
            if (productData != null)
            {
                productData.EffectiveTo = DateTime.Now;
                productData.IsActive = false;
                _priceService.Update(productData);
                await UnitOfWork.SaveChangesAsync();
            }

            priceSetup.CreatedTime = DateTime.Now;
            priceSetup.EffectiveFrom = DateTime.Now;
            priceSetup.IsActive = true;
            priceSetup.CreatedBy = UserSession.UserId;
            priceSetup.OrganizationId = UserSession.OrganizationId;
            priceSetup.ProductId = pro_Id;
            priceSetup.EffectiveTo = null;
            _priceService.Insert(priceSetup);
            await UnitOfWork.SaveChangesAsync();
            var currentObj = JsonConvert.SerializeObject(priceSetup, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.PriceSetup,
                PrimaryKey = priceSetup.PriceSetupId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = currentObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };

            var product_status = await AuditLogCreate(au);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        }

		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}

		return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.LIST_PRODUCTS);
    }

    public async Task<JsonResult> PriceSetups(vmPriceSetup vm)
    {
        try
        {
            var productData = await _priceService.Query()
                .FirstOrDefaultAsync(p => p.ProductId == vm.ProductId && p.IsActive == true,
                    CancellationToken.None);
            if (productData != null)
            {
                productData.EffectiveTo = DateTime.Now;
                productData.IsActive = false;
                _priceService.Update(productData);
            }

            var product = await _service.Query()
                .FirstOrDefaultAsync(p => p.ProductId == vm.ProductId.Value && p.IsActive == true,
                    CancellationToken.None);
            PriceSetup priceSetup = new PriceSetup();
            priceSetup.CreatedTime = DateTime.Now;
            priceSetup.EffectiveFrom = DateTime.Now;
            priceSetup.IsActive = true;
            priceSetup.CreatedBy = UserSession.UserId;
            priceSetup.OrganizationId = UserSession.OrganizationId;
            priceSetup.ProductId = vm.ProductId.Value;
            priceSetup.EffectiveTo = null;
            priceSetup.PurchaseUnitPrice = vm.PurchaseUnitPrice;
            priceSetup.SalesUnitPrice = vm.SalesUnitPrice;
            priceSetup.MeasurementUnitId = product.MeasurementUnitId;
            _priceService.Insert(priceSetup);
            await UnitOfWork.SaveChangesAsync();
            PriceSetupProductCost pspc;
            if (vm.RawMaterialLists != null)
            {
                foreach (var item in vm.RawMaterialLists)
                {
                    pspc = new PriceSetupProductCost();
                    pspc.PriceSetupId = priceSetup.PriceSetupId;
                    pspc.RawMaterialId = item.productId;
                    pspc.RequiredQty = item.RequireQty;
                    pspc.MeasurementUnitId = item.MeasurementUnitId;
                    pspc.WastagePercentage = item.WastagePercentage;
                    pspc.IsRawMaterial = item.IsRawMaterial;

                    pspc.OverHeadCostId = item.OverHeadCostId;
                    pspc.Cost = item.cost;
                    _productCost.Insert(pspc);
                }
            }

            var rawAffected = await UnitOfWork.SaveChangesAsync();
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        }
        catch (Exception ex)
        {
	        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
		}

        return Json(true);
    }

	[HttpGet]
    public async Task<IActionResult> InputOutputCoEfficient(string id)
    {      
        var product = await _service.GetProduct(id);
        var organizationId = UserSession.ProtectedOrganizationId;
        var model = new VmInputOutputCoEfficient();
        model.RawMaterialForInputOutputCoEfficientList =
            _productStoredProcedureService.SpGetRawMaterialForInputOutputCoEfficient(UserSession.OrganizationId);
        model.MeasurementUnitSelectList =
            await _measurementUnitService.GetMeasurementUnitSelectList(organizationId);
        model.OverheadCostSelectList = await _overHeadCost.GetOverHeadCostSelectList(UserSession.OrganizationId);
       
        model.MeasurementUnitName = product.MeasurementUnit.Name;
        model.MeasurementUnitId = product.MeasurementUnit.MeasurementUnitId;
        model.ProductGroupName = product.ProductGroup.Name;
        model.ProductName = product.Name;
        model.ModelNo = product.ModelNo;
        model.Size = product.Size;
        model.ProductCategoryName = product.ProductCategory == null ? "" : product.ProductCategory.Name;
        model.HSCode = product.Hscode;
        model.HiddenProductId = product.ProductId;
        model.Weight = product.Weight;
        model.Variant = product.Variant;
        model.Color = product.Color;
        model.ProductTypeName = product.ProductType.Name;
        model.Code = product.Code;
        //return View(model);
        return View("InputOutputCoEfficientDev", model);
    }

    [HttpPost]
    public async Task<JsonResult> InputOutputCoEfficient(VmInputOutputCoEfficientPost vmInputOutputCoEfficientPost)
    {
        var productData =
            await _priceService.GetPriceSetupByProductId(vmInputOutputCoEfficientPost.HiddenProductId);
        if (productData != null)
        {
            foreach (var item in productData.Where(d => d.IsActive))
            {
                item.EffectiveTo = DateTime.Now;
                item.IsActive = false;
                _priceService.Update(item);
            }
        }

        await _priceService.InsertProductPriceAsync(vmInputOutputCoEfficientPost, UserSession);
        await UnitOfWork.SaveChangesAsync();
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        return Json(new { id = IvatDataProtector.Protect(vmInputOutputCoEfficientPost.HiddenProductId.ToString()) });
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_ADD_SUPPLIMENTARY_DUTY)]
    public async Task<IActionResult> SupplimentaryDuty(string id)
    {
        var productData = await _supplimentaryDutyService.Query()
            .SingleOrDefaultAsync(p => p.ProductId == int.Parse(IvatDataProtector.Unprotect(id)) && p.IsActive == true,
                CancellationToken.None);
        return View(productData);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_ADD_SUPPLIMENTARY_DUTY)]
    public async Task<IActionResult> SupplimentaryDuty(SupplimentaryDuty supplimentary, string id)
    {
        try
        {
            int productId = int.Parse(IvatDataProtector.Unprotect(id));
            var productData = await _supplimentaryDutyService.Query()
                .SingleOrDefaultAsync(p => p.ProductId == productId && p.IsActive == true, CancellationToken.None);
            if (productData != null)
            {
                productData.EffectiveTo = DateTime.Now;
                productData.IsActive = false;
                _supplimentaryDutyService.Update(productData);
                await UnitOfWork.SaveChangesAsync();
            }

            supplimentary.OrganizationId = UserSession.OrganizationId;
            supplimentary.CreatedTime = DateTime.Now;
            supplimentary.EffectiveFrom = DateTime.Now;
            supplimentary.IsActive = true;
            supplimentary.CreatedBy = UserSession.UserId;
            supplimentary.ProductId = productId;
            supplimentary.EffectiveTo = null;
            _supplimentaryDutyService.Insert(supplimentary);
            await UnitOfWork.SaveChangesAsync();
            var currentObj = JsonConvert.SerializeObject(supplimentary, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.SupplimentaryDuty,
                PrimaryKey = supplimentary.SupplimentaryDutyId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = currentObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };

            var product_status = await AuditLogCreate(au);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
        }
        catch (Exception ex)
        {
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
		}

        return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.LIST_PRODUCTS);
    }

    public async Task<JsonResult> GetAllProductType()
    {
        var productType = await _productTypeService.Query().SelectAsync(CancellationToken.None);
        return new JsonResult(productType.Select(x => new
        {
            Id = x.ProductTypeId,
            Name = x.Name
        }).ToList());
    }

    public async Task<JsonResult> GetAllAssignedProductType(int productId)
    {
        var productType = await _productProductTypeMappingService.Query()
            .SelectAsync(CancellationToken.None);
        return new JsonResult(productType.Select(x => new
        {
            Id = 1,
            IAssigned = 1
        }).ToList());
    }

    public IActionResult PriceDeclarMushok(int id)
    {
        return View();
    }

    public async Task<JsonResult> ContractVendorProductAutoComplete(string filterText)
    {
        var organizationId = UserSession.OrganizationId;
        var productList = await _service.Query().Include(c => c.MeasurementUnit)
            .Where(c =>
                c.IsActive == true && c.OrganizationId == UserSession.OrganizationId &&
                c.Name.Contains(filterText)).SelectAsync(CancellationToken.None);
        return new JsonResult(productList.Select(x => new
        {
            Id = x.ProductId,
            Name = x.Name,
            Unit = x.MeasurementUnit.MeasurementUnitId
        }).ToList());
    }

    public async Task<JsonResult> ContractVendorTransferRawMaterialProductAutoComplete(string filterText)
    {
        var organizationId = UserSession.OrganizationId;
        var productList = await _service.Query().Include(c => c.MeasurementUnit)
            .Where(c =>
                c.IsActive == true &&
                c.OrganizationId == UserSession.OrganizationId && c.Name.Contains(filterText))
            .SelectAsync(CancellationToken.None);
        return new JsonResult(productList.Select(x => new
        {
            Id = x.ProductId,
            Name = x.Name,
            Unit = x.MeasurementUnit.MeasurementUnitId
        }).ToList());
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_ADD_SUBMISSION)]
    public async Task<IActionResult> AddSubmissionIndex(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _service.GetProduct(id);
        if (product == null)
        {
            return NotFound();
        }

        vmProductPriceSetup details = new vmProductPriceSetup();
        details.ProductId = product.ProductId;
        details.Name = product.Name;
        details.Code = product.Code;
        details.Hscode = product.Hscode;
        details.IsActive = product.IsActive;
        details.OrganizationName = product.Organization.Name;
        details.MeasurementUnitName = product.MeasurementUnit.Name;
        details.ProductGroupName = product.ProductGroup.Name;
        details.ProductCategoryName = product?.ProductCategory?.Name;
        details.MushakSubmissionDate = DateTime.Now;
        var price = new List<PriceSetup>();
        var productCosts = new List<PriceSetupProductCost>();
        PriceSetup productCostDetails = new PriceSetup();
        PriceSetupProductCost priceSetupProductCost = new PriceSetupProductCost();
        foreach (var item in product.PriceSetups)
        {
            if (item.IsActive == true)
            {
                price.Add(item);
                productCostDetails = await _priceService.Query().Include(c => c.Product)
                    .Include(c => c.PriceSetupProductCosts)
                    .FirstOrDefaultAsync(c => c.PriceSetupId == item.PriceSetupId, CancellationToken.None);
            }

            productCostDetails = await _priceService.Query().Include(c => c.Product)
                .Include(c => c.PriceSetupProductCosts)
                .FirstOrDefaultAsync(c => c.PriceSetupId == item.PriceSetupId, CancellationToken.None);
            foreach (var costDetails in productCostDetails.PriceSetupProductCosts)
            {
                priceSetupProductCost = await _productCost.Query().Include(c => c.OverHeadCost)
                    .Include(c => c.RawMaterial).Include(c => c.MeasurementUnit).FirstOrDefaultAsync(c =>
                        c.PriceSetupProductCostId == costDetails.PriceSetupProductCostId, CancellationToken.None);
                productCosts.Add(priceSetupProductCost);
            }
        }

        details.PriceSetupProductCosts = productCosts;
        details.PriceSetups = price;

        return View(details);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.PRODUCT)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_VIEW)]
    [VmsAuthorize(FeatureList.PRODUCT_PRODUCT_LIST_CAN_ADD_SUBMISSION)]
    public async Task<IActionResult> AddSubmissionIndex(PriceSetup setup)
    {
        var data = await _priceService.Query()
            .FirstOrDefaultAsync(c => c.ProductId == setup.ProductId && c.IsActive == true, CancellationToken.None);
        if (data == null)
        {
            return NotFound();
        }

        data.IsMushakSubmitted = true;
        var item = await _priceService.Query().Where(c => c.SubmissionSl != null)
            .SelectAsync(CancellationToken.None);
        var itemCount = item == null ? 1 : item.Count() + 1;
        data.SubmissionSl = itemCount;
        data.MushakSubmissionDate = setup.MushakSubmissionDate;
        _priceService.Update(data);
        try
        {
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			return RedirectToAction("Index");
        }
    }

    public IActionResult FileUpload(vmProductsIndex requestData)
    {
        if (ModelState.IsValid)
        {
            try
            {
                /** This code should be moved to service as per needed. **/
                IFormFile file = requestData.UploadedFile;
                string folderName = "UploadedExcel\\Products";
                string rootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(rootPath, folderName);
                string fullPath = "";
                string FileName = Guid.NewGuid().ToString();

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                if (file.Length > 0)
                {
                    fullPath = Path.Combine(newPath, FileName + "_" + file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                IWorkbook workbook = null;
                if (file.FileName.IndexOf(".xlsx") > 0)
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (file.FileName.IndexOf(".xls") > 0)
                {
                    workbook = new HSSFWorkbook(fs);
                }

                workbook.MissingCellPolicy = MissingCellPolicy.RETURN_NULL_AND_BLANK;

                // BulkSales Sheet Number One Start
                List<entity.models.Product> productList = new List<entity.models.Product>();
                ISheet sheetProducts = workbook.GetSheetAt(0);
                if (sheetProducts != null)
                {
                    int rowCount = sheetProducts.LastRowNum;
                    for (int i = 1; i <= rowCount; i++)
                    {
                        var data = new entity.models.Product();
                        IRow curRow = sheetProducts.GetRow(i);
                        data.ProductId = Convert.ToInt32(curRow.GetCell(0) == null
                            ? "0"
                            : curRow.GetCell(0).ToString().Trim());
                        data.Name = curRow.GetCell(1) == null ? "" : curRow.GetCell(1).ToString().Trim();

                        data.ModelNo = curRow.GetCell(2) == null ? "" : curRow.GetCell(2).ToString().Trim();
                        data.Code = curRow.GetCell(3) == null ? "" : curRow.GetCell(3).ToString().Trim();
                        data.Hscode = curRow.GetCell(4) == null ? "" : curRow.GetCell(4).ToString().Trim();
                        data.ProductCategoryId = ExcelDataConverter.GetNullableInt(curRow.GetCell(5) == null
                            ? "0"
                            : curRow.GetCell(5).ToString().Trim());
                        data.ProductCategoryName =
                            curRow.GetCell(6) == null ? "" : curRow.GetCell(6).ToString().Trim();
                        data.ProductGroupId =
                            Convert.ToInt32(curRow.GetCell(7) == null ? "0" : curRow.GetCell(7).ToString().Trim());
                        data.ProductGroupName =
                            curRow.GetCell(8) == null ? "" : curRow.GetCell(8).StringCellValue.Trim();
                        data.OrganizationId =
                            Convert.ToInt32(curRow.GetCell(9) == null ? "0" : curRow.GetCell(9).ToString().Trim());
                        data.TotalQuantity = Convert.ToDecimal(curRow.GetCell(10) == null
                            ? "0"
                            : curRow.GetCell(10).ToString().Trim());
                        data.MeasurementUnitId = Convert.ToInt32(curRow.GetCell(11) == null
                            ? "0"
                            : curRow.GetCell(11).ToString().Trim());
                        data.MeasurementUnitName = curRow.GetCell(12) == null
                            ? ""
                            : curRow.GetCell(12).StringCellValue.Trim();
                        data.EffectiveFrom = ExcelDataConverter.GetDatetime(curRow.GetCell(13) == null
                            ? ""
                            : curRow.GetCell(13).StringCellValue.Trim());
                        data.EffectiveTo = ExcelDataConverter.GetDatetime(curRow.GetCell(14) == null
                            ? ""
                            : curRow.GetCell(14).StringCellValue.Trim());
                        data.IsActive = ExcelDataConverter.GetBoolean(curRow.GetCell(15) == null
                            ? ""
                            : curRow.GetCell(15).StringCellValue.Trim());
                        data.CreatedBy = ExcelDataConverter.GetNullableInt(curRow.GetCell(16) == null
                            ? ""
                            : curRow.GetCell(16).ToString().Trim());
                        data.CreatedTime = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(17) == null
                            ? ""
                            : curRow.GetCell(17).StringCellValue.Trim());
                        data.IsSellable = ExcelDataConverter.GetBoolean(curRow.GetCell(18) == null
                            ? ""
                            : curRow.GetCell(18).ToString().Trim());
                        data.IsRawMaterial = ExcelDataConverter.GetBoolean(curRow.GetCell(19) == null
                            ? ""
                            : curRow.GetCell(19).ToString
                                ().Trim());
                        data.IsNonRebateable = ExcelDataConverter.GetBoolean(curRow.GetCell(20) == null
                            ? ""
                            : curRow.GetCell(20).ToString().Trim());
                        data.ReferenceKey = curRow.GetCell(21) == null ? "" : curRow.GetCell(21).ToString().Trim();
                        data.ModifyDate = ExcelDataConverter.GetNullableDatetime(curRow.GetCell(22) == null
                            ? ""
                            : curRow.GetCell(22).ToString().Trim());


                        productList.Add(data);
                    }
                }
                // BulkSales Sheet Number One End


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [VmsAuthorize(FeatureList.PRODUCT)]
    // [VmsAuthorizeAttribute(FeatureList.PRODUCT_PRODUCT_LIST_CAN_SET_INPUT_OUTPUT_COEFFICIENT)]
    public IActionResult InputOutputCoEfficient(int id)
    {
        return View();
    }
}