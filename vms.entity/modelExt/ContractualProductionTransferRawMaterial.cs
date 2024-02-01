using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

public partial class ContractualProductionTransferRawMaterial : VmsBaseModel
{
	[NotMapped]
	public string FromDateEn { get; set; }
	[NotMapped]
	public string TodateEn { get; set; }
}