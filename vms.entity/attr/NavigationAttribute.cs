using System;

namespace vms.entity.attr;

[AttributeUsage(AttributeTargets.Property)]
public class ReportAttribute:Attribute
{
    public bool NavigationTable { get; set; }
    public bool Display { get; set; }
    public bool SearchAble { get; set; }

}