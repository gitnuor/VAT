using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels.ReportsViewModel
{
    public class VmProductSalesListReport
    {
        public int OgrId { get; set; }

        [DisplayName("Branch")]
        public int? OgrBranchId { get; set; }

        [DisplayName("From Date")]
        public DateTime? FromDate { get; set; }

        [DisplayName("To Date")]
        public DateTime? ToDate { get; set; }

        public IEnumerable<CustomSelectListItem> BranchList { get; set; }

        [DisplayName("Report Option")]
        [Range(1, 4, ErrorMessage = "Please select an option")]
        public int ReportProcessOptionId { get; set; }

        public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
    }
}
