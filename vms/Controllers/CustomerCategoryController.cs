using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System;
using URF.Core.EF;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.Services.ProductService;
using vms.utility.StaticData;
using vms.Utility;
using vms.service.Services.ThirdPartyService;
using Microsoft.AspNetCore.DataProtection;
using System.Text;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using vms.entity.viewModels.User;
using AutoMapper;
using vms.entity.viewModels.CustomerViewModel;
using vms.entity.Dto.User;

namespace vms.Controllers
{
	public class CustomerCategoryController : ControllerBase
	{
		private readonly ICustomerCategoryService _customerCategoryService;
		private readonly IMapper _mapper;

		public CustomerCategoryController(ICustomerCategoryService custCategory, IMapper mapper,
			ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
		{
			_customerCategoryService = custCategory;
			_mapper = mapper;
		}

		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_CATEGORY_CAN_VIEW)]
		public async Task<IActionResult> Index()
		{
			return View(await _customerCategoryService.GetCustomerCategory(UserSession.ProtectedOrganizationId));
		}

		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_CATEGORY_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_CATEGORY_CAN_ADD)]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_CATEGORY_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_CATEGORY_CAN_ADD)]
		public async Task<IActionResult> Create(CustomerCategoryCreateViewModel category)
		{
			category.CreatedTime = DateTime.Now;
			category.CreatedBy = UserSession.UserId;
			category.OrganizationId = UserSession.OrganizationId;
			category.IsActive = true;

            var userData = _mapper.Map<CustomerCategoryCreateViewModel, CustomerCategory>(category);
            _customerCategoryService.Insert(userData);
			await UnitOfWork.SaveChangesAsync();
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
			var jObj = JsonConvert.SerializeObject(category, Formatting.None,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});


			AuditLog au = new AuditLog();
			au.ObjectTypeId = (int)EnumObjectType.Brand;
			au.PrimaryKey = category.CustomerCategoryId;
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
		[VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_CATEGORY_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_CATEGORY_CAN_EDIT)]
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}
			int categoryId = int.Parse(IvatDataProtector.Unprotect(id));
			var custCategory = await _customerCategoryService.Query()
				.SingleOrDefaultAsync(p => p.CustomerCategoryId == categoryId, CancellationToken.None);
			if (custCategory == null)
			{
				return NotFound();
			}
			var model = _mapper.Map<CustomerCategory, CustomerCategoryUpdateViewModel>(custCategory);
			return View(model);
		}

        [HttpPost]
		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_CATEGORY_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_CUSTOMER_CATEGORY_CAN_EDIT)]
		public async Task<IActionResult> Edit(CustomerCategoryUpdateViewModel _category, string id)
		{          
            int proCatId = int.Parse(IvatDataProtector.Unprotect(id));
            if (proCatId != _category.CustomerCategoryId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var productCat = await _customerCategoryService.Query().SingleOrDefaultAsync(
                        m => m.CustomerCategoryId == proCatId, CancellationToken.None);

                    _category.ModifiedTime = DateTime.Now;
                    _category.ModifiedBy = UserSession.UserId;
                    _category.OrganizationId = UserSession.OrganizationId;
                    var usr = _mapper.Map(_category, productCat);
                    _customerCategoryService.Update(productCat);
                    await UnitOfWork.SaveChangesAsync();        
                }
                catch (Exception)
                {
                    //skip
                }
                return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ViewStaticData.DISPLAY_CUSTOMER_CATEGORIES);
            }
            return View(_category);
        }
		public async Task<IActionResult> ChangeCustomerCategoryStatus(string id)
		{		
			string unprotectedId = IvatDataProtector.Unprotect(id);
			byte[] idBytes = Encoding.UTF8.GetBytes(unprotectedId);
			var categoryData = await _customerCategoryService.Query()
				.SingleOrDefaultAsync(p => p.CustomerCategoryId == int.Parse(Encoding.UTF8.GetString(idBytes)), CancellationToken.None);
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

				_customerCategoryService.Update(categoryData);
				await UnitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
			return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ViewStaticData.DISPLAY_CUSTOMER_CATEGORIES);
		}
	}
}
