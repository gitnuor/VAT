using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.entity.viewModels.ProductViewModel;
using vms.repository.Repository.sp;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class DataImportService : IDataImportService
{
    private readonly IProductRepository _productrepository;
    private readonly IApiSpInsertRepository _apibulk;

    public DataImportService(IProductRepository productRepository, IApiSpInsertRepository apibulk
    )
    {
        _productrepository = productRepository;
        _apibulk = apibulk;
    }

    public async Task<List<ProductDataImportViewModel>> LoadProduct(string location)
    {
        //string sWebRootFolder = location;
        //string sFileName = System.IO.Path.GetFileName(sWebRootFolder);
        //var output = new List<ProductViewModel>();

        //System.IO.FileInfo file = new System.IO.FileInfo(location);
        //try
        //{
        //    using (ExcelPackage package = new ExcelPackage(file))
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
        //        if (worksheet.Dimension != null)
        //        {
        //            int rowCount = worksheet.Dimension.Rows;
        //            int ColCount = worksheet.Dimension.Columns;
        //            for (int row = 2; row <= rowCount; row++)
        //            {
        //                var item = new ProductViewModel();

        //                if (CheckCellValueContainsData(worksheet.Cells[row, 1]) == true)
        //                    item.ProductId = Convert.ToInt32(worksheet.Cells[row, 1].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 2]) == true)
        //                    item.Name = worksheet.Cells[row, 2].Value.ToString();
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 3]) == true)
        //                    item.ModelNo = worksheet.Cells[row, 3].Value.ToString();
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 4]) == true)
        //                    item.Code = worksheet.Cells[row, 4].Value.ToString();
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 5]) == true)
        //                    item.Hscode = worksheet.Cells[row, 5].Value.ToString();
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 6]) == true)
        //                    item.ProductCategoryId = Convert.ToInt32(worksheet.Cells[row, 6].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 7]) == true)
        //                    item.ProductCategoryName = worksheet.Cells[row, 7].Value.ToString();
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 8]) == true)
        //                    item.ProductGroupId = Convert.ToInt32(worksheet.Cells[row, 8].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 9]) == true)
        //                    item.ProductGroupsName = worksheet.Cells[row, 9].Value.ToString();
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 10]) == true)
        //                    item.OrganizationId = Convert.ToInt32(worksheet.Cells[row, 10].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 11]) == true)
        //                    item.TotalQuantity = Convert.ToDecimal(worksheet.Cells[row, 11].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 12]) == true)
        //                    item.MeasurementUnitId = Convert.ToInt32(worksheet.Cells[row, 12].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 13]) == true)
        //                    item.MeasurementUnitName = worksheet.Cells[row, 13].Value.ToString();
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 14]) == true)
        //                    item.EffectiveFrom = Convert.ToDateTime(worksheet.Cells[row, 14].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 15]) == true)
        //                    item.EffectiveTo = Convert.ToDateTime(worksheet.Cells[row, 15].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 16]) == true)
        //                    item.IsActive = Convert.ToBoolean(worksheet.Cells[row, 16].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 17]) == true)
        //                    item.CreatedBy = Convert.ToInt32(worksheet.Cells[row, 17].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 18]) == true)
        //                    item.CreatedTime = Convert.ToDateTime(worksheet.Cells[row, 18].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 19]) == true)
        //                    item.IsSellable = Convert.ToBoolean(worksheet.Cells[row, 19].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 20]) == true)
        //                    item.IsRawMaterial = Convert.ToBoolean(worksheet.Cells[row, 20].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 21]) == true)
        //                    item.IsNonRebateable = Convert.ToBoolean(worksheet.Cells[row, 21].Value);
        //                if (CheckCellValueContainsData(worksheet.Cells[row, 22]) == true)
        //                    item.ReferenceKey = worksheet.Cells[row, 22].Value.ToString();
        //                output.Add(item);
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Data Not Found");
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}

        //return await Task.FromResult(output);
        return null;
    }

    public async Task<List<ProductionDataImportViewModel>> LoadProduction(string location)
    {
        //string sWebRootFolder = location;
        //string sFileName = System.IO.Path.GetFileName(sWebRootFolder);
        //var output = new List<ProductionDataImportViewModel>();

        //System.IO.FileInfo file = new System.IO.FileInfo(location);
        //try
        //{
        //    using (ExcelPackage package = new ExcelPackage(file))
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
        //        int rowCount = worksheet.Dimension.Rows;
        //        int ColCount = worksheet.Dimension.Columns;
        //        for (int row = 2; row <= rowCount; row++)
        //        {
        //            var item = new ProductionDataImportViewModel();

        //            if (CheckCellValueContainsData(worksheet.Cells[row, 1]) == true)
        //                item.ProductionReciveId = worksheet.Cells[row, 1].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 2]) == true)
        //                item.BatchNo = worksheet.Cells[row, 2].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 3]) == true)
        //                item.ProductId = worksheet.Cells[row, 3].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 4]) == true)
        //                item.ReceiveQuantity = Convert.ToDecimal(worksheet.Cells[row, 4].Value);
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 5]) == true)
        //                item.MeasurementUnitId = worksheet.Cells[row, 5].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 6]) == true)
        //                item.ReceiveTime = Convert.ToDateTime(worksheet.Cells[row, 6].Value);
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 7]) == true)
        //                item.CreatedBy = worksheet.Cells[row, 7].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 8]) == true)
        //                item.CreatedTime = Convert.ToDateTime(worksheet.Cells[row, 8].Value);
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 9]) == true)
        //                item.IsContractual = Convert.ToBoolean(worksheet.Cells[row, 9].Value);
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 10]) == true)
        //                item.ContractualProductionId = worksheet.Cells[row, 10].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet.Cells[row, 11]) == true)
        //                item.ContractualProductionChallanNo = worksheet.Cells[row, 11].Value.ToString();

        //            output.Add(item);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}

        //return await Task.FromResult(output);
        return null;
    }

    public async Task<DataimportPurchaseFinal> LoadPurchase(string location)
    {
        //string sWebRootFolder = location;
        //string sFileName = System.IO.Path.GetFileName(sWebRootFolder);
        //var output = new DataimportPurchaseFinal();

        //System.IO.FileInfo file = new System.IO.FileInfo(location);
        //try
        //{

        //    using (ExcelPackage package = new ExcelPackage(file))
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
        //        ExcelWorksheet worksheet2 = package.Workbook.Worksheets[2];
        //        ExcelWorksheet worksheet3 = package.Workbook.Worksheets[3];

        //        int rowCount = worksheet3.Dimension.Rows;
        //        int ColCount = worksheet3.Dimension.Columns;
        //        for (int row = 2; row <= rowCount; row++)
        //        {
        //            var item = new DatauploadPayment();

        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 1]) == true)
        //                item.PaymentId = worksheet3.Cells[row, 1].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 2]) == true)
        //                item.PaymentMethodId = worksheet3.Cells[row, 2].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 3]) == true)
        //                item.PaymentAmount = Convert.ToDecimal(worksheet3.Cells[row, 3].Value);
        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 4]) == true)
        //                item.PaymentDate = Convert.ToDateTime(worksheet3.Cells[row, 4].Value);
        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 5]) == true)
        //                item.PurchaseId = worksheet3.Cells[row, 5].Value.ToString();

        //            output.Payments.Add(item);
        //        }

        //        int rowCount2 = worksheet2.Dimension.Rows;
        //        int ColCount2 = worksheet2.Dimension.Columns;
        //        for (int row = 2; row <= rowCount2; row++)
        //        {
        //            var item = new DatauploadPurshaseDetails();

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 1]) == true)
        //                item.PurchaseDetailsID = worksheet2.Cells[row, 1].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 2]) == true)
        //                item.ProductId = worksheet2.Cells[row, 2].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 3]) == true)
        //                item.ProductVattypeId = worksheet2.Cells[row, 3].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 4]) == true)
        //                item.Quantity = Convert.ToDecimal(worksheet2.Cells[row, 4].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 5]) == true)
        //                item.UnitPrice = Convert.ToDecimal(worksheet2.Cells[row, 5].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 6]) == true)
        //                item.DiscountPerItem = Convert.ToDecimal(worksheet2.Cells[row, 6].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 7]) == true)
        //                item.CustomDutyPercent = Convert.ToDecimal(worksheet2.Cells[row, 7].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 8]) == true)
        //                item.ImportDutyPercent = Convert.ToDecimal(worksheet2.Cells[row, 8].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 9]) == true)
        //                item.RegulatoryDutyPercent = Convert.ToDecimal(worksheet2.Cells[row, 9].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 10]) == true)
        //                item.SupplementaryDutyPercent = Convert.ToDecimal(worksheet2.Cells[row, 10].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 11]) == true)
        //                item.VATPercent = Convert.ToDecimal(worksheet2.Cells[row, 11].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 12]) == true)
        //                item.AdvanceTaxPercent = Convert.ToDecimal(worksheet2.Cells[row, 12].Value);
        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 13]) == true)
        //                item.AdvanceIncomeTaxPercent = Convert.ToDecimal(worksheet2.Cells[row, 13].Value);
        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 14]) == true)
        //                item.MeasurementUnitId = worksheet2.Cells[row, 14].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 15]) == true)
        //                item.PurchaseID = worksheet2.Cells[row, 15].Value.ToString();

        //            output.purchaseDetails.Add(item);
        //        }

        //        int rowCount1 = worksheet1.Dimension.Rows;
        //        int ColCount1 = worksheet1.Dimension.Columns;
        //        for (int row = 2; row <= rowCount1; row++)
        //        {
        //            var item = new DatauploadPurshase();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 1]) == true)
        //                item.PurchaseID = worksheet1.Cells[row, 1].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 2]) == true)
        //                item.OrganizationId = worksheet1.Cells[row, 2].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 3]) == true)
        //                item.VendorId = worksheet1.Cells[row, 3].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 4]) == true)
        //                item.VatChallanNo = worksheet1.Cells[row, 4].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 5]) == true)
        //                item.VatChallanIssueDate = Convert.ToDateTime(worksheet1.Cells[row, 5].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 6]) == true)
        //                item.VendorInvoiceNo = worksheet1.Cells[row, 6].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 7]) == true)
        //                item.InvoiceNo = worksheet1.Cells[row, 7].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 8]) == true)
        //                item.PurchaseDate = Convert.ToDateTime(worksheet1.Cells[row, 8].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 9]) == true)
        //                item.PurchaseTypeId = worksheet1.Cells[row, 9].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 10]) == true)
        //                item.PurchaseReasonId = worksheet1.Cells[row, 10].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 11]) == true)
        //                item.DiscountOnTotalPrice = Convert.ToDecimal(worksheet1.Cells[row, 11].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 12]) == true)
        //                item.AdvanceTaxPaidAmount = Convert.ToDecimal(worksheet1.Cells[row, 12].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 13]) == true)
        //                item.ATPDate = Convert.ToDateTime(worksheet1.Cells[row, 13].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 14]) == true)
        //                item.ATPBankBranchId = worksheet1.Cells[row, 14].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 15]) == true)
        //                item.ATPNbrEconomicCodeId = worksheet1.Cells[row, 15].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 16]) == true)
        //                item.ATPChallanNo = worksheet1.Cells[row, 16].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 17]) == true)
        //                item.IsVatDeductedInSource = Convert.ToBoolean(worksheet1.Cells[row, 17].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 18]) == true)
        //                item.VDSAmount = Convert.ToDecimal(worksheet1.Cells[row, 18].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 19]) == true)
        //                item.IsVDSCertificatePrinted = Convert.ToBoolean(worksheet1.Cells[row, 19].Value);
        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 20]) == true)
        //                item.VDSCertificateDate = Convert.ToDateTime(worksheet1.Cells[row, 20].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 21]) == true)
        //                item.VDSCertificateDate = Convert.ToDateTime(worksheet1.Cells[row, 21].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 22]) == true)
        //                item.VDSPaymentBookTransferNo = worksheet1.Cells[row, 22].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 23]) == true)
        //                item.VDSNote = worksheet1.Cells[row, 23].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 24]) == true)
        //                item.ExpectedDeliveryDate = Convert.ToDateTime(worksheet1.Cells[row, 24].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 25]) == true)
        //                item.DeliveryDate = Convert.ToDateTime(worksheet1.Cells[row, 25].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 26]) == true)
        //                item.LcNo = worksheet1.Cells[row, 26].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 27]) == true)
        //                item.LcDate = Convert.ToDateTime(worksheet1.Cells[row, 27].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 28]) == true)
        //                item.BillOfEntry = worksheet1.Cells[row, 28].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 29]) == true)
        //                item.BillOfEntryDate = Convert.ToDateTime(worksheet1.Cells[row, 29].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 30]) == true)
        //                item.CustomsAndVATCommissionarateId = worksheet1.Cells[row, 30].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 31]) == true)
        //                item.DueDate = Convert.ToDateTime(worksheet1.Cells[row, 31].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 32]) == true)
        //                item.TermsOfLc = worksheet1.Cells[row, 32].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 33]) == true)
        //                item.PoNumber = worksheet1.Cells[row, 33].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 34]) == true)
        //                item.IsComplete = Convert.ToBoolean(worksheet1.Cells[row, 34].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 35]) == true)
        //                item.CreatedBy = worksheet1.Cells[row, 35].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 36]) == true)
        //                item.CreatedTime = Convert.ToDateTime(worksheet1.Cells[row, 36].Value);

        //            output.purchase.Add(item);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}

        //return await Task.FromResult(output);
        return null;
    }

    public async Task<vmPurchaseBulkPost> LoadPurchaseFromExcel(string location)
    {
        //var purchaseData = new vmPurchaseBulkPost();

        //var file = new System.IO.FileInfo(location);
        //try
        //{
        //    using (var package = new ExcelPackage(file))
        //    {
        //        var purchaseSheet = package.Workbook.Worksheets[1];
        //        var purchaseDetailSheet = package.Workbook.Worksheets[2];
        //        var purchasePaymentSheet = package.Workbook.Worksheets[3];

        //        purchaseData.PurchasePaymentList = GetPurchasePaymentList(purchasePaymentSheet);
        //        purchaseData.PurchaseDetailList = GetPurchaseDetailList(purchaseDetailSheet);
        //        purchaseData.PurchaseList = GetPurchaseList(purchaseSheet);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}

        //return await Task.FromResult(purchaseData);
        return null;
    }

    //private List<vmPurchasePaymentPost> GetPurchasePaymentList(ExcelWorksheet purchasePaymentSheet)
    //{
    //    var purchasePaymentList = new List<vmPurchasePaymentPost>();

    //    var rowCountPayment = purchasePaymentSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPayment; row++)
    //    {
    //        var item = new vmPurchasePaymentPost();

    //        if (CheckCellValueContainsData(purchasePaymentSheet.Cells[row, 1]))
    //            item.PurchasePaymentId = purchasePaymentSheet.Cells[row, 1].Value.ToString();

    //        if (CheckCellValueContainsData(purchasePaymentSheet.Cells[row, 2]))
    //            item.PurchaseId = purchasePaymentSheet.Cells[row, 2].Value.ToString();

    //        if (CheckCellValueContainsData(purchasePaymentSheet.Cells[row, 3]))
    //            item.PaymentMethodId = purchasePaymentSheet.Cells[row, 3].Value.ToString();

    //        if (CheckCellValueContainsData(purchasePaymentSheet.Cells[row, 4]))
    //            item.PaidAmount = Convert.ToDecimal(purchasePaymentSheet.Cells[row, 4].Value);

    //        if (CheckCellValueContainsData(purchasePaymentSheet.Cells[row, 5]))
    //            item.PaymentDate = Convert.ToDateTime(purchasePaymentSheet.Cells[row, 5].Value);

    //        purchasePaymentList.Add(item);
    //    }

    //    return purchasePaymentList;
    //}
    //private List<vmSalesPaymentPost> GetSalePaymentList(ExcelWorksheet salePaymentSheet)
    //{
    //    var salePaymentList = new List<vmSalesPaymentPost>();

    //    var rowCountPayment = salePaymentSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPayment; row++)
    //    {
    //        var item = new vmSalesPaymentPost();

    //        if (CheckCellValueContainsData(salePaymentSheet.Cells[row, 1]))
    //            item.SalesPaymentReceiveId = salePaymentSheet.Cells[row, 1].Value.ToString();

    //        if (CheckCellValueContainsData(salePaymentSheet.Cells[row, 2]))
    //            item.SalesId = salePaymentSheet.Cells[row, 2].Value.ToString();

    //        if (CheckCellValueContainsData(salePaymentSheet.Cells[row, 3]))
    //            item.ReceivedPaymentMethodId = salePaymentSheet.Cells[row, 3].Value.ToString();

    //        if (CheckCellValueContainsData(salePaymentSheet.Cells[row, 4]))
    //            item.ReceiveAmount = Convert.ToDecimal(salePaymentSheet.Cells[row, 4].Value);

    //        if (CheckCellValueContainsData(salePaymentSheet.Cells[row, 5]))
    //            item.ReceiveDate = Convert.ToDateTime(salePaymentSheet.Cells[row, 5].Value);

    //        salePaymentList.Add(item);
    //    }

    //    return salePaymentList;
    //}

    //private List<vmPurchaseDetailPost> GetPurchaseDetailList(ExcelWorksheet purchaseDetailSheet)
    //{
    //    var purchaseDetailList = new List<vmPurchaseDetailPost>();

    //    var rowCountPurchaseDetail = purchaseDetailSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPurchaseDetail; row++)
    //    {
    //        var item = new vmPurchaseDetailPost();

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 1]))
    //            item.PurchaseDetailId = purchaseDetailSheet.Cells[row, 1].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 2]))
    //            item.PurchaseId = purchaseDetailSheet.Cells[row, 2].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 3]))
    //            item.ProductId = purchaseDetailSheet.Cells[row, 3].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 4]))
    //            item.ProductVattypeId = purchaseDetailSheet.Cells[row, 4].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 5]))
    //            item.Quantity = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 5].Value);

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 6]))
    //            item.UnitPrice = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 6].Value);

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 7]))
    //            item.DiscountPerItem = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 7].Value);

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 8]))
    //            item.CustomDutyPercent = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 8].Value);

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 9]))
    //            item.ImportDutyPercent = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 9].Value);

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 10]))
    //            item.RegulatoryDutyPercent = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 10].Value);

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 11]))
    //            item.SupplementaryDutyPercent = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 11].Value);

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 12]))
    //            item.Vatpercent = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 12].Value);

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 13]))
    //            item.AdvanceTaxPercent = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 13].Value);
    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 14]))
    //            item.AdvanceIncomeTaxPercent = Convert.ToDecimal(purchaseDetailSheet.Cells[row, 14].Value);

    //        if (CheckCellValueContainsData(purchaseDetailSheet.Cells[row, 15]))
    //            item.MeasurementUnitId = purchaseDetailSheet.Cells[row, 15].Value.ToString();

    //        purchaseDetailList.Add(item);
    //    }

    //    return purchaseDetailList;
    //}
    //private List<vmSalesDetailPost> GetSaleDetailList(ExcelWorksheet saleDetailSheet)
    //{
    //    var saleDetailList = new List<vmSalesDetailPost>();

    //    var rowCountPurchaseDetail = saleDetailSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPurchaseDetail; row++)
    //    {
    //        var item = new vmSalesDetailPost();

    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 1]))
    //            item.SalesDetailId = saleDetailSheet.Cells[row, 1].Value.ToString();

    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 2]))
    //            item.SalesId = saleDetailSheet.Cells[row, 2].Value.ToString();

    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 3]))
    //            item.ProductId = saleDetailSheet.Cells[row, 3].Value.ToString();

    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 4]))
    //            item.ProductVattypeId = saleDetailSheet.Cells[row, 4].Value.ToString();

    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 5]))
    //            item.Quantity = Convert.ToDecimal(saleDetailSheet.Cells[row, 5].Value);

    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 6]))
    //            item.UnitPrice = Convert.ToDecimal(saleDetailSheet.Cells[row, 6].Value);

    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 7]))
    //            item.DiscountPerItem = Convert.ToDecimal(saleDetailSheet.Cells[row, 7].Value);


    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 8]))
    //            item.Vatpercent = Convert.ToDecimal(saleDetailSheet.Cells[row, 8].Value);

    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 9]))
    //            item.SupplementaryDutyPercent = Convert.ToDecimal(saleDetailSheet.Cells[row, 9].Value);

    //        if (CheckCellValueContainsData(saleDetailSheet.Cells[row, 10]))
    //            item.MeasurementUnitId = saleDetailSheet.Cells[row, 10].Value.ToString();

    //        saleDetailList.Add(item);
    //    }

    //    return saleDetailList;
    //}


    //private List<vmPurchasePost> GetPurchaseList(ExcelWorksheet purchaseSheet)
    //{
    //    var purchaseList = new List<vmPurchasePost>();

    //    var rowCountPurchase = purchaseSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPurchase; row++)
    //    {
    //        var item = new vmPurchasePost();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 1]))
    //            item.PurchaseId = purchaseSheet.Cells[row, 1].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 2]))
    //            item.VendorId = purchaseSheet.Cells[row, 2].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 3]))
    //            item.VatChallanNo = purchaseSheet.Cells[row, 3].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 4]))
    //            item.VatChallanIssueDate = Convert.ToDateTime(purchaseSheet.Cells[row, 4].Value.ToString());

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 5]))
    //            item.VendorInvoiceNo = purchaseSheet.Cells[row, 5].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 6]))
    //            item.InvoiceNo = purchaseSheet.Cells[row, 6].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 7]))
    //            item.PurchaseDate = Convert.ToDateTime(purchaseSheet.Cells[row, 7].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 8]))
    //            item.PurchaseTypeId = purchaseSheet.Cells[row, 8].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 9]))
    //            item.PurchaseReasonId = purchaseSheet.Cells[row, 9].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 10]))
    //            item.DiscountOnTotalPrice = Convert.ToDecimal(purchaseSheet.Cells[row, 10].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 11]))
    //            item.AdvanceTaxPaidAmount = Convert.ToDecimal(purchaseSheet.Cells[row, 11].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 12]))
    //            item.Atpdate = Convert.ToDateTime(purchaseSheet.Cells[row, 12].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 13]))
    //            item.AtpbankBranchId = purchaseSheet.Cells[row, 13].Value.ToString();
    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 14]))
    //            item.AtpnbrEconomicCodeId = purchaseSheet.Cells[row, 14].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 15]))
    //            item.AtpchallanNo = purchaseSheet.Cells[row, 15].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 16]))
    //            item.IsVatDeductedInSource = ExcelDataConverter.GetBoolean(purchaseSheet.Cells[row, 16].Value.ToString());

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 17]))
    //            item.Vdsamount = Convert.ToDecimal(purchaseSheet.Cells[row, 17].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 18]))
    //            item.IsVdscertificatePrinted = ExcelDataConverter.GetBoolean(purchaseSheet.Cells[row, 18].Value.ToString());

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 19]))
    //            item.VdscertificateNo = purchaseSheet.Cells[row, 19].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 20]))
    //            item.VdscertificateDate = Convert.ToDateTime(purchaseSheet.Cells[row, 20].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 21]))
    //            item.VdspaymentBookTransferNo = purchaseSheet.Cells[row, 21].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 22]))
    //            item.Vdsnote = purchaseSheet.Cells[row, 22].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 23]))
    //            item.ExpectedDeliveryDate = Convert.ToDateTime(purchaseSheet.Cells[row, 23].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 24]))
    //            item.DeliveryDate = Convert.ToDateTime(purchaseSheet.Cells[row, 24].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 25]))
    //            item.LcNo = purchaseSheet.Cells[row, 25].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 26]))
    //            item.LcDate = Convert.ToDateTime(purchaseSheet.Cells[row, 26].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 27]))
    //            item.BillOfEntry = purchaseSheet.Cells[row, 27].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 28]))
    //            item.BillOfEntryDate = Convert.ToDateTime(purchaseSheet.Cells[row, 28].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 29]))
    //            item.CustomsAndVatcommissionarateId = purchaseSheet.Cells[row, 29].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 30]))
    //            item.DueDate = Convert.ToDateTime(purchaseSheet.Cells[row, 30].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 31]))
    //            item.TermsOfLc = purchaseSheet.Cells[row, 31].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 32]))
    //            item.PoNumber = purchaseSheet.Cells[row, 32].Value.ToString();

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 33]))
    //            item.IsComplete = ExcelDataConverter.GetBoolean(purchaseSheet.Cells[row, 33].Value.ToString());

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 34]))
    //            item.CreatedBy = Convert.ToInt32(purchaseSheet.Cells[row, 34].Value);

    //        if (CheckCellValueContainsData(purchaseSheet.Cells[row, 35]))
    //            item.CreatedTime = Convert.ToDateTime(purchaseSheet.Cells[row, 35].Value);

    //        purchaseList.Add(item);
    //    }

    //    return purchaseList;
    //}
    //private List<vmSalesPost> GetSaleList(ExcelWorksheet saleSheet)
    //{
    //    var saleList = new List<vmSalesPost>();

    //    var rowCountPurchase = saleSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPurchase; row++)
    //    {
    //        var item = new vmSalesPost();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 1]))
    //            item.SalesId = saleSheet.Cells[row, 1].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 2]))
    //            item.InvoiceNo = saleSheet.Cells[row, 2].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 3]))
    //            item.VatChallanNo = saleSheet.Cells[row, 3].Value.ToString();

    //        //if (CheckCellValueContainsData(saleSheet.Cells[row, 4]))
    //        //    item.OrganizationId = Convert.ToInt32(saleSheet.Cells[row, 4].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 4]))
    //            item.DiscountOnTotalPrice = Convert.ToDecimal(saleSheet.Cells[row, 4].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 5]))
    //            item.IsVatDeductedInSource = ExcelDataConverter.GetBoolean(saleSheet.Cells[row, 5].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 6]))
    //            item.Vdsamount = Convert.ToDecimal(saleSheet.Cells[row, 6].Value);

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 7]))
    //            item.IsVdscertificateReceived = ExcelDataConverter.GetBoolean(saleSheet.Cells[row, 7].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 8]))
    //            item.VdscertificateNo = saleSheet.Cells[row, 8].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 9]))
    //            item.VdscertificateIssueTime = Convert.ToDateTime(saleSheet.Cells[row, 9].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 10]))
    //            item.VdspaymentBankBranchId = Convert.ToInt32(saleSheet.Cells[row, 10].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 11]))
    //            item.VdspaymentDate = Convert.ToDateTime(saleSheet.Cells[row, 11].Value);

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 12]))
    //            item.VdspaymentChallanNo = saleSheet.Cells[row, 12].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 13]))
    //            item.VdspaymentEconomicCode = saleSheet.Cells[row, 13].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 14]))
    //            item.VdspaymentBookTransferNo = saleSheet.Cells[row, 14].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 15]))
    //            item.Vdsnote = saleSheet.Cells[row, 15].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 16]))
    //            item.CustomerId = saleSheet.Cells[row, 16].Value.ToString();


    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 17]))
    //            item.ReceiverName = saleSheet.Cells[row, 17].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 18]))
    //            item.ReceiverContactNo = saleSheet.Cells[row, 18].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 19]))
    //            item.ShippingAddress = saleSheet.Cells[row, 19].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 20]))
    //            item.ShippingCountryId = saleSheet.Cells[row, 20].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 21]))
    //            item.SalesTypeId = saleSheet.Cells[row, 21].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 22]))
    //            item.SalesDeliveryTypeId = saleSheet.Cells[row, 22].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 23]))
    //            item.WorkOrderNo = saleSheet.Cells[row, 23].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 24]))
    //            item.SalesDate = Convert.ToDateTime(saleSheet.Cells[row, 24].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 25]))
    //            item.ExpectedDeliveryDate = Convert.ToDateTime(saleSheet.Cells[row, 25].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 26]))
    //            item.DeliveryDate = Convert.ToDateTime(saleSheet.Cells[row, 26].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 27]))
    //            item.DeliveryMethodId = saleSheet.Cells[row, 27].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 28]))
    //            item.ExportTypeId = saleSheet.Cells[row, 28].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 29]))
    //            item.LcNo = saleSheet.Cells[row, 29].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 30]))
    //            item.LcDate = Convert.ToDateTime(saleSheet.Cells[row, 30].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 31]))
    //            item.BillOfEntry = saleSheet.Cells[row, 31].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 32]))
    //            item.BillOfEntryDate = Convert.ToDateTime(saleSheet.Cells[row, 32].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 33]))
    //            item.DueDate = Convert.ToDateTime(saleSheet.Cells[row, 33].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 34]))
    //            item.TermsOfLc = saleSheet.Cells[row, 34].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 35]))
    //            item.CustomerPoNumber = saleSheet.Cells[row, 35].Value.ToString();

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 36]))
    //            item.IsComplete = ExcelDataConverter.GetBoolean(saleSheet.Cells[row, 36].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 37]))
    //            item.IsTaxInvoicePrined = ExcelDataConverter.GetBoolean(saleSheet.Cells[row, 37].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 38]))
    //            item.TaxInvoicePrintedTime = Convert.ToDateTime(saleSheet.Cells[row, 38].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 39]))
    //            item.CreatedBy = Convert.ToInt32(saleSheet.Cells[row, 39].Value.ToString());

    //        if (CheckCellValueContainsData(saleSheet.Cells[row, 40]))
    //            item.CreatedTime = Convert.ToDateTime(saleSheet.Cells[row, 40].Value.ToString());

    //        saleList.Add(item);
    //    }

    //    return saleList;
    //}

    public async Task<DataimportSalesFinal> LoadSales(string location)
    {
        //string sWebRootFolder = location;
        //string sFileName = System.IO.Path.GetFileName(sWebRootFolder);
        //var output = new DataimportSalesFinal();

        //System.IO.FileInfo file = new System.IO.FileInfo(location);
        //try
        //{
        //    using (ExcelPackage package = new ExcelPackage(file))
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
        //        ExcelWorksheet worksheet2 = package.Workbook.Worksheets[2];
        //        ExcelWorksheet worksheet3 = package.Workbook.Worksheets[3];

        //        int rowCount = worksheet3.Dimension.Rows;
        //        int ColCount = worksheet3.Dimension.Columns;
        //        for (int row = 2; row <= rowCount; row++)
        //        {
        //            //                        var item = new DatauploadPaymentSale();
        //            var item = new vmSalesPaymentPost();

        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 1]) == true)
        //                item.SalesPaymentReceiveId = worksheet3.Cells[row, 1].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 2]) == true)
        //                item.ReceivedPaymentMethodId = worksheet3.Cells[row, 2].Value.ToString();
        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 3]) == true)
        //                item.ReceiveAmount = Convert.ToDecimal(worksheet3.Cells[row, 3].Value);
        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 4]) == true)
        //                item.ReceiveDate = Convert.ToDateTime(worksheet3.Cells[row, 4].Value);
        //            if (CheckCellValueContainsData(worksheet3.Cells[row, 5]) == true)
        //                item.SalesId = worksheet3.Cells[row, 5].Value.ToString();

        //            output.Payments.Add(item);
        //        }

        //        int rowCount2 = worksheet2.Dimension.Rows;
        //        int ColCount2 = worksheet2.Dimension.Columns;
        //        for (int row = 2; row <= rowCount2; row++)
        //        {
        //            //                        var item = new DatauploadSaleDetails();
        //            var item = new vmSalesDetailPost();

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 1]) == true)
        //                item.SalesDetailId = worksheet2.Cells[row, 1].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 2]) == true)
        //                item.ProductId = worksheet2.Cells[row, 2].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 3]) == true)
        //                item.ProductVattypeId = worksheet2.Cells[row, 3].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 4]) == true)
        //                item.Quantity = Convert.ToDecimal(worksheet2.Cells[row, 4].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 5]) == true)
        //                item.UnitPrice = Convert.ToDecimal(worksheet2.Cells[row, 5].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 6]) == true)
        //                item.DiscountPerItem = Convert.ToDecimal(worksheet2.Cells[row, 6].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 7]) == true)
        //                item.SupplementaryDutyPercent = Convert.ToDecimal(worksheet2.Cells[row, 7].Value);

        //            //                        if (CheckCellValueContainsData(worksheet2.Cells[row, 8]) == true)
        //            //                            item.VATPercent = Convert.ToDecimal(worksheet2.Cells[row, 8].Value);

        //            if (CheckCellValueContainsData(worksheet2.Cells[row, 9]) == true)
        //                item.MeasurementUnitId = worksheet2.Cells[row, 9].Value.ToString();

        //            //                        if (CheckCellValueContainsData(worksheet2.Cells[row, 10]) == true)
        //            //                            item.SalesDetailsID = worksheet2.Cells[row, 10].Value.ToString();

        //            //                        output.SalesDetails.Add(item);
        //        }

        //        int rowCount1 = worksheet1.Dimension.Rows;
        //        int ColCount1 = worksheet1.Dimension.Columns;
        //        for (int row = 2; row <= rowCount1; row++)
        //        {
        //            var item = new DatauploadSales();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 1]) == true)
        //                item.SalesID = worksheet1.Cells[row, 1].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 2]) == true)
        //                item.InvoiceNo = worksheet1.Cells[row, 2].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 3]) == true)
        //                item.VatChallanNo = worksheet1.Cells[row, 3].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 4]) == true)
        //                item.OrganizationId = worksheet1.Cells[row, 4].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 5]) == true)
        //                item.DiscountOnTotalPrice = Convert.ToDecimal(worksheet1.Cells[row, 5].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 6]) == true)
        //                item.IsVatDeductedInSource = Convert.ToBoolean(worksheet1.Cells[row, 6].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 7]) == true)
        //                item.VDSAmount = Convert.ToDecimal(worksheet1.Cells[row, 7].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 8]) == true)
        //                item.IsVDSCertificateReceived = Convert.ToBoolean(worksheet1.Cells[row, 8].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 9]) == true)
        //                item.VDSCertificateNo = worksheet1.Cells[row, 9].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 10]) == true)
        //                item.VDSCertificateIssueTime = Convert.ToDateTime(worksheet1.Cells[row, 10].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 11]) == true)
        //                item.VDSPaymentBankBranchId = worksheet1.Cells[row, 11].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 12]) == true)
        //                item.VDSPaymentDate = Convert.ToDateTime(worksheet1.Cells[row, 12].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 13]) == true)
        //                item.VDSPaymentChallanNo = worksheet1.Cells[row, 13].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 14]) == true)
        //                item.VDSPaymentEconomicCode = worksheet1.Cells[row, 14].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 15]) == true)
        //                item.VDSPaymentBookTransferNo = worksheet1.Cells[row, 15].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 16]) == true)
        //                item.VDSNote = worksheet1.Cells[row, 16].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 17]) == true)
        //                item.CustomerId = worksheet1.Cells[row, 17].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 18]) == true)
        //                item.ReceiverName = worksheet1.Cells[row, 18].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 19]) == true)
        //                item.ReceiverContactNo = worksheet1.Cells[row, 19].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 20]) == true)
        //                item.ShippingAddress = worksheet1.Cells[row, 20].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 21]) == true)
        //                item.ShippingCountryId = worksheet1.Cells[row, 21].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 22]) == true)
        //                item.SalesTypeId = worksheet1.Cells[row, 22].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 23]) == true)
        //                item.SalesDeliveryTypeId = worksheet1.Cells[row, 23].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 24]) == true)
        //                item.WorkOrderNo = worksheet1.Cells[row, 24].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 25]) == true)
        //                item.SalesDate = Convert.ToDateTime(worksheet1.Cells[row, 25].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 26]) == true)
        //                item.ExpectedDeliveryDate = Convert.ToDateTime(worksheet1.Cells[row, 26].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 27]) == true)
        //                item.DeliveryDate = Convert.ToDateTime(worksheet1.Cells[row, 27].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 28]) == true)
        //                item.DeliveryMethodId = worksheet1.Cells[row, 28].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 29]) == true)
        //                item.ExportTypeId = worksheet1.Cells[row, 29].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 30]) == true)
        //                item.LcNo = worksheet1.Cells[row, 30].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 31]) == true)
        //                item.LcDate = Convert.ToDateTime(worksheet1.Cells[row, 31].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 32]) == true)
        //                item.BillOfEntry = worksheet1.Cells[row, 32].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 33]) == true)
        //                item.BillOfEntryDate = Convert.ToDateTime(worksheet1.Cells[row, 33].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 34]) == true)
        //                item.DueDate = Convert.ToDateTime(worksheet1.Cells[row, 34].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 35]) == true)
        //                item.TermsOfLc = worksheet1.Cells[row, 35].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 36]) == true)
        //                item.CustomerPoNumber = worksheet1.Cells[row, 36].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 37]) == true)
        //                item.IsComplete = Convert.ToBoolean(worksheet1.Cells[row, 37].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 38]) == true)
        //                item.IsTaxInvoicePrined = Convert.ToBoolean(worksheet1.Cells[row, 38].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 39]) == true)
        //                item.TaxInvoicePrintedTime = Convert.ToDateTime(worksheet1.Cells[row, 39].Value);

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 40]) == true)
        //                item.CreatedBy = worksheet1.Cells[row, 40].Value.ToString();

        //            if (CheckCellValueContainsData(worksheet1.Cells[row, 41]) == true)
        //                item.CreatedTime = Convert.ToDateTime(worksheet1.Cells[row, 41].Value);

        //            //                        output.Sales.Add(item);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}

        //return await Task.FromResult(output);
        return null;
    }




    //private List<MeasurementUnit> GetMeasurementUnitList(ExcelWorksheet measurementUnitSheet)
    //{
    //    var measurementUnitList = new List<MeasurementUnit>();

    //    var rowCountPurchase = measurementUnitSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPurchase; row++)
    //    {
    //        var item = new MeasurementUnit();

    //        if (CheckCellValueContainsData(measurementUnitSheet.Cells[row, 1]))
    //            item.MeasurementUnitId = Convert.ToInt32(measurementUnitSheet.Cells[row, 1].Value.ToString());

    //        if (CheckCellValueContainsData(measurementUnitSheet.Cells[row, 2]))
    //            item.Name = measurementUnitSheet.Cells[row, 2].Value.ToString();

    //        if (CheckCellValueContainsData(measurementUnitSheet.Cells[row, 3]))
    //            item.OrganizationId = Convert.ToInt32(measurementUnitSheet.Cells[row, 3].Value.ToString());


    //        if (CheckCellValueContainsData(measurementUnitSheet.Cells[row, 4]))
    //            item.IsActive = Convert.ToBoolean(measurementUnitSheet.Cells[row, 4].Value.ToString());

    //        if (CheckCellValueContainsData(measurementUnitSheet.Cells[row, 5]))
    //            item.ReferenceKey = measurementUnitSheet.Cells[row, 5].Value.ToString();

    //        if (CheckCellValueContainsData(measurementUnitSheet.Cells[row, 6]))
    //            item.ApiTransactionId = Convert.ToInt32(measurementUnitSheet.Cells[row, 6].Value.ToString());


    //        measurementUnitList.Add(item);
    //    }

    //    return measurementUnitList;
    //}


    //private List<DocumentType> GetDocumentTypeList(ExcelWorksheet documentTypeSheet)
    //{
    //    var documentTypeList = new List<DocumentType>();

    //    var rowCountPurchase = documentTypeSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPurchase; row++)
    //    {
    //        var item = new DocumentType();

    //        if (CheckCellValueContainsData(documentTypeSheet.Cells[row, 1]))
    //            item.DocumentTypeId = Convert.ToInt32(documentTypeSheet.Cells[row, 1].Value.ToString());

    //        if (CheckCellValueContainsData(documentTypeSheet.Cells[row, 2]))
    //            item.OrganizationId = Convert.ToInt32(documentTypeSheet.Cells[row, 2].Value.ToString());

    //        if (CheckCellValueContainsData(documentTypeSheet.Cells[row, 3]))
    //            item.Name = documentTypeSheet.Cells[row, 3].Value.ToString();


    //        if (CheckCellValueContainsData(documentTypeSheet.Cells[row, 4]))
    //            item.IsActive = Convert.ToBoolean(documentTypeSheet.Cells[row, 4].Value.ToString());

    //        if (CheckCellValueContainsData(documentTypeSheet.Cells[row, 5]))
    //            item.ReferenceKey = documentTypeSheet.Cells[row, 5].Value.ToString();


    //        documentTypeList.Add(item);
    //    }

    //    return documentTypeList;
    //}



    //private List<OverHeadCost> GetOverHeadCostList(ExcelWorksheet overHeadCostSheet)
    //{
    //    var overHeadCostList = new List<OverHeadCost>();

    //    var rowCountPurchase = overHeadCostSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPurchase; row++)
    //    {
    //        var item = new OverHeadCost();

    //        if (CheckCellValueContainsData(overHeadCostSheet.Cells[row, 1]))
    //            item.OverHeadCostId = Convert.ToInt32(overHeadCostSheet.Cells[row, 1].Value.ToString());

    //        if (CheckCellValueContainsData(overHeadCostSheet.Cells[row, 2]))
    //            item.OrganizationId = Convert.ToInt32(overHeadCostSheet.Cells[row, 2].Value.ToString());

    //        if (CheckCellValueContainsData(overHeadCostSheet.Cells[row, 3]))
    //            item.Name = overHeadCostSheet.Cells[row, 3].Value.ToString();


    //        if (CheckCellValueContainsData(overHeadCostSheet.Cells[row, 4]))
    //            item.IsActive = Convert.ToBoolean(overHeadCostSheet.Cells[row, 4].Value.ToString());

    //        if (CheckCellValueContainsData(overHeadCostSheet.Cells[row, 5]))
    //            item.IsApplicableAsRawMaterial = Convert.ToBoolean(overHeadCostSheet.Cells[row, 5].Value.ToString());

    //        if (CheckCellValueContainsData(overHeadCostSheet.Cells[row, 6]))
    //            item.ReferenceKey = overHeadCostSheet.Cells[row, 6].Value.ToString();

    //        if (CheckCellValueContainsData(overHeadCostSheet.Cells[row, 7]))
    //            item.ApiTransactionId = Convert.ToInt32(overHeadCostSheet.Cells[row, 7].Value.ToString());

    //        overHeadCostList.Add(item);
    //    }

    //    return overHeadCostList;
    //}


    //private List<ProductCategory> GetProductCategoryList(ExcelWorksheet productCategorySheet)
    //{
    //    var productCategoryList = new List<ProductCategory>();

    //    var rowCountPurchase = productCategorySheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPurchase; row++)
    //    {
    //        var item = new ProductCategory();

    //        if (CheckCellValueContainsData(productCategorySheet.Cells[row, 1]))
    //            item.ProductCategoryId = Convert.ToInt32(productCategorySheet.Cells[row, 1].Value.ToString());

    //        if (CheckCellValueContainsData(productCategorySheet.Cells[row, 2]))
    //            item.OrganizationId = Convert.ToInt32(productCategorySheet.Cells[row, 2].Value.ToString());

    //        if (CheckCellValueContainsData(productCategorySheet.Cells[row, 3]))
    //            item.Name = productCategorySheet.Cells[row, 3].Value.ToString();


    //        if (CheckCellValueContainsData(productCategorySheet.Cells[row, 4]))
    //            item.IsActive = Convert.ToBoolean(productCategorySheet.Cells[row, 4].Value.ToString());


    //        if (CheckCellValueContainsData(productCategorySheet.Cells[row, 5]))
    //            item.ReferenceKey = productCategorySheet.Cells[row, 5].Value.ToString();

    //        if (CheckCellValueContainsData(productCategorySheet.Cells[row, 6]))
    //            item.ApiTransactionId = Convert.ToInt32(productCategorySheet.Cells[row, 6].Value.ToString());

    //        productCategoryList.Add(item);
    //    }

    //    return productCategoryList;
    //}


    //private List<ProductGroup> GetProductGroupList(ExcelWorksheet productGroupSheet)
    //{
    //    var productGroupList = new List<ProductGroup>();

    //    var rowCountPurchase = productGroupSheet.Dimension.Rows;
    //    for (var row = 2; row <= rowCountPurchase; row++)
    //    {
    //        var item = new ProductGroup();

    //        if (CheckCellValueContainsData(productGroupSheet.Cells[row, 1]))
    //            item.ProductGroupId = Convert.ToInt32(productGroupSheet.Cells[row, 1].Value.ToString());

    //        if (CheckCellValueContainsData(productGroupSheet.Cells[row, 2]))
    //            item.OrganizationId = Convert.ToInt32(productGroupSheet.Cells[row, 2].Value.ToString());

    //        if (CheckCellValueContainsData(productGroupSheet.Cells[row, 3]))
    //            item.Name = productGroupSheet.Cells[row, 3].Value.ToString();


    //        if (CheckCellValueContainsData(productGroupSheet.Cells[row, 4]))
    //            item.ParentGroupId = Convert.ToInt32(productGroupSheet.Cells[row, 4].Value.ToString());


    //        if (CheckCellValueContainsData(productGroupSheet.Cells[row, 5]))
    //            item.Node = productGroupSheet.Cells[row, 5].Value.ToString();

    //        if (CheckCellValueContainsData(productGroupSheet.Cells[row, 6]))
    //            item.IsActive = Convert.ToBoolean(productGroupSheet.Cells[row, 6].Value.ToString());

    //        if (CheckCellValueContainsData(productGroupSheet.Cells[row, 7]))
    //            item.ReferenceKey = productGroupSheet.Cells[row, 7].Value.ToString();

    //        if (CheckCellValueContainsData(productGroupSheet.Cells[row, 8]))
    //            item.ApiTransactionId = Convert.ToInt32(productGroupSheet.Cells[row, 8].Value.ToString());

    //        productGroupList.Add(item);
    //    }

    //    return productGroupList;
    //}





    //private static bool CheckCellValueContainsData(ExcelRange item)
    //{
    //    var output = true;
    //    try
    //    {
    //        if (item == null || item.Value == null || item.Value.ToString() == "NULL" || item.ToString() == "")
    //            output = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }

    //    return output;
    //}

    public async Task<bool> InsertBulk(List<ProductDataImportViewModel> products, int organizationId, int createBy)
    {
        var output = false;
        output = await _productrepository.InsertBulk(products, organizationId, createBy);
        return output;
    }

    public async Task<bool> InsertBulkProduction(List<ProductionDataImportViewModel> products, int organizationId, int createBy)
    {
        var output = false;
        output = await _apibulk.InsertProductionExcel(products, organizationId, createBy, "Excel Input");
        return output;
    }

    public async Task<bool> InsertBulkpurchase(DataimportPurchaseFinal purchase, int orgID, int Uid, string security)
    {
        var output = false;
        output = await _apibulk.InsertPurchaseExcel(purchase, orgID, Uid, security);
        return output;
    }

    public async Task<bool> InsertBulkSales(DataimportSalesFinal purchase, int orgID, int Uid, string security)
    {
        var output = false;
        output = await _apibulk.InsertSaleExcel(purchase, orgID, Uid, security);
        return output;
    }

    public async Task<vmSaleBulkPost> LoadSaleFromExcel(string location)
    {
        //var saleData = new vmSaleBulkPost();

        //var file = new System.IO.FileInfo(location);
        //try
        //{
        //    using (var package = new ExcelPackage(file))
        //    {
        //        var saleSheet = package.Workbook.Worksheets[1];
        //        var saleDetailSheet = package.Workbook.Worksheets[2];
        //        var salePaymentSheet = package.Workbook.Worksheets[3];

        //        saleData.SalePaymentList = GetSalePaymentList(salePaymentSheet);
        //        saleData.SaleDetailList = GetSaleDetailList(saleDetailSheet);
        //        saleData.SaleList = GetSaleList(saleSheet);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}

        //return await Task.FromResult(saleData);
        return null;
    }



    public async Task<vmStaticData> LoadStaticDataFromExcel(string location)
    {
        //var staticData = new vmStaticData();

        //var file = new System.IO.FileInfo(location);
        //try
        //{
        //    using (var package = new ExcelPackage(file))
        //    {
        //        var measurementUnitSheet = package.Workbook.Worksheets[1];
        //        var DocumentTypeSheet = package.Workbook.Worksheets[2];
        //        var overHeadCostSheet = package.Workbook.Worksheets[3];
        //        var productCategorySheet = package.Workbook.Worksheets[4];
        //        var productGroupsSheet = package.Workbook.Worksheets[5];



        //        staticData.MeasurementUnitList = GetMeasurementUnitList(measurementUnitSheet);
        //        staticData.DocumentTypeList = GetDocumentTypeList(DocumentTypeSheet);
        //        staticData.OverHeadCostList = GetOverHeadCostList(overHeadCostSheet);
        //        staticData.ProductCategoryList = GetProductCategoryList(productCategorySheet);
        //        staticData.ProductGroupList = GetProductGroupList(productGroupsSheet);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}

        //return await Task.FromResult(staticData);
        return null;
    }


}