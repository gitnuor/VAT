using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.MushakService;
using vms.service.Services.UploadService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class MushakSubmissionsController : ControllerBase
{
    private readonly IMushakSubmissionsService _service;
    private readonly IMushakSubmissionTypeService _typeService;
    private readonly ICRMService _file;
    private IMushakReturnService _mushakReturnService;


    public MushakSubmissionsController(IMushakReturnService mushakReturnService, ControllerBaseParamModel controllerBaseParamModel, ICRMService  file, IWebHostEnvironment hostingEnvironment, IMushakSubmissionsService service, IMushakSubmissionTypeService typeService) : base(controllerBaseParamModel)
    {
        _mushakReturnService = mushakReturnService;
        _service = service;
        _typeService = typeService;
        _file = file;
    }

    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_SUBMISSIONS_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var mushakSubmissions = await _service.Query().Include(m => m.MushakSubmissionType)
            .Include(m => m.Organization)
            .SelectAsync(CancellationToken.None);
        mushakSubmissions = mushakSubmissions.OrderByDescending(x => x.MushakSubmissionId);
        return View(mushakSubmissions);
    }



    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_SUBMISSIONS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_SUBMISSIONS_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        ViewData["MushakSubmissionTypeId"] = new SelectList(await _typeService.Query().SelectAsync(CancellationToken.None), "MushakSubmissionTypeId", "SubmissionTypeName");
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
        vmMushakSubmission model = new vmMushakSubmission();
        model.YearList = previousYears;
        model.MonthList = months;
        model.MushakForMonth = Convert.ToInt32(currentDate.Month);
        model.MushakForYear = Convert.ToInt32(currentDate.Year);
        model.GenerateDate = DateTime.Now;
        return View(model);
    }

        
    [HttpPost]
    [ValidateAntiForgeryToken]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_SUBMISSIONS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_SUBMISSIONS_CAN_ADD)]
    public async Task<IActionResult> Create(MushakSubmission mushakSubmission)
    {
        mushakSubmission.IsActive = true;
        mushakSubmission.CreatedBy = UserSession.UserId;
        mushakSubmission.OrganizationId = UserSession.OrganizationId;
        // VAT Responsible person will be added later.
        mushakSubmission.VatResponsiblePersonId = 0;
        mushakSubmission.CreatedTime=DateTime.Now;
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _mushakReturnService.InsertMushakSubmision(mushakSubmission);

                var jObj = JsonConvert.SerializeObject(mushakSubmission, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog();
                au.ObjectTypeId = (int)EnumObjectType.MushakSubmissions;
                au.PrimaryKey = mushakSubmission.MushakSubmissionId;
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
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> MushakUpload(int id)
    {
        var data = await _service.Query().Include(m => m.MushakSubmissionType)
            .Include(m => m.Organization).
            SingleOrDefaultAsync(x => x.MushakSubmissionId == id, CancellationToken.None);
        if(data !=null)
        {
            vmMushakUpload model = new vmMushakUpload();

            model.Year = data.MushakForYear;
            model.Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(data.MushakForMonth);

            return View(model);
        }
        return NotFound();
           
    }


    //[VmsAuthorizeAttribute(FeatureList.MUSHAK_RETURN)]
    //[VmsAuthorizeAttribute(FeatureList.MUSHAK_RETURN_MUSHAK_SUBMISSIONS_CAN_VIEW)]
    //[VmsAuthorizeAttribute(FeatureList.MUSHAK_RETURN_MUSHAK_SUBMISSIONS_9_1_UPLOAD)]
    //public async Task<IActionResult> MushakUploadAsync(int id)
    //{
    //    var data = await _service.Query().Include(m => m.MushakSubmissionType)
    //       .Include(m => m.Organization).
    //       SingleOrDefaultAsync(x => x.MushakSubmissionId == id, System.Threading.CancellationToken.None);

    //    vmFileUpload model = new vmFileUpload();

    //    model.year = data.MushakForYear;
    //    model.month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(data.MushakForMonth);


    //    return View(model);
    //}


 

       
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_SUBMISSIONS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_SUBMISSIONS_9_1_UPLOAD)]
    [HttpPost]
    public IActionResult MushakUpload(vmMushakUpload model)
    {
        if (ModelState.IsValid)
        {     
            if (model.File != null)
            {
                model.Path = "ApplicationDocument/" + UserSession.OrganizationId.ToString() + "/" + model.Year.ToString() + "/" + model.Month +"/Mushak9_1";

                var upload = new vmFileUpload();
                upload.month = model.Month;
                upload.year = model.Year;
                upload.path = model.Path;
                upload.File = model.File;

                _file.addFile(upload);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;

            }
                 
            return RedirectToAction("Index");
        }

        return View(model);
    }
}