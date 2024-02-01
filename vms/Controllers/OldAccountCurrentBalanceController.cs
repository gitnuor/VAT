using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.PaymentService;
using vms.utility.StaticData;
using vms.Utility;


namespace vms.Controllers;

public class OldAccountCurrentBalanceController : ControllerBase
{

    private readonly IOldAccountCurrentBalanceService _service;

    public OldAccountCurrentBalanceController(IOldAccountCurrentBalanceService service, ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
    {

        _service = service;
    }

    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_REMAINING_AMOUNT_FROM_MUSHAK_18_6_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var itemList = await _service.Query().Where(w => w.OrganizationId == UserSession.OrganizationId).SelectAsync();
        itemList = itemList.OrderByDescending(x => x.OldAccountCurrentBalanceId);
        foreach (var item in itemList)
        {
            item.MonthName = ((EnumMonth)item.MushakMonth).ToString();
        }
        return View(itemList);
    }

    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_REMAINING_AMOUNT_FROM_MUSHAK_18_6_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_REMAINING_AMOUNT_FROM_MUSHAK_18_6_CAN_ADD)]
    public IActionResult Create()
    {
        var model = new vmOldAccountCurrentBalance();
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
        model.MushakMonth = Convert.ToInt32(currentDate.Month);
        model.MushakYear = Convert.ToInt32(currentDate.Year);
        return View(model);
    }
    [HttpPost]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_REMAINING_AMOUNT_FROM_MUSHAK_18_6_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_REMAINING_AMOUNT_FROM_MUSHAK_18_6_CAN_ADD)]
    public async Task<IActionResult> Create(OldAccountCurrentBalance odlBalance)
    {
        try
        {
            odlBalance.OrganizationId = UserSession.OrganizationId;
            if (!await _service.IsMonthlyCurrentBalanceExist(odlBalance))
            {
                if (odlBalance.RemainingVatbalance + odlBalance.RemainingSdbalance > 0)
                {
                    odlBalance.CreatedBy = UserSession.UserId;
                    odlBalance.CreatedTime = DateTime.Now;
                    _service.Insert(odlBalance);
                    await UnitOfWork.SaveChangesAsync();
                    TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;

                    var jObj = JsonConvert.SerializeObject(odlBalance, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });


                    AuditLog au = new AuditLog();
                    au.ObjectTypeId = (int)EnumObjectType.OldAccountCurrentBalance;
                    au.PrimaryKey = odlBalance.OldAccountCurrentBalanceId;
                    au.AuditOperationId = (int)EnumOperations.Add;
                    au.UserId = UserSession.UserId;
                    au.Datetime = DateTime.Now;
                    au.Descriptions = jObj.ToString();
                    au.IsActive = true;
                    au.CreatedBy = UserSession.UserId;
                    au.CreatedTime = DateTime.Now;
                    au.OrganizationId = UserSession.OrganizationId;
                    await AuditLogCreate(au);

                    return RedirectToAction(nameof(Index));
                        
                }
                ModelState.AddModelError("", "SUM of VAT and SD should be greater than Zero");
            }
            else
            {
                ModelState.AddModelError("", "Same Month Balance Add Not Allow");
            }
        }
        catch (Exception ex)
        {
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
		}
        var model = new vmOldAccountCurrentBalance();
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
        model.MushakMonth = Convert.ToInt32(currentDate.Month);
        model.MushakYear = Convert.ToInt32(currentDate.Year);
        model.MushakYear = odlBalance.MushakYear;
        model.MushakMonth = odlBalance.MushakMonth;
        model.RemainingVatbalance = odlBalance.RemainingVatbalance;
        model.RemainingSdbalance = odlBalance.RemainingSdbalance;
        return View(model);
    }
}