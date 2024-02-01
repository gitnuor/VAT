using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vms.entity.models;
using vms.Utility;
using vms.entity.viewModels;
using vms.utility.StaticData;
using Newtonsoft.Json;
using vms.entity.Enums;
using Microsoft.AspNetCore.DataProtection;
using vms.service.Services.ProductService;

namespace vms.Controllers;

public class ProductCategoriesController : ControllerBase
{
    private readonly IProductCategoryService _service;
        

    public ProductCategoriesController(ControllerBaseParamModel controllerBaseParamModel, IProductCategoryService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }


    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var productcategory = await _service.GetPCategories(UserSession.OrganizationId);

        return View(productcategory);
          
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productCategory = await _service.Query().SingleOrDefaultAsync(
            m => m.ProductCategoryId == id && m.OrganizationId == UserSession.OrganizationId, CancellationToken.None);
        if (productCategory == null)
        {
            return NotFound();
        }

        return View(productCategory);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_ADD)]
    public IActionResult Create()
    {
        return View();
    }

       
    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_ADD)]
    public async Task<IActionResult> Create(ProductCategory productCategory)
    {
        if (ModelState.IsValid)
        {
            productCategory.ReferenceKey = productCategory.ReferenceKey;
            productCategory.IsActive = true;
            productCategory.OrganizationId = UserSession.OrganizationId;
            productCategory.CreatedBy = UserSession.UserId;
            productCategory.CreatedTime = DateTime.Now;
            _service.Insert(productCategory);
            await UnitOfWork.SaveChangesAsync();

            var pcObj = JsonConvert.SerializeObject(productCategory, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }); ;


            AuditLog cu = new AuditLog();

            cu.ObjectTypeId = (int)EnumObjectType.ProductCategories;
            cu.PrimaryKey = productCategory.ProductCategoryId;
            cu.AuditOperationId = (int)EnumOperations.Add;
            cu.UserId = UserSession.UserId;
            cu.Datetime = DateTime.Now;
            cu.Descriptions = pcObj.ToString();
            cu.IsActive = true;
            cu.CreatedBy = UserSession.UserId;
            cu.CreatedTime = DateTime.Now;
            cu.OrganizationId = UserSession.OrganizationId;


            var pcadd_status = await AuditLogCreate(cu);


            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME
                ;
            return RedirectToAction(nameof(Index));
        }
        return View(productCategory);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productCategory =  await _service.GetPCat(id);

        if (productCategory == null)
        {              
            return NotFound();                          
        }

        productCategory.EncryptedId = id;
        return View(productCategory);
    }

        
    [HttpPost]
         
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_EDIT)]
    public async Task<IActionResult> Edit(ProductCategory productCategory, string id)
    {
        int proCatId = int.Parse(IvatDataProtector.Unprotect(id));

        if (proCatId != productCategory.ProductCategoryId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            { var productCat = await _service.Query().SingleOrDefaultAsync(
                    m => m.ProductCategoryId == proCatId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(productCat, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                productCat.Name = productCategory.Name;
                productCat.ReferenceKey = productCategory.ReferenceKey;
                _service.Update(productCat);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(productCat, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                await IvatLogChanges(EnumObjectType.ProductCategories, productCat.ProductCategoryId,
                    EnumOperations.Edit, prevObj, jObj);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
            }
            catch (Exception)
            {
                //skip
            }
            return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ViewStaticData.DISPLAY_PRODUCT_CATEGORIES);
        }
            
        return View(productCategory);
    }
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_CATEGORY_CAN_DELETE)]
    public async Task<IActionResult> ChangeProductCategoryStatus(string id)
    {
        var productData = await _service.Query().SingleOrDefaultAsync(p => p.ProductCategoryId == int.Parse(IvatDataProtector.Unprotect(id)), CancellationToken.None);
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

            _service.Update(productData);
            await UnitOfWork.SaveChangesAsync();

            AuditLog pc_del = new AuditLog();

            pc_del.ObjectTypeId = (int)EnumObjectType.ProductCategories;
            pc_del.PrimaryKey = productData.ProductCategoryId;
            pc_del.AuditOperationId = (int)EnumOperations.Delete;
            pc_del.UserId = UserSession.UserId;
            pc_del.Datetime = DateTime.Now;
            pc_del.Descriptions = "IsActive:0";
            pc_del.IsActive = true;
            pc_del.CreatedBy = UserSession.UserId;
            pc_del.CreatedTime = DateTime.Now;
            pc_del.OrganizationId = UserSession.OrganizationId;


            var pcdel_status = await AuditLogCreate(pc_del);

        }
        catch (Exception ex)
        {
	        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
		}           
        return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ViewStaticData.DISPLAY_PRODUCT_CATEGORIES);
            
    }

       
}