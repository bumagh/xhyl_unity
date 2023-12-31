using System;
using System.Reflection;
using UnityEngine;

namespace FullSerializer
{
	public class fsConfig
	{
		public Type[] SerializeAttributes = new Type[2]
		{
			typeof(SerializeField),
			typeof(fsPropertyAttribute)
		};

		public Type[] IgnoreSerializeAttributes = new Type[2]
		{
			typeof(NonSerializedAttribute),
			typeof(fsIgnoreAttribute)
		};

		public Type[] IgnoreSerializeTypeAttributes = new Type[1]
		{
			typeof(fsIgnoreAttribute)
		};

		public fsMemberSerialization DefaultMemberSerialization = fsMemberSerialization.Default;

		public Func<string, MemberInfo, string> GetJsonNameFromMemberName = (string name, MemberInfo info) => name;

		public bool EnablePropertySerialization = true;

		public bool SerializeNonAutoProperties;

		public bool SerializeNonPublicSetProperties = true;

		public string CustomDateTimeFormatString;

		public bool Serialize64BitIntegerAsString;

		public bool CoerceStringsToNumbers;

		public bool SerializeEnumsAsInteger;
	}
}
