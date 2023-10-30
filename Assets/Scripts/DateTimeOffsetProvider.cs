using System;
using System.Collections.Generic;
using System.Globalization;

public class DateTimeOffsetProvider : TestProvider<DateTimeOffset>
{
	public override bool Compare(DateTimeOffset original, DateTimeOffset deserialized)
	{
		return original == deserialized;
	}

	public override IEnumerable<DateTimeOffset> GetValues()
	{
		yield return new DateTimeOffset(5500, 2, 15, 0, 0, 0, 5, new HebrewCalendar(), default(TimeSpan));
		yield return DateTimeOffset.Now;
		yield return DateTimeOffset.MaxValue.Subtract(TimeSpan.FromTicks(1L));
		yield return DateTimeOffset.MinValue;
		yield return default(DateTimeOffset);
		yield return DateTimeOffset.UtcNow;
	}
}
