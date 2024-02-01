using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using vms.entity.StoredProcedureModel;
using vms.service.Services.ProductService;
using vms.Utility;

namespace vms.Controllers.Api;

[Route("api/numberofrawmaterialwithnotifiablechange")]
[ApiController]
public class NumberOfRawMaterialWithNotifiableChangeController(ControllerBaseParamModel controllerBaseParamModel,
		IProductStoredProcedureService productStoredProcedureService)
	: ControllerBase(controllerBaseParamModel)
{
	// GET: api/<NumberOfRawMaterialWithNotifiableChangeController>
	[HttpGet("{productId}")]
	public IEnumerable<SpGetProductForSale> Get(int productId)
	{
		return productStoredProcedureService.GetProductForSale(UserSession.OrganizationId, productId);
	}
}