namespace M__M.HaiWang.Fish
{
	public class FishOrder
	{
		public FishType type;

		public int baseOrder;

		public int capacity;

		public int index;

		public FishOrder(FishType type, int baseOrder, int capacity)
		{
			this.type = type;
			this.baseOrder = baseOrder;
			this.capacity = capacity;
			index = 0;
		}

		public int GetOrder()
		{
			index++;
			index %= capacity;
			return baseOrder + index;
		}
	}
}
