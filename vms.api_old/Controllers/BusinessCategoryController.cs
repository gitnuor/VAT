using System;
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
    [Route("api/BusinessCategory")]
    [ApiController]
    public class BusinessCategoryController : ControllerBase
    {
        private readonly IBusinessCategoryService _service;

        public BusinessCategoryController(IBusinessCategoryService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "BusinessCategoryController";
        }
        [HttpGet]
        [Route("GetBusinessCategoryById/{id}")]
        public async Task<IActionResult> GetBusinessCategoryById(int id)
        {
            try
            {
                var response = await _service.Query().Where(c => c.BusinessCategoryId == id).SelectAsync(CancellationToken.None);
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
    }
}