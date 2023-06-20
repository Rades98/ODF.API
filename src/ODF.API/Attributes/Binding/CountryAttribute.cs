﻿using System.Reflection;
using ODF.API.Extensions;

namespace ODF.API.Attributes.Binding
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
	public class CountryAttribute : BindingAttribute
	{
		public override void Bind(PropertyInfo propInfo, object obj)
			=> propInfo.SetValue(obj, HttpContextAccessor.HttpContext!.GetCountryCode(), null);
	}
}
