using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.viewModels;

namespace vms.service.Services.ReportService;

public interface IReportOptionService
{
    IEnumerable<CustomSelectListItem> GetReportDisplayOrExportTypeSelectList();

	IEnumerable<CustomSelectListItem> GetYearSelectList();
	IEnumerable<CustomSelectListItem> GetMonthSelectList();
}