using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.entity.StoredProcedureModel;

namespace vms.entity.models;

public partial class VmsContext
{
	[NotMapped]
	public DbSet<SpGetProductAutocompleteForPurchase> SpGetProductAutocompleteForPurchases { get; set; }

	[NotMapped]
	public DbSet<SpGetProductAutocompleteForSale> SpGetProductAutocompleteForSales { get; set; }
	[NotMapped]
	public DbSet<SpGetProductAutocompleteForProductionReceive> SpGetProductAutocompleteForProductionReceive { get; set; }

	[NotMapped]
	public DbSet<SpDamageInvoiceList> SpDamageInvoiceList { get; set; }
	[NotMapped]
	public DbSet<SpGetProductAutocompleteForBom> SpGetProductAutocompleteForBom { get; set; }
	[NotMapped]
	public DbSet<SpGetProductAutocompleteForPriceSetu> SpGetProductAutocompleteForPriceSetu { get; set; }
	[NotMapped]
	public DbSet<SpDamage> SpDamage { get; set; }
	[NotMapped]
	public DbSet<spGet6P3View> spGet6P3View { get; set; }
	[NotMapped]
	public DbSet<spGetSalePaged> spGetSalePaged { get; set; }
	[NotMapped]
	public DbSet<SpGetVatType> SpGetVatType { get; set; }
	[NotMapped]
	public DbSet<SpGetRawMaterialForProduction> SpGetRawMaterialForProduction { get; set; }
	//[NotMapped]
	// public DbSet<SpDashboard> SpDashboard { get; set; }
	[NotMapped]
	public DbSet<SpPurchaseCalcBook> SpPurchaseCalcBook { get; set; }
	[NotMapped]
	public DbSet<SpSalesCalcBook> SpSalesCalcBook { get; set; }
	[NotMapped]
	public DbSet<SpPurchaseSaleCalcBook> SpPurchaseSaleCalcBook { get; set; }
	[NotMapped]
	public DbSet<Sp6p6> Sp6p6 { get; set; }
	[NotMapped]
	public DbSet<SpCreditMushak> SpCreditMushak { get; set; }
	[NotMapped]
	public DbSet<SpDebitMushak> SpDebitMushak { get; set; }
	[NotMapped]
	public DbSet<SpSalesTaxInvoice> SpSalesTaxInvoice { get; set; }
	[NotMapped]
	public DbSet<Sp6p4> Sp6p4 { get; set; }
	[NotMapped]
	public DbSet<Sp6p5> Sp6p5 { get; set; }
	[NotMapped]
	public DbSet<SpCreditNoteMushak> SpCreditNoteMushak { get; set; }


	[NotMapped]
	public DbSet<SpDebiNotetMushak> SpDebiNotetMushak { get; set; }

}