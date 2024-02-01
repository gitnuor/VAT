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
using vms.entity.viewModels.VendorViewModel;
using vms.entity.viewModels.CustomerViewModel;
using AutoMapper;

namespace vms.Controllers
{
	public class VendorCategoryController : ControllerBase
	{
		private readonly IVendorCategoryService _vendorCategoryService;
        private readonly IMapper _mapper;
        public VendorCategoryController(IVendorCategoryService vendorCategory, IMapper mapper,
            ControllerBaseParamModel controllerBaseParamModel) : base(controllerBaseParamModel)
		{
			_vendorCategoryService = vendorCategory;
			_mapper = mapper;
		}

		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_VENDOR_CATEGORY_CAN_VIEW)]
		public async Task<IActionResult> Index()
		{
			return View(await _vendorCategoryService.GetVendorCategory(UserSession.ProtectedOrganizationId));
		}

		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_VENDOR_CATEGORY_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_VENDOR_CATEGORY_CAN_ADD)]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_VENDOR_CATEGORY_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_VENDOR_CATEGORY_CAN_ADD)]
		public async Task<IActionResult> Create(VndorCategoryCreateViewModel category)
		{
			category.CreatedTime = DateTime.Now;
			category.CreatedBy = UserSession.UserId;
			category.OrganizationId = UserSession.OrganizationId;
			category.IsActive = true;

            var data = _mapper.Map<VndorCategoryCreateViewModel, VendorCategory>(category);
            _vendorCategoryService.Insert(data);
			await UnitOfWork.SaveChangesAsync();
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
			var jObj = JsonConvert.SerializeObject(category, Formatting.None,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});


			AuditLog au = new AuditLog();
			au.ObjectTypeId = (int)EnumObjectType.Brand;
			au.PrimaryKey = category.VendorCategoryId;
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
		[VmsAuthorize(FeatureList.ADMINSTRATION_VENDOR_CATEGORY_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_VENDOR_CATEGORY_CAN_EDIT)]
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			int categoryId = int.Parse(IvatDataProtector.Unprotect(id));
			var custVendor = await _vendorCategoryService.Query()
				.SingleOrDefaultAsync(p => p.VendorCategoryId == categoryId, CancellationToken.None);
			if (custVendor == null)
			{
				return NotFound();
			}
            var model = _mapper.Map<VendorCategory, VndorCategoryUpdateViewModel>(custVendor);
            return View(model);
		}

		[HttpPost]
		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_VENDOR_CATEGORY_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_VENDOR_CATEGORY_CAN_EDIT)]
		public async Task<IActionResult> Edit(VndorCategoryUpdateViewModel _category,string id)
		{  
            int proCatId = int.Parse(IvatDataProtector.Unprotect(id));
            if (proCatId != _category.VendorCategoryId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var productCat = await _vendorCategoryService.Query().SingleOrDefaultAsync(
                        m => m.VendorCategoryId == proCatId, CancellationToken.None);

                    _category.ModifiedTime = DateTime.Now;
                    _category.ModifiedBy = UserSession.UserId;
                    _category.OrganizationId = UserSession.OrganizationId;
                    var usr = _mapper.Map(_category, productCat);
                    _vendorCategoryService.Update(productCat);
                    await UnitOfWork.SaveChangesAsync();
                }
                catch (Exception)
                {
                    //skip
                }
                return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ViewStaticData.DISPLAY_VENDOR_CATEGORIES);
            }
            return View(_category);
        }
		public async Task<IActionResult> ChangeVendorCategoryStatus(string id)
		{			
			string unprotectedId = IvatDataProtector.Unprotect(id);
			byte[] idBytes = Encoding.UTF8.GetBytes(unprotectedId);
			var categoryData = await _vendorCategoryService.Query()
				.SingleOrDefaultAsync(p => p.VendorCategoryId == int.Parse(Encoding.UTF8.GetString(idBytes)), CancellationToken.None);
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
				_vendorCategoryService.Update(categoryData);
				await UnitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ErrorIndicator + ex.Message;
			}
			return RedirectToAction(ControllerStaticData.DISPLAY_INDEX, ViewStaticData.DISPLAY_VENDOR_CATEGORIES);

		}

	}
}
