using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmCreditMushak
{
    public int CreditNoteId { get; set; }
    public List<SpCreditMushak> CreditMushakList { get; set; }
}