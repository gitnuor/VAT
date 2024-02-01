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
    [Route("api/OverHeadCost")]
    [ApiController]
    public class OverHeadCostController : ControllerBase
    {
        private readonly IOverHeadCostService _service;

        public OverHeadCostController(IOverHeadCostService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "OverHeadCostController";
        }
        [HttpGet]
        [Route("GetOverHeadCostById/{id}")]
        public async Task<IActionResult> GetOverHeadCostById(int id)
        {
            try
            {
                var response = await _service.Query().Where(c => c.OverHeadCostId == id).SelectAsync(CancellationToken.None);
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
        [Route("GetOverHeadCostByOrgId/{id}")]
        public async Task<IActionResult> GetOverHeadCostByOrgId(int id)
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
        [Route("GetOverHeadCostByModifyDate/{date}")]
        public async Task<IActionResult> GetOverHeadCostByModifyDate(DateTime? date)
        {
            int companyId = Convert.ToInt32(await GetCompanyIdFromClaim());
            try
            {
                var overHeadCostsList = await _service.Query().SelectAsync();
                var overHeadCosts = overHeadCostsList.Where(c => c.ModifiedTime > date).ToList();
                if (overHeadCosts != null)
                {
                    return Ok(overHeadCosts);
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