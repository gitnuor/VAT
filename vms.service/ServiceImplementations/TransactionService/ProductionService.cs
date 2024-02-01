using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class ProductionService : ServiceBase<ProductionReceive>, IProductionService
{
    private readonly IMapper _mapper;
    private readonly IProductionRepository _repository;

    public ProductionService(IProductionRepository repository, IMapper mapper) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<string> InsertData(vmProductionReceive production)
    {
        return await _repository.InsertData(production);
    }

    // private async Task<List<FileSaveFeedbackDto>> SaveFiles(int organizationId, int produtionId, IList<FIleDocumentInfo> saveFileDataMap)
    // {
    //     var fileSaveDto = new FileSaveDto();
    //     fileSaveDto.FileRootPath = ControllerStaticData.FileRootPath;
    //     fileSaveDto.FileModulePath = ControllerStaticData.FileSaleModulePath;
    //     fileSaveDto.OrganizationId = organizationId;
    //     fileSaveDto.TransactionId = produtionId;
    //     fileSaveDto.FormFileList = saveFileDataMap;
    //     return await _fileOperationService.SaveFiles(fileSaveDto);
    // }


    // private void SaveDocumentsPath(List<FileSaveFeedbackDto> resultData, int organizationId, int createdBy, int productionId)
    // {
    //     foreach (var item in resultData)
    //     {
    //         var content = new Content()
    //         {
    //             DocumentTypeId = item.DocumentTypeId,
    //             OrganizationId = organizationId,
    //             FileUrl = item.FileUrl,
    //             MimeType = item.MimeType,
    //             Node = null,
    //             ObjectId = (int)EnumObjectType.ProductionReceive,
    //             ObjectPrimaryKey = productionId,
    //             IsActive = true,
    //             CreatedBy = createdBy,
    //             CreatedTime = DateTime.Now,
    //             Remarks = item.DocumentRemarks,
    //         };
    //         _contentService.Insert(content);
    //     }
    // }


    // private async Task<bool> SaveFileAndDocument(int organizationId, int createdBy, int produtionId, IList<FIleDocumentInfo> saveFileDataMap)
    // {
    //     var resultData = await SaveFiles(organizationId, produtionId, saveFileDataMap);
    //     SaveDocumentsPath(resultData, organizationId, createdBy, produtionId);
    //     return true;
    // }
    public async Task<string> InserSelfProductiontData(VmSelfProduction production, VmUserSession userSession)
    {
        try
        {
            var insertData = _mapper.Map<VmSelfProduction, vmProductionReceive>(production);
            insertData.CreatedBy = userSession.UserId;
            insertData.OrganizationId = userSession.OrganizationId;
            insertData.CreatedTime = DateTime.Now;
            insertData.IsContractual = false;
            return await _repository.InsertData(insertData);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<string> InsertContractualProductionData(VmContractualProduction production, VmUserSession userSession)
    {
        try
        {
            var insertData = _mapper.Map<VmContractualProduction, vmProductionReceive>(production);
            insertData.CreatedBy = userSession.UserId;
            insertData.OrganizationId = userSession.OrganizationId;
            insertData.CreatedTime = DateTime.Now;
            insertData.IsContractual = true;
            return await _repository.InsertData(insertData);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<IEnumerable<ProductionReceive>> GetProductions(int orgIdEnc)
    {
        return await _repository.GetProductions(orgIdEnc);
    }

    public async Task<IEnumerable<ViewProductionReceive>> ViewProductionReceive(string orgIdEnc)
    {
        return await _repository.ViewProductionReceive(orgIdEnc);
    }

    public async Task<IEnumerable<ViewProductionReceive>> ViewProductionReceiveByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.ViewProductionReceiveByOrgAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }
}