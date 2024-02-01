using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class VmExcelSimplifiedLocalSalesWithoutPayment
{
	[MaxLength(100, ErrorMessage = "The 'SalesId'  {1} characters long.")]
	public string SalesId { get; set; }

	public DateTime? SalesDate { get; set; }

	[MaxLength(50, ErrorMessage = "The 'InvoiceNo'  {1} characters long.")]
	public string InvoiceNo { get; set; }

	[MaxLength(100, ErrorMessage = "The 'EmhCode'  {1} characters long.")]
	public string EmhCode { get; set; }

	[MaxLength(100, ErrorMessage = "The 'BranchId'  {1} characters long.")]
	public string BranchId { get; set; }

	[MaxLength(250, ErrorMessage = "The 'BranchName'  {1} characters long.")]
	public string BranchName { get; set; }

	[MaxLength(500, ErrorMessage = "The 'BranchAddress'  {1} characters long.")]
	public string BranchAddress { get; set; }

	[MaxLength(100, ErrorMessage = "The 'CustomerId'  {1} characters long.")]
	public string CustomerId { get; set; }

	[MaxLength(200, ErrorMessage = "The 'CustomerName'  {1} characters long.")]
	public string CustomerName { get; set; }

	[MaxLength(50, ErrorMessage = "The 'CustomerNid'  {1} characters long.")]
	public string CustomerNid { get; set; }

	[MaxLength(50, ErrorMessage = "The 'CustomerTin'  {1} characters long.")]
	public string CustomerTin { get; set; }

	[MaxLength(50, ErrorMessage = "The 'CustomerBin'  {1} characters long.")]
	public string CustomerBin { get; set; }

	[MaxLength(500, ErrorMessage = "The 'CustomerAddress'  {1} characters long.")]
	public string CustomerAddress { get; set; }

	[MaxLength(20, ErrorMessage = "The 'Mobile No.'  {1} characters long.")]
	public string CustomerMobile { get; set; }

	[MaxLength(100, ErrorMessage = "The 'Email Address'  {1} characters long.")]
	public string CustomerEmail { get; set; }

	[MaxLength(100, ErrorMessage = "The 'Receiver Name'  {1} characters long.")]
	public string ReceiverName { get; set; }

	[MaxLength(20, ErrorMessage = "The 'Receiver Mobile No.'  {1} characters long.")]
	public string ReceiverMobile { get; set; }

	[MaxLength(100, ErrorMessage = "The 'Vehicle Type Name'  {1} characters long.")]
	public string VehicleTypeName { get; set; }

	[MaxLength(500, ErrorMessage = "The 'Delivery Address'  {1} characters long.")]
	public string DeliveryAddress { get; set; }

	[MaxLength(50, ErrorMessage = "The 'Delivery Method'  {1} characters long.")]
	public string DeliveryMethod { get; set; }

	[MaxLength(50, ErrorMessage = "The 'Vehicle Registration No.'  {1} characters long.")]
	public string VehicleRegistrationNo { get; set; }

	[MaxLength(100, ErrorMessage = "The 'Vehicle Driver Name'  {1} characters long.")]
	public string VehicleDriverName { get; set; }

	[MaxLength(20, ErrorMessage = "The 'Vehicle Driver Mobile No.'  {1} characters long.")]
	public string VehicleDriverMobile { get; set; }

	[MaxLength(100, ErrorMessage = "The 'SalesDetailId'  {1} characters long.")]
	public string SalesDetailId { get; set; }

	[MaxLength(100, ErrorMessage = "The 'ProductId'  {1} characters long.")]
	public string ProductId { get; set; }

	[MaxLength(200, ErrorMessage = "The 'ProductName'  {1} characters long.")]
	public string ProductName { get; set; }

	[MaxLength(50, ErrorMessage = "The 'HsCode'  {1} characters long.")]
	public string HsCode { get; set; }

	[MaxLength(200, ErrorMessage = "The 'ProductGroupName'  {1} characters long.")]
	public string ProductGroupName { get; set; }

	[MaxLength(200, ErrorMessage = "The 'ProductType'  {1} characters long.")]
	public string ProductType { get; set; }

	[MaxLength(50, ErrorMessage = "The 'MeasurementUnitName'  {1} characters long.")]
	public string MeasurementUnitName { get; set; }

	[Range(0.00, 9999999999999999.99, ErrorMessage = "Quantity must be greater than 0.00")]
	public decimal Quantity { get; set; }

	[Range(0.00, 9999999999999999.99, ErrorMessage = "UnitPrice must be greater than 0.00")]
	public decimal UnitPrice { get; set; }

	[Range(0.00, 9999999999999999.99, ErrorMessage = "DiscountPerItem must be greater than 0.00")]
	public decimal DiscountPerItem { get; set; }

	[MaxLength(500, ErrorMessage = "The 'VatType' {1} characters long.")]
	public string VatType { get; set; }

	[Range(0.00, 9999999999999999.99, ErrorMessage = "VatPercent must be greater than 0.00")]
	public decimal? VatPercent { get; set; }

	[Range(0.00, 9999999999999999.99, ErrorMessage = "SupplementaryDutyPercent must be greater than 0.00")]
	public decimal? SupplementaryDutyPercent { get; set; }
}