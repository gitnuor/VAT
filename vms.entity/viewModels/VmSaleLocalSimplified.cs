using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels;

public class VmSaleLocalSimplified : VmSaleLocalBase
{
    public VmSaleLocalSimplified()
    {
        VmSaleLocalDetails = new List<VmSaleLocalDetail>();
        VmSaleLocalDocuments = new List<VmSaleLocalDocument>();
        VmSaleLocalPayments = new List<VmSaleLocalPayment>();
        CustomerList = new List<Customer>();
        DeliveryMethodSelectList = new List<CustomSelectListItem>();
        ProductForSaleList = new List<SpGetProductForSale>();
        MeasurementUnitSelectList = new List<CustomSelectListItem>();
        ProductVatTypes = new List<ProductVattype>();
        DocumentTypeSelectList = new List<CustomSelectListItem>();
        BankSelectList = new List<CustomSelectListItem>();
        PaymentMethodList = new List<PaymentMethod>();
        VehicleTypesList = new List<VehicleType>();
    }


    public IEnumerable<VmSaleLocalDetail> VmSaleLocalDetails { get; set; }
    public IEnumerable<VmSaleLocalDocument> VmSaleLocalDocuments { get; set; }
    public IEnumerable<VmSaleLocalPayment> VmSaleLocalPayments { get; set; }
    public IEnumerable<Customer> CustomerList { get; set; }
    public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; }
    public IEnumerable<CustomSelectListItem> DeliveryMethodSelectList { get; set; }
    public IEnumerable<SpGetProductForSale> ProductForSaleList { get; set; }
    public IEnumerable<CustomSelectListItem> MeasurementUnitSelectList { get; set; }
    public IEnumerable<ProductVattype> ProductVatTypes { get; set; }
    public IEnumerable<CustomSelectListItem> DocumentTypeSelectList { get; set; }
    public IEnumerable<PaymentMethod> PaymentMethodList { get; set; }
    public IEnumerable<CustomSelectListItem> BankSelectList { get; set; }
    public IEnumerable<VehicleType> VehicleTypesList { get; set; }

    // [MaxLength(50, ErrorMessage = "Max Length 50 allowed")]
    // public string InvoiceNo { get; set; }
    //     
    // public DateTime? InvoiceDate { get; set; }

    #region Vehicle Information
    [DisplayName("Driver Name")]
    [MaxLength(50)]
    public string DriverName { get; set; }
    [DisplayName("Driver Mobile")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string DriverMobile { get; set; }
    [DisplayName("Is Require Registration")]
    public Boolean IsRequireRegistration { get; set; }
    [DisplayName("Vehicle Type")]
    [Required]
    public int? VehicleTypeId { get; set; }
    [DisplayName("Vehicle Registration No")]
    [MaxLength(50)]
    public string VehicleRegistrationNo { get; set; }
    #endregion

}