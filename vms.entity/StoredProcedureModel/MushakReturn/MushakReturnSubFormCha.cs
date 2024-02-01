using System;

namespace vms.entity.StoredProcedureModel.MushakReturn;

public class MushakReturnSubFormCha
{
    public long? SlNo { get; set; }
    public string BillOfEntryNo { get; set; }
    public DateTime? BillOfEntryDate { get; set; }
    public string CustomsAndVATCommissionarateName { get; set; }
    public string CustomsAndVATCommissionarateNameInBangla { get; set; }
    public decimal? AdvanceTaxAmount { get; set; }
    public decimal? AdvanceTaxPaidAmount { get; set; }
}