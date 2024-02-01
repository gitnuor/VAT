using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;
using vms.api.Models;
using vms.service.dbo;

namespace vms.api.Controllers
{
    [Produces("application/json")]
    [Route("api/PriceDeclare")]
    [ApiController]
    public class PriceDeclareController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IPriceSetupService _priceService;
        public PriceDeclareController(IPriceSetupService priceSetup,IProductService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._priceService = priceSetup;
            this._service = service;
            ClassName = "PriceDeclareController";
        }
        [HttpGet]
        [Route("ProductByOrgId/{id}")]
        public async Task<IActionResult> ProductByOrgId(int id)
        {
            try
            {
                var response = await _service.Query().Where(c=>c.OrganizationId==id).SelectAsync(CancellationToken.None);
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
        [Route("ProductByProductId/{id}")]
        public async Task<IActionResult> ProductByProductId(int id)
        {
            try
            {
                var response = await _service.Query().SingleOrDefaultAsync(x => x.ProductId == id, CancellationToken.None);
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
        [Route("PriceByProductId/{prodId}")]
        public async Task<IActionResult> PriceByProductId(int prodId)
        {
            try
            {
                var response = await _priceService.Query()
                    .Include(c=>c.Organization)
                    .Include(c=>c.Product.MeasurementUnit)
                    .Include("PriceSetupProductCosts.RawMaterial.MeasurementUnit")
                    .Include("PriceSetupProductCosts.OverHeadCost")
                    .Where(c=>c.ProductId== prodId && c.IsActive==true)
                    .SelectAsync(CancellationToken.None);
                var data=response.Select(c => new PriceInfo
                {
                    PriceSetupId = c.PriceSetupId,
                    ProductId = c.ProductId,
                    OrganizationId=c.OrganizationId,
                    OrganizationName=c.Organization.Name,
                    ProductName = c.Product.Name,
                    MeasurementUnitName = c.Product.MeasurementUnit.Name,
                    BaseTp=c.BaseTp,
                    Mrp = c.Mrp,
                    SalesUnitPrice = c.SalesUnitPrice,
                    IsMushakSubmitted=c.IsMushakSubmitted,
                    SubmissionSl=c.SubmissionSl,
                    MushakSubmissionDate=c.MushakSubmissionDate,
                    EffectiveFrom=c.EffectiveFrom,
                    EffectiveTo = c.EffectiveTo,
                    ReferenceKey=c.ReferenceKey,
                    PriceSetupProductCosts = c.PriceSetupProductCosts.ToList()
                }).ToList();
                if (data != null)
                {
                    return Ok(data);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("ProductByModifyDate/{modifyDate}")]
        public async Task<IActionResult> ProductByModifyDate(DateTime? modifyDate)
        {
            try
            {
                var response = await _service.Query().Where(c=>c.ModifyDate>=modifyDate)
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