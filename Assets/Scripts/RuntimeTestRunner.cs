using FullSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeTestRunner : MonoBehaviour
{
	public Text MessageOutput;

	public bool PrintSerializedData;

	public string Serialize(Type type, object value)
	{
		new fsSerializer().TrySerialize(type, value, out fsData data).AssertSuccessWithoutWarnings();
		return fsJsonPrinter.CompressedJson(data);
	}

	public object Deserialize(Type type, string serializedState)
	{
		fsData data = fsJsonParser.Parse(serializedState);
		object result = null;
		new fsSerializer().TryDeserialize(data, type, ref result).AssertSuccessWithoutWarnings();
		return result;
	}

	private void ExecuteTests<TProvider>() where TProvider : ITestProvider, new()
	{
		Log("Executing test provider " + typeof(TProvider).Name);
		List<TestObject> list = new List<TestObject>();
		foreach (TestItem value in new TProvider().GetValues())
		{
			TestItem current = value;
			list.Add(new TestObject
			{
				StorageType = current.ItemStorageType,
				Original = current.Item,
				EqualityComparer = current.Comparer
			});
		}
		List<TestObject> list2 = new List<TestObject>();
		for (int i = 0; i < list.Count; i++)
		{
			TestObject testObject = list[i];
			try
			{
				Exception ex = null;
				try
				{
					testObject.Serialized = Serialize(testObject.StorageType, testObject.Original);
					testObject.Deserialized = Deserialize(testObject.StorageType, testObject.Serialized);
					if (PrintSerializedData)
					{
						Log(testObject.Serialized);
					}
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
				list[i] = testObject;
				if (ex != null || !testObject.EqualityComparer(testObject.Original, testObject.Deserialized))
				{
					if (ex != null)
					{
						LogError("Caught exception " + ex);
					}
					LogError("Item " + i + " with type " + testObject.Original.GetType() + " is not equal to it's deserialized object. The serialized state was " + testObject.Serialized);
				}
			}
			catch (Exception msg)
			{
				list2.Add(testObject);
				LogError(msg);
			}
		}
		if (list2.Count == 0)
		{
			Log("- Verified all values");
		}
		else
		{
			LogError("!!! Failed " + list2.Count + " values");
		}
	}

	private void LogError(object msg)
	{
		if (Application.isPlaying && MessageOutput != null)
		{
			Text messageOutput = MessageOutput;
			string text = messageOutput.text;
			messageOutput.text = text + "<color=red><b>ERROR</b>: " + msg.ToString() + "</color>" + Environment.NewLine;
		}
		UnityEngine.Debug.LogError(msg);
	}

	private void Log(object msg)
	{
		if (Application.isPlaying && MessageOutput != null)
		{
			Text messageOutput = MessageOutput;
			messageOutput.text = messageOutput.text + msg.ToString() + Environment.NewLine;
		}
		UnityEngine.Debug.Log(msg);
	}

	private void ExecuteAllTests()
	{
		ExecuteTests<CustomIEnumerableProviderWithAdd>();
		ExecuteTests<CustomIEnumerableProviderWithoutAdd>();
		ExecuteTests<CustomListProvider>();
		ExecuteTests<CyclesProvider>();
		ExecuteTests<DateTimeOffsetProvider>();
		ExecuteTests<DateTimeProvider>();
		ExecuteTests<EncodedDataProvider>();
		ExecuteTests<FlagsEnumProvider>();
		ExecuteTests<GuidProvider>();
		ExecuteTests<IDictionaryIntIntProvider>();
		ExecuteTests<IDictionaryStringIntProvider>();
		ExecuteTests<IDictionaryStringStringProvider>();
		ExecuteTests<KeyedCollectionProvider>();
		ExecuteTests<KeyValuePairProvider>();
		ExecuteTests<NullableDateTimeOffsetProvider>();
		ExecuteTests<NullableDateTimeProvider>();
		ExecuteTests<NullableTimeSpanProvider>();
		ExecuteTests<NumberProvider>();
		ExecuteTests<OptOutProvider>();
		ExecuteTests<PrivateFieldsProvider>();
		ExecuteTests<PropertiesProvider>();
		ExecuteTests<QueueProvider>();
		ExecuteTests<SimpleEnumProvider>();
		ExecuteTests<SortedDictionaryProvider>();
		ExecuteTests<StackProvider>();
		ExecuteTests<TimeSpanProvider>();
		ExecuteTests<TypeListProvider>();
		ExecuteTests<TypeProvider>();
	}

	protected void OnEnable()
	{
		ExecuteAllTests();
	}
}
