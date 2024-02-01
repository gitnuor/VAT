using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.SecurityService;
using vms.service.Services.SettingService;
using vms.utility;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class OrganizationBranchController : ControllerBase
{
    private readonly IOrgBranchService _organizationBranchService;
    private readonly ICustomsAndVatcommissionarateService _cusAndVatService;
    private readonly IRoleRightService _roleRightService;
    private readonly IRoleService _roleService;
    private readonly IConfiguration _configuration;
    private readonly IOrgBranchTypeService _orgBranchTypeService;
    private readonly IDistrictService _districtService;

    public OrganizationBranchController(ControllerBaseParamModel controllerBaseParamModel, IOrgBranchService organizationBranchService, ICustomsAndVatcommissionarateService cusAndVatService, IRoleService roleService, IRoleRightService roleRightService, IOrgBranchTypeService orgBranchTypeService, IDistrictService districtService) :base(controllerBaseParamModel)
    {
        _organizationBranchService = organizationBranchService;
        _cusAndVatService = cusAndVatService;
        _roleService = roleService;
        _roleRightService = roleRightService;
        _configuration = Configuration;
        _orgBranchTypeService = orgBranchTypeService;
        _districtService = districtService;
    }
    public async Task<IActionResult> Index()
    {
        var organization = await _organizationBranchService.Query()
            .Where(m => m.OrganizationId == UserSession.OrganizationId).SelectAsync(CancellationToken.None);
                        
        if (organization == null)
        {
            foreach(var org in organization)
            {
                org.BusinessCategoryDescription = org.BusinessCategoryDescription.Length > 80 ? org.BusinessCategoryDescription[..79]+"...." : org.BusinessCategoryDescription;
            }
                
            return NotFound();
        }
        foreach (var org in organization)
        {
            org.EncryptedId = IvatDataProtector.Protect(org.OrgBranchId.ToString());
        }
        return View(organization);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        var og = new OrgBranch
        {
            CountryIds = Converter.GetEnumList<CountryEnum>(),
            CityIds = Converter.GetEnumList<CityEnum>(),
            BusinessNatures = Converter.GetEnumList<entity.Enums.BusinessNature>(),
            FinancialActivityNatures = Converter.GetEnumList<EnumFinancialActivityNature>(),
            BusinessCategories = Converter.GetEnumList<entity.Enums.EnumBusinessCategory>(),
        };
        var OrgBranchTypeList = await _orgBranchTypeService.GetOrgBranchTypeSelectList();

        var cv = await _cusAndVatService.Query().SelectAsync(CancellationToken.None);
        var districtList = await _districtService.GetDistrictList();
        IEnumerable<CustomSelectListItem> CusAndVatList = cv.Select(s => new CustomSelectListItem
        {
            Id = s.CustomsAndVatcommissionarateId,
            Name = s.Name
        }); 
            
        IEnumerable<CustomSelectListItem> orgbranchtypelist = OrgBranchTypeList.Select(s => new CustomSelectListItem
        {
            Id = s.OrgBranchTypeId,
            Name = s.BranchTypeName
        });

        IEnumerable<CustomSelectListItem> itemDistrictList = districtList.Select(s => new CustomSelectListItem
        {
            Id = s.DistrictId,
            Name = s.Name
        });
        og.DistrictList = itemDistrictList;
        og.CustomsAndVatCommissionarates = CusAndVatList;
        og.OrgBranchTypeList = orgbranchtypelist;

        return View(og);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_ADD)]
    public async Task<IActionResult> Create(OrgBranch orgBranch)
    {

        string defaultPassword = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_DEFAULT_PASSWORD);
        string encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);

        if (ModelState.IsValid)
        {
            try
            {
                var branch = new OrgBranch
                {
                    EmailAddress=orgBranch.EmailAddress,
                    Mobile=orgBranch.Mobile,
                    Name = orgBranch.Name,
                    DistrictId = orgBranch.DistrictId,
                    OrgBranchTypeId = orgBranch.OrgBranchTypeId,
                    CountryId = orgBranch.SelectedCountryId,
                    Address = orgBranch.Address,
                    VatResponsiblePersonName = orgBranch.VatResponsiblePersonName,
                    VatResponsiblePersonDesignation = orgBranch.VatResponsiblePersonDesignation,
                    VatResponsiblePersonEmailAddress = orgBranch.VatResponsiblePersonEmailAddress,
                    VatResponsiblePersonMobileNo = orgBranch.VatResponsiblePersonMobileNo,
                    PostalCode = orgBranch.PostalCode,
                    OrganizationId = UserSession.OrganizationId,
                    //Bin = organization.Bin,
                    //CertificateNo = organization.CertificateNo,
                    BusinessCategoryDescription = orgBranch.BusinessCategoryDescription,
                    Code = orgBranch.Code,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    //EnlistedNo = organization.EnlistedNo,
                    IsActive = true,
                    //VatregNo = organization.VatregNo,
                    BusinessNatureId = orgBranch.SelectedBusinessNatureId,
                    BusinessCategoryId = orgBranch.SelectedBusinessCategoryId,
                    FinancialActivityNatureId = orgBranch.SelectedFinancialActivityNatureId,
                    //IsDeductVatInSource = organization.IsDeductVatInSource,
                    CustomsAndVatcommissionarateId = orgBranch.CustomsAndVatcommissionarateId
                };

                _organizationBranchService.Insert(branch);
                await UnitOfWork.SaveChangesAsync();

                var jObj = JsonConvert.SerializeObject(branch, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                var au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.OrgBranch,
                    PrimaryKey = branch.OrgBranchId,
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
            catch (Exception ex)
            {
	            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
        }
        return RedirectToAction(nameof(Index));

    }

    //[VmsAuthorizeAttribute(FeatureList.ADMINSTRATION)]
    //[VmsAuthorizeAttribute(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    //[VmsAuthorizeAttribute(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW_BRANCH_DETAILS)]
    //public async Task<IActionResult> Details(string id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }
    //    try
    //    {
    //        var organization = await _organizationBranchService.Query()
    //            .Include(x => x.CustomsAndVatcommissionarate)
    //            .Include(x => x.BusinessCategory)
    //            .Include(x => x.BusinessNature)
    //            .SingleOrDefaultAsync(m => m.OrganizationId == int.Parse(IvatDataProtector.Unprotect(id)), CancellationToken.None);
    //        if (organization == null)
    //        {
    //            return NotFound();
    //        }
    //        organization.EncryptedId = id;
    //        return View(organization);
    //    }
    //    catch
    //    {
    //        return NotFound();
    //    }

    //}

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organization = await _organizationBranchService.GetOrgBranch(id);
        if (organization == null)
        {
            return NotFound();
        }

        organization.CountryIds = Converter.GetEnumList<CountryEnum>();
        organization.CityIds = Converter.GetEnumList<CityEnum>();
        organization.BusinessNatures = Converter.GetEnumList<entity.Enums.BusinessNature>();
        organization.FinancialActivityNatures = Converter.GetEnumList<EnumFinancialActivityNature>();
        organization.BusinessCategories = Converter.GetEnumList<entity.Enums.EnumBusinessCategory>();
        var OrgBranchTypeList = await _orgBranchTypeService.GetOrgBranchTypeSelectList();

        var cv = await _cusAndVatService.Query().SelectAsync(CancellationToken.None);
        var districtList = await _districtService.GetDistrictList();
        IEnumerable<CustomSelectListItem> CusAndVatList = cv.Select(s => new CustomSelectListItem
        {
            Id = s.CustomsAndVatcommissionarateId,
            Name = s.Name
        });

        IEnumerable<CustomSelectListItem> orgbranchtypelist = OrgBranchTypeList.Select(s => new CustomSelectListItem
        {
            Id = s.OrgBranchTypeId,
            Name = s.BranchTypeName
        });

        IEnumerable<CustomSelectListItem> itemDistrictList = districtList.Select(s => new CustomSelectListItem
        {
            Id = s.DistrictId,
            Name = s.Name
        });
        organization.DistrictList = itemDistrictList;
        organization.CustomsAndVatCommissionarates = CusAndVatList;
        organization.OrgBranchTypeList = orgbranchtypelist;
        organization.EncryptedId = id;
        return View(organization);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(OrgBranch orgBranch, string id)
    {
        int branchId = int.Parse(IvatDataProtector.Unprotect(orgBranch.EncryptedId));
        if (branchId != orgBranch.OrgBranchId)
        {
            return NotFound();
        }

        try
        {
            var organizationData = await _organizationBranchService.Query().SingleOrDefaultAsync(w => w.OrgBranchId == branchId, CancellationToken.None);
            var prevObj = JsonConvert.SerializeObject(organizationData, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            organizationData.Name = orgBranch.Name;
            organizationData.DistrictId = orgBranch.DistrictId;
            organizationData.OrgBranchTypeId = orgBranch.OrgBranchTypeId;
            organizationData.Code = orgBranch.Code;
            organizationData.BusinessCategoryDescription = orgBranch.BusinessCategoryDescription;
            //organizationData.VatregNo = organization.VatregNo;
            organizationData.Address = orgBranch.Address;
            organizationData.EmailAddress = orgBranch.EmailAddress;
            organizationData.Mobile = orgBranch.Mobile;
            //organizationData.Bin = organization.Bin;
            //organizationData.CertificateNo = organization.CertificateNo;
            //organizationData.CityId = organization.CityId;
            organizationData.CountryId = orgBranch.CountryId;
            organizationData.BusinessCategoryId = orgBranch.BusinessCategoryId;
            organizationData.BusinessNatureId = orgBranch.BusinessNatureId;
            organizationData.FinancialActivityNatureId = orgBranch.FinancialActivityNatureId;
            organizationData.PostalCode = orgBranch.PostalCode;
            organizationData.VatResponsiblePersonName = orgBranch.VatResponsiblePersonName;
            organizationData.VatResponsiblePersonDesignation = orgBranch.VatResponsiblePersonDesignation;
            organizationData.VatResponsiblePersonEmailAddress = orgBranch.VatResponsiblePersonEmailAddress;
            organizationData.VatResponsiblePersonMobileNo = orgBranch.VatResponsiblePersonMobileNo;
            organizationData.IsActive = orgBranch.IsActive;
            //organizationData.IsDeductVatInSource = organization.IsDeductVatInSource;
            //organizationData.EnlistedNo = organization.EnlistedNo;
            organizationData.CustomsAndVatcommissionarateId = orgBranch.CustomsAndVatcommissionarateId;

            _organizationBranchService.Update(organizationData);
            await UnitOfWork.SaveChangesAsync();


            var jObj = JsonConvert.SerializeObject(orgBranch, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.OrgBranch,
                PrimaryKey = orgBranch.OrgBranchId,
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
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await OrganizationExistsAsync(orgBranch.OrganizationId))
            {
                return NotFound();
            }
            else
            {
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
                return View(orgBranch);
            }
        }
    }

    private async Task<bool> OrganizationExistsAsync(int id)
    {
        return await _organizationBranchService.Query().AnyAsync(e => e.OrganizationId == id, CancellationToken.None);
    }

    //public async Task<IActionResult> ChangeBranchStatus(string id)
    //{
    //    try
    //    {

    //        var org = await _organizationBranchService.GetOrganization(id);

    //        if (org is not null)
    //        {
    //            if (org.IsActive == true)
    //            {
    //                org.IsActive = false;
    //                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;

    //            }
    //            else
    //            {
    //                org.IsActive = true;
    //                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;
    //            }

    //            _organizationBranchService.Update(org);
    //            await UnitOfWork.SaveChangesAsync();
    //            AuditLog au = new AuditLog
    //            {
    //                ObjectTypeId = (int)ObjectTypeEnum.User,
    //                PrimaryKey = org.OrganizationId,
    //                AuditOperationId = (int)Operations.Edit,
    //                UserId = _session.UserId,
    //                Datetime = DateTime.Now,
    //                Descriptions = "IsActive:0",
    //                IsActive = true,
    //                CreatedBy = _session.UserId,
    //                CreatedTime = DateTime.Now,
    //                OrganizationId = _session.OrganizationId
    //            };
    //            await auditLogCreate(au);
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch
    //    {
    //        return NotFound();
    //    }

    //}
}