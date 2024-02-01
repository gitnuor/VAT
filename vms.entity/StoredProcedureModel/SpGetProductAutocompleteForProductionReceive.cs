﻿using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel;

public class SpGetProductAutocompleteForProductionReceive
{
    [Key]
    public int ProductId { get; set; }

    public string ProductName { get; set; }
    public string ModelNo { get; set; }
    public string Code { get; set; }
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
}