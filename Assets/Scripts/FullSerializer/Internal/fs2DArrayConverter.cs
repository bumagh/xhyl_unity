using System;
using System.Collections;
using System.Collections.Generic;

namespace FullSerializer.Internal
{
	public class fs2DArrayConverter : fsConverter
	{
		public override bool CanProcess(Type type)
		{
			return type.IsArray && type.GetArrayRank() == 2;
		}

		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			Array array = (Array)instance;
			IList list = array;
			Type elementType = storageType.GetElementType();
			fsResult success = fsResult.Success;
			serialized = FullSerializer.fsData.CreateDictionary();
			Dictionary<string, fsData> asDictionary = serialized.AsDictionary;
			fsData fsData = fsData.CreateList(list.Count);
			asDictionary.Add("c", new fsData(array.GetLength(1)));
			asDictionary.Add("r", new fsData(array.GetLength(0)));
			asDictionary.Add("a", fsData);
			List<fsData> asList = fsData.AsList;
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					object value = array.GetValue(i, j);
					fsData data;
					fsResult result = Serializer.TrySerialize(elementType, value, out data);
					success.AddMessages(result);
					if (!result.Failed)
					{
						asList.Add(data);
					}
				}
			}
			return success;
		}

		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult success = FullSerializer.fsResult.Success;
			fsResult fsResult = success += CheckType(data, fsDataType.Object);
			if (fsResult.Failed)
			{
				return success;
			}
			Type elementType = storageType.GetElementType();
			Dictionary<string, fsData> asDictionary = data.AsDictionary;
			int value;
			fsResult fsResult2 = success += DeserializeMember(asDictionary, null, "c", out value);
			if (fsResult2.Failed)
			{
				return success;
			}
			int value2;
			fsResult fsResult3 = success += DeserializeMember(asDictionary, null, "r", out value2);
			if (fsResult3.Failed)
			{
				return success;
			}
			if (!asDictionary.TryGetValue("a", out fsData value3))
			{
				success.AddMessage("Failed to get flattened list");
				return success;
			}
			fsResult fsResult4 = success += CheckType(value3, fsDataType.Array);
			if (fsResult4.Failed)
			{
				return success;
			}
			Array array = Array.CreateInstance(elementType, value2, value);
			List<fsData> asList = value3.AsList;
			if (value * value2 > asList.Count)
			{
				success.AddMessage("Serialised list has more items than can fit in multidimensional array");
			}
			for (int i = 0; i < value2; i++)
			{
				for (int j = 0; j < value; j++)
				{
					fsData data2 = asList[j + i * value];
					object result = null;
					fsResult result2 = Serializer.TryDeserialize(data2, elementType, ref result);
					success.AddMessages(result2);
					if (!result2.Failed)
					{
						array.SetValue(result, i, j);
					}
				}
			}
			instance = array;
			return success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			return fsMetaType.Get(Serializer.Config, storageType).CreateInstance();
		}
	}
}
