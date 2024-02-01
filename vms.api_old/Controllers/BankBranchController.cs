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
    [Route("api/BankBranch")]
    [ApiController]
    public class BankBranchController : ControllerBase
    {
        private readonly IBankBranchService _service;

        public BankBranchController(IBankBranchService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "BankBranchController";
        }
        [HttpGet]
        [Route("GetBankBranchById/{id}")]
        public async Task<IActionResult> GetBankBranchById(int id)
        {
            try
            {
                var response = await _service.Query()
                    // .Where(c => c.BankBranchId == id)
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
    }
}