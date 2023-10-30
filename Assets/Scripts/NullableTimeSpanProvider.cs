using System;
using System.Collections.Generic;

public class NullableTimeSpanProvider : TestProvider<ValueHolder<TimeSpan?>>
{
	public override bool Compare(ValueHolder<TimeSpan?> before, ValueHolder<TimeSpan?> after)
	{
		TimeSpan? value = before.Value;
		bool hasValue = value.HasValue;
		TimeSpan? value2 = after.Value;
		return hasValue == value2.HasValue && (!value.HasValue || value.GetValueOrDefault() == value2.GetValueOrDefault());
	}

	public override IEnumerable<ValueHolder<TimeSpan?>> GetValues()
	{
		yield return new ValueHolder<TimeSpan?>(null);
		yield return new ValueHolder<TimeSpan?>(TimeSpan.FromSeconds(35.0));
	}
}
