using Microsoft.AspNetCore.Mvc;
using vms.api.Authentication;
using vms.api.VmsApiUtility;
using vms.entity.Dto.Customer;
using vms.entity.Dto.Vendor;
using vms.entity.models;
using vms.service.ServiceImplementations.ThirdPartyService;
using vms.service.Services.ThirdPartyService;

namespace vms.api.Controllers;

[Route("api/vendors")]
[ApiController]
public class VendorsController : VmsApiBaseController
{
    private readonly IVendorService _vendorService;

    public VendorsController(ControllerBaseParamModel baseModel, IVendorService vendorService) : base(baseModel)
    {
        _vendorService = vendorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VendorDto>>> GetAll()
    {
        return Ok(await _vendorService.GetVendorDtoListByOrg(CurrentUser.ProtectedOrganizationId));
    }

    [HttpGet("local")]
    public async Task<ActionResult<IEnumerable<VendorLocalDto>>> GetLocal()
    {
        return Ok(await _vendorService.GetVendorLocalDtoListByOrg(CurrentUser.ProtectedOrganizationId));
    }

    [HttpGet("local/{id:int}")]
    [ActionName("GetLocal")]
    public async Task<ActionResult<CustomerLocalDto>> GetLocal(int id)
    {
        return Ok(await _vendorService.GetVendorListByOrg(id));
    }

    [HttpGet("foreign")]
    public async Task<ActionResult<IEnumerable<VendorForeignDto>>> GetForeign()
    {
        return Ok(await _vendorService.GetVendorForeignDtoListByOrg(CurrentUser.ProtectedOrganizationId));
    }

    [HttpPost("local")]
    public async Task<ActionResult> PostLocal([FromBody] VendorLocalPostDto vendorLocal)
    {
        Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var key);
        try
        {
            var id = await _vendorService.InsertOrUpdateVendorLocalFromApi(vendorLocal, key);
            return CreatedAtAction("GetLocal", new { id }, id);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}