using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels;

public class VmDamage : DamageType
{
    public int DamageId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Please select product")]
    public int ProductId { get; set; }
    public long StockInId { get; set; }
    [Range(.001, double.MaxValue, ErrorMessage = "Damage qty should be greater than 0")]
    public decimal DamageQty { get; set; }
    public string Description { get; set; }
    public string VoucherNo { get; set; }

    public string purchageInvoice { get; set; }
    public string BachNo { get; set; }
    public List<vmDamageType> vMDamageType { get; set; }
}

public class vmDamageType
{
    public int Id { get; set; }
    public string Name { get; set; }

}