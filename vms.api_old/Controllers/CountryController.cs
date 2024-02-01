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
    [Route("api/Country")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _service;
       
        public CountryController(ICountryService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "CountryController";
        }

        [HttpGet]
        [Route("CountryById/{id}")]
        public async Task<IActionResult> CountryById(int id)
        {
            try
            {
                var response = await _service.Query().Where(c => c.CountryId == id).SelectAsync(CancellationToken.None);
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