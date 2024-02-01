using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.PaymentService;
using vms.service.Services.SettingService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class MushakReturnRefundController : ControllerBase
{
    private readonly IMushakReturnRefundService _service;
    private readonly IMushakReturnPaymentTypeService _typeService;
    private readonly ICustomsAndVatcommissionarateService _customsAndVatcommissionarateService;
    private readonly IBankBranchService _branchService;
    private readonly IBankService _bankService;

    public MushakReturnRefundController(IBankBranchService branchService, IBankService bankService, ICustomsAndVatcommissionarateService customsAndVatcommissionarateService, IMushakReturnPaymentTypeService typeService, IMushakReturnRefundService service, ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
    {
        _branchService = branchService;
        _bankService = bankService;
        _customsAndVatcommissionarateService = customsAndVatcommissionarateService;
        _service = service;
        _typeService = typeService;
    }
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_VIEW)]
    public async Task<IActionResult> Index(int? page, string search = null)
    {
        var itemList = await _service.Query().Where(c => c.OrganizationId == UserSession.OrganizationId).SelectAsync(CancellationToken.None);

        ViewBag.PageCount = itemList.Count();
        string txt = search;

        if (search != null)
        {
            search = search.ToLower().Trim();
            itemList = itemList.Where(c => c.MushakYear.ToString().Contains(search)
            );
        }

        var pageNumber = page ?? 1;
        var listOfData = itemList.OrderByDescending(x => x.MushakReturnRefundId); //.ToPagedList(pageNumber, 10);
        if (txt != null)
        {
            ViewData[ViewStaticData.SEARCH_TEXT] = txt;
        }
        else
        {
            ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
        }
        return View(listOfData);
    }
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        var model = new vmMushakReturnRefund();

        var bankBranchList = await _branchService.Query().SelectAsync(CancellationToken.None);
        IEnumerable<CustomSelectListItem> branchList = bankBranchList.Select(s => new CustomSelectListItem
        {
            Id = 0, 
            Name = "Have to fix" 
        });

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

        model.BrankBranch = branchList;
        model.YearList = previousYears;
        model.MonthList = months;
        model.MushakMonth = Convert.ToInt32(currentDate.Month);
        model.MushakYear = Convert.ToInt32(currentDate.Year);
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_ADD)]
    public async Task<IActionResult> Create(MushakReturnRefund model)
    {
        model.CreatedBy = UserSession.UserId;
        model.CreatedTime = DateTime.Now;
        model.OrganizationId = UserSession.OrganizationId;
            
        _service.Insert(model);
        try
        {
            await UnitOfWork.SaveChangesAsync();
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;

            var jObj = JsonConvert.SerializeObject(model, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog();
            au.ObjectTypeId = (int)EnumObjectType.MushakReturnRefund;
            au.PrimaryKey = model.MushakReturnRefundId;
            au.AuditOperationId = (int)EnumOperations.Add;
            au.UserId = UserSession.UserId;
            au.Datetime = DateTime.Now;
            au.Descriptions = jObj.ToString();
            au.IsActive = true;
            au.CreatedBy = UserSession.UserId;
            au.CreatedTime = DateTime.Now;
            au.OrganizationId = UserSession.OrganizationId;
            await AuditLogCreate(au);

		}
        catch (Exception ex)
        {
	        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
        }
		return RedirectToAction(nameof(Index));
	}
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_ADD_APPROVED_REFUND)]
    public IActionResult AddRefund(int id)
    {
        var model = new MushakReturnRefund();
        model.MushakReturnRefundId = id;
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_ADD_APPROVED_REFUND)]
    public async Task<IActionResult> AddRefund(MushakReturnRefund model)
    {
        var data = await _service.Query().SingleOrDefaultAsync(c => c.MushakReturnRefundId == model.MushakReturnRefundId, CancellationToken.None);
        data.RefundedVatamount = model.RefundedVatamount;
        data.RefundedVatchequeNo = model.RefundedVatchequeNo;
        data.RefundedSdchequeDate = model.RefundedSdchequeDate;
        data.RefundedSdamount = model.RefundedSdamount;
        data.RefundedSdchequeNo = model.RefundedSdchequeNo;
        data.RefundedSdchequeDate = model.RefundedSdchequeDate;
        _service.Update(data);
        try
        {
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(model, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog();
            au.ObjectTypeId = (int)EnumObjectType.MushakReturnRefund;
            au.PrimaryKey = model.MushakReturnRefundId;
            au.AuditOperationId = (int)EnumOperations.Add;
            au.UserId = UserSession.UserId;
            au.Datetime = DateTime.Now;
            au.Descriptions = jObj.ToString();
            au.IsActive = true;
            au.CreatedBy = UserSession.UserId;
            au.CreatedTime = DateTime.Now;
            au.OrganizationId = UserSession.OrganizationId;
            await AuditLogCreate(au);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			return RedirectToAction(nameof(Index));
        }
    }
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_ADD_REFUND_INFO)]
    public IActionResult AddRefused(int id)
    {
        var model = new MushakReturnRefund();
        model.MushakReturnRefundId = id;
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_REFUND_CAN_ADD_REFUND_INFO)]
    public async Task<IActionResult> AddRefused(MushakReturnRefund model)
    {
        var data = await _service.Query().SingleOrDefaultAsync(c => c.MushakReturnRefundId == model.MushakReturnRefundId, CancellationToken.None);
        data.ApprovedToRefundVatamount = model.ApprovedToRefundVatamount;
        data.ApprovedToRefundSdamount = model.ApprovedToRefundSdamount;
        _service.Update(data);
        try
        {
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(model, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog();
            au.ObjectTypeId = (int)EnumObjectType.MushakReturnRefund;
            au.PrimaryKey = model.MushakReturnRefundId;
            au.AuditOperationId = (int)EnumOperations.Add;
            au.UserId = UserSession.UserId;
            au.Datetime = DateTime.Now;
            au.Descriptions = jObj.ToString();
            au.IsActive = true;
            au.CreatedBy = UserSession.UserId;
            au.CreatedTime = DateTime.Now;
            au.OrganizationId = UserSession.OrganizationId;
            await AuditLogCreate(au);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			return RedirectToAction(nameof(Index));
        }
    }
}