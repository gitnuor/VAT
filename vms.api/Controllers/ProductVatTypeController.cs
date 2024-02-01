using Microsoft.AspNetCore.Mvc;
using vms.api.VmsApiUtility;
using vms.entity.Dto.Product;
using vms.service.Services.SettingService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vms.api.Controllers;

[Route("api/products-vat-type")]
[ApiController]
public class ProductVatTypeController : VmsApiBaseController
{
	private readonly IProductVatTypeService _productVatTypeService;
	public ProductVatTypeController(ControllerBaseParamModel baseModel, IProductVatTypeService productVatTypeService) : base(baseModel)
	{
		_productVatTypeService = productVatTypeService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ProductVatTypeDto>>> GetAll()
	{
		return Ok(await _productVatTypeService.GetProductVatTypeListDto());
	}
}