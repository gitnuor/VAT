﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.Abstractions;
using vms.entity.viewModels;
using vms.service.dbo;
using vms.service.dbo.StoredProdecure;

namespace vms.api.Controllers
{
    //    [Authorize]
    [Produces("application/json")]
    [Route("api/PurchaseNew")]
    [ApiController]
    public class PurchaseNewController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        private readonly IHostingEnvironment _environment;
        private readonly IApiSpInsertService _apiSpInsert;
        private readonly IUnitOfWork _unityOfWork;

        public PurchaseNewController(IApiSpInsertService apiSpInsert, IPurchaseOrderService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            _apiSpInsert = apiSpInsert;
            this._service = service;
            ClassName = "PurchaseController";
            this._environment = environment;
            this._unityOfWork = unityOfWork;
        }

        [HttpGet]
        [Route("GetPurchaseByOrgId/{id}")]
        public async Task<IActionResult> GetPurchaseByOrgId(int id)
        {
            try
            {
                var response = await _service.Query()
                    .Include(c => c.DebitNotes)
                    .Include("PurchaseDetails.Product")
                    .Include(c => c.CustomsAndVatcommissionarate)
                    .Include(c => c.PurchaseReason)
                    .Include(c => c.PurchasePayments)
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
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _service.Query().SingleOrDefaultAsync(x => x.PurchaseId == id, CancellationToken.None);
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

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]vmPurchasePost purchase)
        {
            if (ModelState.IsValid)
            {
                //                purchase.ApiCreatedBy = Convert.ToInt32(await GetUserIdFromClaim());
                purchase.ApiCreatedTime = DateTime.Now;
                //                purchase.OrganizationId = Convert.ToInt32(await GetCompanyIdFromClaim());
            }

            try
            {
                var status = await _apiSpInsert.InsertPurchase(purchase);

                return status ? Ok() : StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}