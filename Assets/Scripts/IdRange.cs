using System;

namespace M__M.HaiWang.Tests.SpawnFishTest
{
	public struct IdRange
	{
		public int startId;

		public int endId;

		public string info;

		public IdRange(int startId, int endId, string info = "")
		{
			this.startId = startId;
			this.endId = endId;
			this.info = info;
		}

		public void ForEach(Action<int> action)
		{
			for (int i = startId; i <= endId; i++)
			{
				action(i);
			}
		}

		public bool Contain(int value)
		{
			return value <= endId && value >= startId;
		}

		public int[] GetArray()
		{
			int num = endId - startId + 1;
			int[] array = new int[num];
			int num2 = 0;
			int num3 = startId;
			while (num3 <= endId)
			{
				array[num2] = num3;
				num3++;
				num2++;
			}
			return array;
		}

		public int GetSize()
		{
			return endId - startId + 1;
		}
	}
}
