using System;
using System.Collections.Generic;

public class DateTimeProvider : TestProvider<DateTime>
{
	public override bool Compare(DateTime original, DateTime deserialized)
	{
		return original == deserialized;
	}

	public override IEnumerable<DateTime> GetValues()
	{
		yield return new DateTime(2009, 2, 15, 0, 0, 0, DateTimeKind.Utc);
		yield return DateTime.Now;
		yield return DateTime.MaxValue.Subtract(TimeSpan.FromTicks(1L));
		yield return DateTime.MinValue;
		yield return default(DateTime);
		yield return DateTime.UtcNow;
		yield return DateTime.Now.AddDays(5.0).AddHours(3.0).AddTicks(1L);
		yield return Convert.ToDateTime("2016-01-22T12:06:57.503005Z");
	}
}
