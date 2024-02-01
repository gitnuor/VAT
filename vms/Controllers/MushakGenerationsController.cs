using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.service.Services.MushakService;
using vms.service.Services.SettingService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class MushakGenerationsController : ControllerBase
{
    private readonly IMushakGenerationService _service;
    private readonly INbrEconomicCodeService _economicCode;
    private readonly IBankBranchService _branchService;
    private readonly IBankService _bankService;
    public MushakGenerationsController(IBankService bankService, IBankBranchService branchService, INbrEconomicCodeService economicCode, ControllerBaseParamModel controllerBaseParamModel, IMushakGenerationService service) : base(controllerBaseParamModel)
    {
        _branchService = branchService;
        _service = service;
        _economicCode = economicCode;
        _bankService = bankService;
    }

    // [VmsAuthorize(FeatureList.ADMINSTRATION)]
    // public async Task<IActionResult> Index(int? page, string search = null)
    // {
    //     var data = await _service.GetMushakGenerations(UserSession.OrganizationId);
    //     ViewBag.PageCount = data.Count();
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         data = data.Where(c => c.MushakForMonth.ToString().ToLower().Contains(search) ||
    //                                c.MushakForYear.ToString().ToLower().Contains(search) ||
    //                                c.GenerateDate.ToString().Contains(search));
    //     }
    //
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //
    //     }
    //
    //         
    //     var pageNumber = page ?? 1;
    //     var listOfdata = data.ToPagedList(pageNumber, 10);
    //     return View(data);
    //
    // }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mushakGeneration = await _service.Query()
            .SingleOrDefaultAsync(m => m.MushakGenerationId == id, CancellationToken.None);
        if (mushakGeneration == null)
        {
            return NotFound();
        }

        return View(mushakGeneration);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    public async Task<IActionResult> Create()
    {
        SpAddMushakReturnBasicInfo mushok = new SpAddMushakReturnBasicInfo();
        var data = await _service.Query().SelectAsync();

        ViewBag.Months = new SelectList(Enumerable.Range(1, 12).Select(x =>
            new SelectListItem()
            {
                Text = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[x - 1], 
                Value = x.ToString()
            }), ControllerStaticData.VALUE, ControllerStaticData.TEXT_CLASSNAME);

        ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year, 20).Select(x =>
            new SelectListItem()
            {
                Text = x.ToString(),
                Value = x.ToString()
            }), ControllerStaticData.VALUE, ControllerStaticData.TEXT_CLASSNAME);


        mushok.Year = 2019;
        return View(mushok);
    }
    // [HttpPost]
    // [VmsAuthorize(FeatureList.ADMINSTRATION)]
    // public async Task<IActionResult> Create(SpAddMushakReturnBasicInfo mushakGeneration)
    // {
    //     if (ModelState.IsValid)
    //     {
    //
    //         try
    //         {
    //             mushakGeneration.OrganizationId = UserSession.OrganizationId;
    //             mushakGeneration.GenerateDate = DateTime.Now;
    //             mushakGeneration.IsWantToGetBackClosingAmount = true;
    //             var rawAffected = await _service.InsertMushak(mushakGeneration);
    //             var jObj = JsonConvert.SerializeObject(mushakGeneration, Formatting.None,
    //                 new JsonSerializerSettings()
    //                 {
    //                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //                 });
    //
    //
    //             AuditLog au = new AuditLog
    //             {
    //                 ObjectTypeId = (int)EnumObjectType.MushakGeneration,
    //                 PrimaryKey = mushakGeneration.OrganizationId,
    //                 AuditOperationId = (int)EnumOperations.Add,
    //                 UserId = UserSession.UserId,
    //                 Datetime = DateTime.Now,
    //                 Descriptions = jObj.ToString(),
    //                 IsActive = true,
    //                 CreatedBy = UserSession.UserId,
    //                 CreatedTime = DateTime.Now,
    //                 OrganizationId = UserSession.OrganizationId
    //             };
    //             await AuditLogCreate(au);
    //             TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //             return RedirectToAction(nameof(Index));
    //         }
    //         catch (Exception e)
    //         {
    //             
    //             
    //         }
    //
    //     }
    //     ViewBag.Months = new SelectList(Enumerable.Range(1, 12).Select(x =>
    //         new SelectListItem()
    //         {
    //             Text = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[x - 1], 
    //             Value = x.ToString()
    //         }), ControllerStaticData.VALUE, ControllerStaticData.TEXT_CLASSNAME);
    //
    //     ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year, 20).Select(x =>
    //         new SelectListItem()
    //         {
    //             Text = x.ToString(),
    //             Value = x.ToString()
    //         }), ControllerStaticData.VALUE, ControllerStaticData.TEXT_CLASSNAME);
    //
    //
    //     mushakGeneration.Year = 2019;
    //     return View(mushakGeneration);
    // }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mushakGeneration = await _service.Query()
            .SingleOrDefaultAsync(m => m.MushakGenerationId == id, CancellationToken.None);
        if (mushakGeneration == null)
        {
            return NotFound();
        }

        return View(mushakGeneration);
    }


    // [HttpPost]
    // [VmsAuthorize(FeatureList.ADMINSTRATION)]
    // public async Task<IActionResult> Edit(int id, MushakGeneration mushakGeneration)
    // {
    //     if (id != mushakGeneration.MushakGenerationId)
    //     {
    //         return NotFound();
    //     }
    //
    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             _service.Update(mushakGeneration);
    //             TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME
    //                 ;
    //             await UnitOfWork.SaveChangesAsync();
    //         }
    //         catch (Exception ex)
    //         {
    //
    //         }
    //         return RedirectToAction(nameof(Index));
    //     }
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //     return View(mushakGeneration);
    // }

      
    // [VmsAuthorize(FeatureList.ADMINSTRATION)]
    // public async Task<IActionResult> Delete(int? id)
    // {
    //     if (id == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var mushakGeneration = await _service.Query()
    //         .SingleOrDefaultAsync(m => m.MushakGenerationId == id, CancellationToken.None);
    //
    //     if (mushakGeneration == null)
    //     {
    //         return NotFound();
    //     }
    //     TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;
    //     return RedirectToAction(nameof(Index));
    // }
    //
    // public async Task<IActionResult> MushakReturnPlanToPaymentInfo(string id)
    // {
    //     int mushakGenerationID = int.Parse(IvatDataProtector.Unprotect(id));
    //     SpAddMushakReturnPlanToPaymentInfo model = new SpAddMushakReturnPlanToPaymentInfo();
    //     var data = await _service.GetMushakGeneration(id);
    //     model.MushakGenerationId = mushakGenerationID;
    //     var createdBy = UserSession.UserId;
    //     var organizationId = UserSession.OrganizationId;
    //         
    //
    //     ViewData["NbrEconomicCodeId"] = new SelectList(await _economicCode.Query().Where(c => c.NbrEconomicCodeTypeId == 1).SelectAsync(), "NbrEconomicCodeId", "EconomicTitle");
    //     ViewData["BankBranchId"] = new SelectList(await _branchService.Query().SelectAsync(), "BankBranchId", "Name");
    //     return View(model);
    //
    // }
    // [HttpPost]
    //
    //    
    // public async Task<IActionResult> MushakReturnPlanToPaymentInfo(SpAddMushakReturnPlanToPaymentInfo model)
    // {
    //
    //     try
    //     {
    //
    //         var rawAffected = await _service.InsertReturnPlanToPaymentInfo(model);
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //         return RedirectToAction(nameof(Index));
    //     }
    //     catch (Exception e)
    //     {
    //         model.MushakGenerationId = model.MushakGenerationId;
    //         var createdBy = UserSession.UserId;
    //         var organizationId = UserSession.OrganizationId;
    //         ViewData["NbrEconomicCodeId"] = new SelectList(await _economicCode.Query().Where(c => c.NbrEconomicCodeTypeId == 1).SelectAsync(), "NbrEconomicCodeId", "EconomicTitle");
    //         ViewData["BankBranchId"] = new SelectList(await _branchService.Query().SelectAsync(), "BankBranchId", "Name");
    //         return View(model);
    //     }
    //
    // }


    public async Task<IActionResult> AddMushakReturnPaymentInfo(string id)
    {
        int mushakGenerationID = int.Parse(IvatDataProtector.Unprotect(id));
        SpAddMushakReturnPaymentInfo model = new SpAddMushakReturnPaymentInfo();
        model.MushakGenerationId = mushakGenerationID;
        var createdBy = UserSession.UserId;
        var organizationId = UserSession.OrganizationId;
        ViewData["NbrEconomicCodeId"] = new SelectList(await _economicCode.Query().Where(c => c.NbrEconomicCodeTypeId == 1).SelectAsync(), "NbrEconomicCodeId", "EconomicTitle");
        ViewData["BankBranchId"] = new SelectList(await _branchService.Query().SelectAsync(), "BankBranchId", "Name");
        return View(model);

    }

    // [HttpPost]
    // public async Task<IActionResult> AddMushakReturnPaymentInfo(SpAddMushakReturnPaymentInfo model)
    // {
    //
    //     try
    //     {
    //
    //         var rawAffected = await _service.InsertReturnPaymentInfo(model);
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //         return RedirectToAction(nameof(Index));
    //     }
    //     catch (Exception e)
    //     {
    //         model.MushakGenerationId = model.MushakGenerationId;
    //         var createdBy = UserSession.UserId;
    //         var organizationId = UserSession.OrganizationId;
    //         ViewData["NbrEconomicCodeId"] = new SelectList(await _economicCode.Query().Where(c => c.NbrEconomicCodeTypeId == 1).SelectAsync(), "NbrEconomicCodeId", "EconomicTitle");
    //         ViewData["BankBranchId"] = new SelectList(await _branchService.Query().SelectAsync(), "BankBranchId", "Name");
    //         return View(model);
    //     }
    //
    // }

    // public async Task<IActionResult> AddMushakReturnSubmissionInfo(string id)
    // {
    //     int mushakGenerationID = int.Parse(IvatDataProtector.Unprotect(id));
    //     SpAddMushakReturnSubmissionInfo model = new SpAddMushakReturnSubmissionInfo();
    //     model.MushakGenerationId = mushakGenerationID;
    //     var createdBy = UserSession.UserId;
    //     var organizationId = UserSession.OrganizationId;
    //     model.SubmissionDate = DateTime.Now;
    //     return await Task.Run(() => View(model));
    //
    // }
    //
    // [HttpPost]
    // public async Task<IActionResult> AddMushakReturnSubmissionInfo(SpAddMushakReturnSubmissionInfo model)
    // {
    //     try
    //     {
    //         var rawAffected = await _service.InsertSubmissionInfo(model);
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //         return RedirectToAction(nameof(Index));
    //     }
    //     catch (Exception e)
    //     {
    //         model.MushakGenerationId = model.MushakGenerationId;
    //         var createdBy = UserSession.UserId;
    //         var organizationId = UserSession.OrganizationId;
    //         ViewData["NbrEconomicCodeId"] = new SelectList(await _economicCode.Query().Where(c => c.NbrEconomicCodeTypeId == 1).SelectAsync(), "NbrEconomicCodeId", "EconomicTitle");
    //         ViewData["BankBranchId"] = new SelectList(await _branchService.Query().SelectAsync(), "BankBranchId", "Name");
    //         return View(model);
    //     }
    //
    // }

    // public async Task<IActionResult> AddMushakReturnReturnedAmountInfo(string id)
    // {
    //     int mushakGenerationID = int.Parse(IvatDataProtector.Unprotect(id));
    //     SpAddMushakReturnReturnedAmountInfo model = new SpAddMushakReturnReturnedAmountInfo();
    //     model.MushakGenerationId = mushakGenerationID;
    //     var createdBy = UserSession.UserId;
    //     var organizationId = UserSession.OrganizationId;
    //     ViewData["BankBranchId"] = new SelectList(await _bankService.Query().SelectAsync(), "BankId", "NameInBangla");
    //
    //     return View(model);
    //
    // }
    //
    // [HttpPost]
    // public async Task<IActionResult> AddMushakReturnReturnedAmountInfo(SpAddMushakReturnReturnedAmountInfo model)
    // {
    //     try
    //     {
    //         var rawAffected = await _service.InsertReturnReturnedAmountInfo(model);
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //         return RedirectToAction(nameof(Index));
    //     }
    //     catch (Exception e)
    //     {
    //         model.MushakGenerationId = model.MushakGenerationId;
    //         var createdBy = UserSession.UserId;
    //         var organizationId = UserSession.OrganizationId;
    //         ViewData["BankBranchId"] = new SelectList(await _bankService.Query().SelectAsync(), "BankId", "NameInBangla");
    //         return View(model);
    //     }
    //
    // }

    // public async Task<IActionResult> AddMushakReturnCompleteProcess(string id)
    // {
    //     int mushakGenerationID = int.Parse(IvatDataProtector.Unprotect(id));
    //     try
    //     {
    //         var rawAffected = await _service.InsertAddMushakReturnCompleteProcess(mushakGenerationID);
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
    //         return RedirectToAction(nameof(Index));
    //     }
    //     catch (Exception e)
    //     {
    //         TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
    //         return RedirectToAction(nameof(Index));
    //     }
    //
    // }

}