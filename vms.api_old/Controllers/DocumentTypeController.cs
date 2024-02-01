using System;
using System.Linq;
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
    [Route("api/DocumentType")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeService _service;

        public DocumentTypeController(IDocumentTypeService service, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._service = service;
            ClassName = "DocumentTypeController";
        }
        [HttpGet]
        [Route("GetDocumentTypeById/{id}")]
        public async Task<IActionResult> GetDocumentTypeById(int id)
        {
            try
            {
                var response = await _service.Query().Where(c => c.DocumentTypeId == id).SelectAsync(CancellationToken.None);
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
        [Route("GetDocumentTypeByOrgId/{id}")]
        public async Task<IActionResult> GetDocumentTypeByOrgId(int id)
        {
            try
            {
                var response = await _service.Query().Where(c => c.OrganizationId == id).SelectAsync(CancellationToken.None);
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
        [Route("GetDocumentTypeByModifyDate/{date}")]
        public async Task<IActionResult> GetDocumentTypeByModifyDate(DateTime? date)
        {
            int companyId = Convert.ToInt32(await GetCompanyIdFromClaim());
            try
            {
                var documentTypesList = await _service.Query().SelectAsync();
                var documentTypes = documentTypesList.Where(c => c.ModifiedTime > date).ToList();
                if (documentTypes != null)
                {
                    return Ok(documentTypes);
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
