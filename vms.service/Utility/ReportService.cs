using System;
using System.Collections.Generic;
using System.Linq;
using vms.entity.attr;
using vms.entity.viewModels;

namespace vms.service.Utility;

public static class ReportService
{
    public static Report GetReportAttributes<T>()
    {
        var instance = Activator.CreateInstance(typeof(T));
        if (instance == null)
        {
            throw new NullReferenceException("Null object!");
        }
        var properties = instance.GetType().GetProperties();
        var report = new Report();
        var columns = new List<Clolumn>();
        foreach (var property in properties)
        {
            var items = property.GetCustomAttributes(typeof(ReportAttribute), true).FirstOrDefault();
            if (items == null) continue;
            var column = new Clolumn();
            var attr = (ReportAttribute)items;
            column.Name = property.Name;
            column.CanDisplay = attr.Display;
            column.CanSearh = attr.SearchAble;
            column.IsNavigation = attr.NavigationTable;
            columns.Add(column);
        }
        report.Columns = columns;
        return report;
    }

}