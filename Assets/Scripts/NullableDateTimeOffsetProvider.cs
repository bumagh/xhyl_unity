using System;
using System.Collections.Generic;

public class NullableDateTimeOffsetProvider : TestProvider<ValueHolder<DateTimeOffset?>>
{
	public override bool Compare(ValueHolder<DateTimeOffset?> before, ValueHolder<DateTimeOffset?> after)
	{
		DateTimeOffset? value = before.Value;
		bool hasValue = value.HasValue;
		DateTimeOffset? value2 = after.Value;
		return hasValue == value2.HasValue && (!value.HasValue || value.GetValueOrDefault() == value2.GetValueOrDefault());
	}

	public override IEnumerable<ValueHolder<DateTimeOffset?>> GetValues()
	{
		yield return new ValueHolder<DateTimeOffset?>(null);
		yield return new ValueHolder<DateTimeOffset?>(DateTimeOffset.UtcNow);
	}
}
