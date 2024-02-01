using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.ProductMeasurementUnit;
using vms.service.Services.ProductService;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers
{
    public class ProductMeasurementUnitController : ControllerBase
	{
		private readonly IProductMeasurementUnitService _service;
		private readonly IProductService _productService;
		private readonly IMeasurementUnitService _iMeasurementUnitService;
		private readonly IMapper _iMapper;

		public ProductMeasurementUnitController(ControllerBaseParamModel controllerBaseParamModel, IProductMeasurementUnitService service, IMeasurementUnitService iMeasurementUnitService, IProductService productService, IMapper iMapper) : base(controllerBaseParamModel)
		{
			_service = service;
			_iMeasurementUnitService = iMeasurementUnitService;
			_productService = productService;
			_iMapper = iMapper;
		}


		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_MEASURMENT_UNIT_CAN_VIEW)]
		public async Task<IActionResult> Index()
		{
			var organizationId = UserSession.ProtectedOrganizationId;
			var measurementUnits = await _service.GetProductMeasurementUnits(organizationId);
			measurementUnits.OrderByDescending(x => x.ProductMeasurementUnitId);
			return View(measurementUnits);
		}

		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_MEASURMENT_UNIT_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_MEASURMENT_UNIT_CAN_ADD)]
		public async Task<IActionResult> Create()
		{
			var model = new ProductMeasurementUnitCreateViewModel
			{
				ProductList = await _productService.GetProductsSelectList(UserSession.ProtectedOrganizationId),
				MeasurementUnitList = await _iMeasurementUnitService.GetMeasurementUnitSelectList(UserSession.ProtectedOrganizationId)

			};
			return View(model);
		}

		[HttpPost]

		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_MEASURMENT_UNIT_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_MEASURMENT_UNIT_CAN_ADD)]
		public async Task<IActionResult> Create(ProductMeasurementUnitCreateViewModel productMeasurementUnitCreateVm)
		{
			if (ModelState.IsValid)
			{				
				productMeasurementUnitCreateVm.CreatedBy = UserSession.UserId;
				productMeasurementUnitCreateVm.CreatedTime = DateTime.Now;
				productMeasurementUnitCreateVm.IsActive = true;
				productMeasurementUnitCreateVm.OrganizationId = UserSession.OrganizationId;

				var data = _iMapper.Map<ProductMeasurementUnit>(productMeasurementUnitCreateVm);
				_service.Insert(data);

				await UnitOfWork.SaveChangesAsync();

				var jObj = JsonConvert.SerializeObject(productMeasurementUnitCreateVm, Formatting.None,
					new JsonSerializerSettings()
					{
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					});

				AuditLog au = new AuditLog
				{
					ObjectTypeId = (int)EnumObjectType.ProductMeasurementUnit,
					PrimaryKey = productMeasurementUnitCreateVm.ProductMeasurementUnitId,
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
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME
					;
				return RedirectToAction(nameof(Index));
			}
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
			return View(productMeasurementUnitCreateVm);
		}

		[VmsAuthorize(FeatureList.ADMINSTRATION)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_MEASURMENT_UNIT_CAN_VIEW)]
        [VmsAuthorize(FeatureList.ADMINSTRATION_PRODUCT_MEASURMENT_UNIT_CAN_EDIT)]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int productMeasurementUnitId = int.Parse(IvatDataProtector.Unprotect(id));
            var productMeasurementUnit = await _service.Query().SingleOrDefaultAsync(x => x.ProductMeasurementUnitId == productMeasurementUnitId);
            productMeasurementUnit.EncryptedId = id;

            if (productMeasurementUnit == null)
            {
                return NotFound();
            }

            var model = new ProductMeasurementUnitCreateViewModel
			{
				ProductMeasurementUnitId = productMeasurementUnit.ProductMeasurementUnitId,
				OrganizationId = productMeasurementUnit.OrganizationId,
                ProductId = productMeasurementUnit.ProductId,
                MeasurementUnitId = productMeasurementUnit.MeasurementUnitId,
                ConversionRatio = productMeasurementUnit.ConversionRatio,
                ProductList = await _productService.GetProductsSelectList(UserSession.ProtectedOrganizationId),
				MeasurementUnitList = await _iMeasurementUnitService.GetMeasurementUnitSelectList(UserSession.ProtectedOrganizationId),
                CreatedBy = productMeasurementUnit.CreatedBy,
                CreatedTime = productMeasurementUnit.CreatedTime,
				ModifiedBy = productMeasurementUnit.ModifiedBy,
				ModifiedTime = productMeasurementUnit.ModifiedTime,
            };          
            return View(model);
        }


		[HttpPost]

		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_EDIT)]
		public async Task<IActionResult> Edit(ProductMeasurementUnitCreateViewModel productMeasurementUnitCreateVm, string id)
		{
			int productMeasurementUnitId = int.Parse(IvatDataProtector.Unprotect(id));
			if (productMeasurementUnitId != productMeasurementUnitCreateVm.ProductMeasurementUnitId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var productMeasurementUnit = await _service.Query().SingleOrDefaultAsync(p => p.ProductMeasurementUnitId == productMeasurementUnitId, CancellationToken.None);
					var prevObj = JsonConvert.SerializeObject(productMeasurementUnit, Formatting.None,
						new JsonSerializerSettings()
						{
							ReferenceLoopHandling = ReferenceLoopHandling.Ignore
						});
					productMeasurementUnit.ProductId = productMeasurementUnitCreateVm.ProductId;
					productMeasurementUnit.MeasurementUnitId = productMeasurementUnitCreateVm.MeasurementUnitId;
					productMeasurementUnit.ConversionRatio = productMeasurementUnitCreateVm.ConversionRatio;

					_service.Update(productMeasurementUnit);
					await UnitOfWork.SaveChangesAsync();
					var jObj = JsonConvert.SerializeObject(productMeasurementUnitCreateVm, Formatting.None,
						new JsonSerializerSettings()
						{
							ReferenceLoopHandling = ReferenceLoopHandling.Ignore
						});


					AuditLog au = new AuditLog
					{
						ObjectTypeId = (int)EnumObjectType.ProductMeasurementUnit,
						PrimaryKey = productMeasurementUnitCreateVm.ProductMeasurementUnitId,
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
					TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME
						;
				}

				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				return RedirectToAction(nameof(Index));
			}

			return View(productMeasurementUnitCreateVm);
		}

		[VmsAuthorize(FeatureList.ADMINSTRATION)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_VIEW)]
		[VmsAuthorize(FeatureList.ADMINSTRATION_MEASURMENT_UNIT_CAN_DELETE)]
		public async Task<IActionResult> ChangeProductMeasurementUnitStatus(string id)

		{
			var msu = await _service.Query().SingleOrDefaultAsync(p => p.ProductMeasurementUnitId == int.Parse(IvatDataProtector.Unprotect(id)), CancellationToken.None);

			if (msu.IsActive == true)
			{
				msu.IsActive = false;
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;

			}
			else
			{
				msu.IsActive = true;
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;

			}
			_service.Update(msu);
			await UnitOfWork.SaveChangesAsync();

			AuditLog au = new AuditLog
			{
				ObjectTypeId = (int)EnumObjectType.ProductMeasurementUnit,
				PrimaryKey = msu.ProductMeasurementUnitId,
				AuditOperationId = (int)EnumOperations.Delete,
				UserId = UserSession.UserId,
				Datetime = DateTime.Now,
				Descriptions = "IsActive:0",
				IsActive = true,
				CreatedBy = UserSession.UserId,
				CreatedTime = DateTime.Now,
				OrganizationId = UserSession.OrganizationId
			};


			var au_status = await AuditLogCreate(au);
			return RedirectToAction(nameof(Index));
		}
	}
}
