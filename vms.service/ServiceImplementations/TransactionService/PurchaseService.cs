using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.Dto.PurchaseLocal;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.ParamModel;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;
using vms.service.Services.UploadService;
using vms.utility.StaticData;

namespace vms.service.ServiceImplementations.TransactionService;

public class PurchaseService : ServiceBase<Purchase>, IPurchaseService
{
    private readonly IMapper _mapper;
    private readonly IPurchaseRepository _repository;
    private readonly IFileOperationService _fileOperationService;
    private readonly IContentService _contentService;

    public PurchaseService(IPurchaseRepository repository, IMapper mapper, IFileOperationService fileOperationService, IContentService contentService) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
        _fileOperationService = fileOperationService;
        _contentService = contentService;
    }


    private async Task<List<FileSaveFeedbackDto>> SaveFiles(int organizationId, int purchaseId, IList<FIleDocumentInfo> saveFileDataMap)
    {
        var fileSaveDto = new FileSaveDto
        {
            FileRootPath = ControllerStaticData.FileRootPath,
            FileModulePath = ControllerStaticData.FilePurchaseModulePath,
            OrganizationId = organizationId,
            TransactionId = purchaseId,
            FormFileList = saveFileDataMap
        };
        return await _fileOperationService.SaveFiles(fileSaveDto);
    }


    private void SaveDocumentsPath(List<FileSaveFeedbackDto> resultData, int organizationId, int createdBy, int purchaseId)
    {
        foreach (var item in resultData)
        {
            var content = new Content()
            {
                DocumentTypeId = item.DocumentTypeId,
                OrganizationId = organizationId,
                FileUrl = item.FileUrl,
                MimeType = item.MimeType,
                Node = null,
                ObjectId = (int)EnumObjectType.Purchase,
                ObjectPrimaryKey = purchaseId,
                IsActive = true,
                CreatedBy = createdBy,
                CreatedTime = DateTime.Now,
                Remarks = item.DocumentRemarks,
            };
            _contentService.Insert(content);
        }
    }


    private async Task<bool> SaveFileAndDocument(int organizationId, int createdBy, int purchaseId, IList<FIleDocumentInfo> saveFileDataMap)
    {
        var resultData = await SaveFiles(organizationId, purchaseId, saveFileDataMap);
        SaveDocumentsPath(resultData, organizationId, createdBy, purchaseId);
        return true;
    }

    public async Task<int> InsertLocalPurchase(VmPurchaseLocalPost purchase)
    {
        try
        {
            purchase.PurchaseTypeId = (int)EnumPurchaseType.PurchaseTypeLocal;
            var purchaseParam = _mapper.Map<VmPurchaseLocalPost, SpInsertPurchaseCombinedParam>(purchase);
            purchaseParam.IsComplete = true;
            var saveFileDataMap = _mapper.Map<IEnumerable<VmPurchaseLocalDocument>, IList<FIleDocumentInfo>>(purchase.Documents);
            purchaseParam.IsComplete = true;
            purchaseParam.ContentInfoList = null;
            var purchaseId = await _repository.InsertPurchase(purchaseParam);
            await SaveFileAndDocument(purchase.OrganizationId, purchase.CreatedBy, purchaseId, saveFileDataMap);
            return purchaseId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }



    public async Task<int> InsertImportPurchase(VmPurchaseImportPost purchase)
    {
        try
        {
            purchase.PurchaseTypeId = (int)EnumPurchaseType.PurchaseTypeImport;
            var purchaseParam = _mapper.Map<VmPurchaseImportPost, SpInsertPurchaseCombinedParam>(purchase);
            purchaseParam.IsComplete = true;
            // purchaseParam.PurchaseDate = DateTime.Now;
            var saveFileDataMap = _mapper.Map<IEnumerable<VmPurchaseImportDocument>, IList<FIleDocumentInfo>>(purchase.Documents);
            purchaseParam.IsComplete = true;
            purchaseParam.ContentInfoList = null;
            var purchaseId = await _repository.InsertPurchase(purchaseParam);
            await SaveFileAndDocument(purchase.OrganizationId, purchase.CreatedBy, purchaseId, saveFileDataMap);
            return purchaseId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

	public async Task<int> InsertImportPurchaseSubsription(VmPurchaseImportSubscriptionPost purchase)
	{
		try
		{
			purchase.PurchaseTypeId = (int)EnumPurchaseType.PurchaseTypeImportSubscription;
			var purchaseParam = _mapper.Map<VmPurchaseImportSubscriptionPost, SpInsertPurchaseCombinedParam>(purchase);
			purchaseParam.IsComplete = true;
			// purchaseParam.PurchaseDate = DateTime.Now;
			var saveFileDataMap = /*_mapper.Map<IEnumerable<VmPurchaseImportDocument>, IList<FIleDocumentInfo>>(purchase.Documents);*/
			purchaseParam.IsComplete = true;
			purchaseParam.ContentInfoList = null;
			var purchaseId = await _repository.InsertPurchase(purchaseParam);
			//await SaveFileAndDocument(purchase.OrganizationId, purchase.CreatedBy, purchaseId, saveFileDataMap);
			return purchaseId;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

	}

	public async Task<int> InsertLocalPurchaseDraft(VmPurchaseLocalPost purchase)
    {
        try
        {
            purchase.PurchaseTypeId = (int)EnumPurchaseType.PurchaseTypeLocal;
            var purchaseParam = _mapper.Map<VmPurchaseLocalPost, SpInsertPurchaseCombinedParam>(purchase);
            var saveFileDataMap = _mapper.Map<IEnumerable<VmPurchaseLocalDocument>, IList<FIleDocumentInfo>>(purchase.Documents);
            purchaseParam.IsComplete = false;

            purchaseParam.ContentInfoList = null;
            var purchaseId = await _repository.InsertPurchase(purchaseParam);
            await SaveFileAndDocument(purchase.OrganizationId, purchase.CreatedBy, purchaseId, saveFileDataMap);
            return purchaseId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<int> InsertImportPurchaseDraft(VmPurchaseImportPost purchase)
    {
        try
        {
            purchase.PurchaseTypeId = (int)EnumPurchaseType.PurchaseTypeImport;
            var purchaseParam = _mapper.Map<VmPurchaseImportPost, SpInsertPurchaseCombinedParam>(purchase);
            // purchaseParam.PurchaseDate = DateTime.Now;
            var saveFileDataMap = _mapper.Map<IEnumerable<VmPurchaseImportDocument>, IList<FIleDocumentInfo>>(purchase.Documents);
            purchaseParam.ContentInfoList = null;
            purchaseParam.IsComplete = false;
            var purchaseId = await _repository.InsertPurchase(purchaseParam);
            await SaveFileAndDocument(purchase.OrganizationId, purchase.CreatedBy, purchaseId, saveFileDataMap);
            return purchaseId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> InsertDebitNote(vmDebitNote vmDebitNote)
    {
        return await _repository.InsertDebitNote(vmDebitNote);
    }

    public async Task<bool> ManagePurchaseDue(vmPurchasePayment vmPurchase)
    {
        return await _repository.ManagePurchaseDue(vmPurchase);
    }

    public async Task<Purchase> GetPurchaseDetails(string pEncryptedId)
    {
        return await _repository.GetPurchaseDetails(pEncryptedId);

    }

    public async Task<IEnumerable<Purchase>> GetPurchaseDue(int orgId)
    {
        return await _repository.GetPurchaseDue(orgId);
    }

    public async Task<bool> InsertTransferReceive(vmTransferReceive vm)
    {
        return await _repository.InsertTransferReceive(vm);
    }

    public async Task<IEnumerable<Purchase>> GetPurchases(int pOrgId)
    {
        return await _repository.GetPurchases(pOrgId);
    }

    public Task<IEnumerable<Purchase>> GetPurchaseByOrganization(string orgIdEnc)
    {
        return _repository.GetPurchaseByOrganization(orgIdEnc);
    }

    public Task<IEnumerable<ViewPurchase>> GetPurchaseListByOrganization(string orgIdEnc)
    {
        return _repository.GetPurchaseListByOrganization(orgIdEnc);
    }

    public Task<IEnumerable<ViewPurchase>> GetPurchaseListByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return _repository.GetPurchaseListByOrganizationAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }
    public Task<IEnumerable<ViewVdsPurchase>> GetVdsPurchaseListByOrganization(string orgIdEnc)
    {
        return _repository.GetVdsPurchaseListByOrganization(orgIdEnc);
    }
    public Task<IEnumerable<ViewVdsPurchase>> GetVdsPurchaseListByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return _repository.GetVdsPurchaseListByOrganizationAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }

    public Task<IEnumerable<ViewPurchaseLocal>> GetPurchaseLocalListByOrganization(string orgIdEnc)
    {
        return _repository.GetPurchaseLocalListByOrganization(orgIdEnc);
    }

    public Task<IEnumerable<ViewPurchaseLocal>> GetPurchaseLocalListByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return _repository.GetPurchaseLocalListByOrganizationAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }

    public Task<IEnumerable<ViewPurchaseImport>> GetPurchaseImportListByOrganization(string orgIdEnc)
    {
        return _repository.GetPurchaseImportListByOrganization(orgIdEnc);
    }


    public async Task<IEnumerable<Purchase>> GetVdsPurchases(int pOrgId)
    {
        return await _repository.GetVdsPurchases(pOrgId);
    }

    public Task<IEnumerable<Purchase>> GetVdsPurchasesWithVdsPayment(string orgIdEnc)
    {
        return _repository.GetVdsPurchasesWithVdsPayment(orgIdEnc);
    }

    public Task<IEnumerable<Purchase>> GetVdsPurchasesWithVdsPaymentAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return _repository.GetVdsPurchasesWithVdsPaymentAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }

    public async Task<IEnumerable<Purchase>> GetVdsPurchasesWithDueTdsPayment(int pOrgId)
    {
        return await _repository.GetTdsPurchasesWithDueTdsPayment(pOrgId);
    }

    public Task<IEnumerable<Purchase>> GetVdsPurchasesWithTdsPayment(string orgIdEnc)
    {
        return _repository.GetTdsPurchasesWithTdsPayment(orgIdEnc);
    }

    public async Task<IEnumerable<Purchase>> GetPurchasesIncludingOtherTables(int pOrgId)
    {
        return await _repository.Query().Where(c => c.OrganizationId == pOrgId).Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.DebitNotes).Include(c => c.Vendor).OrderByDescending(c => c.PurchaseId)
            .SelectAsync();
    }

    public async Task<List<SpMonthlyPurchaseReport>> MonthlyPurchaseReport(int PurReason, int organizationId, int vendorId, string invoiceNo, DateTime? fromDate, DateTime? toDate, int userId)
    {
        return await _repository.MonthlyPurchaseReport(userId, PurReason,  organizationId, vendorId, invoiceNo, fromDate, toDate);
    }

    public async Task<List<SpGetPurchaseReportByProduct>> GetPurchaseReportByProduct(int purReason, int organizationId, int productId, string invoiceNo, DateTime? fromDate,
        DateTime? toDate, int userId)
    {
        return await _repository.GetPurchaseReportByProduct(purReason, organizationId, productId, invoiceNo, fromDate, toDate, userId);
    }

    public async Task<List<ViewPurchaseLocal>> GetPurchaseVdsList(int organizationId, DateTime? fromDate, DateTime? toDate)
    {
        return await _repository.GetPurchaseVdsList(organizationId, fromDate, toDate);
    }

    public async Task<int> InsertApiLocalPurchase(PurchaseLocalPostDto purchase, string apiData = null, string token = null)
    {
        try
        {
            var purchaseParam = _mapper.Map<PurchaseLocalPostDto, SpInsertPurchaseFromApiCombinedParam>(purchase);
            purchaseParam.PurchaseTypeId = (int)EnumPurchaseType.PurchaseTypeLocal;
            purchaseParam.IsComplete = true;
            purchaseParam.IsComplete = true;
            purchaseParam.ContentInfoList = null;
            purchaseParam.Token = token;
            var purchaseId = await _repository.InsertApiPurchase(purchaseParam);
            return purchaseId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<ViewPurchaseLocal>> GetPurchaseVdsListByOrgAndBranch(int organizationId, DateTime? fromDate, DateTime? toDate, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.GetPurchaseVdsListByOrgAndBranch(organizationId, fromDate, toDate, branchIds, isRequiredBranch);
    }

    public async Task<List<SpGetProductPurchase>> ProductPurchaseListReport(int organizationId, int branchId, DateTime? fromDate,
        DateTime? toDate)
    {
        return await _repository.ProductPurchaseListReport(organizationId, branchId, fromDate, toDate);
    }

    public async Task<bool> ProcessUploadedSimplifiedPurchase(long fileUploadId, int organizationId)
    {
        return await _repository.ProcessUploadedSimplifiedPurchase(fileUploadId, organizationId);
    }

    public async Task<bool> ProcessUploadedSimplifiedLocalPurchase(long fileUploadId, int organizationId)
    {
        return await _repository.ProcessUploadedSimplifiedLocalPurchase(fileUploadId, organizationId);
    }

    public async Task<IEnumerable<Purchase>> GetVdsPurchasesWithDueVdsPayment(int pOrgId)
    {
        return await _repository.GetVdsPurchasesWithDueVdsPayment(pOrgId);
    }
}