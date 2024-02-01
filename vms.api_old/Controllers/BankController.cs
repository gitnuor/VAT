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
    [Route("api/Bank")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _service;
        //this is test comments
        public BankController(IBankService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "BankController";
        }
        [HttpGet]
        [Route("GetBankById/{id}")]
        public async Task<IActionResult> GetBankById(int id)
        {
            try
            {
                var response = await _service.Query().Where(c => c.BankId == id).SelectAsync(CancellationToken.None);
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