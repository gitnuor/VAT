using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.entity.viewModels.ProductReport;
using vms.service.Services.ReportService;
using vms.service.Services.SettingService;
using vms.utility;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers
{
	public class ProductReportController(ControllerBaseParamModel controllerBaseParamModel,
			IProductReportService iProductReportService, IProductTypeService productTypeService,
			IReportOptionService reportOptionService,
			IOrgBranchService branchService)
		: ControllerBase(controllerBaseParamModel)
	{
		public async Task<IActionResult> ProductCurrentStock()
		{
			var model = new ProductReportParameterViewModel
			{
				BranchList = await branchService.GetOrgBranchSelectListByUser(UserSession.ProtectedOrganizationId,
					UserSession.BranchIds, UserSession.IsRequireBranch),
				ProductTypeSelectListItems = await productTypeService.GetProductTypeOnlyInventorySelectList(),
				ReportOptionSelectListItems = reportOptionService.GetReportDisplayOrExportTypeSelectList()
			};
			return View(model);
		}

		[HttpPost]
		[VmsAuthorize(FeatureList.REPORTS_PRODUCT_CURRENT_STOCK)]
		public async Task<IActionResult> ProductCurrentStock(ProductReportParameterViewModel model)
		{
			model.OgrId = UserSession.OrganizationId;
			var purchaseCalcBook =
				await iProductReportService.ProductCurrentStockReport(model.OgrId, model.OgrBranchId ?? 0,
					model.ProductTypeId ?? 0);

			return ProcessReport(purchaseCalcBook,
				RdlcReportFileOption.ProductCurrentStockReportUrl,
				RdlcReportFileOption.ProductCurrentStockReportDsName,
				StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.ProductCurrentStockReportFileName),
				GetParameterForProductReport("Product Current Stock"), model.ReportProcessOptionId
			);
		}

		public async Task<IActionResult> ProductStock()
		{
			var currentDate = DateTime.Today;
			var model = new ProductStockParameterViewModel
			{
				BranchSelectListItems = await branchService.GetOrgBranchSelectListByUser(
					UserSession.ProtectedOrganizationId, UserSession.BranchIds, UserSession.IsRequireBranch),
				ProductTypeSelectListItems = await productTypeService.GetProductTypeOnlyInventorySelectList(),
				ToDate = currentDate,
				ReportOptionSelectListItems = reportOptionService.GetReportDisplayOrExportTypeSelectList()
			};
			return View(model);
		}

		[HttpPost]
		//[VmsAuthorize(FeatureList.REPORTS_PRODUCT_CURRENT_STOCK)]
		public async Task<IActionResult> ProductStock(ProductStockParameterViewModel model)
		{
			string selectDate = model.ToDate.ToString("dd/MM/yyyy");
			string reportName = "Product Stock";
			string rptHeader = $"{reportName} On {selectDate}";

			model.OgrId = UserSession.OrganizationId;
			var productStockData =
				await iProductReportService.ProductStockReport(model.OgrId, model.OgrBranchId ?? 0,
					model.ProductTypeId ?? 0, model.ToDate);

			return ProcessReport(productStockData,
				RdlcReportFileOption.ProductStockReportUrl,
				RdlcReportFileOption.ProductStockReportDsName,
				StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.ProductStockReportFileName),
				GetParameterForProductReport(rptHeader), model.ReportProcessOptionId
			);
		}

		private Dictionary<string, string> GetParameterForProductReport(string reportHeaderName)
		{
			return new Dictionary<string, string>
			{
				{ "ReportNameHeader", reportHeaderName }
			};
		}
	}
}