using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.Abstractions;
using vms.entity.viewModels;
using vms.service.dbo;
using vms.service.dbo.StoredProdecure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vms.api.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/sale")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _service;

        //private readonly IHostingEnvironment _environment;
        private readonly IUnitOfWork _unityOfWork;

        private readonly IApiSpInsertService _apiSpInsert;

        public SaleController(IApiSpInsertService apiSpInsert, ISaleService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            _apiSpInsert = apiSpInsert;
            this._service = service;
            ClassName = "SaleController";
            this._environment = environment;
            this._unityOfWork = unityOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "hi", "reza" };
        }

        [HttpGet]
        [Route("GetSaleByOrgId/{id}")]
        public async Task<IActionResult> GetSaleByOrgId(int id)
        {
            try
            {
                var response = await _service.Query()
                    .Include(c => c.CreditNotes)
                    .Include("SalesDetails.Product")
                    .Include(c => c.SalesPaymentReceives)
                    .Include(c => c.Organization)
                    .Where(c => c.OrganizationId == id)
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

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetSaleById")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var email = await GetEmailFromClaim();
                var response = await _service.Query().SingleOrDefaultAsync(x => x.SalesId == id, CancellationToken.None);
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

        [HttpPost("{id}", Name = "CreditNote")]
        public async Task<IActionResult> CreditNote([FromBody]vmCreditNote vm)
        {
            var createdBy = Convert.ToInt32(await GetUserIdFromClaim());
            var organizationId = Convert.ToInt32(await GetCompanyIdFromClaim());
            bool status = false;
            try
            {
                if (vm.CreditNoteDetails.Count > 0)
                {
                    vm.CreatedBy = createdBy;
                    vm.CreatedTime = DateTime.Now;

                    status = await _service.InsertCreditNote(vm);
                }

                if (status == true)
                {
                    return Ok();
                }
                else
                    return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}