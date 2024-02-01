using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Reporting.NETCore;
using URF.Core.Abstractions;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.utility.StaticData;
using vms.Utility;
using vms.service.Services.SecurityService;

namespace vms.Controllers;

[SessionExpireFilter]
public class ControllerBase : Controller
{
	protected IWebHostEnvironment Environment;
	protected IUnitOfWork UnitOfWork;
	protected static IConfiguration Configuration;
	protected VmUserSession UserSession;
	protected IDataProtector IvatDataProtector;
	protected IAuditLogService IvatLogService;

	protected ControllerBase(ControllerBaseParamModel controllerBaseParamModel)
	{
		IvatLogService = controllerBaseParamModel.VatAuditLogService;
		Environment = controllerBaseParamModel.HostingEnvironment;
		UnitOfWork = controllerBaseParamModel.UnitOfWork;
		IvatDataProtector =
			controllerBaseParamModel.ProtectionProvider.CreateProtector(controllerBaseParamModel
				.VatPurposeStringConstants.UserIdQueryString);
		if (controllerBaseParamModel.HttpContextAccessor.HttpContext != null)
			UserSession =
				controllerBaseParamModel.HttpContextAccessor.HttpContext.Session.GetComplexData<VmUserSession>(
					ControllerStaticData.SESSION);
		if (Configuration != null) return;
		var builder = new ConfigurationBuilder()
			.SetBasePath(Environment.ContentRootPath)
			.AddJsonFile(ControllerStaticData.APPLICATION_JSON, true, true);
		Configuration = builder.Build();
	}

	public async Task<bool> IvatLogChanges(EnumObjectType objectType, int? primaryKey, EnumOperations operations,
		string sourceJsonString, string targetJasonString)
	{
		var auditLog = new AuditLog
		{
			ObjectTypeId = (int)objectType,
			PrimaryKey = primaryKey,
			AuditOperationId = (int)operations,
			UserId = UserSession.UserId,
			Datetime = DateTime.Now,
			Descriptions = GetChangeInformation(sourceJsonString, targetJasonString),
			IsActive = true,
			CreatedBy = UserSession.UserId,
			CreatedTime = DateTime.Now,
			OrganizationId = UserSession.OrganizationId
		};
		return await AuditLogCreate(auditLog);
	}

	public async Task<bool> AuditLogCreate(AuditLog auditLog)
	{
		try
		{
			IvatLogService.Insert(auditLog);
			await UnitOfWork.SaveChangesAsync();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	protected string GetChangeInformation(string sourceJsonString, string targetJsonString)
	{
		var sourceJObject = JsonConvert.DeserializeObject<JObject>(sourceJsonString);
		var targetJObject = JsonConvert.DeserializeObject<JObject>(targetJsonString);
		var item = new List<string>();

		if (JToken.DeepEquals(sourceJObject, targetJObject)) return string.Join(",", item);

		if (sourceJObject == null) return string.Join("<br />", item);
		foreach (var sourceProperty in sourceJObject)
		{
			if (targetJObject == null) continue;
			var targetProp = targetJObject.Property(sourceProperty.Key);
			if (targetProp != null && JToken.DeepEquals(sourceProperty.Value, targetProp.Value)) continue;

			if (sourceProperty.Key == "TrackingState" || sourceProperty.Key.ToLower() == "encryptedid") continue;
			if (targetProp != null)
				item.Add(sourceProperty.Key + ": " + sourceProperty.Value + " -> " + targetProp.Value);
		}

		return string.Join("<br />", item);
	}

	protected OperatingUserViewModel GetOperatingUserInformation()
	{
		return new OperatingUserViewModel
		{
			OrganizationId = UserSession.OrganizationId,
			UserId = UserSession.UserId,
			OperationTime = DateTime.Now
		};
	}

	protected Stream GetPdfWithOpenStream<T>(List<T> dataSource, string rdlcFileUrl, string dataSetName,
		string fileName, Dictionary<string, string> parameters = null)
	{
		var filePath = Path.Combine(Environment.ContentRootPath, "wwwroot", rdlcFileUrl);
		var reportDefinition = new FileStream(filePath, FileMode.Open);
		var report = new LocalReport();
		report.LoadReportDefinition(reportDefinition);
		if (parameters != null)
		{
			foreach (var parameter in parameters)
			{
				report.SetParameters(new ReportParameter { Name = parameter.Key, Values = { parameter.Value } });
			}
		}

		report.DataSources.Add(new ReportDataSource(dataSetName, dataSource));
		var fileByte = report.Render(RdlcStaticData.PdfFormatName);
		reportDefinition.Close();
		return new MemoryStream(fileByte);
	}

	private IActionResult GetReport<T>(List<T> dataSource, string rdlcFileUrl, string dataSetName, string fileFormat,
		string contentType, string fileExtension, string fileName, Dictionary<string, string> parameters = null,
		bool isDisplayInBrowser = false)
	{
		var filePath = Path.Combine(Environment.ContentRootPath, "wwwroot", rdlcFileUrl);
		var reportDefinition = new FileStream(filePath, FileMode.Open);
		var report = new LocalReport();
		report.LoadReportDefinition(reportDefinition);
		if (parameters != null)
		{
			foreach (var parameter in parameters)
			{
				report.SetParameters(new ReportParameter { Name = parameter.Key, Values = { parameter.Value } });
			}
		}

		report.DataSources.Add(new ReportDataSource(dataSetName, dataSource));
		var fileByte = report.Render(fileFormat);
		reportDefinition.Close();
		if (fileFormat == RdlcStaticData.PdfFormatName && isDisplayInBrowser)
		{
			return new FileContentResult(fileByte, contentType);
		}

		return File(fileByte, contentType, fileName + "." + fileExtension);
	}

	protected IActionResult DisplayPdfReport<T>(List<T> dataSource, string rdlcFileUrl, string dataSetName,
		Dictionary<string, string> parameters = null)
	{
		return GetReport(dataSource, rdlcFileUrl, dataSetName, RdlcStaticData.PdfFormatName,
			RdlcStaticData.PdfContentType, null, null, parameters, true);
	}

	protected IActionResult GetReportPdf<T>(List<T> dataSource, string rdlcFileUrl, string dataSetName, string fileName,
		Dictionary<string, string> parameters = null)
	{
		return GetReport(dataSource, rdlcFileUrl, dataSetName, RdlcStaticData.PdfFormatName,
			RdlcStaticData.PdfContentType, RdlcStaticData.PdfExtensionName, fileName, parameters);
	}

	protected IActionResult GetReportExcel<T>(List<T> dataSource, string rdlcFileUrl, string dataSetName,
		string fileName, Dictionary<string, string> parameters = null)
	{
		return GetReport(dataSource, rdlcFileUrl, dataSetName, RdlcStaticData.ExcelFormatName,
			RdlcStaticData.ExcelContentType, RdlcStaticData.ExcelExtensionName, fileName, parameters);
	}

	protected IActionResult GetReportWord<T>(List<T> dataSource, string rdlcFileUrl, string dataSetName,
		string fileName, Dictionary<string, string> parameters = null)
	{
		return GetReport(dataSource, rdlcFileUrl, dataSetName, RdlcStaticData.WordFormatName,
			RdlcStaticData.WordContentType, RdlcStaticData.WordExtensionName, fileName, parameters);
	}

	protected IActionResult ProcessReport<T>(List<T> dataSource, string rdlcFileUrl, string dataSetName,
		string fileName, Dictionary<string, string> parameters = null, int reportOption = 1)
	{
		switch (reportOption)
		{
			case 1:
				return DisplayPdfReport(dataSource, rdlcFileUrl, dataSetName, parameters);
			case 2:
				return GetReportPdf(dataSource, rdlcFileUrl, dataSetName, fileName, parameters);
			case 3:
				return GetReportExcel(dataSource, rdlcFileUrl, dataSetName, fileName, parameters);
			case 4:
				return GetReportWord(dataSource, rdlcFileUrl, dataSetName, fileName, parameters);
			default:
				return DisplayPdfReport(dataSource, rdlcFileUrl, dataSetName, parameters);
		}
	}
}