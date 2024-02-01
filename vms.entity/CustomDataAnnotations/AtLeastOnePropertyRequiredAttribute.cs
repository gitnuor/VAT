using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace vms.entity.CustomDataAnnotations;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AtLeastOnePropertyRequiredAttribute : ValidationAttribute
{
	private string[] PropertyList { get; }

	public AtLeastOnePropertyRequiredAttribute(params string[] propertyList)
	{
		PropertyList = propertyList;
	}

	public override object TypeId => this;

	public override bool IsValid(object value)
	{
		return PropertyList.Select(propertyName => value.GetType().GetProperty(propertyName)).Any(propertyInfo =>
			propertyInfo != null && propertyInfo.GetValue(value, null) != null);
	}
}