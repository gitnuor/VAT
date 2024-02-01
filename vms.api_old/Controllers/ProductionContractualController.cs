using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;
using vms.entity.models;
using vms.entity.viewModels;
using vms.service.dbo;

namespace vms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionContractualController : vms.api.Controllers.ControllerBase
    {
        private readonly IContractVendorService _contractVendorService;
        private readonly IContractVendorProductDetailsService _productDetails;
        private readonly IContractVendorTransferRawMaterialService _rawMaterialService;
        private readonly IContractVendorTransferRawMaterialDetailsService _rawMaterialDetailsService;
        public ProductionContractualController(IContractVendorTransferRawMaterialDetailsService rawMaterialDetailsService, IContractVendorTransferRawMaterialService rawMaterialService, IContractVendorProductDetailsService productDetails, IContractVendorService contractVendorService, IHostingEnvironment environment, IUnitOfWork unityOfWork) : base(environment, unityOfWork)
        {
            this._contractVendorService = contractVendorService;
            this._productDetails = productDetails;
            this._rawMaterialService = rawMaterialService;
            this._rawMaterialDetailsService = rawMaterialDetailsService;

            ClassName = "ProductionContractualController";
        }

        public IActionResult Index()
        {
            return View();
        }
        // GET: api/Address
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //int clientId = Convert.ToInt32(await GetCompanyIdFromClaim());
                var response = await _contractVendorService.Query().SelectAsync();

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
        public async Task<IActionResult> Post([FromBody]vmContractVendor contract)
        {
            try
            {
                var createdBy = 8;
                var organizationId = 6;
                bool status = false;
                ContractualProduction contractVendor = new ContractualProduction();
                contractVendor.OrganizationId = organizationId;
                contractVendor.CreatedBy = createdBy;
                contractVendor.ContractNo = contract.ContractNo;
                contractVendor.VendorId = contract.VendorId;
                contractVendor.ContractDate = contract.ContractDate;
                contractVendor.ClosingDate = contract.ClosingDate;
                contractVendor.CreatedTime = DateTime.Now;
                contractVendor.ChallanNo = contract.ChallanNo;
                contractVendor.IssueDate = contract.IssueDate;
                contractVendor.CustomerId = contract.CustomerId;
                contractVendor.ContractTypeId = contract.ContractTypeId;
                _contractVendorService.Insert(contractVendor);
                await _unitOfWork.SaveChangesAsync();
                if (contractVendor.ContractualProductionId > 0)
                {
                    foreach (var item in contract.ContractVendorProductDetailses)
                    {
                        ContractualProductionProductDetail detail = new ContractualProductionProductDetail();
                        detail.ContractualProductionId = contractVendor.ContractualProductionId;
                        detail.ProductId = item.ProductId;
                        detail.Quantity = item.Quantity;
                        detail.MeasurementUnitId = item.MeasurementUnitId;
                        _productDetails.Insert(detail);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    status = true;
                }
                else
                {
                    status = false;
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
          
        }
        [HttpPost]
        public async Task<IActionResult> TransferRawMaterial([FromBody]vmTransferRawMaterial vm)
        {
            var createdBy = 8;
            var organizationId = 6;
            
            try
            {
                bool status = false;
                ContractualProductionTransferRawMaterial model = new ContractualProductionTransferRawMaterial();
                model.ContractualProductionId = vm.ContractualProductionId;
                model.TransfereDate = vm.TransfereDate;
                model.Location = vm.Location;
                model.ChallanNo = vm.ChallanNo;
                model.ChallanIssueDate = vm.ChallanIssueDate;
                model.CreatedBy = createdBy;
                model.CreatedDate = DateTime.Now;
                _rawMaterialService.Insert(model);
                await _unitOfWork.SaveChangesAsync();
                if (model.ContractualProductionTransferRawMaterialId > 0)
                {
                    foreach (var item in vm.Details)
                    {
                        ContractualProductionTransferRawMaterialDetail detail = new ContractualProductionTransferRawMaterialDetail();
                        detail.ContractualProductionTransferRawMaterialId = model.ContractualProductionTransferRawMaterialId;
                        detail.RawMaterialId = item.RawMaterialId;
                        detail.Quantity = item.Quantity;
                        detail.MeasurementUnitId = item.MeasurementUnitId;
                        _rawMaterialDetailsService.Insert(detail);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    status = true;
                }
                else
                {
                    status = false;
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
    }
}
