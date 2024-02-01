﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;
using vms.entity.models;
using vms.service.dbo;
using vms.utility.StaticData;

namespace vms.api.Controllers
{
    [Produces(ControllerStaticData.RESPONSE_TYPE_JSON)]
    [Route("api/measurementunit")]
    public class MeasurementUnitController : vms.api.Controllers.ControllerBase
    {
        private readonly IMeasurementUnitService _service;
        public MeasurementUnitController(IMeasurementUnitService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "MeasurementUnitController";
        }
        // GET: api/Address
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //int clientId = Convert.ToInt32(await GetCompanyIdFromClaim());
                var response = await _service.Query().SelectAsync();

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
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _service.Query().SingleOrDefaultAsync(x => x.MeasurementUnitId == id, CancellationToken.None);
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
        [Route("GetByOrgId/{id}")]
        public async Task<IActionResult> GetByOrgId(int id)
        {
            try
            {
                var response = await _service.Query().Where(x => x.OrganizationId == id).SelectAsync(CancellationToken.None);
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
        // POST: api/Address
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MeasurementUnit p_measurementUnit)
        {
            try
            {
                //int CompanyId = Convert.ToInt32(await GetCompanyIdFromClaim());
               // int UserId = Convert.ToInt32(await GetUserIdFromClaim());
                _service.Insert(p_measurementUnit);
                await _unitOfWork.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // PUT: api/Address/
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]MeasurementUnit p_address)
        {
            try
            {
                _service.Update(p_address);
                await _unitOfWork.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/Address/
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(MeasurementUnit Address)
        {
            try
            {
                int CompanyId = Convert.ToInt32(await GetCompanyIdFromClaim());
                _service.Delete(Address);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetMeasurementUnitByModifyDate/{date}")]
        public async Task<IActionResult> GetMeasurementUnitByModifyDate(DateTime? date)
        {
            int companyId = Convert.ToInt32(await GetCompanyIdFromClaim());
            try
            {
                var measurementUnitsList = await _service.Query().SelectAsync();
                var measurementUnits = measurementUnitsList.Where(c => c.ModifiedTime > date).ToList();
                if (measurementUnits != null)
                {
                    return Ok(measurementUnits);
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