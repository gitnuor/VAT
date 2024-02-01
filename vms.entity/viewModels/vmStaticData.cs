using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmStaticData
{      
    public IList<MeasurementUnit> MeasurementUnitList { get; set; }
    public IList<DocumentType> DocumentTypeList { get; set; }
    public IList<OverHeadCost> OverHeadCostList { get; set; }
    public IList<ProductCategory> ProductCategoryList { get; set; }
    public IList<ProductGroup> ProductGroupList { get; set; }

}