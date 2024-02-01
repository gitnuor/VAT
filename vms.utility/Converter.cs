using System;
using System.Collections.Generic;
using System.Linq;

namespace vms.utility;

public static class Converter
{
    public static List<KeyValuePair<int, string>> GetEnumList<T>()
    {
        return (from object e in Enum.GetValues(typeof(T)) select new KeyValuePair<int, string>((int)e, e.ToString())).ToList();
    }

    public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
    {
        var attrType = typeof(T);
        var property = instance.GetType().GetProperty(propertyName);
        if (property == null)
            throw new NullReferenceException("Attribute not found!");
        return (T)property.GetCustomAttributes(attrType, false).First();
    }

}