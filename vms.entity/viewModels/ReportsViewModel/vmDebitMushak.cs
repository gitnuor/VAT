using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmDebitMushak
{
    public int DebitNoteId { get; set; }
    public int Language { get; set; }
    public List<SpDebitMushak> DebitMushakList { get; set; }
}