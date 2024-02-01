using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.SecurityService;
using vms.service.Services.SettingService;
using vms.service.Services.TransactionService;
using vms.service.Services.UploadService;
using vms.utility;
using vms.utility.StaticData;
using vms.Utility;
using BusinessNature = vms.entity.Enums.BusinessNature;

namespace vms.Controllers;

public class OrganizationsController : ControllerBase1

{
    private readonly ICustomsAndVatcommissionarateService _cusAndVatService;
    private readonly IOrganizationService _service;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IUserTypeService _userTypeService;

    private readonly IDataImportService _dataImportService;

    private readonly IApiSpInsertService _apiSpInsertService;

    private readonly IRoleRightService _roleRightService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public OrganizationsController(ControllerBaseParamModel controllerBaseParamModel, IOrganizationService service, IUserService userService,
        IRoleService roleService, IUserTypeService userTypeService, IRoleRightService roleRightService, ICustomsAndVatcommissionarateService cusAndVatService, IWebHostEnvironment hostingEnvironment, IDataImportService dataImportService, IApiSpInsertService apiSpInsertService) : base(controllerBaseParamModel)
    {
        _service = service;
        _userService = userService;
        _hostingEnvironment = hostingEnvironment;
        _configuration = Configuration;
        _roleService = roleService;
        _userTypeService = userTypeService;
        _cusAndVatService = cusAndVatService;
        _roleRightService = roleRightService;
        _dataImportService = dataImportService;
        _apiSpInsertService = apiSpInsertService;
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    public async Task<IActionResult> Index(int? page, string search = null)
    {
                        
        var organization = await _service.Query()
            .Include(x=>x.CustomsAndVatcommissionarate)
            .Include(x=>x.BusinessCategory)
            .Include(x=>x.BusinessNature)                
            .SingleOrDefaultAsync(m => m.OrganizationId == UserSession.OrganizationId, CancellationToken.None);
        organization.EncryptedId = _dataProtector.Protect(organization.OrganizationId.ToString());
        if (organization == null)
        {
            return NotFound();
        }
        return View(organization);

    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW_BRANCH_DETAILS)]
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }
        try
        {
            var organization = await _service.Query()
                .SingleOrDefaultAsync(m => m.OrganizationId == int.Parse(_dataProtector.Unprotect(id)), CancellationToken.None);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }
        catch
        {
            return NotFound();
        }
                        
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_ADD)]
    public async Task<IActionResult> Create()
    {
        Organization og = new Organization
        {
            CountryIds = Converter.GetEnumList<CountryEnum>(),
            CityIds = Converter.GetEnumList<CityEnum>(),
            BusinessNatures = Converter.GetEnumList<BusinessNature>(),
            FinancialActivityNatures = Converter.GetEnumList<EnumFinancialActivityNature>(),
            BusinessCategories = Converter.GetEnumList<EnumBusinessCategory>(),
        };

        var cv = await _cusAndVatService.Query().SelectAsync(CancellationToken.None);
        IEnumerable<CustomSelectListItem> CusAndVatList = cv.Select(s => new CustomSelectListItem
        {
            Id = s.CustomsAndVatcommissionarateId,
            Name = s.Name
        });

        og.CustomsAndVatCommissionarates = CusAndVatList;

        return View(og);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_ADD)]
    public async Task<IActionResult> Create(Organization organization)
    {

        string defaultPassword = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_DEFAULT_PASSWORD);
        string encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);

        if (ModelState.IsValid)
        {
            try
            {
                Organization org = new Organization
                {
                    Name = organization.Name,
                    CityId = organization.SelectedCityId,
                    CountryId = organization.SelectedCountryId,
                    Address = organization.Address,
                    VatResponsiblePersonName = organization.VatResponsiblePersonName,
                    VatResponsiblePersonDesignation = organization.VatResponsiblePersonDesignation,
                    VatResponsiblePersonEmailAddress = organization.VatResponsiblePersonEmailAddress,
                    VatResponsiblePersonMobileNo = organization.VatResponsiblePersonMobileNo,
                    PostalCode = organization.PostalCode,
                    ParentOrganizationId = organization.ParentOrganizationId,
                    Bin = organization.Bin,
                    CertificateNo = organization.CertificateNo,
                    Code = organization.Code,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    EnlistedNo = organization.EnlistedNo,
                    IsActive = true,
                    VatregNo = organization.VatregNo,
                    BusinessNatureId = organization.SelectedBusinessNatureId,
                    BusinessCategoryId = organization.SelectedBusinessCategoryId,
                    FinancialActivityNatureId = organization.SelectedFinancialActivityNatureId,
                    IsDeductVatInSource = organization.IsDeductVatInSource,
                    CustomsAndVatcommissionarateId = organization.CustomsAndVatcommissionarateId
                };

                _service.Insert(org);
                await UnitOfWork.SaveChangesAsync();

                var jObj = JsonConvert.SerializeObject(org, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.Organization,
                    PrimaryKey = org.OrganizationId,
                    AuditOperationId = (int)EnumOperations.Add,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = jObj.ToString(),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await auditLogCreate(au);

                Role comRole = new Role();

                comRole.RoleName = org.Name + " Admin";
                comRole.OrganizationId = org.OrganizationId;
                comRole.IsActive = true;
                comRole.CreatedBy = UserSession.UserId;
                comRole.CreatedTime = DateTime.Now;
                _roleService.Insert(comRole);
                await UnitOfWork.SaveChangesAsync();

                var roleRights = await _roleRightService.Query().Where(x => x.Role.OrganizationId == null).SelectAsync();
                var roleRightsNew = new List<RoleRight>();
                foreach (var rr in roleRights)
                {
                    var nrr = new RoleRight();

                    nrr.RoleId = comRole.RoleId;
                    nrr.RightId = rr.RightId;

                    nrr.CreatedBy = UserSession.UserId;
                    nrr.CreatedTime = DateTime.Now;

                    roleRightsNew.Add(nrr);
                }

                if (roleRightsNew.Any())
                {
                    _roleRightService.InsertObjectList(roleRightsNew.ToList());
                }

                await UnitOfWork.SaveChangesAsync();

                User user = new User
                {
                    FullName = organization.VatResponsiblePersonName,
                    UserName = organization.VatResponsiblePersonEmailAddress,
                    Password = new PasswordGenerate().Encrypt(defaultPassword, encryptionKey),
                    IsDefaultPassword = true,
                    CreatedTime = DateTime.Now,
                    CreatedBy = UserSession.UserId,
                    RoleId = comRole.RoleId,
                    UserTypeId = 2,
                    IsActive = true,
                    OrganizationId = org.OrganizationId,
                    Mobile = organization.VatResponsiblePersonMobileNo,
                    EmailAddress = organization.VatResponsiblePersonEmailAddress,
                    IsCompanyRepresentative = true
                };
                _userService.Insert(user);
                await UnitOfWork.SaveChangesAsync();
                var jUserObj = JsonConvert.SerializeObject(user, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                AuditLog aud = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.User,
                    PrimaryKey = user.UserId,
                    AuditOperationId = (int)EnumOperations.Add,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = jUserObj.ToString(),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await auditLogCreate(aud);
                TempData[ViewStaticData.SEARCH_TEXT] = ControllerStaticData.SUCCESS_CLASSNAME;
                return RedirectToAction(nameof(Index));


            }

			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
        organization.CountryIds = Converter.GetEnumList<CountryEnum>();
        organization.CityIds = Converter.GetEnumList<CityEnum>();
        return View(organization);

    }

    public IActionResult ForgetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgetPassword(vmUsers us)
    {
        string encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);
        var usrs = await _userService.Query().SingleOrDefaultAsync(m => m.UserName == us.UserName, CancellationToken.None);
        if (usrs != null)
        {
            try
            {
                var code = new PasswordGenerate().RandomPassword(6);
                var encryptedCode = new PasswordGenerate().Encrypt(code, encryptionKey);
                string mailSub = "Password Recovery Email";
                var userId = us.UserName + "WinterIsComing" + code;
                var encryptedUserId = new PasswordGenerate().Encrypt(userId, encryptionKey);
                string strBody = string.Empty;
                strBody += "<html><head><body><p>Dear " + usrs.FullName + " </p><p>Your recovery Code: " + encryptedCode + "</p>";
                strBody += "<p>Click <a href='http://localhost:25539/Organizations/ChangePassWordFrmEmail?code=" + encryptedUserId.ToString() + "'>Here</a> to recover your password.</p>";
                strBody += "<br>Thanks!";
                strBody += "<p>Team VMS</p></body></html>";

                var mailBody = strBody;
                MailMaster.SendMail(usrs.EmailAddress, mailSub, mailBody);
                ViewData[ControllerStaticData.MESSAGE] = "Check your Email!";
            }
            catch (Exception ex)
            {
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
        }
        else
        {
            ViewData[ControllerStaticData.MESSAGE] = "User does not exist";
        }
        ModelState.Clear();
        return View(us);
    }

    public IActionResult ChangePassWordFrmEmail(string code)
    {
        if (code == null)
        {
            return NotFound();
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassWordFrmEmail(vmUsers us, string code)
    {
        string encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);
        try
        {
            var decryptedId = new PasswordGenerate().Decrypt(code, encryptionKey);
            var splitString = decryptedId.Split("WinterIsComing");
            var userName = splitString[0];
            var encrytedCode = splitString[1];
            var userDecryted = new PasswordGenerate().Decrypt(us.UserCode, encryptionKey);
            if (encrytedCode == userDecryted)
            {
                var usrs = await _userService.Query().SingleOrDefaultAsync(m => m.UserName == userName, CancellationToken.None);
                var encryptedPass = new PasswordGenerate().Encrypt(us.ConfirmPassword, encryptionKey);
                usrs.Password = encryptedPass;
                _userService.Update(usrs);
                await UnitOfWork.SaveChangesAsync();
                ViewData[ControllerStaticData.MESSAGE] = "Successfull";
                ModelState.Clear();
            }
            else
            {
                ViewData[ControllerStaticData.MESSAGE] = "Code does not match!";
            }
        }
        catch (Exception ex)
        {
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
		}

        return View(us);
    }

    public IActionResult Registration()
    {
        Organization og = new Organization
        {
            CountryIds = Converter.GetEnumList<CountryEnum>(),
            CityIds = Converter.GetEnumList<CityEnum>()
        };

        return View(og);
    }

    [HttpPost]
    public async Task<IActionResult> Registration(Organization organization)
    {
        string defaultPassword = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_DEFAULT_PASSWORD);
        string encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);
        if (ModelState.IsValid)
        {
            Role role = new Role
            {
                RoleName = ControllerStaticData.ADMIN,
                IsActive = true,
                CreatedTime = DateTime.Now
            };
            _roleService.Insert(role);
            await UnitOfWork.SaveChangesAsync();
            UserType userType = new UserType
            {
                UserTypeName = ControllerStaticData.COMPANY,

            };

            _userTypeService.Insert(userType);
            await UnitOfWork.SaveChangesAsync();
            Organization org = new Organization
            {
                Name = organization.Name,
                CityId = organization.SelectedCityId,
                CountryId = organization.SelectedCityId,
                Address = organization.Address,
                VatResponsiblePersonName = organization.VatResponsiblePersonName,
                VatResponsiblePersonDesignation = organization.VatResponsiblePersonDesignation,
                PostalCode = organization.PostalCode,
                ParentOrganizationId = organization.ParentOrganizationId,
                Bin = organization.Bin,
                CertificateNo = organization.CertificateNo,
                Code = organization.Code,
                CreatedBy = 0,
                CreatedTime = DateTime.Now,
                EnlistedNo = organization.EnlistedNo,
                IsActive = false,
                VatregNo = organization.VatregNo,
                IsDeductVatInSource = organization.IsDeductVatInSource
            };

            _service.Insert(org);
            await UnitOfWork.SaveChangesAsync();
            User user = new User
            {
                UserName = organization.UserName,
                Password = new PasswordGenerate().Encrypt(defaultPassword, encryptionKey),
                IsDefaultPassword = true,
                CreatedTime = DateTime.Now,
                CreatedBy = 0,
                RoleId = role.RoleId,
                UserTypeId = userType.UserTypeId,
                IsActive = false,
                OrganizationId = org.OrganizationId,
                Mobile = organization.Mobile,
                EmailAddress = organization.EmailAddress,
                IsCompanyRepresentative = true
            };
            _userService.Insert(user);
            await UnitOfWork.SaveChangesAsync();

            TempData[ViewStaticData.SEARCH_TEXT] = MessageStaticData.MESSAGE_CREATE_COMPANY;

            return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ControllerStaticData.HOME, TempData[ViewStaticData.SEARCH_TEXT]);
        }
        return View(organization);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_EDIT)]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organization = await _service.GetOrganization(id);
        if (organization == null)
        {
            return NotFound();
        }

        organization.CountryIds = Converter.GetEnumList<CountryEnum>();
        organization.CityIds = Converter.GetEnumList<CityEnum>();
        organization.BusinessNatures = Converter.GetEnumList<BusinessNature>();
        organization.FinancialActivityNatures = Converter.GetEnumList<EnumFinancialActivityNature>();
        organization.BusinessCategories = Converter.GetEnumList<EnumBusinessCategory>();

        var cv = await _cusAndVatService.Query().SelectAsync(CancellationToken.None);
        IEnumerable<CustomSelectListItem> CusAndVatList = cv.Select(s => new CustomSelectListItem
        {
            Id = s.CustomsAndVatcommissionarateId,
            Name = s.Name
        });

        organization.CustomsAndVatCommissionarates = CusAndVatList;

        organization.EncryptedId = id;
        return View(organization);
    }


    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_EDIT)]
    public async Task<IActionResult> Edit(Organization organization, string id)
    {
        int roleId = int.Parse(_dataProtector.Unprotect(organization.EncryptedId));
        if (roleId != organization.OrganizationId)
        {
            return NotFound();
        }

        try
        {
            var organizationData = await _service.Query().SingleOrDefaultAsync(w => w.OrganizationId == roleId, CancellationToken.None);
            var prevObj = JsonConvert.SerializeObject(organizationData, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            organizationData.Name = organization.Name;
            organizationData.Code = organization.Code;
            organizationData.VatregNo = organization.VatregNo;
            organizationData.Address = organization.Address;
            organizationData.EmailAddress = organization.EmailAddress;
            organizationData.Mobile = organization.Mobile;
            organizationData.Bin = organization.Bin;
            organizationData.CertificateNo = organization.CertificateNo;
            organizationData.CityId = organization.CityId;
            organizationData.CountryId = organization.CountryId;
            organizationData.BusinessCategoryId = organization.BusinessCategoryId;
            organizationData.BusinessNatureId = organization.BusinessNatureId;
            organizationData.FinancialActivityNatureId = organization.FinancialActivityNatureId;
            organizationData.PostalCode = organization.PostalCode;
            organizationData.VatResponsiblePersonName = organization.VatResponsiblePersonName;
            organizationData.VatResponsiblePersonDesignation = organization.VatResponsiblePersonDesignation;
            organizationData.VatResponsiblePersonEmailAddress = organization.VatResponsiblePersonEmailAddress;
            organizationData.VatResponsiblePersonMobileNo = organization.VatResponsiblePersonMobileNo;
            organizationData.IsActive = organization.IsActive;
            organizationData.IsDeductVatInSource = organization.IsDeductVatInSource;
            organizationData.EnlistedNo = organization.EnlistedNo;
            organizationData.CustomsAndVatcommissionarateId = organization.CustomsAndVatcommissionarateId;

            _service.Update(organizationData);
            await UnitOfWork.SaveChangesAsync();


            var jObj = JsonConvert.SerializeObject(organization, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.Organization,
                PrimaryKey = organization.OrganizationId,
                AuditOperationId = (int)EnumOperations.Edit,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = change(prevObj.ToString(), jObj.ToString()),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };
            await auditLogCreate(au);

            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await OrganizationExistsAsync(organization.OrganizationId))
            {
                return NotFound();
            }
            else
            {
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
                return View(organization);
            }
        }
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organization = await _service.Query()
            .SingleOrDefaultAsync(m => m.OrganizationId == id, CancellationToken.None);
        if (organization == null)
        {
            return NotFound();
        }

        return View(organization);
    }

    [HttpPost, ActionName(ControllerStaticData.DELETE_CLASSNAME)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var organization = await _service.Query().SingleOrDefaultAsync(p => p.OrganizationId == id, CancellationToken.None);

        await UnitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> OrganizationExistsAsync(int id)
    {
        return await _service.Query().AnyAsync(e => e.OrganizationId == id, CancellationToken.None);
    }

    [HttpGet]
    public async Task<JsonResult> GetBin(int orgId)
    {
        var org = await _service.Query().FirstOrDefaultAsync(c => c.OrganizationId == orgId, CancellationToken.None);
        return Json(org);
    }

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_ADD_BRANCH)]
    public async Task<IActionResult> AddBranch(string id)
    {
        Organization og = new Organization
        {
            CountryIds = Converter.GetEnumList<CountryEnum>(),
            CityIds = Converter.GetEnumList<CityEnum>(),
            BusinessNatures = Converter.GetEnumList<BusinessNature>(),
            FinancialActivityNatures = Converter.GetEnumList<EnumFinancialActivityNature>(),
            BusinessCategories = Converter.GetEnumList<EnumBusinessCategory>(),
        };
        try
        {
            Organization item = await _service.Query().FirstOrDefaultAsync(c => c.OrganizationId == int.Parse(_dataProtector.Unprotect(id)) && c.IsActive==true, CancellationToken.None);           

            if (item is not null)
            {
                var cv = await _cusAndVatService.Query().SelectAsync(CancellationToken.None);
                IEnumerable<CustomSelectListItem> CusAndVatList = cv.Select(s => new CustomSelectListItem
                {
                    Id = s.CustomsAndVatcommissionarateId,
                    Name = s.Name
                });

                og.CustomsAndVatCommissionarates = CusAndVatList;
            }
            else
            {
                return NotFound();
            }
        }
        catch
        {
            return NotFound();
        }
                        
        return View(og);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_ADD_BRANCH)]
    public async Task<IActionResult> AddBranch(Organization organization)
    {
        string defaultPassword = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_DEFAULT_PASSWORD);
        string encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);
        var isExistUser = await _userService.IsUserExists(organization.UserName);
        if (isExistUser)
        {
            ModelState.AddModelError("UserName", "User already exists!!!");
        }
        if (ModelState.IsValid)
        {
            Organization org = new Organization
            {
                Name = organization.Name,
                CityId = organization.SelectedCityId,
                CountryId = organization.SelectedCountryId,
                Address = organization.Address,
                EmailAddress = organization.EmailAddress,
                Mobile = organization.Mobile,
                VatResponsiblePersonName = organization.VatResponsiblePersonName,
                VatResponsiblePersonDesignation = organization.VatResponsiblePersonDesignation,
                VatResponsiblePersonEmailAddress = organization.VatResponsiblePersonEmailAddress,
                VatResponsiblePersonMobileNo = organization.VatResponsiblePersonMobileNo,
                PostalCode = organization.PostalCode,
                ParentOrganizationId = UserSession.OrganizationId,
                Bin = organization.Bin,
                CertificateNo = organization.CertificateNo,
                Code = organization.Code,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                EnlistedNo = organization.EnlistedNo,
                IsActive = true,
                VatregNo = organization.VatregNo,
                IsDeductVatInSource = organization.IsDeductVatInSource,
                CustomsAndVatcommissionarateId = organization.CustomsAndVatcommissionarateId,
                BusinessNatureId = organization.SelectedBusinessNatureId,
                BusinessCategoryId = organization.SelectedBusinessCategoryId,
                FinancialActivityNatureId = organization.SelectedFinancialActivityNatureId
            };

            _service.Insert(org);
            await UnitOfWork.SaveChangesAsync();
            var jObj = JsonConvert.SerializeObject(org, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            AuditLog au = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.Organization,
                PrimaryKey = org.OrganizationId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = jObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };
            await auditLogCreate(au);
            User user = new User
            {
                UserName = organization.UserName,
                Password = new PasswordGenerate().Encrypt(defaultPassword, encryptionKey),
                IsDefaultPassword = true,
                CreatedTime = DateTime.Now,
                CreatedBy = UserSession.UserId,
                RoleId = 2,
                UserTypeId = 2,
                IsActive = true,
                OrganizationId = org.OrganizationId,
                Mobile = organization.Mobile,
                EmailAddress = organization.EmailAddress,
                IsCompanyRepresentative = true
            };
            _userService.Insert(user);
            await UnitOfWork.SaveChangesAsync();
            var jUserObj = JsonConvert.SerializeObject(user, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            AuditLog aud = new AuditLog
            {
                ObjectTypeId = (int)EnumObjectType.User,
                PrimaryKey = user.UserId,
                AuditOperationId = (int)EnumOperations.Add,
                UserId = UserSession.UserId,
                Datetime = DateTime.Now,
                Descriptions = jUserObj.ToString(),
                IsActive = true,
                CreatedBy = UserSession.UserId,
                CreatedTime = DateTime.Now,
                OrganizationId = UserSession.OrganizationId
            };
            await auditLogCreate(aud);
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            return RedirectToAction(nameof(Index));
        }

        organization.CountryIds = Converter.GetEnumList<CountryEnum>();
        organization.CityIds = Converter.GetEnumList<CityEnum>();
        organization.BusinessNatures = Converter.GetEnumList<BusinessNature>();
        organization.FinancialActivityNatures = Converter.GetEnumList<EnumFinancialActivityNature>();
        organization.BusinessCategories = Converter.GetEnumList<EnumBusinessCategory>();
        var cv = await _cusAndVatService.Query().SelectAsync(CancellationToken.None);
        IEnumerable<CustomSelectListItem> CusAndVatList = cv.Select(s => new CustomSelectListItem
        {
            Id = s.CustomsAndVatcommissionarateId,
            Name = s.Name
        });
        organization.CustomsAndVatCommissionarates = CusAndVatList;

        return View(organization);
    }

    // [VmsAuthorize(FeatureList.ADMINSTRATION)]
    // [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    // [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW_BRANCH_DETAILS)]
    // public async Task<IActionResult> BranchDetails(int? page, string search = null)
    // {
    //     var organization = await _service.GetParentOrganizations(UserSession.OrganizationId);
    //
    //     string txt = search;
    //
    //     if (search != null)
    //     {
    //         search = search.ToLower().Trim();
    //         organization = organization.Where(c => 
    //             c.Name.ToLower().Contains(search)
    //             || c.Address.ToLower().Contains(search)
    //             || c.PostalCode.ToString().Contains(search)
    //         );
    //     }
    //
    //     ViewBag.PageCount = organization.Count();
    //     var pageNumber = page ?? 1;
    //     var listOfBranchDetails = organization.ToPagedList(pageNumber, 10);
    //     if (txt != null)
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = txt;
    //     }
    //     else
    //     {
    //         ViewData[ViewStaticData.SEARCH_TEXT] = string.Empty;
    //     }
    //     return View(listOfBranchDetails);
    // }

    [HttpGet]
    public async Task<IActionResult> ImportProduct(int organizationId)
    {
        return await Task.FromResult(View());
    }

    [HttpPost]
    public async Task<IActionResult> ImportProduct(IFormFile file)
    {
        if (file.FileName.Contains(".xls"))
        {
            try
            {
                var orgfolder = _hostingEnvironment.WebRootPath + "/ApplicationDocument/" + UserSession.OrganizationId;
                var fileLocation = Path.Combine(orgfolder + "/", file.FileName);
                if (!System.IO.File.Exists(orgfolder))
                {
                    Directory.CreateDirectory(orgfolder);
                }
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(orgfolder, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                var obj = await _dataImportService.LoadProduct(fileLocation);
                var output = await _dataImportService.InsertBulk(obj, UserSession.OrganizationId, UserSession.UserId);


            }

            catch (Exception ex)
            {
                TempData.Add("ErrorMessage", ex.Message);
                return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
            }

            TempData.Add("SuccessMessage", "File Uploaded Successfully!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });

        }
        else
        {
            TempData.Add("ErrorMessage", "Not a valid file type!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ImportPurchase(IFormFile file)
    {
        if (file.FileName.Contains(".xlsx"))
        {
            try
            {
                var orgfolder = _hostingEnvironment.WebRootPath + "/ApplicationDocument/" + UserSession.OrganizationId;
                var fileLocation = Path.Combine(orgfolder + "/", file.FileName);
                if (!System.IO.File.Exists(orgfolder))
                {
                    Directory.CreateDirectory(orgfolder);
                }
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(orgfolder, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                var obj = await _dataImportService.LoadPurchaseFromExcel(fileLocation);
                obj.CreatedBy = UserSession.UserId;
                obj.CreatedTime = DateTime.Now;
                obj.OrganizationId = UserSession.OrganizationId;
                obj.SecurityToken = "Excel Import";
                var output = await _apiSpInsertService.InsertBulkPurchase(obj);


            }
            catch (Exception ex)
            {
                TempData.Add("ErrorMessage", ex.Message);
                return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
            }

            TempData.Add("SuccessMessage", "File Uploaded Successfully!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });

        }
        else
        {
            TempData.Add("ErrorMessage", "Not a valid file type!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
        }
    }



    [HttpPost]
    public async Task<IActionResult> ImportSale(IFormFile file)
    {
        if (file.FileName.Contains(".xlsx"))
        {
            try
            {
                var orgfolder = _hostingEnvironment.WebRootPath + "/ApplicationDocument/" + UserSession.OrganizationId;
                var fileLocation = Path.Combine(orgfolder + "/", file.FileName);
                if (!System.IO.File.Exists(orgfolder))
                {
                    Directory.CreateDirectory(orgfolder);
                }
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(orgfolder, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                var obj = await _dataImportService.LoadSaleFromExcel(fileLocation);
                obj.CreatedBy = UserSession.UserId;
                obj.CreatedTime = DateTime.Now;
                obj.OrganizationId = UserSession.OrganizationId;
                obj.SecurityToken = "Excel Import";
                var output = await _apiSpInsertService.InsertBulkSale(obj);
            }
            catch (Exception ex)
            {
                TempData.Add("ErrorMessage", ex.Message);
                return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
            }

            TempData.Add("SuccessMessage", "File Uploaded Successfully!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
        }
        else
        {
            TempData.Add("ErrorMessage", "Not a valid file type!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ImportProduction(IFormFile file)
    {
        if (file.FileName.Contains(".xlsx"))
        {
            try
            {
                var orgfolder = _hostingEnvironment.WebRootPath + "/ApplicationDocument/" + UserSession.OrganizationId;
                var fileLocation = Path.Combine(orgfolder + "/", file.FileName);
                if (!System.IO.File.Exists(orgfolder))
                {
                    Directory.CreateDirectory(orgfolder);
                }
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(orgfolder, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                var obj = await _dataImportService.LoadProduction(fileLocation);
                var output = await _dataImportService.InsertBulkProduction(obj, UserSession.OrganizationId, UserSession.UserId);
            }
            catch (Exception ex)
            {
                TempData.Add("ErrorMessage", ex.Message);
                return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
            }

            TempData.Add("SuccessMessage", "File Uploaded Successfully!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });

        }
        else
        {
            TempData.Add("ErrorMessage", "Not a valid file type!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
        }
    }



    [HttpPost]

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_COMPANY_BASIC_INFO_CAN_IMPORT_DATA)]
    public async Task<IActionResult> ImportStaticData(IFormFile file)
    {
        if (file.FileName.Contains(".xlsx"))
        {
            try
            {
                var orgfolder = _hostingEnvironment.WebRootPath + "/ApplicationDocument/" + UserSession.OrganizationId;
                var fileLocation = Path.Combine(orgfolder + "/", file.FileName);
                if (!System.IO.File.Exists(orgfolder))
                {
                    Directory.CreateDirectory(orgfolder);
                }
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(orgfolder, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                var obj = await _dataImportService.LoadStaticDataFromExcel(fileLocation);
            }
            catch (Exception ex)
            {
                TempData.Add("ErrorMessage", ex.Message);
                return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
            }

            TempData.Add("SuccessMessage", "File Uploaded Successfully!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });

        }
        else
        {
            TempData.Add("ErrorMessage", "Not a valid file type!");
            return RedirectToAction("ImportProduct", new { id = UserSession.OrganizationId });
        }
    }



    public async Task<IActionResult> Download(string filename)
    {
        if (filename == null)
            return Content("filename not present");

        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot", filename);

        var memory = new MemoryStream();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        return File(memory, GetContentType(path), Path.GetFileName(path));
    }

    private string GetContentType(string path)
    {
        var types = GetMimeTypes();
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return types[ext];
    }

    private Dictionary<string, string> GetMimeTypes()
    {
        return new Dictionary<string, string>
        {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"}
        };
    }
}