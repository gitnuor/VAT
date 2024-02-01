namespace vms.Utility;

public static class FilterChanges
{
    public static string GetChangesDetails<T, T2>(T obj1, T2 obj2)
    {
        var oOldRecord = obj1;
        var oNewRecord = obj2;

        var changes = "";
        System.Reflection.PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (System.Reflection.PropertyInfo property in properties)
        {
            var oOldValue = property.GetValue(oOldRecord, null);
            var oNewValue = property.GetValue(oNewRecord, null);
            if (property.PropertyType.Name != "IEnumerable`1"
                && property.PropertyType.Name != "ICollection`1" 
                && !property.PropertyType.FullName.Contains("vms.entity.model"))
            {
                if (!Equals(oOldValue, oNewValue) && oNewValue != null)
                {
                    // Handle the display values when the underlying value is null
                    var sOldValue = oOldValue == null ? "null" : oOldValue.ToString();
                    var sNewValue = oNewValue == null ? "null" : oNewValue.ToString();
                    changes += property.Name + ":" + sOldValue + "=>" + sNewValue + ", ";
                }
            }
        }
         
        return changes;
    }
}