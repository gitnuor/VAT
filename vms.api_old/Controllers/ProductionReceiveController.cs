using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;
using vms.entity.models;
using vms.service.dbo;

namespace vms.api.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/productionreceive")]
    [ApiController]
    public class ProductionReceiveController : ControllerBase
    {
        private readonly IProductionService _service;
        public ProductionReceiveController(IProductionService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "ProductionReceiveController";
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _service.Query().SingleOrDefaultAsync(x => x.ProductionId == id, CancellationToken.None);
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
        public async Task<IActionResult> Post([FromBody]Utility.vmProductionReceive vmProduction)
        {
            var createdBy = 7;// Convert.ToInt32(await GetUserIdFromClaim());
            var organizationId = 5;// Convert.ToInt32(await GetCompanyIdFromClaim());
            string status = "";

            try
            {
                if (vmProduction.ContentInfoJson!=null)
                {
                    Content content;
                    foreach (var contentInfo in vmProduction.ContentInfoJson)
                    {
                        content = new Content();
                        vmProduction.ContentInfoJsonTest = new List<Content>();
                        var File = contentInfo.UploadFile;
                        var FileSaveFeedbackDto = await FileSaveAsync(File);
                        content.FileUrl = FileSaveFeedbackDto.FileUrl;
                        content.MimeType = FileSaveFeedbackDto.MimeType;
                        content.DocumentTypeId = contentInfo.DocumentTypeId;
                        vmProduction.ContentInfoJsonTest.Add(content);
                    }
                }
                if (vmProduction.ProductionReceiveDetailList.Count>0)
                {
                    vmProduction.CreatedBy = createdBy;
                    vmProduction.OrganizationId = organizationId;
                    entity.viewModels.vmProductionReceive productionReceive = new entity.viewModels.vmProductionReceive
                        {
                            BatchNo = vmProduction.BatchNo,
                            OrganizationId = vmProduction.OrganizationId,
                            OrgBranchId = vmProduction.OrgBranchId,
                            ProductionId = 1,
                            ProductId = vmProduction.ProductId,
                            ReceiveQuantity = vmProduction.ReceiveQuantity,
                            MeasurementUnitId = vmProduction.MeasurementUnitId,
                            ReceiveTime = vmProduction.ReceiveTime,
                            CreatedBy = createdBy,
                            CreatedTime = DateTime.Now,
                            ProductionReceiveDetailList = vmProduction.ProductionReceiveDetailList,
                            ContentInfoJson = vmProduction.ContentInfoJsonTest
                        };

                    status = await _service.InsertData(productionReceive);
                }

                if (status.Equals("Successful"))
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
    }
}