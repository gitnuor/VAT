using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.Utility;
using vms.entity.models;
using Newtonsoft.Json;
using vms.entity.Enums;
using vms.utility.StaticData;
using Newtonsoft.Json.Linq;
using vms.entity.viewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class DistrictOrCityController : ControllerBase
{
    private readonly IDivisionOrStateService _divisionOrStateService;
    private readonly IDistrictOrCityService _districtOrCityService;
    public DistrictOrCityController(ControllerBaseParamModel controllerBaseParamModel, IDivisionOrStateService divisionOrStateService, IDistrictOrCityService districtOrCityService) :base(controllerBaseParamModel)
    {
        _divisionOrStateService = divisionOrStateService;
        _districtOrCityService = districtOrCityService;
    }
                

    public async Task<IActionResult> Index()
    {
        var model = new vmDistrictOrCityIndex
        {
	        DistrictOrCityList = await _districtOrCityService.GetDistrictOrCities(UserSession.OrganizationId)
        };
        foreach(var item in model.DistrictOrCityList)
        {
            item.EncryptedId = IvatDataProtector.Protect(item.DistrictOrCityId.ToString());
        }
          
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new vmDistrictOrCity
        {
	        DivisionList = await _divisionOrStateService.GetDivisionSelectListItem(UserSession.OrganizationId)
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(vmDistrictOrCity districtOrCity)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var model = new DistrictOrCity
                {
	                DivisionOrStateId = districtOrCity.DivisionOrStateId,
	                DistrictOrCityName = districtOrCity.DistrictOrCityName,
	                DistrictOrCityShortName = districtOrCity.DistrictOrCityShortName,
	                CreatedTime = DateTime.Now,
	                CreatedBy = UserSession.UserId,
	                OrganizationId = UserSession.OrganizationId
                };
                _districtOrCityService.Insert(model);
                await UnitOfWork.SaveChangesAsync();

                var jObj = JObject.Parse(JsonConvert.SerializeObject(districtOrCity));

                var au = new AuditLog
                {
	                ObjectTypeId = (int)EnumObjectType.DistrictOrCity,
	                PrimaryKey = districtOrCity.DistrictOrCityId,
	                AuditOperationId = 1,
	                UserId = UserSession.UserId,
	                Datetime = DateTime.Now,
	                Descriptions = jObj.ToString(),
	                IsActive = true,
	                CreatedBy = UserSession.UserId,
	                CreatedTime = DateTime.Now,
	                OrganizationId = UserSession.OrganizationId
                };

                var status = await AuditLogCreate(au);

                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
			{
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
				return View(districtOrCity);
            }
        }
        else
        {
            return View(districtOrCity);
        }    

    }

    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var districtOrCity = await _districtOrCityService.GetDistrictOrCity(id);
            if (districtOrCity is null)
            {
                return NotFound();
            }
            var model = new vmDistrictOrCity
            {
	            EncryptedId = id,
	            DistrictOrCityName = districtOrCity.DistrictOrCityName,
	            DistrictOrCityShortName = districtOrCity.DistrictOrCityShortName,
	            DivisionOrStateId = districtOrCity.DivisionOrStateId,
	            DistrictOrCityId = districtOrCity.DistrictOrCityId,
	            DivisionList = await _divisionOrStateService.GetDivisionSelectListItem(UserSession.OrganizationId)
            };
            return View(model);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(vmDistrictOrCity model)
    {
        if(ModelState.IsValid)
        {
            var districtOrCity = await _districtOrCityService.GetDistrictOrCity(model.EncryptedId);
            try
            {
                    
                var prevObj = JsonConvert.SerializeObject(districtOrCity, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                districtOrCity.DivisionOrStateId = model.DivisionOrStateId;
                districtOrCity.DistrictOrCityName = model.DistrictOrCityName;
                districtOrCity.DistrictOrCityShortName = model.DistrictOrCityShortName;
                districtOrCity.CreatedTime = DateTime.Now;
                districtOrCity.CreatedBy = UserSession.UserId;
                districtOrCity.OrganizationId = UserSession.OrganizationId;
                _districtOrCityService.Update(districtOrCity);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(districtOrCity, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.DistrictOrCity,
                    PrimaryKey = districtOrCity.DistrictOrCityId,
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
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetDistrictsByDivisionId(int id)
    {
        return await _districtOrCityService.GetDistrictsByDivisionId(UserSession.OrganizationId, id);
    }

    public async Task<IEnumerable<SelectListItem>> GetDistrictsByCountryId(int id)
    {            
        return await _districtOrCityService.GetDistrictsByCountryId(UserSession.OrganizationId,id);
    }

}