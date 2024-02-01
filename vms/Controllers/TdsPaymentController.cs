using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.Payment;
using vms.service.Services.PaymentService;
using vms.service.Services.SettingService;
using vms.service.Services.TransactionService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class TdsPaymentController : ControllerBase
{
    private readonly IMushakReturnPaymentService _service;
    private readonly ITdsPaymentService _tdsPaymentService;
    private readonly IMushakReturnPaymentTypeService _typeService;
    private readonly ICustomsAndVatcommissionarateService _customsAndVatcommissionarateService;
    private readonly IBankBranchService _branchService;
    private readonly IBankService _bankService;
    private readonly IPurchaseService _purchaseService;
    private readonly ICountryService _countryService;
    private readonly IDistrictOrCityService _districtOrCityService;

    public TdsPaymentController(IBankBranchService branchService, IBankService bankService, ICustomsAndVatcommissionarateService customsAndVatcommissionarateService, IMushakReturnPaymentTypeService typeService, IMushakReturnPaymentService service, ITdsPaymentService tdsPaymentService, ControllerBaseParamModel controllerBaseParamModel, IPurchaseService purchaseService,
        ICountryService countryService,
        IDistrictOrCityService districtOrCityService
    ) : base(controllerBaseParamModel)
    {
        _branchService = branchService;
        _bankService = bankService;
        _customsAndVatcommissionarateService = customsAndVatcommissionarateService;
        _service = service;
        _tdsPaymentService = tdsPaymentService;
        _typeService = typeService;
        _purchaseService = purchaseService;
        _countryService = countryService;
        _districtOrCityService = districtOrCityService;
    }


    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_PAYMENT_CAN_VIEW)]
    public IActionResult Index()
    {
        return View(_tdsPaymentService.GetTdsPayments(UserSession.OrganizationId));
    }

    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_PAYMENT_CAN_VIEW)]
    public async Task<IActionResult> TdsPaymentChallan(int id)
    {
        try
        {
            var model = new TdsPaymentChallanViewModel();
            model.ChallanList = await _tdsPaymentService.TdsPaymentChallan(id);
            if (model.ChallanList.Any())
            {
                model.TdsPaymentId = id;
                return View(model);
            }
            return NotFound();
        }

		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}


    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_PAYMENT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_PAYMENT_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        var model = new VmTdsPayment();
        var typeList = await _typeService.Query().SelectAsync(CancellationToken.None);
        var customVatCommission =
            await _customsAndVatcommissionarateService.Query().SelectAsync(CancellationToken.None);
        var banks = await _bankService.GetBankSelectListItemByOrg(UserSession.OrganizationId);
        var country = await _countryService.CountrySelectList();
        var districtorCity = await _districtOrCityService.GetDistrictOrCitySelectList(UserSession.OrganizationId);
        

        IEnumerable<CustomSelectListItem> customVatCommissionList = customVatCommission.Select(s => new CustomSelectListItem
        {
            Id = s.CustomsAndVatcommissionarateId,
            Name = s.Name
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
        //todo: have to manage
        model.BankBranchCountryId = 14;
        model.BankList = banks;
        model.CustomsAndVATCommissionarate = await _customsAndVatcommissionarateService.CustomsAndVatcommissionarates();
        model.YearList = previousYears;
        model.MonthList = months;
        model.MushakMonth = Convert.ToInt32(currentDate.Month);
        model.MushakYear = Convert.ToInt32(currentDate.Year);
        model.purchaseList = await _purchaseService.GetVdsPurchasesWithDueTdsPayment(UserSession.OrganizationId);
        model.CountryList = country;

        model.DistrictOrCItyList = districtorCity;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VmTdsPaymentPost tdsPayment)
    {
        tdsPayment.CreatedBy = UserSession.UserId;
        tdsPayment.CreatedTime =DateTime.Now;
        tdsPayment.OrganizationId =UserSession.OrganizationId;
        int insertId = await _tdsPaymentService.InsertTdsPayment(tdsPayment);
        return Json(1);
    }

    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_PAYMENT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_PAYMENT_CAN_ADD_CHALAN_NO)]
    public IActionResult AddChalan(int id)
    {
        var model = new vmAddChallanForMushakReturnPayment();
        model.MushakReturnPaymentId = id;
        model.SubmissionDate = DateTime.Now;
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_PAYMENT_CAN_VIEW)]
    [VmsAuthorize(FeatureList.MUSHAK_RETURN_MUSHAK_RETURN_PAYMENT_CAN_ADD_CHALAN_NO)]
    public async Task<IActionResult> AddChalan(vmAddChallanForMushakReturnPayment model)
    {
        if (ModelState.IsValid)
        {

            var data = await _tdsPaymentService.Query()
                .SingleOrDefaultAsync(c => c.TdsPaymentId == model.MushakReturnPaymentId,
                    CancellationToken.None);
            data.TreasuryChallanNo = model.TreasuryChallanNo;
            data.SubimissionDate = model.SubmissionDate;
            data.IsSubmitted = true;
            data.SubmissionEntryBy = UserSession.UserId;
            data.SubmissionEntryDate = DateTime.Now;
            _tdsPaymentService.Update(data);
            try
            {
                await UnitOfWork.SaveChangesAsync();

                var jObj = JsonConvert.SerializeObject(model, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                AuditLog au = new AuditLog();
                au.ObjectTypeId = (int)EnumObjectType.TdsPayment;
                au.PrimaryKey = model.MushakReturnPaymentId;
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
        else
        {
            return View(model);
        }
    }
}