using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using vms.api.Authentication;
using vms.api.VmsApiUtility;
using vms.entity.Dto.Sales;
using vms.entity.Dto.SalesExport;
using vms.entity.Dto.SalesLocal;
using vms.service.Services.TransactionService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vms.api.Controllers;

[Route("api/sales")]
[ApiController]
public class SalesController : VmsApiBaseController
{
	private readonly ISaleService _saleService;

	public SalesController(ControllerBaseParamModel baseModel, ISaleService saleService) : base(baseModel)
	{
		_saleService = saleService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<SaleDto>>> GetAll()
	{
		return Ok(await _saleService.GetSalesDtoListByOrganization(CurrentUser.ProtectedOrganizationId));
	}


	[HttpGet("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesDefaultResponseType]
	[ActionName("Get")]
	public async Task<ActionResult<SalesLocalWithDetailDto>> Get(int id)
	{
		var sale = await _saleService.GetSalesLocalDto(VatDataProtector.Protect(id.ToString()));
		if (sale == null)
		{
			return NotFound();
		}

		return Ok(sale);
	}

	[HttpPost]
	public async Task<ActionResult> CreateSale([FromBody] SalesCombinedPostDto sales)
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
			return CreatedAtAction("Get", new { id }, id);
		}
		catch (Exception exception)
		{
			throw new Exception(exception.Message);
		}
	}

	[HttpGet("local")]
	public async Task<ActionResult<IEnumerable<SalesLocalDto>>> GetLocal()
	{
		return Ok(await _saleService.GetSalesLocalDtoListByOrganization(CurrentUser.ProtectedOrganizationId));
	}

	[HttpGet("local/{id}", Name = "GetLocalSale")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesDefaultResponseType]
	public async Task<ActionResult<SalesLocalWithDetailDto>> GetLocal(int id)
	{
		var sale = await _saleService.GetSalesLocalDto(VatDataProtector.Protect(id.ToString()));
		if (sale == null)
		{
			return NotFound();
		}

		return Ok(sale);
	}

	[HttpGet("foreign")]
	public async Task<ActionResult<IEnumerable<SalesExportDto>>> GetForeign()
	{
		return Ok(await _saleService.GetSalesExportDtoListByOrganization(CurrentUser.ProtectedOrganizationId));
	}

	[HttpGet("foreign/{id}", Name = "GetForeignSale")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesDefaultResponseType]
	public async Task<ActionResult<IEnumerable<SalesExportDto>>> GetForeign(int id)
	{
		var sale = await _saleService.GetSalesExportDto(VatDataProtector.Protect(id.ToString()));
		if (sale == null)
		{
			return NotFound();
		}

		return Ok(sale);
	}
}