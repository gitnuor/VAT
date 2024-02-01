using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;
using vms.entity.models;
using vms.service.dbo;

namespace vms.api.Controllers
{
    [Produces("application/json")]
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomerController(ICustomerService service, IHostingEnvironment hostingEnvironment, IUnitOfWork unitOfWork) : base(hostingEnvironment, unitOfWork)
        {
            this._service = service;
            ClassName = "CustomerController";
        }
    
        [HttpGet]
        [Route("GetCustomer/{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            int companyId = Convert.ToInt32(await GetCompanyIdFromClaim());
            try
            {
                var customerList = await _service.Query().SelectAsync() ;
                var customer = customerList.Where(c => c.CustomerId == id).SingleOrDefault();
                if (customer != null)
                {
                    return Ok(customer);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCustomerByOrgId/{id}")]
        public async Task<IActionResult> GetCustomerByOrgId(int id)
        {
            int companyId = Convert.ToInt32(await GetCompanyIdFromClaim());
            try
            {
                var customerList = await _service.Query().SelectAsync();
                var customer = customerList.Where(c => c.OrganizationId == id).SingleOrDefault();
                if (customer != null)
                {
                    return Ok(customer);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetCustomerByModifyDate/{date}")]
        public async Task<IActionResult> GetCustomerByModifyDate(DateTime? date)
        {
            int companyId = Convert.ToInt32(await GetCompanyIdFromClaim());
            try
            {
                var customerList = await _service.Query().SelectAsync();
                var customer = customerList.Where(c => c.ModifiedTime >date).ToList();
                if (customer != null)
                {
                    return Ok(customer);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Customer customer)
        {
            try
            {
                customer.CreatedTime = DateTime.Now;
                customer.CreatedBy = 1;
                customer.IsActive = true;
                customer.OrganizationId = 6;
                _service.Insert(customer);
                await _unitOfWork.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}