using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.SettingService;
using vms.service.Services.TransactionService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class AdjustmentController : ControllerBase
{
    private readonly IAdjustmentService _adjustment;
    private readonly IAdjustmentTypeService _adjustmentType;

    public AdjustmentController(IAdjustmentService adjustment, IAdjustmentTypeService adjustmentType, ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
    {
        _adjustment = adjustment;
        _adjustmentType = adjustmentType;
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MISCELLANEOUS_ADJUSTMENT_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        return View(await _adjustment.GetByOrg(UserSession.ProtectedOrganizationId));
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MISCELLANEOUS_ADJUSTMENT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MISCELLANEOUS_ADJUSTMENT_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        var model = new vmAdjustment();

        var adTy = await _adjustmentType.GetAdjustmentTypeSelectList();
        model.AdjustmentTypes = adTy;

        var currentDate = DateTime.Today;
        int currentYear = Convert.ToInt32(currentDate.Year);
        var previousYears = new List<SelectListItem>();
        for (int i = 0; i < 10; i++)
        {
            previousYears.Add(new SelectListItem
            {
                Value = currentYear.ToString(),
                Text = currentYear.ToString()
            });
            currentYear--;
        }

        var months = new List<SelectListItem>();
        for (int i = 1; i <= 12; i++)
        {
            months.Add(new SelectListItem
            {
                Value = i.ToString(),
                Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)
            });
        }

        model.YearList = previousYears;
        model.MonthList = months;
        model.Month = Convert.ToInt32(currentDate.Month);
        model.Year = Convert.ToInt32(currentDate.Year);

        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MISCELLANEOUS_ADJUSTMENT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MISCELLANEOUS_ADJUSTMENT_CAN_ADD)]
    public async Task<IActionResult> Create(Adjustment ad)
    {
        var jObj = "";
        try
        {
            ad.OrganizationId = UserSession.OrganizationId;
            ad.IsActive = true;
            ad.CreatedBy = UserSession.UserId;
            ad.CreatedTime = DateTime.Now;
            _adjustment.Insert(ad);
            await UnitOfWork.SaveChangesAsync();

            jObj = JsonConvert.SerializeObject(ad, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.Adjustment,
                PrimaryKey = ad.AdjustmentId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = jObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };

            await AuditLogCreate(au);
        }
        catch
        {
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        }

        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;

        return RedirectToAction(nameof(Index));
    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MISCELLANEOUS_ADJUSTMENT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_MISCELLANEOUS_ADJUSTMENT_CAN_DELETE)]
    public async Task<IActionResult> Delete(int? id)
    {
        var Adj = await _adjustment.Query().SingleOrDefaultAsync(p => p.AdjustmentId == id, CancellationToken.None);

        if (Adj.IsActive == true)
        {
            Adj.IsActive = false;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;

        }
        else
        {
            Adj.IsActive = true;
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;

        }
        _adjustment.Update(Adj);
        await UnitOfWork.SaveChangesAsync();

        AuditLog au = new AuditLog
        {
            ObjectTypeId = (int)EnumObjectType.Adjustment,
            PrimaryKey = Adj.AdjustmentId,
            AuditOperationId = (int)EnumOperations.Delete,
            UserId = UserSession.UserId,
            Datetime = DateTime.Now,
            Descriptions = "IsActive:0",
            IsActive = true,
            CreatedBy = UserSession.UserId,
            CreatedTime = DateTime.Now,
            OrganizationId = UserSession.OrganizationId
        };

        await AuditLogCreate(au);
        return RedirectToAction(nameof(Index));
    }
}