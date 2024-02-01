using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vms.entity.StoredProcedureModel;
using vms.service.Services.ProductService;
using vms.Utility;


namespace vms.Controllers.Api
{
    [Route("api/measurementunit")]
	[ApiController]
	public class MeasurementUnitController(ControllerBaseParamModel controllerBaseParamModel,
			IProductMeasurementUnitService productMeasurementUnitService)
		: ControllerBase(controllerBaseParamModel)
	{
		[HttpGet("{productId}")]
		public async Task<ActionResult<IEnumerable<SpGetMeasurementUnitByProductModel>>> SpGetMeasurementUnitByProduct(int productId)
		{
			return Ok(await productMeasurementUnitService.SpGetMeasurementUnitByProduct(productId));
		}
	}
}
