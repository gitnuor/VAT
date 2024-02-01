using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.ProductService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class ProductGroupsController : ControllerBase
{
    private readonly IProductGroupService _service;

    public ProductGroupsController(ControllerBaseParamModel controllerBaseParamModel, IProductGroupService service) : base(controllerBaseParamModel)
    {
        _service = service;
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var Productcategory = await _service.GetProductGroups();
        Productcategory = Productcategory.Where(x => x.OrganizationId == UserSession.OrganizationId);
        return View(Productcategory);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productGroup = await _service.Query()
            .SingleOrDefaultAsync(m => m.ProductGroupId == id, CancellationToken.None);
        if (productGroup == null)
        {
            return NotFound();
        }

        return View(productGroup);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_ADD)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_ADD)]
    public async Task<IActionResult> Create( ProductGroup productGroup)
    {
        if (ModelState.IsValid)
        {
            productGroup.OrganizationId = UserSession.OrganizationId;
            productGroup.CreatedBy = UserSession.UserId;
            productGroup.CreatedTime = DateTime.Now;
            productGroup.IsActive = true;
            productGroup.ReferenceKey = productGroup.ReferenceKey;
            _service.Insert(productGroup);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(productGroup, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.ProductGroup,
                PrimaryKey = productGroup.ProductGroupId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = jObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };

            var au_status = await AuditLogCreate(au);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME
                ;
            return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(productGroup);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productGroup = await _service.GetProductGroup(id);
        if (productGroup == null)
        {
            return NotFound();
        }

        productGroup.EncryptedId = id;
        return View(productGroup);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id, ProductGroup productGroup)
    {
        int productGroupsId = int.Parse(IvatDataProtector.Unprotect(id));
        if (productGroupsId != productGroup.ProductGroupId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var productGroups = await _service.Query().SingleOrDefaultAsync(p => p.ProductGroupId == productGroupsId, CancellationToken.None);
                var prevObj = JsonConvert.SerializeObject(productGroups, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                productGroups.Name = productGroup.Name;
                _service.Update(productGroups);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(productGroups, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.ProductGroup,
                    PrimaryKey = productGroups.ProductGroupId,
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
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME
                    ;
            }
            catch (Exception ex)
            {
	            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
            return RedirectToAction(nameof(Index));
        }
        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        return View(productGroup);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_GROUPS_CAN_DELETE)]
    public async Task<IActionResult> ChangeProductGroupStatus(string id)
    {
        var pG = await _service.Query().SingleOrDefaultAsync(p => p.ProductGroupId == int.Parse(IvatDataProtector.Unprotect(id)), CancellationToken.None);

        if (pG.IsActive == true)
        {
            pG.IsActive = false;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;

        }
        else
        {
            pG.IsActive = true;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;

        }
        _service.Update(pG);
        await UnitOfWork.SaveChangesAsync();
        AuditLog au = new AuditLog
        {
            ObjectTypeId = (int)EnumObjectType.ProductGroup,
            PrimaryKey = pG.ProductGroupId,
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
        return RedirectToAction(nameof(Index));
    }
}