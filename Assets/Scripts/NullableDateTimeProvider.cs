using System;
using System.Collections.Generic;

public class NullableDateTimeProvider : TestProvider<ValueHolder<DateTime?>>
{
	public override bool Compare(ValueHolder<DateTime?> before, ValueHolder<DateTime?> after)
	{
		DateTime? value = before.Value;
		bool hasValue = value.HasValue;
		DateTime? value2 = after.Value;
		return hasValue == value2.HasValue && (!value.HasValue || value.GetValueOrDefault() == value2.GetValueOrDefault());
	}

	public override IEnumerable<ValueHolder<DateTime?>> GetValues()
	{
		yield return new ValueHolder<DateTime?>(null);
		yield return new ValueHolder<DateTime?>(DateTime.UtcNow);
	}
}
