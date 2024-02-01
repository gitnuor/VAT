using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Reporting.NETCore;
using vms.api.Authentication;
using vms.api.VmsApiUtility;
using vms.entity.Dto.SalesLocal;
using vms.entity.Enums;
using vms.entity.viewModels;
using vms.service.Services.TransactionService;
using vms.utility.StaticData;
using vms.service.Services.MushakService;
using vms.utility;

namespace vms.api.Controllers;

[Route("api/sales-with-mushak")]
[ApiController]
public class SalesWithMushakController : VmsApiBaseController
{
	private readonly ISaleService _saleService;
	private readonly IMushakGenerationService _mushakGenerationService;

	public SalesWithMushakController(ControllerBaseParamModel baseModel, ISaleService saleService,
		IMushakGenerationService mushakGenerationService) : base(baseModel)
	{
		_saleService = saleService;
		_mushakGenerationService = mushakGenerationService;
	}

	[HttpPost]
	public async Task<ActionResult> CreateSale([FromBody] SalesCombinedPostDto sales)
	// public async Task<ActionResult> CreateSale()
	{
		string apiData;
		using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
		{
			apiData = await reader.ReadToEndAsync();
		}

		if (!ModelState.IsValid)
			return BadRequest(modelState: ModelState);

		try
		{
			Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var key);
			var id = await _saleService.InsertApiSale(sales, apiData, key);
			var salestaxInvoices = await _mushakGenerationService.Mushak6P3(id);
			var amountInWords =
				VmsNumberToWord.ConvertAmountUsingTakaPoishaInEng(salestaxInvoices.Sum(x => x.ProdPriceInclVATAndDuty)
					.Value);
			Dictionary<string, string> paramReport =
				new Dictionary<string, string>();
			paramReport.Add("AmountInWordsParam", amountInWords);
			var model = new vmMushak6P3ById()
			{
				Language = (int)EnumLanguage.English,
				SalesTaxInvoices = salestaxInvoices
			};
			return GetReportPdf(salestaxInvoices,
				RdlcReportFileOption.MushakSixPointThreeRdlcUrl,
				RdlcReportFileOption.MushakSixPointThreeRdlcUrlDsName,
				StringGenerator.AddCurrentTimeToString(RdlcReportFileOption.MushakSixPointThreeRdlcUrlFileName),
				paramReport);
		}
		catch (Exception exception)
		{
			throw new Exception(exception.Message);
		}
	}

	private ActionResult GetReport<T>(List<T> dataSource, string rdlcFileUrl, string dataSetName, string fileFormat,
		string contentType, string fileExtension, string fileName, Dictionary<string, string> parameters = null,
		bool isDisplayInBrowser = false)
	{
		var filePath = Path.Combine(WhEnvironment.ContentRootPath, "wwwroot", rdlcFileUrl);
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

	protected ActionResult GetReportPdf<T>(List<T> dataSource, string rdlcFileUrl, string dataSetName, string fileName,
		Dictionary<string, string> parameters = null)
	{
		return GetReport(dataSource, rdlcFileUrl, dataSetName, RdlcStaticData.PdfFormatName,
			RdlcStaticData.PdfContentType, RdlcStaticData.PdfExtensionName, fileName, parameters);
	}
}