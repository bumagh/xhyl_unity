namespace M__M.HaiWang.Fish
{
	public class FK3_FishOrder
	{
		public FK3_FishType type;

		public int baseOrder;

		public int capacity;

		public int index;

		public FK3_FishOrder(FK3_FishType type, int baseOrder, int capacity)
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
