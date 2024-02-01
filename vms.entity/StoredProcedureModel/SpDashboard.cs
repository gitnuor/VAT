using System;
using System.Collections.Generic;

namespace vms.entity.StoredProcedureModel;

public class SpDashboard
{
    public SpVatCurrent VatCurrentStatus { get; set; }
    public List<SpVatPayment> VatPaymentList { get; set; }
    public string strVatPaymentList { get; set; }
}
public class SpVatCurrent
{
    public decimal SalesAmount { get; set; }
    public decimal PurchaseAmount { get; set; }
    public decimal VdsAmount { get; set; }
    public decimal MiscIncrementalAdjustAmount { get; set; }
    public decimal MiscDecrementalAdjustAmount { get; set; }
    public decimal Rebate { get; set; }
    public DateTime NextSubmissionDate { get; set; }
    public decimal PreviousDue { get; set; }
    public decimal TotalVAT { get; set; }
}
public class SpVatPayment
{
    public string Year { get; set; }
    public string Month { get; set; }
    public decimal TotalVAT { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalDue { get; set; }
    public decimal TotalRebeat { get; set; }
}