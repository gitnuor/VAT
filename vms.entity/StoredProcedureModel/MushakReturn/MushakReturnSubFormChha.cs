using System;

namespace vms.entity.StoredProcedureModel.MushakReturn;

public class MushakReturnSubFormChha
{
    public long? SlNo { get; set; }
    public int MushakReturnPaymentId { get; set; }
    public int OrganizationId { get; set; }
    public int MushakYear { get; set; }
    public int MushakMonth { get; set; }
    public int MushakReturnPaymentTypeId { get; set; }
    public string SubFormId { get; set; }
    public string SubFormName { get; set; }
    public string SubFormNameEng { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int BankId { get; set; }
    public string BankName { get; set; }
    public string BankNameInBangla { get; set; }
    public int BankBranchId { get; set; }
    public string BankBranchName { get; set; }
    public string BankBranchNameInBangla { get; set; }
    public bool IsSubmitted { get; set; }
    public string TreasuryChallanNo { get; set; }
    public DateTime? SubimissionDate { get; set; }
    public int NbrEconomicCodeId { get; set; }
    public string EconomicCode { get; set; }
}