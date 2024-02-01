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
using vms.service.Services.SettingService;

namespace vms.Controllers;

public class DivisionOrStateController : ControllerBase
{
    private readonly IDivisionOrStateService _divisionOrStateService;
    private readonly ICountryService _countryService;
    public DivisionOrStateController(ControllerBaseParamModel controllerBaseParamModel, IDivisionOrStateService divisionOrStateService,ICountryService countryService) :base(controllerBaseParamModel)
    {
        _divisionOrStateService = divisionOrStateService;
        _countryService = countryService;
    }
                

    public async Task<IActionResult> Index()
    {
        vmDivisionOrStateIndex model = new vmDivisionOrStateIndex();
        model.DivisionOrStateList = await _divisionOrStateService.GetDivisionOrStates(UserSession.OrganizationId);
        foreach(var item in model.DivisionOrStateList)
        {
            item.EncryptedId = IvatDataProtector.Protect(item.DivisionOrStateId.ToString());
        }
          
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        vmDivisionOrState model = new vmDivisionOrState();
        model.CountryList  =  await _countryService.CountrySelectList();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(vmDivisionOrState divisionOrState)
    {
        if (ModelState.IsValid)
        {
            try
            {
                DivisionOrState model = new DivisionOrState();
                model.CountryId = divisionOrState.CountryId;
                model.DivisionOrStateName = divisionOrState.DivisionOrStateName;
                model.DivisionOrStateShortName = divisionOrState.DivisionOrStateShortName;

                model.CreatedTime = DateTime.Now;
                model.CreatedBy = UserSession.UserId;
                model.OrganizationId = UserSession.OrganizationId;
                _divisionOrStateService.Insert(model);
                await UnitOfWork.SaveChangesAsync();

                var jObj = JObject.Parse(JsonConvert.SerializeObject(divisionOrState));

                AuditLog au = new AuditLog();
                au.ObjectTypeId = (int)EnumObjectType.DivisionOrState;
                au.PrimaryKey = divisionOrState.DivisionOrStateId;
                au.AuditOperationId = 1;
                au.UserId = UserSession.UserId;
                au.Datetime = DateTime.Now;
                au.Descriptions = jObj.ToString();
                au.IsActive = true;
                au.CreatedBy = UserSession.UserId;
                au.CreatedTime = DateTime.Now;
                au.OrganizationId = UserSession.OrganizationId;

                var status = await AuditLogCreate(au);

                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
			{
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
				return View(divisionOrState);
            }
        }
        else
        {
            return View(divisionOrState);
        }    

    }

    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var divisionOrState = await _divisionOrStateService.GetDivisionOrState(id);
            if (divisionOrState is null)
            {
                return NotFound();
            }
            vmDivisionOrState model = new vmDivisionOrState();
            model.EncryptedId = id;
            model.DivisionOrStateName = divisionOrState.DivisionOrStateName;
            model.DivisionOrStateShortName = divisionOrState.DivisionOrStateShortName;
            model.DivisionOrStateId = divisionOrState.DivisionOrStateId;
            model.CountryId = divisionOrState.CountryId;
            var countryList = await _countryService.CountrySelectList();
            model.CountryList = countryList;
            return View(model);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(vmDivisionOrState model)
    {
        if(ModelState.IsValid)
        {
            var divisionOrState = await _divisionOrStateService.GetDivisionOrState(model.EncryptedId);
            try
            {
                    
                var prevObj = JsonConvert.SerializeObject(divisionOrState, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    
                divisionOrState.CountryId = model.CountryId;
                divisionOrState.DivisionOrStateName = model.DivisionOrStateName;
                divisionOrState.DivisionOrStateShortName = model.DivisionOrStateShortName;
                divisionOrState.CreatedTime = DateTime.Now;
                divisionOrState.CreatedBy = UserSession.UserId;
                divisionOrState.OrganizationId = UserSession.OrganizationId;

                _divisionOrStateService.Update(divisionOrState);
                await UnitOfWork.SaveChangesAsync();
                var jObj = JsonConvert.SerializeObject(divisionOrState, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.DivisionOrState,
                    PrimaryKey = divisionOrState.DivisionOrStateId,
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

    public async Task<IEnumerable<CustomSelectListItem>> GetDivisionsByCountryId(int id)
    {
        return await _divisionOrStateService.GetDivisionsByCountryId(UserSession.OrganizationId, id);
    }

}