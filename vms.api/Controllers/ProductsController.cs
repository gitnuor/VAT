using Microsoft.AspNetCore.Mvc;
using vms.api.Authentication;
using vms.api.VmsApiUtility;
using vms.entity.Dto.Product;
using vms.entity.models;
using vms.service.Services.ProductService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vms.api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : VmsApiBaseController
{
	private readonly IProductService _productService;
	public ProductsController(ControllerBaseParamModel baseModel, IProductService productService) : base(baseModel)
	{
		_productService = productService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
	{
		return Ok(await _productService.GetProductDtoListByOrg(CurrentUser.ProtectedOrganizationId));
	}

	[HttpPost]
	public async Task Post([FromBody] ProductPostDto product)
	{
		Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var key);
		try
		{
			await _productService.InsertProduct(product, key);
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception);
			throw;
		}
	}
}