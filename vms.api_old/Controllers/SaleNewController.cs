using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using URF.Core.Abstractions;
using vms.entity.viewModels;
using vms.service.dbo;
using vms.service.dbo.StoredProdecure;

namespace vms.api.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/SaleNew")]
    [ApiController]
    public class SaleNewController : ControllerBase
    {
        private readonly ISaleService _service;
        private readonly IHostingEnvironment _environment;
        private readonly IApiSpInsertService _apiSpInsert;
        private readonly IUnitOfWork _unityOfWork;

        public SaleNewController(IApiSpInsertService apiSpInsert, ISaleService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            _apiSpInsert = apiSpInsert;
            _service = service;
            ClassName = "SaleController";
            _environment = environment;
            _unityOfWork = unityOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]vmSalesPost sales)
        {
            if (ModelState.IsValid)
            {
                //                sales.ApiCreatedBy   = Convert.ToInt32(await GetUserIdFromClaim());
                sales.ApiCreatedTime = DateTime.Now;
                //                sales.OrganizationId = Convert.ToInt32(await GetCompanyIdFromClaim());
            }

            try
            {
                var status = await _apiSpInsert.InsertSale(sales);
                return status ? Ok() : StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}