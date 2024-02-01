using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.service.Services.SettingService;
using vms.service.Services.TransactionService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class ProductVattypesController : ControllerBase
{
    private readonly IProductVatTypeService _service;
    private readonly IPurchaseTypeService _purchaseTypeService;
    private readonly ISalesTypeService _salesTypeService;
    private readonly ITransectionTypeService _transactionTypeService;
    private readonly IAutocompleteService _autocompleteService;
    public ProductVattypesController(IAutocompleteService autocompleteService,ControllerBaseParamModel controllerBaseParamModel, IProductVatTypeService service, IPurchaseTypeService purchaseTypeService,
        ISalesTypeService salesTypeService, ITransectionTypeService transactionTypeService) : base(controllerBaseParamModel)
    {
        _autocompleteService = autocompleteService;
        _service = service;
        _purchaseTypeService = purchaseTypeService;
        _salesTypeService = salesTypeService;
        _transactionTypeService = transactionTypeService;
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_VAT_TYPES_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var productVat = await _service.GetProductVattypes();
        productVat = productVat.OrderByDescending(x => x.ProductVattypeId);

        return View(productVat);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productVattype = await _service.Query()
                
            .SingleOrDefaultAsync(m => m.ProductVattypeId == id, CancellationToken.None);
        if (productVattype == null)
        {
            return NotFound();
        }

        return View(productVattype);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_VAT_TYPES_CAN_VIEW)]

    public async Task<IActionResult> Create()
    {
        ViewData[ControllerStaticData.PURCHASE_TYPE_ID] = new SelectList(await _purchaseTypeService.Query().SelectAsync(), ControllerStaticData.PURCHASE_TYPE_ID, ViewStaticData.NAME);
        ViewData[ControllerStaticData.SALES_TYPE_ID] = new SelectList(await _salesTypeService.Query().SelectAsync(), ControllerStaticData.SALES_TYPE_ID, ControllerStaticData.SALES_TYPE_NAME);
        ViewData[ControllerStaticData.TRANSACTION_TYPE_ID] = new SelectList(await _transactionTypeService.Query().SelectAsync(), ControllerStaticData.TRANSACTION_TYPE_ID, ViewStaticData.NAME);
        return View();
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_VAT_TYPES_CAN_VIEW)]

    public async Task<IActionResult> Create( ProductVattype productVattype)
    {
        if (ModelState.IsValid)
        {
            try
            {
                productVattype.NameInBangla =productVattype.Name;
                productVattype.CreatedBy = UserSession.UserId;
                productVattype.CreatedTime = DateTime.Now;
                productVattype.IsActive = true;
                productVattype.EffectiveFrom = DateTime.Now; 
                _service.Insert(productVattype);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(productVattype, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.ProductVATType,
                    PrimaryKey = productVattype.ProductVattypeId,
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
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
	            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
        }
        ViewData[ControllerStaticData.PURCHASE_TYPE_ID] = new SelectList(await _purchaseTypeService.Query().SelectAsync(), ControllerStaticData.PURCHASE_TYPE_ID, ViewStaticData.NAME);
        ViewData[ControllerStaticData.SALES_TYPE_ID] = new SelectList(await _salesTypeService.Query().SelectAsync(), ControllerStaticData.SALES_TYPE_ID, ControllerStaticData.SALES_TYPE_NAME);
        ViewData[ControllerStaticData.TRANSACTION_TYPE_ID] = new SelectList(await _transactionTypeService.Query().SelectAsync(), ControllerStaticData.TRANSACTION_TYPE_ID, ViewStaticData.NAME);
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(productVattype);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_VAT_TYPES_CAN_VIEW)]

    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productVattype = await _service.GetProductVattype(id);
        if (productVattype == null)
        {
            return NotFound();
        }
        ViewData[ControllerStaticData.PURCHASE_TYPE_ID] = new SelectList(await _purchaseTypeService.Query().SelectAsync(), ControllerStaticData.PURCHASE_TYPE_ID, ViewStaticData.NAME);
        ViewData[ControllerStaticData.SALES_TYPE_ID] = new SelectList(await _salesTypeService.Query().SelectAsync(), ControllerStaticData.SALES_TYPE_ID, ControllerStaticData.SALES_TYPE_NAME);
        ViewData[ControllerStaticData.TRANSACTION_TYPE_ID] = new SelectList(await _transactionTypeService.Query().SelectAsync(), ControllerStaticData.TRANSACTION_TYPE_ID, ViewStaticData.NAME);

        productVattype.EncryptedId = id;
        return View(productVattype);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_VAT_TYPES_CAN_VIEW)]

    public async Task<IActionResult> Edit(string id, ProductVattype productVattype)
    {
        int productVatTypeId = int.Parse(IvatDataProtector.Unprotect(id));
        if (productVatTypeId != productVattype.ProductVattypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var proVat = await _service.Query().SingleOrDefaultAsync(p => p.ProductVattypeId == productVatTypeId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(proVat, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                proVat.Name = productVattype.Name;
                proVat.DefaultVatPercent = productVattype.DefaultVatPercent;
                    
                _service.Update(proVat);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(proVat, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.ProductVATType,
                    PrimaryKey = proVat.ProductVattypeId,
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
            catch (Exception ex)
            {
	            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
            return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(productVattype);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_VAT_TYPES_CAN_VIEW)]

    public async Task<IActionResult> Delete(int? id)
    {
        var proV = await _service.Query().SingleOrDefaultAsync(p => p.ProductVattypeId == id, CancellationToken.None);
        proV.EffectiveTo = DateTime.Now; 
        proV.IsActive = false;
        _service.Update(proV);
        await UnitOfWork.SaveChangesAsync();
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;
        return RedirectToAction(nameof(Index));
    }
    public async Task<JsonResult> GetAllProductType(bool value)
    {
        var vatTypes = new List<SpGetVatType>
        {
            new SpGetVatType
            {
                ProductVATTypeId = 0,
                VatTypeName = ""
            }
        };
        List<SpGetVatType> objVatTypes=null;
        if (value)
        {
            objVatTypes = await _autocompleteService.GetProductVatType(false, false, false, true, false);
        }
        else
        {
            objVatTypes = await _autocompleteService.GetProductVatType(false, false, true, false, false);

        }

        if (objVatTypes.Any())
        {
            vatTypes.AddRange(objVatTypes);
        }
        return new JsonResult(vatTypes.Select(x => new
        {
            Id = x.ProductVATTypeId,
            Name = x.VatTypeName
        }).ToList());
    }
    public async Task<JsonResult> GetAllProductTypeForVDS(bool value)
    {
        var vatTypes = new List<SpGetVatType>
        {
            new SpGetVatType
            {
                ProductVATTypeId = 0,
                VatTypeName = ""
            }
        };
        List<SpGetVatType> objVatTypes = null;
        if (value)
        {
            objVatTypes = await _autocompleteService.GetProductVatType(false, false, true, false, true);
        }
        else
        {
            objVatTypes = await _autocompleteService.GetProductVatType(false, false, true, false, false);

        }

        if (objVatTypes.Any())
        {
            vatTypes.AddRange(objVatTypes);
        }
        return new JsonResult(vatTypes.Select(x => new
        {
            Id = x.ProductVATTypeId,
            Name = x.VatTypeName
        }).ToList());
    }

    public async Task<JsonResult> GetAllProductTypeSale(bool value)
    {
        var vatTypes = new List<SpGetVatType>
        {
            new SpGetVatType
            {
                ProductVATTypeId = 0,
                VatTypeName = ""
            }
        };
        List<SpGetVatType> objVatTypes = null;
        if (value)
        {
            objVatTypes = await _autocompleteService.GetProductVatType(false, true, false, false, false);
        }
        else
        {
            objVatTypes = await _autocompleteService.GetProductVatType(true, false, false, false, false);

        }

        if (objVatTypes.Any())
        {
            vatTypes.AddRange(objVatTypes);
        }
        return new JsonResult(vatTypes.Select(x => new
        {
            Id = x.ProductVATTypeId,
            Name = x.VatTypeName
        }).ToList());
    }
    public async Task<JsonResult> GetAllProductTypeForSaleVDS(bool value)
    {
        var vatTypes = new List<SpGetVatType>
        {
            new SpGetVatType
            {
                ProductVATTypeId = 0,
                VatTypeName = ""
            }
        };
        List<SpGetVatType> objVatTypes = null;
        if (value)
        {
            objVatTypes = await _autocompleteService.GetProductVatType(true, false, false, false, true);
        }
        else
        {
            objVatTypes = await _autocompleteService.GetProductVatType(true, false, false, false, false);

        }

        if (objVatTypes.Any())
        {
            vatTypes.AddRange(objVatTypes);
        }
        return new JsonResult(vatTypes.Select(x => new
        {
            Id = x.ProductVATTypeId,
            Name = x.VatTypeName
        }).ToList());
    }
}