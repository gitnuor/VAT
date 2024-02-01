namespace vms.entity.StoredProcedureModel;

public class SpGetUnBilledSubscriptions
{

	public int CustomerSubscriptionId { get; set; }

	public int OrganizationId { get; set; }

	public int BillingOfficeId { get; set; }

	public string BillingOfficeName { get; set; }

	public string BillingOfficeAddress { get; set; }

	public int CollectionOfficeId { get; set; }

	public string CollectionOfficeName { get; set; }

	public string CollectionOfficeAddress { get; set; }

	public int CustomerId { get; set; }

	public string CustomerName { get; set; }

	public string CustomerBin { get; set; }

	public string CustomerNidNo { get; set; }

	public string CustomerAddress { get; set; }

	public decimal? SubscriptionPriceWithoutTax { get; set; }

	public decimal? SupplementaryDuty { get; set; }

	public decimal? VatAmount { get; set; }
}