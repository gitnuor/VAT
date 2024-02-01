using System;
using System.Collections.Generic;
using System.Globalization;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.service.Services.ReportService;

namespace vms.service.ServiceImplementations.ReportService;

public class ReportOptionService : IReportOptionService
{
    public IEnumerable<CustomSelectListItem> GetReportDisplayOrExportTypeSelectList()
    {
        //var items = new List<SelectListItem>
        //{
        //    new() { Value = "1", Text = "View" },
        //    new() { Value = "2", Text = "Export to PDF" },
        //    new() { Value = "3", Text = "Export to Excel" },
        //    new() { Value = "4", Text = "Export to Word" },
        //};
        //return items;

        //var students = new List<object>() {
        //    new object(){ Id = 1, Name="Bill"},
        //        new object(){ Id = 2, Name="Steve"},
        //        new object(){ Id = 3, Name="Ram"},
        //        new object(){ Id = 4, Name="Abdul"}
        //    };

        var items = new List<CustomSelectListItem>
        {
            new() { Id = 1, Name = "View" },
            new() { Id = 2, Name = "Export to PDF" },
            new() { Id = 3, Name = "Export to Excel" },
            new() { Id = 4, Name = "Export to Word" },
        };
        return items.ConvertToCustomSelectList(nameof(CustomSelectListItem.Id),
            nameof(CustomSelectListItem.Name));
        //return items;
    }

	public IEnumerable<CustomSelectListItem> GetYearSelectList()
	{
		var currentDate = DateTime.Today;
		int currentYear = Convert.ToInt32(currentDate.Year);
		var previousYears = new List<CustomSelectListItem>();
		for (int i = 0; i < 10; i++)
		{
			previousYears.Add(new CustomSelectListItem
			{
				Id = currentYear,
				Name = currentYear.ToString()
			});
			currentYear--;
		}
		return previousYears.ConvertToCustomSelectList(nameof(CustomSelectListItem.Id),
			nameof(CustomSelectListItem.Name));
	}

	public IEnumerable<CustomSelectListItem> GetMonthSelectList()
	{
		var listMonth = new List<CustomSelectListItem>();
		int i = 1;
		foreach (var item in DateTimeFormatInfo.CurrentInfo.MonthNames)
		{
			if (!string.IsNullOrEmpty(item))
			{
				listMonth.Add(new CustomSelectListItem
				{
					Id = i,
					Name = item.ToString()
				});
			}
			i++;
		}
		return listMonth.ConvertToCustomSelectList(nameof(CustomSelectListItem.Id),
			nameof(CustomSelectListItem.Name));
	}
}