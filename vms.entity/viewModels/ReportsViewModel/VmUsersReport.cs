using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using vms.entity.Enums;
using vms.entity.models;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmUsersReport : VmReportBase
{
    public IEnumerable<models.User> Users { get; set; }
    public EnumUserStatus UserStatus { get; set; }
    [DisplayName("Report Option")]
    [Range(1, 4, ErrorMessage = "Please select an option")]
    public int ReportProcessOptionId { get; set; }

    public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }

}