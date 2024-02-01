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
    [Route("api/DamageType")]
    [ApiController]
    public class DamageTypeController : ControllerBase
    {
        private readonly IDamageTypeService _service;

        public DamageTypeController(IDamageTypeService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "DamageTypeController";
        }
        [HttpGet]
        [Route("GetDamageTypeById/{id}")]
        public async Task<IActionResult> GetDamageTypeById(int id)
        {
            try
            {
                var response = await _service.Query().Where(c => c.DamageTypeId == id).SelectAsync(CancellationToken.None);
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
        [Route("GetDamageTypeByOrgId/{id}")]
        public async Task<IActionResult> GetDamageTypeByOrgId(int id)
        {
            try
            {
                var response = await _service.Query()//.Where(c => c.OrganizationId == id)
                    .SelectAsync(CancellationToken.None);
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
        [Route("GetDamageTypeByModifyDate/{date}")]
        public async Task<IActionResult> GetDamageTypeByModifyDate(DateTime? date)
        {
            int companyId = Convert.ToInt32(await GetCompanyIdFromClaim());
            try
            {
                var damageTypesList = await _service.Query().SelectAsync();
                var damageTypes = damageTypesList//.Where(c => c.ModifiedTime > date)
                    .ToList();
                if (damageTypes != null)
                {
                    return Ok(damageTypes);
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