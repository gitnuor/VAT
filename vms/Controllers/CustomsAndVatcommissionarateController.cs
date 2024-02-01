using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.SettingService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class CustomsAndVatcommissionarateController : ControllerBase
{
    private readonly ICustomsAndVatcommissionarateService _cusAndVatService;
    private readonly IDistrictService _districtService;
    public CustomsAndVatcommissionarateController(ICustomsAndVatcommissionarateService cusAndVatService, IDistrictService districtService, ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
    {

        _cusAndVatService = cusAndVatService;
        _districtService = districtService;

    }

        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMS_AND_VAT_COMMISSIONARATE_CAN_VIEW)]
    public async Task<IActionResult> Index()
    {
        var customsAndVatcommissionarate = await _cusAndVatService.Query().SelectAsync();
        customsAndVatcommissionarate = customsAndVatcommissionarate.OrderByDescending(x => x.CustomsAndVatcommissionarateId);
        return View(customsAndVatcommissionarate);
    }




        
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMS_AND_VAT_COMMISSIONARATE_CAN_VIEW)]
   
    public async Task<IActionResult> Create()
    {

        CustomsAndVatcommissionarate model = new CustomsAndVatcommissionarate();

        var district = await _districtService.Query().SelectAsync(CancellationToken.None);
        IEnumerable<CustomSelectListItem> DistrictsList = district.Select(s => new CustomSelectListItem
        {
            Id = s.DistrictId,
            Name = s.Name
        });

        model.Districts = DistrictsList;


        return View(model);
    }


    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMS_AND_VAT_COMMISSIONARATE_CAN_VIEW)]
      
    public async Task<IActionResult> Create(CustomsAndVatcommissionarate cusVatcom)
    {
        var jObj = "";
        try
        {
            cusVatcom.IsActive = true;
            _cusAndVatService.Insert(cusVatcom);
            await UnitOfWork.SaveChangesAsync();

            jObj = JsonConvert.SerializeObject(cusVatcom, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.CustomsAndVatcommissionarate,
                PrimaryKey = cusVatcom.CustomsAndVatcommissionarateId,
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


}