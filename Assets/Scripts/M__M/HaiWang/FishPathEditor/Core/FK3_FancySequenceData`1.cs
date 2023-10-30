namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FK3_FancySequenceData<T>
	{
		public T value;

		public int count;

		public FK3_FancySequenceData<T> Clone()
		{
			FK3_FancySequenceData<T> fK3_FancySequenceData = new FK3_FancySequenceData<T>();
			fK3_FancySequenceData.value = value;
			fK3_FancySequenceData.count = count;
			return fK3_FancySequenceData;
		}

		public T[] GetEnums()
		{
			T[] array = new T[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = value;
			}
			return array;
		}
	}
}
