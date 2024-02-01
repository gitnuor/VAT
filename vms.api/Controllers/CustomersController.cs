using Microsoft.AspNetCore.Mvc;
using vms.api.Authentication;
using vms.api.VmsApiUtility;
using vms.entity.Dto.Customer;
using vms.entity.models;
using vms.service.Services.ThirdPartyService;

namespace vms.api.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController : VmsApiBaseController
{
	private readonly ICustomerService _customerService;

	public CustomersController(ControllerBaseParamModel baseModel, ICustomerService customerService) : base(baseModel)
	{
		_customerService = customerService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
	{
		return Ok(await _customerService.GetCustomerDtoListByOrg(CurrentUser.ProtectedOrganizationId));
	}

	[HttpGet("local")]
	public async Task<ActionResult<IEnumerable<CustomerLocalDto>>> GetLocal()
	{
		return Ok(await _customerService.GetCustomerLocalDtoListByOrg(CurrentUser.ProtectedOrganizationId));
	}

	[HttpGet("local/{id:int}")]
    [ActionName("GetLocal")]
	public async Task<ActionResult<ViewCustomerLocal>> GetLocal(int id)
	{
		return Ok(await _customerService.GetCustomerLocalById(id));
	}

	[HttpGet("foreign")]
	public async Task<ActionResult<IEnumerable<CustomerForeignDto>>> GetForeign()
	{
		return Ok(await _customerService.GetCustomerForeignDtoListByOrg(CurrentUser.ProtectedOrganizationId));
	}

	[HttpPost("local")]
	public async Task<ActionResult> PostLocal([FromBody] CustomerLocalPostDto customer)
	{
		Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var key);
		try
		{
			var id = await _customerService.InsertOrUpdateCustomerLocalFromApi(customer, key);
            return CreatedAtAction("GetLocal", new { id }, id);
        }
		catch (Exception exception)
		{
			Console.WriteLine(exception);
			throw;
		}
	}
}