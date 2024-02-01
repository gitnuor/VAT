using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using vms.api.Authentication;
using vms.api.VmsApiUtility;
using vms.entity.Dto.Sales;
using vms.entity.Dto.SalesLocal;
using vms.service.Services.TransactionService;


namespace vms.api.Controllers;

[Route("api/local-sales")]
[ApiController]
public class LocalSalesController : VmsApiBaseController
{
	private readonly ISaleService _saleService;
	public LocalSalesController(ControllerBaseParamModel baseModel, ISaleService saleService) : base(baseModel)
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
	public async Task<ActionResult> CreateLocalSale([FromBody] SalesLocalPostDto sales)
	{
		if (!ModelState.IsValid)
			return BadRequest(modelState: ModelState);

		//if (sales.Token != "62e74fd7-5fa9-42ca-8542-9d046289302a")
		//{
		//	ModelState.AddModelError("Token", "Invalid Token");
		//	return BadRequest(modelState: ModelState);
		//}

		string apiData;
		using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
		{
			apiData = await reader.ReadToEndAsync();
		}
		try
		{
			Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var key);
			var id = await _saleService.InsertLocalSale(sales, apiData, key);
			return CreatedAtAction("Get",  new { id }, id);
		}
		catch (Exception exception)
		{
			throw new Exception(exception.Message);
		}
			
	}
}