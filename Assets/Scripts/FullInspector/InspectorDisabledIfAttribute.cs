using System;

namespace FullInspector
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class InspectorDisabledIfAttribute : Attribute
	{
		public string[] ConditionalMemberNames;

		public fiLogicalOperator Operator;

		public string ConditionalMemberName
		{
			set
			{
				ConditionalMemberNames = new string[1]
				{
					value
				};
			}
		}

		public InspectorDisabledIfAttribute(fiLogicalOperator op, params string[] memberNames)
		{
			Operator = op;
			ConditionalMemberNames = memberNames;
		}

		public InspectorDisabledIfAttribute(string conditionalMemberName)
		{
			ConditionalMemberName = conditionalMemberName;
		}
	}
}
