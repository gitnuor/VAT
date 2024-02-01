using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;
using vms.service.dbo;

namespace vms.api.Controllers
{
    [Produces("application/json")]
    [Route("api/ProductCategory")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _service;

        public ProductCategoryController(IProductCategoryService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "ProductCategoryController";
        }
        [HttpGet]
        [Route("GetProductCategoryById/{id}")]
        public async Task<IActionResult> GetProductCategoryById(int id)
        {
            try
            {
                var response = await _service.Query().Where(c => c.ProductCategoryId == id).SelectAsync(CancellationToken.None);
                if (response != null)
                {
                    return Ok(response);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetProductCategoryByOrgId/{id}")]
        public async Task<IActionResult> GetProductCategoryByOrgId(int id)
        {
            try
            {
                var response = await _service.Query().Where(c => c.OrganizationId == id).SelectAsync(CancellationToken.None);
                if (response != null)
                {
                    return Ok(response);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetProductCategoryByModifyDate/{date}")]
        public async Task<IActionResult> GetProductCategoryByModifyDate(DateTime? date)
        {
            int companyId = Convert.ToInt32(await GetCompanyIdFromClaim());
            try
            {
                var productCategoriesList = await _service.Query().SelectAsync();
                var productCategories = productCategoriesList.Where(c => c.ModifiedTime > date).ToList();
                if (productCategories != null)
                {
                    return Ok(productCategories);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}