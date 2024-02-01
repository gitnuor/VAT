using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vms.entity.models;
using vms.service.Services.ThirdPartyService;
using vms.Utility;

namespace vms.Controllers.Api;

[Route("api/customerbybranch")]
[ApiController]
public class CustomerByBranchController(ControllerBaseParamModel controllerBaseParamModel,
		ICustomerService customerService)
	: ControllerBase(controllerBaseParamModel)
{
	// GET: api/<CustomerByBranchController>
	[HttpGet("{branchId}")]
	public async Task<IEnumerable<ViewCustomerWithBranch>> Get(int branchId)
	{
		return await customerService.GetCustomerWithBranchListByBranch(UserSession.OrganizationId, branchId);
	}
}