using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using vms.entity.Dto.Sales;
using vms.entity.Dto.SalesCreate;
using vms.entity.Dto.SalesExport;
using vms.entity.Dto.SalesLocal;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.entity.viewModels.ReportsViewModel;
using vms.entity.viewModels.VmSalesCombineParamsModels;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;
using vms.service.Services.UploadService;
using vms.utility.StaticData;

namespace vms.service.ServiceImplementations.TransactionService;

public class SaleService : ServiceBase<Sale>, ISaleService
{
    private readonly ISaleRepository _repository;
    private readonly ISalesDetailRepository _detailRepository;
    private readonly DbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileOperationService _fileOperationService;
    private readonly IContentService _contentService;

    public SaleService(ISaleRepository repository, IMapper mapper, IFileOperationService fileOperationService,
        IContentService contentService, ISalesDetailRepository detailRepository) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
        _fileOperationService = fileOperationService;
        _contentService = contentService;
        _detailRepository = detailRepository;
    }

    public async Task<bool> InsertCreditNote(vmCreditNote vmCreditNote)
    {
        return await _repository.InsertCreditNote(vmCreditNote);
    }

    public async Task<bool> InsertCreditNote(VmSalesCreditNotePost vmCreditNote)
    {
        var creditNoteData = _mapper.Map<VmSalesCreditNotePost, vmCreditNote>(vmCreditNote);
        return await _repository.InsertCreditNote(creditNoteData);
    }

    public async Task<bool> InsertData(vmSaleOrder saleOrder)
    {
        return await _repository.InsertData(saleOrder);
    }

    public async Task<IList<Sale>> GetSalesDetailsAsync(vmSalesDetails salesDetails)
    {
        return await _repository.Queryable().Where(s =>
                (string.IsNullOrEmpty(salesDetails.InvoiceNo) || s.InvoiceNo.Contains(salesDetails.InvoiceNo)) &&
                (string.IsNullOrEmpty(salesDetails.CustomerName) ||
                 s.Customer.Name.Contains(salesDetails.CustomerName)) &&
                (salesDetails.FromDate == null || s.SalesDate >= salesDetails.FromDate) &&
                (salesDetails.ToDate == null || s.SalesDate <= salesDetails.ToDate))
            .Include(s => s.Customer)
            .ToListAsync(CancellationToken.None);
    }

    public async Task<IEnumerable<Sale>> GetSalesDetails(int orgId)
    {
        return await _repository.GetSalesDetails(orgId);
    }

    public async Task<IEnumerable<Sale>> GetSalesByOrganization(string orgIdEnc)
    {
        return await _repository.GetSalesByOrganization(orgIdEnc);
    }

    public async Task<IEnumerable<Sale>> GetSalesByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.GetSalesByOrganizationAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }

    public Task<IEnumerable<ViewSale>> GetSalesListByOrganization(string orgIdEnc)
    {
        return _repository.GetSalesListByOrganization(orgIdEnc);
    }

    public Task<IEnumerable<ViewSale>> GetSalesListByOrganizationByBranch(string orgIdEnc, List<int> branchIds, bool isBranchRequired)
    {
        return _repository.GetSalesListByOrganizationByBranch(orgIdEnc, branchIds, isBranchRequired);
    }

    public Task<IEnumerable<ViewSalesLocal>> GetSalesLocalListByOrganization(string orgIdEnc)
    {
        return _repository.GetSalesLocalListByOrganization(orgIdEnc);
    }

    public Task<IEnumerable<ViewSalesExport>> GetSalesExportListByOrganization(string orgIdEnc)
    {
        return _repository.GetSalesExportListByOrganization(orgIdEnc);
    }

    public async Task<IEnumerable<Sale>> GetSalesDueByOrganization(string orgIdEnc)
    {
        return await _repository.GetSalesDueByOrganization(orgIdEnc);
    }

    public async Task<IEnumerable<Sale>> GetSalesDueByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.GetSalesDueByOrganizationAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }

    public async Task<IEnumerable<ViewVdsSale>> GetSalesViewByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.GetSalesViewByOrgAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }

    public async Task<Sale> GetSaleData(string idEnc)
    {
        return await _repository.GetSaleData(idEnc);
    }

    public async Task<IEnumerable<Sale>> GetSalesDue(int orgId)
    {
        return await _repository.GetSalesDue(orgId);
    }


    private async Task<List<FileSaveFeedbackDto>> SaveFiles(int OrganizationId, int saleId,
        IList<FIleDocumentInfo> saveFileDataMap)
    {
        var fileSaveDto = new FileSaveDto();
        fileSaveDto.FileRootPath = ControllerStaticData.FileRootPath;
        fileSaveDto.FileModulePath = ControllerStaticData.FileSaleModulePath;
        fileSaveDto.OrganizationId = OrganizationId;
        fileSaveDto.TransactionId = saleId;
        fileSaveDto.FormFileList = saveFileDataMap;
        return await _fileOperationService.SaveFiles(fileSaveDto);
    }

    private async Task<List<FileSaveFeedbackDto>> SaveSaleBreakdownFiles(int OrganizationId, int saleId,
        IList<FIleDocumentInfo> saveFileDataMap)
    {
        var fileSaveDto = new FileSaveDto();
        fileSaveDto.FileRootPath = ControllerStaticData.FileRootPath;
        fileSaveDto.FileModulePath = ControllerStaticData.FileSalesBreakdownModulePath;
        fileSaveDto.OrganizationId = OrganizationId;
        fileSaveDto.TransactionId = saleId;
        fileSaveDto.FormFileList = saveFileDataMap;
        return await _fileOperationService.SaveFiles(fileSaveDto);
    }


    private void SaveDocumentsPath(List<FileSaveFeedbackDto> resultData, int OrganizationId, int CreatedBy, int saleId)
    {
        foreach (var item in resultData)
        {
            var content = new Content()
            {
                DocumentTypeId = item.DocumentTypeId,
                OrganizationId = OrganizationId,
                FileUrl = item.FileUrl,
                MimeType = item.MimeType,
                Node = null,
                ObjectId = (int)EnumObjectType.Sale,
                ObjectPrimaryKey = saleId,
                IsActive = true,
                CreatedBy = CreatedBy,
                CreatedTime = DateTime.Now,
                Remarks = item.DocumentRemarks,
            };
            _contentService.Insert(content);
        }
    }

    private void SaveSaleBreakdownDocumentsPath(List<FileSaveFeedbackDto> resultData, int OrganizationId, int CreatedBy,
        int saleId)
    {
        foreach (var item in resultData)
        {
            var content = new Content()
            {
                DocumentTypeId = item.DocumentTypeId,
                OrganizationId = OrganizationId,
                FileUrl = item.FileUrl,
                MimeType = item.MimeType,
                Node = null,
                ObjectId = (int)EnumObjectType.SalesDetailBreakdown,
                ObjectPrimaryKey = saleId,
                IsActive = true,
                CreatedBy = CreatedBy,
                CreatedTime = DateTime.Now,
                Remarks = item.DocumentRemarks,
            };
            _contentService.Insert(content);
        }
    }


    private async Task<bool> SaveFileAndDocument(int OrganizationId, int CreatedBy, int saleId,
        IList<FIleDocumentInfo> saveFileDataMap)
    {
        var resultData = await SaveFiles(OrganizationId, saleId, saveFileDataMap);
        SaveDocumentsPath(resultData, OrganizationId, CreatedBy, saleId);
        return true;
    }

    private async Task<bool> SaveSalesDetailBreakdownFileAndDocument(int OrganizationId, int CreatedBy, int saleId,
        IList<FIleDocumentInfo> saveFileDataMap)
    {
        var resultData = await SaveSaleBreakdownFiles(OrganizationId, saleId, saveFileDataMap);
        SaveSaleBreakdownDocumentsPath(resultData, OrganizationId, CreatedBy, saleId);
        return true;
    }

    #region SaleLocalWithBreakdownForSpecialClient
    public async Task<int> InsertLocalSaleWithBreakdown(VmSaleLocalPostWithBreakdown vmSaleLocalPostWithBreakdown)
    {
        var saleLocal = _mapper.Map<VmSaleLocalPostWithBreakdown, VmSalesCombineParamsModel>(vmSaleLocalPostWithBreakdown);
        var saveFileDataMap =
            _mapper.Map<IEnumerable<VmSaleLocalDocument>, IList<FIleDocumentInfo>>(vmSaleLocalPostWithBreakdown.Documents);

        var saveFileSaleBreakdownDataMap = new List<FIleDocumentInfo>()
             {
                 new FIleDocumentInfo
                 {
                     DocumentRemarks = "Sales Breakdown",
                     FormFIle = vmSaleLocalPostWithBreakdown.FileSalesBreakDown
                 }
             };
        saleLocal.IsComplete = true;
        saleLocal.ContentInfoJson = null;
        var saleId = await _repository.InsertSale(saleLocal);

        await SaveFileAndDocument(vmSaleLocalPostWithBreakdown.OrganizationId, vmSaleLocalPostWithBreakdown.CreatedBy, saleId, saveFileDataMap);
        await SaveSalesDetailBreakdownFileAndDocument(vmSaleLocalPostWithBreakdown.OrganizationId, vmSaleLocalPostWithBreakdown.CreatedBy, saleId, saveFileSaleBreakdownDataMap);
        return saleId;
    }

    #endregion

    public async Task<int> InsertLocalSale(VmSaleLocalPost vmSaleLocalPost)
    {
        var saleLocal = _mapper.Map<VmSaleLocalPost, VmSalesCombineParamsModel>(vmSaleLocalPost);
        var saveFileDataMap =
            _mapper.Map<IEnumerable<VmSaleLocalDocument>, IList<FIleDocumentInfo>>(vmSaleLocalPost.Documents);

        // var saveFileSaleBreakdownDataMap = new List<FIleDocumentInfo>()
        // 	{
        // 		new FIleDocumentInfo
        // 		{
        // 			DocumentRemarks = "Sales Breakdown",
        // 			FormFIle = vmSaleLocalPost.FileSalesBreakDown
        // 		}
        // 	};
        saleLocal.IsComplete = true;
        saleLocal.ContentInfoJson = null;
        var saleId = await _repository.InsertSale(saleLocal);

        await SaveFileAndDocument(vmSaleLocalPost.OrganizationId, vmSaleLocalPost.CreatedBy, saleId, saveFileDataMap);
        // await SaveSalesDetailBreakdownFileAndDocument(vmSaleLocalPost.OrganizationId, vmSaleLocalPost.CreatedBy, saleId, saveFileSaleBreakdownDataMap);
        return saleId;
    }

    public async Task<int> InsertLocalSale(SalesLocalPostDto sales, string apiData = null, string token = null)
	{
        var saleLocal = _mapper.Map<SalesLocalPostDto, SalesCombinedInsertParamDto>(sales);
        saleLocal.IsComplete = true;
        saleLocal.OrganizationId = 7;
        saleLocal.CreatedTime = DateTime.Now;
        saleLocal.SalesTypeId = (int)EnumSalesType.SalesTypeLocal;
        saleLocal.Token = token;
        var saleId = await _repository.InsertSaleFromApi(saleLocal, apiData);
        return saleId;
    }

    public async Task<int> InsertApiSale(SalesCombinedPostDto sales, string apiData = null, string token = null)
    {
        var saleLocal = _mapper.Map<SalesCombinedPostDto, SalesCombinedInsertParamDto>(sales);
        saleLocal.IsComplete = true;
        saleLocal.OrganizationId = 7;
        saleLocal.CreatedTime = DateTime.Now;
        saleLocal.Token = token;
        var saleId = await _repository.InsertSaleFromApi(saleLocal, apiData);
        return saleId;
    }

    public async Task<int> InsertLocalSaleExport(VmSaleExportPost vmSaleExportPost)
    {
        try
        {
            var saleLocalExport = _mapper.Map<VmSaleExportPost, VmSalesCombineParamsModel>(vmSaleExportPost);
            var saveFileDataMap =
                _mapper.Map<IEnumerable<VmSaleExportDocument>, IList<FIleDocumentInfo>>(vmSaleExportPost
                    .VmSaleLocalDocuments);
            saleLocalExport.IsComplete = true;
            saleLocalExport.ContentInfoJson = null;
            int saleId = await _repository.InsertSale(saleLocalExport);
            await SaveFileAndDocument(vmSaleExportPost.OrganizationId, vmSaleExportPost.CreatedBy, saleId,
                saveFileDataMap);
            return saleId;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<int> InsertLocalSaleDraft(VmSaleLocalPost vmSaleLocalPost)
    {
        var saleLocal = _mapper.Map<VmSaleLocalPost, VmSalesCombineParamsModel>(vmSaleLocalPost);
        saleLocal.IsComplete = false;
        var saveFileDataMap =
            _mapper.Map<IEnumerable<VmSaleLocalDocument>, IList<FIleDocumentInfo>>(vmSaleLocalPost.Documents);
        saleLocal.ContentInfoJson = null;
        int saleId = await _repository.InsertSale(saleLocal);
        await SaveFileAndDocument(vmSaleLocalPost.OrganizationId, vmSaleLocalPost.CreatedBy, saleId, saveFileDataMap);
        return saleId;
    }

    public async Task<int> InsertLocalSaleExportDraft(VmSaleExportPost vmSaleExportPost)
    {
        try
        {
            var saleLocalExport = _mapper.Map<VmSaleExportPost, VmSalesCombineParamsModel>(vmSaleExportPost);
            var saveFileDataMap =
                _mapper.Map<IEnumerable<VmSaleExportDocument>, IList<FIleDocumentInfo>>(vmSaleExportPost
                    .VmSaleLocalDocuments);
            saleLocalExport.IsComplete = false;
            saleLocalExport.ContentInfoJson = null;
            int saleId = await _repository.InsertSale(saleLocalExport);
            await SaveFileAndDocument(vmSaleExportPost.OrganizationId, vmSaleExportPost.CreatedBy, saleId,
                saveFileDataMap);
            return saleId;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public Task<bool> ProcessUploadedSimplifiedSale(long fileUploadId, int organizationId)
    {
        return _repository.ProcessUploadedSimplifiedSale(fileUploadId, organizationId);
    }

    public Task<bool> ProcessUploadedSimplifiedLocalSaleCalculatedByVat(long fileUploadId, int organizationId)
	{
		return _repository.ProcessUploadedSimplifiedLocalSaleCalculatedByVat(fileUploadId, organizationId);
	}

    public async Task<Sale> Approve(SaleApproveOrRejectViewModel model)
    {
        var sale = await _repository.FindAsync(model.SalesId);
        sale.IsApproved = true;
        sale.ApproveMessage = model.Remarks;
        sale.ApprovedBy = model.UserId;
        _repository.Update(sale);
        return sale;
    }

    public async Task<Sale> Reject(SaleApproveOrRejectViewModel model)
    {
        var sale = await _repository.FindAsync(model.SalesId);
        sale.IsRejected = true;
        sale.RejectMessage = model.Remarks;
        sale.RejectedBy = model.UserId;
        _repository.Update(sale);
        return sale;
    }

    public Task<ViewSale> GetViewSale(string idEnc)
    {
        return _repository.GetViewSale(idEnc);
    }

    public Task<ViewSalesLocal> GetViewSaleLocal(string idEnc)
    {
        return _repository.GetViewSaleLocal(idEnc);
    }

    public Task<ViewSalesExport> GetViewSaleExport(string idEnc)
    {
        return _repository.GetViewSaleExport(idEnc);
    }

    public async Task<IEnumerable<SaleDto>> GetSalesDtoListByOrganization(string orgIdEnc)
    {
        var sales = await _repository.GetSalesListByOrganization(orgIdEnc);
        return _mapper.Map<IEnumerable<ViewSale>, IEnumerable<SaleDto>>(sales);
    }

    public async Task<SaleWithDetailDto> GetSalesDto(string idEnc)
    {
        var sale = await _repository.GetViewSale(idEnc);
        if (sale == null)
            return null;
        var saleDto = _mapper.Map<ViewSale, SaleWithDetailDto>(sale);
        var salesDetails = await _detailRepository.GetSalesDetailsBySales(idEnc);
        saleDto.Details = _mapper.Map<IEnumerable<ViewSalesDetail>, IEnumerable<SalesDetailDto>>(salesDetails);
        return saleDto;
    }

    public async Task<IEnumerable<SalesLocalDto>> GetSalesLocalDtoListByOrganization(string orgIdEnc)
    {
        var sales = await _repository.GetSalesLocalListByOrganization(orgIdEnc);
        return _mapper.Map<IEnumerable<ViewSalesLocal>, IEnumerable<SalesLocalDto>>(sales);
    }

    public async Task<SalesLocalWithDetailDto> GetSalesLocalDto(string idEnc)
    {
        var sale = await _repository.GetViewSaleLocal(idEnc);
        if (sale == null)
            return null;
        var saleDto = _mapper.Map<ViewSalesLocal, SalesLocalWithDetailDto>(sale);
        var salesDetails = await _detailRepository.GetSalesDetailsBySales(idEnc);
        saleDto.Details = _mapper.Map<IEnumerable<ViewSalesDetail>, IEnumerable<SalesLocalDetailDto>>(salesDetails);
        return saleDto;
    }

    public async Task<IEnumerable<SalesExportDto>> GetSalesExportDtoListByOrganization(string orgIdEnc)
    {
        var sales = await _repository.GetSalesExportListByOrganization(orgIdEnc);
        return _mapper.Map<IEnumerable<ViewSalesExport>, IEnumerable<SalesExportDto>>(sales);
    }

    public async Task<SalesExportWithDetailDto> GetSalesExportDto(string idEnc)
    {
        var sale = await _repository.GetViewSaleExport(idEnc);
        if (sale == null)
            return null;
        var saleDto = _mapper.Map<ViewSalesExport, SalesExportWithDetailDto>(sale);
        var salesDetails = await _detailRepository.GetSalesDetailsBySales(idEnc);
        saleDto.Details = _mapper.Map<IEnumerable<ViewSalesDetail>, IEnumerable<SalesExportDetailDto>>(salesDetails);
        return saleDto;
    }

    public Task<IEnumerable<ViewSalesPaymentAgingReport>> GetSalesAgingReport(string orgIdEnc)
    {
        return _repository.GetSalesAgingReport(orgIdEnc);
    }

    public async Task<List<SpGetProductSale>> ProductSalesListReport(int organizationId, int branchId, DateTime? fromDate,
        DateTime? toDate, int userId)
    {
        return await _repository.ProductSalesListReport(organizationId, branchId, fromDate, toDate, userId);
    }
}