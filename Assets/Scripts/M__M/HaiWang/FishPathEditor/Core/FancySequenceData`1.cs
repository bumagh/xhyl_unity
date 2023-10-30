namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FancySequenceData<T>
	{
		public T value;

		public int count;

		public FancySequenceData<T> Clone()
		{
			FancySequenceData<T> fancySequenceData = new FancySequenceData<T>();
			fancySequenceData.value = value;
			fancySequenceData.count = count;
			return fancySequenceData;
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
