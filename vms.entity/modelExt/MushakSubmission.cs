using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using vms.entity.viewModels;


namespace vms.entity.models;

public partial class MushakSubmission : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> AdjustmentTypes;

	[NotMapped]
	public string ClosingBalanceStatus => IsWantToGetBackClosingBalance ? "Yes" : "No";
	[NotMapped]
	public string MonthName => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MushakForMonth);


}