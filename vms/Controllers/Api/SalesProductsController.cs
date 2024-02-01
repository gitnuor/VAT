using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using vms.entity.StoredProcedureModel;
using vms.service.Services.ProductService;
using vms.Utility;

namespace vms.Controllers.Api;

[Route("api/salesproducts")]
[ApiController]
public class SalesProductsController(ControllerBaseParamModel controllerBaseParamModel,
		IProductStoredProcedureService productStoredProcedureService)
	: ControllerBase(controllerBaseParamModel)
{
	// GET: api/<SalesProductsController>
	[HttpGet("{branchId}")]
	public IEnumerable<SpGetProductForSale> Get(int branchId)
	{
		return productStoredProcedureService.GetProductForSale(UserSession.OrganizationId, branchId);
	}
}