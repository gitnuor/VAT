namespace vms.utility.StaticData;

public class RdlcStaticData
{
    public const string PdfFormatName = "PDF";
    public const string PdfContentType = "application/pdf";
    public const string PdfExtensionName = "pdf";
    public const string ExcelFormatName = "EXCELOPENXML";
    public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string ExcelExtensionName = "xlsx";
    public const string WordFormatName = "WORDOPENXML";
    public const string WordContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    public const string WordExtensionName = "docx";
    public const string HtmlFormatName = "HTML5";
    public const string HtmlContentType = "text/html";
    public const string HtmlExtensionName = "html";
}
public class RdlcReportFileOption
{
    #region Vendor Report

    /*** Vendor Report **/
    public const string VendorReportUrl = "RdlcFiles/Report/RdlcVendorReport.rdlc";
    public const string VendorReportFileName = "VendorReport";
    public const string VendorReportDsName = "DataSetVendorReport";

    #endregion

    #region Customer Report

    /*** Customer Report **/
    public const string CustomerReportUrl = "RdlcFiles/Report/RdlcCustomerrReport.rdlc";
    public const string CustomerReportFileName = "CustomerReport";
    public const string CustomerReportDsName = "DataSetCustomerReport";

    /*** All Customer Report **/
    public const string AllCustomerReportUrl = "RdlcFiles/Report/RdlcAllCustomerrReport.rdlc";
    public const string AllCustomerReportFileName = "AllCustomerReport";
    public const string AllCustomerReportDsName = "DataSetCustomerReport";

    #endregion

    #region Sales Report

    /*** Sales Report **/
    public const string SalesrReportUrl = "RdlcFiles/Report/RdlcSalesReport.rdlc";
    public const string SalesReportFileName = "SalesReport";
    public const string SalesReportDsName = "DataSetSalesReport";

    /*** All Sales Report **/
    public const string SalesAllReportUrl = "RdlcFiles/Report/RdlcSales.rdlc";
    public const string SalesAllReportFileName = "AllSales";
    public const string SalesAllReportDsName = "DataSetSalesReport";

    /*** All Sales Report **/
    public const string SalesSummeryAllReportUrl = "RdlcFiles/Report/RdlcSalesSummery.rdlc";
    public const string SalesSummeryAllReportFileName = "AllSalesSummery";
    public const string SalesSummeryAllReportDsName = "DataSetSalesReport";

    /*** All Sales Report Customer Grouped **/
    public const string SalesAllCustomerGroupedReportUrl = "RdlcFiles/Report/RdlcSalesCustomerGrouped.rdlc";
    public const string SalesAllCustomerGroupedReportFileName = "AllSalesCustomerGrouped";
    public const string SalesAllCustomerGroupedReportDsName = "DataSetSalesReport";

    /*** Sales Report By Branch And Customer **/
    public const string SalesReportByCustomerUrl = "RdlcFiles/Report/RdlcSalesByCustomer.rdlc";
    public const string SalesReportByCustomerFileName = "SalesReportByCustomer";
    public const string SalesReportByCustomerDsName = "DataSetSalesReport";

    /*** Sales Report By Branch **/
    public const string SalesReportByBranchUrl = "RdlcFiles/Report/RdlcSalesByBranch.rdlc";
    public const string SalesReportByBranchFileName = "SalesReportByBranch";
    public const string SalesReportByBranchDsName = "DataSetSalesReport";

    /*** Sales Report By Branch And Customer **/
    public const string SalesReportByBranchAndCustomerUrl = "RdlcFiles/Report/RdlcSalesByBranchAndCustomer.rdlc";
    public const string SalesReportByBranchAndCustomerFileName = "SalesReportByBranchAndCustomer";
    public const string SalesReportByBranchAndCustomerDsName = "DataSetSalesReport";

    /*** Sales Due Aging **/
    public const string SalesDueAgingReportUrl = "RdlcFiles/Report/RdlcSalesPaymentAgingReport.rdlc";
    public const string SalesDueAgingReportFileName = "SalesDueAging";
    public const string SalesDueAgingReportDsName = "DataSetSalesPaymentAgingReport";

    /*** Product Sales Report all **/
    public const string ProductSalesListReportUrl = "RdlcFiles/Report/RdlcProductSalesListReport.rdlc";
    public const string ProductSalesListReportFileName = "ProductSalesListReport";
    public const string ProductSalesListReportDsName = "DataSetProductSalesListReport";

    /*** Sales VDS List Report **/
    public const string SalesVdsListReportUrl = "RdlcFiles/Report/RdlcSalesVDSListReport.rdlc";
    public const string SalesVdsListReportFileName = "SalesVdsListReport";
    public const string SalesVdsListReportDsName = "DataSetSalesReport";

	/*** Only Sales Details List **/
	public const string SalesDetailsListReportUrl = "RdlcFiles/Report/RdlcSalesDetailsListReport.rdlc";
	public const string SalesDetailsListReportFileName = "SalesDetailsListReport";
	public const string SalesDetailsListReportDsName = "DataSetSalesDetailsListReport";

	/*** Credit Note Details List **/
	public const string CreditNoteDetailsListReportUrl = "RdlcFiles/Report/RdlcCreditNoteDetailsListReport.rdlc";
	public const string CreditNoteDetailsListReportFileName = "CreditNoteDetailsListReport";
	public const string CreditNoteDetailsListReportDsName = "DataSetCreditNoteDetailsListReport";

	#endregion

	#region Purchase Report


	/*** All Purchase Report **/
	public const string PurchaseAllReportUrl = "RdlcFiles/Report/RdlcPurchase.rdlc";
    public const string PurchaseAllReportFileName = "AllPurchase";
    public const string PurchaseAllReportDsName = "DataSetPurchaseReport";

    /*** All Purchase Report **/
    public const string PurchaseSummeryAllReportUrl = "RdlcFiles/Report/RdlcPurchaseSummery.rdlc";
    public const string PurchaseSummeryAllReportFileName = "AllPurchaseSummery";
    public const string PurchaseSummeryAllReportDsName = "DataSetPurchaseReport";

    /*** All Purchase Report Vendor Grouped **/
    public const string PurchaseAllVendorGroupedReportUrl = "RdlcFiles/Report/RdlcPurchaseVendorGrouped.rdlc";
    public const string PurchaseAllVendorGroupedReportFileName = "AllPurchaseVendorGrouped";
    public const string PurchaseAllVendorGroupedReportDsName = "DataSetPurchaseReport";

    /*** Purchase Report By Branch And Vendor **/
    public const string PurchaseReportByVendorUrl = "RdlcFiles/Report/RdlcPurchaseByVendor.rdlc";
    public const string PurchaseReportByVendorFileName = "PurchaseReportByVendor";
    public const string PurchaseReportByVendorDsName = "DataSetPurchaseReport";

    /*** Purchase Report By Branch **/
    public const string PurchaseReportByBranchUrl = "RdlcFiles/Report/RdlcPurchaseByBranch.rdlc";
    public const string PurchaseReportByBranchFileName = "PurchaseReportByBranch";
    public const string PurchaseReportByBranchDsName = "DataSetPurchaseReport";

    /*** Purchase Report By Branch And Vendor **/
    public const string PurchaseReportByBranchAndVendorUrl = "RdlcFiles/Report/RdlcPurchaseByBranchAndVendor.rdlc";
    public const string PurchaseReportByBranchAndVendorFileName = "PurchaseReportByBranchAndVendor";
    public const string PurchaseReportByBranchAndVendorDsName = "DataSetPurchaseReport";

    /*** Product Purchase Report all **/
    public const string ProductPurchaseListReportUrl = "RdlcFiles/Report/RdlcProductPurchaseListReport.rdlc";
    public const string ProductPurchaseListReportFileName = "ProductPurchaseListReport";
    public const string ProductPurchaseListReportDsName = "DataSetProductPurchaseListReport";

    /*** Monthly Purchase report **/
    public const string MonthlyPurchaseReportUrl = "RdlcFiles/Report/RdlcMonthlyPurchaseReport.rdlc";
    public const string MonthlyPurchaseReportFileName = "MonthlyPurchaseReport";
    public const string MonthlyPurchaseReportDsName = "DsMonthlyPurchaseReport";

    /*** Purchase Report By Vendor **/
    public const string PurchaseOldReportByVendorUrl = "RdlcFiles/Report/RdlcPurchaseReportByVendor.rdlc";
    public const string PurchaseOldReportByVendorFileName = "PurchaseReportByVendor";
    public const string PurchaseOldReportByVendorDsName = "DsMonthlyPurchaseReport";

    /*** Expense Report **/
    public const string ExpenseReportUrl = "RdlcFiles/Report/RdlcExpenseReport.rdlc";
    public const string ExpenseReportFileName = "ExpenseReport";
    public const string ExpenseReportDsName = "DsMonthlyPurchaseReport";

    /*** Purchase VDS List Report **/
    //public const string PurchaseVDSListReportUrl = "RdlcFiles/Report/RdlcPurchaseVDSListReport.rdlc";
    //public const string PurchaseVDSListReportFileName = "PurchaseVDSListReport";
    //public const string PurchaseVDSListReportDsName = "DataSetPurchaseReport";



    /*** Purchase VDS Certificate Report **/
    public const string VdsCertificateUrl = "RdlcFiles/Mushak/VdsCertificate_En.rdlc";
    public const string VdsCertificateFileName = "VDS_Certificate";
    public const string VdsCertificateDsName = "VdsCertificateDataSet";



    /*** Purchase VDS Certificate Report **/
    public const string TdsCertificateUrl = "RdlcFiles/Mushak/TdsCertificate_En.rdlc";
    public const string TdsCertificateFileName = "TDS_Certificate";
    public const string TdsCertificateDsName = "TdsCertificateDataSet";

    /*** Purchase Report **/
    public const string PurchaseReportUrl = "RdlcFiles/Report/RdlcPurchaseReport.rdlc";
    public const string PurchaseReportFileName = "PurchaseReport";
    public const string PurchaseReportDsName = "DataSetPurchaseReport";

    /*** Purchase Report By Product**/
    public const string GetPurchaseReportByProductUrl = "RdlcFiles/Report/RdlcGetPurchaseReportByProduct.rdlc";
    public const string GetPurchaseReportByProductFileName = "GetPurchaseReportByProduct";
    public const string GetPurchaseReportByProductDsName = "DataSetGetPurchaseReportByProduct";

    /*** Purchase VDS List **/
    public const string PurchaseVdsListReportUrl = "RdlcFiles/Report/RdlcPurchaseVdsListReport.rdlc";
    public const string PurchaseVdsListReportFileName = "PurchaseVdsListReport";
    public const string PurchaseVdsListReportDsName = "DataSetPurchaseVdsListReport";

    /*** Only Purchase Details List **/
    public const string PurchaseDetailsListReportUrl = "RdlcFiles/Report/RdlcPurchaseDetailsListReport.rdlc";
    public const string PurchaseDetailsListReportFileName = "PurchaseDetailsListReport";
    public const string PurchaseDetailsListReportDsName = "DataSetPurchaseDetailsListReport";

	/*** Debit Note Details List **/
	public const string DebitNoteDetailsListReportUrl = "RdlcFiles/Report/RdlcDebitNoteDetailsListReport.rdlc";
	public const string DebitNoteDetailsListReportFileName = "DebitNoteDetailsListReport";
	public const string DebitNoteDetailsListReportDsName = "DataSetDebitNoteDetailsListReport";

	#endregion

	#region Mushak Report


	/*** Mushak Report (6.1) **/
	public const string MushakSixPointOneReportUrl = "RdlcFiles/Mushak/MushakSixPointOne.rdlc";
    public const string MushakSixPointOneReportFileName = "MushakSixPointOne";
    public const string MushakSixPointOneReportDsName = "DataSetMushakSixPointOneReport";

    /*** Mushak Report (6.2) **/
    public const string MushakSixPointTwoReportUrl = "RdlcFiles/Mushak/MushakSixPointTwo.rdlc";
    public const string MushakSixPointTwoReportFileName = "MushakSixPointTwo";
    public const string MushakSixPointTwoReportDsName = "DataSetMushakSixPointTwoReport";

    /*** Mushak Six Point Three RDLC **/
    public const string MushakSixPointThreeRdlcUrl = "RdlcFiles/Mushak/MushakSixPointThree_En.rdlc";
    public const string MushakSixPointThreeRdlcUrlFileName = "MushakSixPointThree";
    public const string MushakSixPointThreeRdlcUrlDsName = "MushakSixPointThreeDataSet";

    /*** Mushak Six Point Two Point One RDLC **/
    public const string MushakSixPointTowPointOneRdlcUrl = "RdlcFiles/Mushak/MushakSixPointTowPointOne.rdlc";
    public const string MushakSixPointTowPointOneRdlcUrlFileName = "MushakSixPointTowPointOne";
    public const string MushakSixPointTowPointOneRdlcUrlDsName = "MushakSixPointTowPointOneDataSet";

    #endregion

    #region Product Report

    /*** Product Current Stock Report**/
    public const string ProductCurrentStockReportUrl = "RdlcFiles/ProductReport/ProductCurrentStock.rdlc";
    public const string ProductCurrentStockReportFileName = "ProductCurrentStock";
    public const string ProductCurrentStockReportDsName = "ProductCurrentStockDataSet";
	/*** Product  Stock Report**/
	public const string ProductStockReportUrl = "RdlcFiles/ProductReport/ProductStockReport.rdlc";
	public const string ProductStockReportFileName = "ProductStock";
	public const string ProductStockReportDsName = "ProductCurrentStockDataSet";


	#endregion

	#region Miscellaneous Report

	/*** User Report **/
	public const string UserReportUrl = "RdlcFiles/Report/RdlcUserReport.rdlc";
    public const string UserReportFileName = "UsersReport";
    public const string UserReportDsName = "DataSetUserReport";

    /*** Yearly Comparison Report **/
    public const string YearlyComparisonReportUrl = "RdlcFiles/Report/RdlcYearlyComparisonReport.rdlc";
    public const string YearlyComparisonReportFileName = "YearlyComparisonReport";
    public const string YearlyComparisonReportDsName = "DataSetYearlyComparisonReport";

    /*** Monthly Comparison Report **/
    public const string MonthlyComparisonReportUrl = "RdlcFiles/Report/RdlcMonthlyComparisonReport.rdlc";
    public const string MonthlyComparisonReportFileName = "MonthlyComparisonReport";
    public const string MonthlyComparisonReportDsName = "DataSetMonthlyComparisonReport";

    /*** Input Output CoEfficient Report **/
    public const string InputOutputCoEfficientReportUrl = "RdlcFiles/Report/RdlcInputOutputCoEfficientReportReport.rdlc";
    public const string InputOutputCoEfficientReportFileName = "InputOutputCoEfficientReportReport";
    public const string InputOutputCoEfficientReportDsName = "DataSetInputOutputCoEfficientReportReport";

    #endregion

    #region Production Report

    /*** Contractual Production Report **/
    public const string ContractualProductionReportUrl = "RdlcFiles/Report/RdlcContractualProductionReport.rdlc";
    public const string ContractualProductionReportFileName = "ContractualProductionReport";
    public const string ContractualProductionReportDsName = "DataSetContractualProduction";

    #endregion

}