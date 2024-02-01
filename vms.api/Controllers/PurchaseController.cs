using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using vms.api.Authentication;
using vms.api.VmsApiUtility;
using vms.entity.Dto.PurchaseLocal;
using vms.entity.Dto.SalesLocal;
using vms.entity.models;
using vms.service.Services.TransactionService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vms.api.Controllers;

[Route("api/purchase")]
[ApiController]
public class PurchaseController : VmsApiBaseController
{
	private readonly IPurchaseService _purchaseService;

	public PurchaseController(ControllerBaseParamModel baseModel, IPurchaseService purchaseService) : base(baseModel)
	{
		_purchaseService = purchaseService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ViewSale>>> GetAll()
	{
		return Ok(await _purchaseService.GetPurchaseListByOrganization(CurrentUser.ProtectedOrganizationId));
	}

	[HttpGet("local")]
	public async Task<ActionResult<IEnumerable<ViewPurchaseLocal>>> GetLocal()
	{
		return Ok(await _purchaseService.GetPurchaseLocalListByOrganization(CurrentUser.ProtectedOrganizationId));
	}

	[HttpGet("foreign")]
	public async Task<ActionResult<IEnumerable<ViewPurchaseImport>>> GetForeign()
	{
		return Ok(await _purchaseService.GetPurchaseImportListByOrganization(CurrentUser.ProtectedOrganizationId));
	}





	[HttpGet("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesDefaultResponseType]
	[ActionName("Get")]
	public async Task<ActionResult<SalesLocalWithDetailDto>> Get(int id)
	{
		var purchase = await _purchaseService.GetPurchaseDetails(VatDataProtector.Protect(id.ToString()));
		if (purchase == null)
		{
			return NotFound();
		}
		return Ok(purchase);
	}

	[HttpPost("local")]
	public async Task<ActionResult> Post([FromBody] PurchaseLocalPostDto purchase)
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
			var id = await _purchaseService.InsertApiLocalPurchase(purchase, apiData, key);
			return CreatedAtAction("Get", new { id }, id);
		}
		catch (Exception exception)
		{
			throw new Exception(exception.Message);
		}
	}
}