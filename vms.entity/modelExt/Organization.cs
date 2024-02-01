using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(OrganizationMetadata))]
public partial class Organization : VmsBaseModel
{

       
	public IEnumerable<CustomSelectListItem> CustomsAndVatCommissionarates;
        

	[NotMapped]
	public List<KeyValuePair<int, string>> CountryIds { get; set; }
	[NotMapped]
	public List<KeyValuePair<int, string>> BusinessNatures { get; set; }
	[NotMapped]
	[DisplayName("Business Nature")]
	[Required(ErrorMessage = "Business Nature required")]
	public int SelectedBusinessNatureId { get; set; }

	[NotMapped]
	public List<KeyValuePair<int, string>> FinancialActivityNatures { get; set; }
	[NotMapped]
	[DisplayName("Financial Activity")]
	[Required(ErrorMessage = "Financial Activity is required")]
	public int SelectedFinancialActivityNatureId { get; set; }


	[NotMapped]
	public List<KeyValuePair<int, string>> BusinessCategories { get; set; }
	[NotMapped]
	[DisplayName("Business Category")]
	[Required(ErrorMessage = "Business Category is required")]
	public int SelectedBusinessCategoryId { get; set; }


	[NotMapped]
	[DisplayName("Country")]
	[Required(ErrorMessage = "Country is required")]
	public int SelectedCountryId { get; set; }
	[NotMapped]
	public List<KeyValuePair<int, string>> CityIds { get; set; }
	[NotMapped]
	[DisplayName("City")]
	[Required(ErrorMessage = "City is required")]
	public int SelectedCityId { get; set; }
	[NotMapped]
	public string UserName { get; set; }
	//[NotMapped]
	//public string Mobile { get; set; }
	//[NotMapped]
	//public string EmailAddress { get; set; }
	[NotMapped]
	public string VatPersonName { get; set; }
	[NotMapped]
	public string VatPersonDesignation { get; set; }
	[NotMapped]
	public string Status => IsActive ? "Active" : "Inactive";

}
public class OrganizationMetadata
{
	[StringLength(240, ErrorMessage = "Name cannot be longer than 240 characters.")]
	[Required(ErrorMessage ="Name is required")]
	public string Name { get; set; }

	[RegularExpression("^[0-9]*$", ErrorMessage = "Entered Code is not valid.")]
	[StringLength(50, ErrorMessage = "Code cannot be longer than 50 characters.")]        
	public string Code { get; set; }

	[RegularExpression("^[0-9]*$", ErrorMessage = "Entered Vat Reg. No. is not valid.")]
	[StringLength(50, ErrorMessage = "Vat Reg No cannot be longer than 50 characters.")]       
	[Display(Name = "Vat Reg. No.")]
	public string VatregNo { get; set; }

	[RegularExpression("^[0-9]*$", ErrorMessage = "Entered Bin is not valid.")]
	[StringLength(50, ErrorMessage = "Bin cannot be longer than 50 characters.")]
	[Display(Name = "Bin")]
	public string Bin { get; set; }

	[RegularExpression("^[0-9]*$", ErrorMessage = "Entered Certificate No. is not valid.")]
	[StringLength(50, ErrorMessage = "Certificate No cannot be longer than 50 characters.")]
	[Display(Name = "Certificate")]       
	public string CertificateNo { get; set; }

	[RegularExpression("^[0-9]*$", ErrorMessage = "Entered Enlisted No. is not valid.")]
	[Display(Name = "Enlisted No.")]        
	public int? EnlistedNo { get; set; }

	[RegularExpression("^[0-9]*$", ErrorMessage = "Entered Postal Code is not valid.")]
	[Display(Name = "Postal Code")]
	public int? PostalCode { get; set; }


	//[RegularExpression("^[0-9]*$", ErrorMessage = "Entered Mobile is not valid.")]
	//[StringLength(11, ErrorMessage = "Mobile cannot be longer than 11 characters.")]
	//[Required]
	//[Display(Name = "Mobile")]
	//public string Mobile { get; set; }
	[Required(ErrorMessage = "Customs And Vat Commissionarate is required")]
	public int CustomsAndVatcommissionarateId { get; set; }

	[StringLength(100, ErrorMessage = "Vat Responsible Person Name cannot be longer than 100 characters.")]
	[Required(ErrorMessage = "Vat Person Name is required")]
	[Display(Name = "Vat Person Name")]
	public string VatResponsiblePersonName { get; set; }
           
        
	[StringLength(50, ErrorMessage = "Vat Responsible Person Designation cannot be longer than 50 characters.")]
	[Required(ErrorMessage = "Vat Responsible Person Designation is required")]
	public string VatResponsiblePersonDesignation { get; set; }

	[RegularExpression("^[A-Z0-9a-z._%+-]+@[a-zA-Z_-]+?.[a-zA-Z]{2,3}$", ErrorMessage = "Entered Email Address is not valid.")]
	[StringLength(100, ErrorMessage = "Vat Responsible Person Email address cannot be longer than {1} characters.")]
	[Required(ErrorMessage = "Vat Responsible Person Email Address is required")]
	[Display(Name = "Vat Responsible Person Email Address")]
	public string VatResponsiblePersonEmailAddress { get; set; }

	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01(3-9)xxxxxxxx!!!")]
	[StringLength(11, ErrorMessage = "Vat Responsible Person Mobile No. cannot be longer than 11 digits.")]
	[Required(ErrorMessage = "Vat Responsible Person MobileNo is required")]
	public string VatResponsiblePersonMobileNo { get; set; }
      
	[StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
	[Required]
	public string Address { get; set; }

	[StringLength(64, ErrorMessage = "User Name cannot be longer than 64 characters.")]
	[Required(ErrorMessage ="User name is required")]
	public string UserName { get; set; }

	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01(3-9)xxxxxxxx!!!")]
	[StringLength(11, ErrorMessage = "Mobile No cannot be longer than 11 characters.")]
	[Display(Name = "Mobile")]
	public string Mobile { get; set; }

	[RegularExpression("^[A-Z0-9a-z._%+-]+@[a-zA-Z_-]+?.[a-zA-Z]{2,3}$", ErrorMessage = "Entered Email Address is not valid.")]
	[StringLength(100, ErrorMessage ="Email address can not be longer than {1} characters")]
	[Display(Name = "Email Address")]
	public string EmailAddress { get; set; }
	[DisplayName("Description")]
	[StringLength(300,ErrorMessage ="Description can not be greater than 300 characters")]
	public string BusinessCategoryDescription { get; set; }
}