using System;

namespace FullInspector
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public sealed class InspectorCategoryAttribute : Attribute
	{
		public string Category;

		public InspectorCategoryAttribute(string category)
		{
			Category = category;
		}
	}
}
