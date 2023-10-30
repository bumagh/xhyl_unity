using System;
using System.Globalization;

namespace FullSerializer.Internal
{
	public class fsDateConverter : fsConverter
	{
		private const string DefaultDateTimeFormatString = "o";

		private const string DateTimeOffsetFormatString = "o";

		private string DateTimeFormatString => Serializer.Config.CustomDateTimeFormatString ?? "o";

		public override bool CanProcess(Type type)
		{
			return type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan);
		}

		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			if (instance is DateTime)
			{
				serialized = new fsData(((DateTime)instance).ToString(DateTimeFormatString));
				return fsResult.Success;
			}
			if (instance is DateTimeOffset)
			{
				serialized = new fsData(((DateTimeOffset)instance).ToString("o"));
				return fsResult.Success;
			}
			if (instance is TimeSpan)
			{
				serialized = new fsData(((TimeSpan)instance).ToString());
				return fsResult.Success;
			}
			throw new InvalidOperationException("FullSerializer Internal Error -- Unexpected serialization type");
		}

		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			if (!data.IsString)
			{
				return fsResult.Fail("Date deserialization requires a string, not " + data.Type);
			}
			if (storageType == typeof(DateTime))
			{
				if (DateTime.TryParse(data.AsString, null, DateTimeStyles.RoundtripKind, out DateTime result))
				{
					instance = result;
					return fsResult.Success;
				}
				if (fsGlobalConfig.AllowInternalExceptions)
				{
					try
					{
						instance = Convert.ToDateTime(data.AsString);
						return fsResult.Success;
					}
					catch (Exception ex)
					{
						return fsResult.Fail("Unable to parse " + data.AsString + " into a DateTime; got exception " + ex);
					}
				}
				return fsResult.Fail("Unable to parse " + data.AsString + " into a DateTime");
			}
			if (storageType == typeof(DateTimeOffset))
			{
				if (DateTimeOffset.TryParse(data.AsString, null, DateTimeStyles.RoundtripKind, out DateTimeOffset result2))
				{
					instance = result2;
					return fsResult.Success;
				}
				return fsResult.Fail("Unable to parse " + data.AsString + " into a DateTimeOffset");
			}
			if (storageType == typeof(TimeSpan))
			{
				if (TimeSpan.TryParse(data.AsString, out TimeSpan result3))
				{
					instance = result3;
					return fsResult.Success;
				}
				return fsResult.Fail("Unable to parse " + data.AsString + " into a TimeSpan");
			}
			throw new InvalidOperationException("FullSerializer Internal Error -- Unexpected deserialization type");
		}
	}
}
