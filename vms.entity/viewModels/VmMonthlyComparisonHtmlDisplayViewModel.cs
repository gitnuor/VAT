using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vms.entity.viewModels
{
    public class VmMonthlyComparisonHtmlDisplayViewModel
    {
        [DisplayName("From Year")]
        public int FromYear { get; set; }

        [DisplayName("To Year")]
        public int ToYear { get; set; }

        [DisplayName("From Month")]
        public int FromMonth { get; set; }

        [DisplayName("To Month")]
        public int ToMonth { get; set; }

        [DisplayName("Report Option")]
        [Range(1, 4, ErrorMessage = "Please select an option")]
        public int ReportProcessOptionId { get; set; }
        public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
        public IEnumerable<CustomSelectListItem> MonthList { get; set; }
    }
}
