using System;
using System.Collections.Generic;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels.NewReports;

public  class VmDamageRepot 
{

    public  List<SpDamage> SpDamageList { get; set; }
    public HeaderModel head { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }


}