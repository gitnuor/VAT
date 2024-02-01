using System;
using System.Collections.Generic;
using System.Linq;
using vms.entity.viewModels;

namespace vms.entity.Utility;

public static class ListConverter
{
    public static IEnumerable<CustomSelectListItem> ConvertToCustomSelectList
    (
        this IEnumerable<object> list,
        string idPropertyName,
        string namePropertyName
    )
    {
        return list.Select(x => new CustomSelectListItem
        {
            Id = int.Parse(x.GetType().GetProperty(idPropertyName)?.GetValue(x)?.ToString() ??
                           throw new Exception("Value not found!")),
            Name = x.GetType().GetProperty(namePropertyName)?.GetValue(x)?.ToString() ??
                   throw new Exception("Value not found!")
        });
    }
}