using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.CustomerViewModel;
using vms.entity.viewModels.VendorViewModel;
using vms.service.Services.ThirdPartyService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers
{
	public class CustomerSubscriptionCategoryController : ControllerBase
    {
        private readonly ICustomerSubscriptionCategoryService _subscriptionCategoryService;
        private readonly IMapper _mapper;

        public CustomerSubscriptionCategoryController(ICustomerSubscriptionCategoryService subscriptionCategory, IMapper mapper,
            ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
        {
            _subscriptionCategoryService = subscriptionCategory;
            _mapper = mapper;
        }

        [VmsAuthorize(FeatureList.ADMINSTRATION)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_SUBSCRIPTION_CATEGORY_CAN_VIEW)]
        public async Task<IActionResult> Index()
        {
            return View(await _subscriptionCategoryService.GetCustomerCategory(UserSession.ProtectedOrganizationId));
        }

        [VmsAuthorize(FeatureList.ADMINSTRATION)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_SUBSCRIPTION_CATEGORY_CAN_VIEW)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_SUBSCRIPTION_CATEGORY_CAN_ADD)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [VmsAuthorize(FeatureList.ADMINSTRATION)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_SUBSCRIPTION_CATEGORY_CAN_VIEW)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_SUBSCRIPTION_CATEGORY_CAN_ADD)]
        public async Task<IActionResult> Create(CustomerSubscriptionCategoryCreateViewModel category)
        {
            category.CreatedTime = DateTime.Now;
            category.CreatedBy = UserSession.UserId;
            category.OrganizationId = UserSession.OrganizationId;
            category.IsActive = true;


            var data = _mapper.Map<CustomerSubscriptionCategoryCreateViewModel, CustomerSubscriptionCategory>(category);
            _subscriptionCategoryService.Insert(data);
            await UnitOfWork.SaveChangesAsync();
            TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
            var jObj = JsonConvert.SerializeObject(category, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            AuditLog au = new AuditLog();
            au.ObjectTypeId = (int)EnumObjectType.Brand;
            au.PrimaryKey = category.CustomerSubscriptionCategoryId;
            au.AuditOperationId = (int)EnumOperations.Add;
            au.UserId = UserSession.UserId;
            au.Datetime = DateTime.Now;
            au.Descriptions = jObj;
            au.IsActive = true;
            au.CreatedBy = UserSession.UserId;
            au.CreatedTime = DateTime.Now;
            au.OrganizationId = UserSession.OrganizationId;
            await AuditLogCreate(au);

            return RedirectToAction(nameof(Index));
        }

        [VmsAuthorize(FeatureList.ADMINSTRATION)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_SUBSCRIPTION_CATEGORY_CAN_VIEW)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_SUBSCRIPTION_CATEGORY_CAN_EDIT)]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int categoryId = int.Parse(IvatDataProtector.Unprotect(id));
            var custSubscription = await _subscriptionCategoryService.Query()
                .SingleOrDefaultAsync(p => p.CustomerSubscriptionCategoryId == categoryId, CancellationToken.None);
            if (custSubscription == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<CustomerSubscriptionCategory, CustomerSubscriptionCategoryUpdateViewModel>(custSubscription);
            return View(model);
        }

        [HttpPost]
        [VmsAuthorize(FeatureList.ADMINSTRATION)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_SUBSCRIPTION_CATEGORY_CAN_VIEW)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_SUBSCRIPTION_CATEGORY_CAN_EDIT)]
        public async Task<IActionResult> Edit(CustomerSubscriptionCategoryUpdateViewModel _category, string id)
        {
            int proCatId = int.Parse(IvatDataProtector.Unprotect(id));
            if (proCatId != _category.CustomerSubscriptionCategoryId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var productCat = await _subscriptionCategoryService.Query().SingleOrDefaultAsync(
                        m => m.CustomerSubscriptionCategoryId == proCatId, CancellationToken.None);

                    _category.ModifiedTime = DateTime.Now;
                    _category.ModifiedBy = UserSession.UserId;
                    _category.OrganizationId = UserSession.OrganizationId;
                    var usr = _mapper.Map(_category, productCat);
                    _subscriptionCategoryService.Update(productCat);
                    await UnitOfWork.SaveChangesAsync();
                }
                catch (Exception)
                {
                    //skip
                }
                return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ViewStaticData.DISPLAY_SUBSCRIPTION_CATEGORIES);
            }
            return View(_category);
        }
        public async Task<IActionResult> ChangeCustomerSubscripCategoryStatus(string id)
        {
            string unprotectedId = IvatDataProtector.Unprotect(id);
            byte[] idBytes = Encoding.UTF8.GetBytes(unprotectedId);
            var categoryData = await _subscriptionCategoryService.Query()
                .SingleOrDefaultAsync(p => p.CustomerSubscriptionCategoryId == int.Parse(Encoding.UTF8.GetString(idBytes)), CancellationToken.None);
            try
            {

                if (categoryData.IsActive == true)
                {
                    categoryData.IsActive = false;
                    TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;
                }
                else
                {
                    categoryData.IsActive = true;
                    TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;

                }

                _subscriptionCategoryService.Update(categoryData);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
            }
            return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ViewStaticData.DISPLAY_SUBSCRIPTION_CATEGORIES);
        }
    }
}
